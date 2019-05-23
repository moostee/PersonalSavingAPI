using Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL
{
    public interface IPersonalSavingsRepo
    {
        Task<UserDetails> GetUserDetails();
        Task<PlanDetailsAndBalance> GetPlanDetailsForUser();
        Task<int> AddCardDetails(TransactionDetails transactionDetails);
        Task<List<TransactionDetails>> GetAllCardDetails();
        Task<List<string>> GetAllPlanName();
        Task<int> AddOnetimeSaving(OneTimeSavings oneTimeSavings);
        Task<int> AddPlanDetails(PlanDetails planDetails);
        Task<PlanDetails> CheckDateAgainstWithdrawalDate(string planName);
        Task<int> Withdrawal(string planName, decimal AmountToDeduct);
    }
}
