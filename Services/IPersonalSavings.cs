using Models;
using System.Threading.Tasks;

namespace Services
{
    public interface IPersonalSavings
    {
        Task<ResponseModel> GetUserDetails();
        Task<ResponseModel> GetPlanDetailsForUser();
        Task<ResponseModel> AddCardDetails(TransactionDetails transactionDetails);
        Task<ResponseModel> GetAllCardDetails();
        Task<ResponseModel> GetAllPlan();
        Task<ResponseModel> AddOnetimeSaving(OneTimeSavings oneTimeSavings);
        Task<ResponseModel> AddPlan(PlanDetails planDetails);
        Task<ResponseModel> CheckDate(string planName);
        Task<ResponseModel> Withdrawal(string planName, decimal AmountToDeduct);
    }
}
