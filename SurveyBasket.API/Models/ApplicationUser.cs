using Microsoft.AspNetCore.Identity;

namespace SurveyBasket.API.Models
{
	public sealed class ApplicationUser:IdentityUser
	{
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
