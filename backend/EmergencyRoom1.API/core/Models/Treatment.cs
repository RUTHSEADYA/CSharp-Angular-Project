

namespace core.Models
{
    public class Treatment
    {

        public int Id { get; set; }
        public string Diagnosis { get; set; }  // אבחנה
        public string Procedure { get; set; }  // פעולה רפואית שבוצעה
        public string Recommendations { get; set; }  // המלצות להמשך
        
    }
}
