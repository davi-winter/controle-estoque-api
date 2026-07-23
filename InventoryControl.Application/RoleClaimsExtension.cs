using InventoryControl.Domain.Entities;
using System.Security.Claims;

namespace InventoryControl.Application
{
    public static class RoleClaimsExtension
    {
        public static IEnumerable<Claim> GetClaims(this User user)
        {
            var result = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            switch (user.Role)
            {
                case "admin":
                    result.Add(new Claim(ClaimTypes.Role, "admin"));
                    result.Add(new Claim(ClaimTypes.Role, "manager"));
                    result.Add(new Claim(ClaimTypes.Role, "operator"));
                    break;
                case "manager":
                    result.Add(new Claim(ClaimTypes.Role, "manager"));
                    result.Add(new Claim(ClaimTypes.Role, "operator"));
                    break;
                case "operator":
                    result.Add(new Claim(ClaimTypes.Role, "operator"));
                    break;
                //default:
                //    throw new ArgumentException($"Invalid role: {user.Role}");
            }

            return result;
        }
    }
}
