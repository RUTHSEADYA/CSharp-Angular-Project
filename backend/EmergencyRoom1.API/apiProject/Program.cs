
using core.Repositories;
using core.Service;
using data;
using data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using service.Service;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
builder.Services.AddEndpointsApiExplorer();
builder.Logging.ClearProviders();
builder.Logging.AddConsole(); 

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "Bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", builder =>
    {
        builder.WithOrigins("http://localhost:4200", "http://localhost:64364", "http://localhost:63027", "https://localhost:63026")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials(); 
    });
});

builder.Services.AddHostedService<PatientScoreUpdaterService>();
builder.Services.AddScoped<IClientAttributeService, ClientAttributeService>();
builder.Services.AddScoped<IClientAttributeRepository, ClientAttributeRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<IPatientFileService, PatientFileService>();
builder.Services.AddScoped<IPatientFileRepository, PatientFileRepository>();
builder.Services.AddScoped<IQueueManagerService, QueueManagerService>();
builder.Services.AddScoped<IQueueManagerRepository, QueueManagerRepository>();
builder.Services.AddScoped<ITreatmentService, TreatmentService>();
builder.Services.AddScoped<ITreatmentRepository, TreatmentRepository>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<PdfService>();

builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(@"Server=MOE-KT-8269087\MSSQLSERVER06;Database=EmergencyDB;TrustServerCertificate=True;Trusted_Connection=True"));


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var queueService = scope.ServiceProvider.GetRequiredService<IQueueManagerService>();
    queueService.StartAutoUpdate();
}


app.UseCors("AllowAngular");

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
