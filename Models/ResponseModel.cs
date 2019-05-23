using System.Collections.Generic;

namespace Models
{
    public class ResponseModel
    {
        public string RequestId { get; set; }
        public string ResponseCode { get; set; }
        public string ResponseMessage { get; set; }
        public object Data { get; set; }
    }

    public class PlanDetailsAndBalance
    {
        public List<PlanDetails> planDetails { get; set; }
        public decimal TotalBalance { get; set; }
        public decimal TotalSafeLockBalance { get; set; }
        public decimal TotalInterest { get; set; }
    }
}
