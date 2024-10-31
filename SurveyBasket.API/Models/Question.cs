﻿namespace SurveyBasket.API.Models
{
	public sealed class Question : AuditModel
	{
		public int Id { get; set; }
		public string Content { get; set; } = string.Empty;
		public bool IsActive { get; set; } = true;



		public int PollId { get; set; }
		public Poll Poll { get; set; } = default!;

		public ICollection<Answer> Answers { get; set; } = [];
	}
}
