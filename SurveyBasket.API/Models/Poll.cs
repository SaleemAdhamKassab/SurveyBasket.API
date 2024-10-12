using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.API.Models
{
	public class Poll
	{
		[Key]
		public int Id { get; set; }
		public string Title { get; set; }
		public string Summary { get; set; }
		public bool IsPublished { get; set; }
        public DateOnly StartsAt { get; set; }
        public DateOnly EndsAt { get; set; }
    }
}
