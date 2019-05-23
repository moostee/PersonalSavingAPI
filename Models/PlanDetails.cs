namespace Models
{
    public class PlanDetails
    {
        public string PlanName { get; set; }
        public string PlanType { get; set; }
        public int IsInterest { get; set; }
        public string StartDate { get; set; }
        public string WithdrawalDate { get; set; }
        public decimal InterestValue { get; set; }
        public decimal Target { get; set; }
        public decimal PlanBalance { get; set; }
        public decimal AmountToSave { get; set; }
        public int UserId { get; set; }
        public string WithdrawalInterval { get; set; }
        public decimal InterestRate { get; set; }
    }

    public class OneTimeSavings
    {
        public decimal Amount { get; set; }
        public string PlanName { get; set; }
    }
}
