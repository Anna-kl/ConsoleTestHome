using System.ComponentModel.DataAnnotations;

namespace TestHomeWork.Models
{
    public class Currency
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string CurrencyName { get; set; }
        public int CurrencyCount { get; set; }
        public double CurrencyRate { get; set; }

    }
}
