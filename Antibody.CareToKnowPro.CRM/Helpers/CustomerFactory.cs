using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Antibody.CareToKnowPro.CRM.Models;
using CustomerIOSharp;
using Microsoft.AspNetCore.Http;

namespace Antibody.CareToKnowPro.CRM.Helpers
{
    public class CustomerFactory : ICustomerFactory
    {
        private static DbAntibodyCareToKnowProContext _dbContext;
        private static User _user;


        public CustomerFactory(User newUser)
        {
            _user = newUser;
        }

        public ICustomerDetails GetCustomerDetails()
        {

            var user = GetCurrentUser();
            return user?.AsCustomer();
        }

        public string GetCustomerId()
        {
            var user = GetCurrentUser();
            return user?.UserId.ToString();
        }

        public static User GetCurrentUser()
        {
            return _user;

        }
    }
}
