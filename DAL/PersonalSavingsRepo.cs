using Dapper;
using Microsoft.Extensions.Configuration;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace DAL
{
    public class PersonalSavingsRepo : IPersonalSavingsRepo
    {
        private readonly IConfiguration _config;
        public PersonalSavingsRepo(IConfiguration configuration)
        {
            _config = configuration;
        }

        public async Task<int> AddCardDetails(TransactionDetails transactionDetails)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@CardNumber", transactionDetails.CardNumber);
                parameter.Add("@Cvv2", transactionDetails.Cvv2);
                parameter.Add("@ExpiryDate", transactionDetails.ExpiryDate);
                result = await con.QueryFirstAsync<int>("Hackathon_GroupD_AddCardDetails", parameter, commandType: CommandType.StoredProcedure);
            }

            return result;
        }

        public async Task<int> AddOnetimeSaving(OneTimeSavings oneTimeSavings)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@Amount", oneTimeSavings.Amount);
                parameter.Add("@PlanName", oneTimeSavings.PlanName);
                result = await con.QueryFirstAsync<int>("Hackathon_GroupD_AddOneTimeSaving", parameter, commandType: CommandType.StoredProcedure);
            }

            return result;
        }

        public async Task<int> AddPlanDetails(PlanDetails planDetails)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@PlanName", planDetails.PlanName);
                parameter.Add("@PlanType", planDetails.PlanType);
                parameter.Add("@IsInterest", planDetails.IsInterest);
                parameter.Add("@StartDate", planDetails.StartDate);
                parameter.Add("@WithdrawalDate", planDetails.WithdrawalDate);
                parameter.Add("@Target", planDetails.Target);
                parameter.Add("@AmountToSave", planDetails.AmountToSave);
                parameter.Add("@CardDetailsId", 1);
                parameter.Add("@UserId", 1);
                parameter.Add("@InterestRate", planDetails.InterestRate = (planDetails.IsInterest == 1) ? 10 : 0);
                parameter.Add("@WithdrawalInterval", planDetails.WithdrawalInterval);
                result = await con.QueryFirstAsync<int>("Hackathon_GroupD_AddPlan", parameter, commandType: CommandType.StoredProcedure);
            }

            return result;
        }

        public async Task<PlanDetails> CheckDateAgainstWithdrawalDate(string planName)
        {
            //int response = 0;
            PlanDetails plan = new PlanDetails();

            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@PlanName", planName);
                var result = await con.QueryFirstAsync<PlanDetails>("Hackathon_GroupD_CheckWithdrawalDate", parameter, commandType: CommandType.StoredProcedure);
                if (!String.IsNullOrEmpty(result.WithdrawalDate))
                {
                    plan.WithdrawalDate = result.WithdrawalDate;
                    plan.PlanBalance = result.PlanBalance;
                }

            }

            return plan;
        }

        public async Task<List<TransactionDetails>> GetAllCardDetails()
        {
            List<TransactionDetails> transaction = new List<TransactionDetails>();
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameter = new DynamicParameters();
                var result = await con.QueryAsync<TransactionDetails>("Hackathon_GroupD_GetAllCardDetails", parameter, commandType: CommandType.StoredProcedure);
                if (result.ToList().Count > 0)
                {
                    foreach (var item in result.ToList())
                    {
                        transaction.Add(new TransactionDetails()
                        {
                            CardNumber = item.CardNumber,
                            Cvv2 = item.Cvv2,
                            ExpiryDate = item.ExpiryDate
                        });
                    }
                }
            }
            return transaction;
        }

        public async Task<List<string>> GetAllPlanName()
        {
            List<string> planName = new List<string>();
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameter = new DynamicParameters();
                var result = await con.QueryAsync<PlanDetails>("Hackathon_GroupD_GetAllPlan", parameter, commandType: CommandType.StoredProcedure);
                if (result.ToList().Count > 0)
                {
                    foreach (var item in result.ToList())
                    {
                        planName.Add(item.PlanName);
                    }
                }
            }

            return planName;
        }

        public async Task<PlanDetailsAndBalance> GetPlanDetailsForUser()
        {
            PlanDetailsAndBalance planAndTotalAndBalance = new PlanDetailsAndBalance();
            List<PlanDetails> planDetails = new List<PlanDetails>();

            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameter = new DynamicParameters();
                var result = await con.QueryAsync<PlanDetails>("Hackathon_GroupD_GetPlanForUser", parameter, commandType: CommandType.StoredProcedure);
                if (result.ToList().Count > 0)
                {
                    foreach (var item in result.ToList())
                    {
                        planDetails.Add(new PlanDetails()
                        {
                            PlanName = item.PlanName,
                            PlanType = item.PlanType,
                            PlanBalance = item.PlanBalance,
                            InterestValue = item.InterestValue,
                            AmountToSave = item.AmountToSave,
                            StartDate = item.StartDate,
                            WithdrawalDate = item.WithdrawalDate,
                            Target = item.Target,
                            WithdrawalInterval = item.WithdrawalInterval,
                            InterestRate = item.InterestRate
                        });

                        planAndTotalAndBalance.TotalBalance = planAndTotalAndBalance.TotalBalance + item.PlanBalance;
                        planAndTotalAndBalance.TotalSafeLockBalance = (item.PlanType == "Safe Lock") ? planAndTotalAndBalance.TotalSafeLockBalance + item.PlanBalance : planAndTotalAndBalance.TotalSafeLockBalance;
                        planAndTotalAndBalance.TotalInterest = planAndTotalAndBalance.TotalInterest + item.InterestValue;
                    }

                    planAndTotalAndBalance.planDetails = planDetails;
                }

            }
            return planAndTotalAndBalance;
        }

        public async Task<UserDetails> GetUserDetails()
        {
            UserDetails user = new UserDetails();
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameter = new DynamicParameters();
                var result = await con.QuerySingleOrDefaultAsync<UserDetails>("Hackathon_GroupD_GetUserDetails", parameter, commandType: CommandType.StoredProcedure);
                if (result.UserId > 0 && result.Name != null)
                {
                    user.Name = result.Name;
                    user.UserId = result.UserId;
                }

            }
            return user;
        }

        public async Task<int> Withdrawal(string planName, decimal AmountToDeduct)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(_config.GetConnectionString("DefaultConnection")))
            {

                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters parameter = new DynamicParameters();
                parameter.Add("@PlanName", planName);
                parameter.Add("@AmountToDeduct", AmountToDeduct);
                result = await con.QueryFirstAsync<int>("Hackathon_GroupD_WithdrawAmount", parameter, commandType: CommandType.StoredProcedure);
            }

            return result;
        }
    }
}
