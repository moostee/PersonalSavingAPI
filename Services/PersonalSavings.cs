using DAL;
using Microsoft.Extensions.Logging;
using Models;
using System;
using System.Threading.Tasks;

namespace Services
{
    public class PersonalSavings : IPersonalSavings
    {
        private ILogger<PersonalSavings> _logger;
        private readonly IPersonalSavingsRepo _personalSavingsRepo;
        public PersonalSavings(IPersonalSavingsRepo personalSavingsRepo, ILogger<PersonalSavings> logger)
        {
            _personalSavingsRepo = personalSavingsRepo;
            _logger = logger;
        }

        public async Task<ResponseModel> GetUserDetails()
        {
            ResponseModel response = new ResponseModel();

            try
            {
                var result = await _personalSavingsRepo.GetUserDetails();
                //_logger.LogInformation("");
                if (result.UserId > 0)
                {
                    response.ResponseCode = "00";
                    _logger.LogInformation("Records gotten from the database");
                    response.ResponseMessage = "Successful";
                    response.Data = result;
                }
                else
                {
                    response.ResponseCode = "02";
                    response.ResponseMessage = "User details doesnt exist";
                    response.Data = null;
                    _logger.LogError($"An error occured.. couldn't get details from the database to get user details");
                }

            }
            catch (Exception ex)
            {
                response.ResponseCode = "99";
                response.ResponseMessage = "An error occured while processing the request";
                _logger.LogError($"An error occured.. couldn't get details from the database to get user details{Environment.NewLine} {ex.Message}");
            }

            return response;
        }

        public async Task<ResponseModel> GetPlanDetailsForUser()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var result = await _personalSavingsRepo.GetPlanDetailsForUser();
                if (result.planDetails.Count > 0)
                {
                    response.Data = result;
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Successful";
                }
                else
                {
                    response.Data = null;
                    response.ResponseMessage = "No Plan details available";
                    response.ResponseCode = "02";
                }
            }
            catch (Exception e)
            {
                response.ResponseCode = "99";
                response.ResponseMessage = "An error occured while processing the request";
                _logger.LogError($"An Error occured while processing the request {e.Message}");
            }

            return response;
        }

        public async Task<ResponseModel> AddCardDetails(TransactionDetails transactionDetails)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                int result = await _personalSavingsRepo.AddCardDetails(transactionDetails);
                if (result > 0)
                {
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Successfully added your card";
                }
                else
                {
                    response.ResponseCode = "99";
                    response.ResponseMessage = "An error occured while adding your card details. please check your network";
                }
            }
            catch (Exception e)
            {
                response.ResponseCode = "99";
                response.ResponseMessage = "An error occured while processing the request";
                _logger.LogError($"An Error occured while processing the request {e.Message}");
            }

            return response;
        }

        public async Task<ResponseModel> GetAllCardDetails()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var result = await _personalSavingsRepo.GetAllCardDetails();
                if (result != null)
                {
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Successful";
                    response.Data = result;
                }
                else
                {
                    response.ResponseCode = "02";
                    response.ResponseMessage = "No card details for customer";
                    response.Data = null;
                }
            }
            catch (Exception e)
            {
                response.ResponseCode = "99";
                response.ResponseMessage = "An error occured while processing the request. check connection";
                _logger.LogError($"An Error occured while processing the request {e.Message}");
            }

            return response;
        }

        public async Task<ResponseModel> GetAllPlan()
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var result = await _personalSavingsRepo.GetAllPlanName();
                if (result != null)
                {
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Successful";
                    response.Data = result;
                }
                else
                {
                    response.ResponseCode = "02";
                    response.ResponseMessage = "No plan created for customer";
                    response.Data = null;
                }
            }
            catch (Exception e)
            {
                response.ResponseCode = "99";
                response.ResponseMessage = "An error occured while processing the request. check connection";
                _logger.LogError($"An Error occured while processing the request {e.Message}");
            }

            return response;
        }

        public async Task<ResponseModel> AddOnetimeSaving(OneTimeSavings oneTimeSavings)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var result = await _personalSavingsRepo.AddOnetimeSaving(oneTimeSavings);
                if (result > 0)
                {
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Successful one time saving";

                }
                else
                {
                    response.ResponseCode = "99";
                    response.ResponseMessage = "An error occured while adding your card details. please check your network";
                }
            }
            catch (Exception e)
            {
                response.ResponseCode = "99";
                response.ResponseMessage = "An error occured while processing the request. check connection";
                _logger.LogError($"An Error occured while processing the request {e.Message}");
            }

            return response;
        }

        public async Task<ResponseModel> AddPlan(PlanDetails planDetails)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var result = await _personalSavingsRepo.AddPlanDetails(planDetails);
                if (result > 0)
                {
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Successfully added a new plan";

                }
                else
                {
                    response.ResponseCode = "99";
                    response.ResponseMessage = "An error occured while adding a new plan. please check your network";
                }
            }
            catch (Exception e)
            {
                response.ResponseCode = "99";
                response.ResponseMessage = "An error occured while processing the request. check connection";
                _logger.LogError($"An Error occured while processing the request {e.Message}");
            }

            return response;
        }

        public async Task<ResponseModel> CheckDate(string planName)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var result = await _personalSavingsRepo.CheckDateAgainstWithdrawalDate(planName);
                if (result.WithdrawalDate != null)
                {
                    if (DateTime.Now <= DateTime.Parse(result.WithdrawalDate))
                    {
                        response.ResponseMessage =
                            "2.5% commission will be deducted for amount less than 80% of your plan balance and 5% commission will be deducted for amount greather than 80% of your plan balance";
                    }
                    else
                    {
                        response.ResponseMessage = "No commission to be deducted";
                    }
                    response.ResponseCode = "00";

                    response.Data = result;
                }
                else
                {
                    response.ResponseCode = "99";
                    response.ResponseMessage = "An error occured while adding a new plan. please check your network";
                }
            }
            catch (Exception e)
            {
                response.ResponseCode = "99";
                response.ResponseMessage = "An error occured while processing the request. check connection";
                _logger.LogError($"An Error occured while processing the request {e.Message}");
            }

            return response;
        }

        public async Task<ResponseModel> Withdrawal(string planName, decimal AmountToDeduct)
        {
            ResponseModel response = new ResponseModel();
            try
            {
                var result = await _personalSavingsRepo.Withdrawal(planName, AmountToDeduct);
                if (result > 0)
                {
                    response.ResponseCode = "00";
                    response.ResponseMessage = "Successful withdrawal made";

                }
                else
                {
                    response.ResponseCode = "99";
                    response.ResponseMessage = "An error occured while trying to withdraw. please check your network";
                }
            }
            catch (Exception e)
            {
                response.ResponseCode = "99";
                response.ResponseMessage = "An error occured while processing the request. check connection";
                _logger.LogError($"An Error occured while processing the request {e.Message}");
            }

            return response;
        }
    }
}
