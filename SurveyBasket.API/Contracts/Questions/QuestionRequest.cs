﻿namespace SurveyBasket.API.Contracts.Questions
{
	public class QuestionRequest
	{
		public string Content { get; set; } = string.Empty;
		public List<string> Answers { get; set; } = [];
	}
}