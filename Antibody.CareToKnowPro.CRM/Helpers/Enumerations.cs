using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Antibody.CareToKnowPro.CRM.Helpers
{
    public enum AccountType
    {
        Member = 1,
        Admin = 2
    }

    public enum UserType
    {
        Member = 1,
        Admin = 2
    }

    public enum AccountStatus
    {
        Active = 1,
        InActive = 2,
        Delete = 3
    }

    public enum TokenType
    {
        NewAccount = 1,
        PasswordReset = 2
    }

    public enum UserStatus
    {
        [Description("Practicing")] Practicing,
        [Description("Retired")] Retired,
        [Description("Moved")] Moved
    }

    public enum WebsiteAliasType
    {
        Redirect = 1,
        Alternate = 2
    }

    public enum Language
    {
        English = 1,
        French = 2
    }

    public enum GoogleAnalyticsDimension
    {
        ProductID = 1,
        ProductName = 2,
        Account = 3,
        PageType = 4,
        PageSlug = 5
    }

    public enum GoogleAnalyticsMetric
    {
        AdImpression = 1
    }

    public enum ResourceType
    {
        Podcast = 1,
        Video = 2,
        Resource = 3

    }

    public enum EventType
    {
        [Display(Name = "HCP Created")] UserCreated = 0,
        [Display(Name = "HCP Updated")] UserUpdated = 1,
        [Display(Name = "HCP Deleted")] UserDeleted = 2
    }

    public enum ActionType
    {
        [Display(Name = "Added")]
        Added = 1,

        [Display(Name = "Modified")]
        Modified = 2,

        [Display(Name = "Deleted")]
        Deleted = 3,

        [Display(Name = "Unknown")]
        Unknown = 4
    }
}
