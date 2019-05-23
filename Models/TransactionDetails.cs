using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class TransactionDetails
    {
        [Required]
        public long CardNumber { get; set; }
        [Required]
        public string ExpiryDate { get; set; }
        [Required]
        public int Cvv2 { get; set; }
    }
}
