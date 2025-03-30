
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace core.Models
{
    public class PatientFile
    {
        [Key]
        public int Id { get; set; }

        public DateTime EntryTime { get; set; }=DateTime.Now;
        public DateTime? LastScoreUpdate { get; set; }

        public string Descreption { get; set; }

        public string Status { get; set; }

        [ForeignKey("Patient")]
        public int PatientId { get; set; }

      [JsonIgnore]
        public Patient? Patient { get; set; }

        public ICollection<ClientAttribute> PatientAttributes { get; set; } = new List<ClientAttribute>();

        public double? FinalScore { get; set; }

        
        public void UpdateFinalScore()
        {
            if (FinalScore == null)
                FinalScore = 0;

            if (LastScoreUpdate == null)
                LastScoreUpdate = EntryTime; 

            TimeSpan elapsedTime = DateTime.Now - LastScoreUpdate.Value;

            int pointsToAdd = (int)(elapsedTime.TotalMinutes / 2); 

            if (pointsToAdd > 0)
            {
                FinalScore += pointsToAdd;
                LastScoreUpdate = LastScoreUpdate.Value.AddMinutes(pointsToAdd * 2); 
            }
        }


    }

}

