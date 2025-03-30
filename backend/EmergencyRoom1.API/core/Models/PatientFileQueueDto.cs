

namespace core.Models
{
    public class PatientFileQueueDto
    {

        public int Id { get; set; }
        public DateTime EntryTime { get; set; }
        public string Descreption { get; set; }
        public string Status { get; set; }
        public int PatientId { get; set; }
        public double? FinalScore { get; set; }
        public PatientDto Patient { get; set; }
    }
}
