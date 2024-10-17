using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.API.Contracts.Polls
{
    public record PollRequest(
        string Title,
        string Summary,
        bool IsPublished,
        DateOnly StartsAt,
        DateOnly EndsAt
    );
}
