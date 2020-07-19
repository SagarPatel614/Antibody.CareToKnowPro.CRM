using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Antibody.CareToKnowPro.CRM.Models;
using Microsoft.AspNetCore.Mvc;

namespace Antibody.CareToKnowPro.CRM.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly DbAntibodyCareToKnowProContext _dbContext;

        public BaseController(DbAntibodyCareToKnowProContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected BaseController()
        {
            
        }

        protected LoginProfile _CurrentUser
        {
            get
            {
                if (User.Identity.IsAuthenticated)
                {
                    var existingClaimsIdentity = (ClaimsIdentity)User.Identity;
                    int currentUserId = Convert.ToInt32(existingClaimsIdentity.Name);
                    var authenticatedUser = _dbContext.LoginProfile.FirstOrDefault(a=>a.LoginProfileId == currentUserId);

                    if (authenticatedUser != null)
                    {
                        return authenticatedUser;
                    }

                    return new LoginProfile();
                }

                return new LoginProfile();
            }
        }

        // look at current culture and return a 1 or 0
        protected int _LangInt
        {
            get
            {
                switch (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLower())
                {
                    case "fr-ca":
                        return 1;
                    default:
                        return 0;
                }
            }
        }

        // look at current culture and return "en" or "fr"
        protected string _LangStr
        {
            get
            {
                switch (System.Threading.Thread.CurrentThread.CurrentUICulture.Name.ToLower())
                {
                    case "fr-ca":
                        return "fr";
                    default:
                        return "en";
                }
            }
        }
    }
}