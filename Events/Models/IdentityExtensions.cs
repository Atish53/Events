using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Events.Models
{
    public static class IdentityExtensions
    {
        public static string GetName(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Name");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetAddress(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Address");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetCity(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("City");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetState(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("State");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetPostalCode(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("PostalCode");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetCountry(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("Country");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string GetPhoneNo(this IIdentity identity)
        {
            var claim = ((ClaimsIdentity)identity).FindFirst("PhoneNo");
            // Test for null to avoid issues during local testing
            return (claim != null) ? claim.Value : string.Empty;
        }
    }
}