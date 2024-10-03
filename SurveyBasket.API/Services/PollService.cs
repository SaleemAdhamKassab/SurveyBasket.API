using SurveyBasket.API.Model;

namespace SurveyBasket.API.Services
{
	public interface IPollService
	{
		Poll? Get(int id);
		List<Poll> GetAll();
		Poll Add(Poll poll);
		bool Update(int id, Poll poll);
		bool Delete(int id);
	}
	public class PollService : IPollService
	{
		private static readonly List<Poll> _polls = [
			new Poll{Id =1,Title="poll1 Title",Description = "Poll1 Desc"}
		];

		public Poll? Get(int id) => _polls.SingleOrDefault(e => e.Id == id);
		public List<Poll> GetAll() => _polls;

		public Poll Add(Poll poll)
		{
			poll.Id = _polls.Count + 1;
			_polls.Add(poll);
			return poll;
		}
		public bool Update(int id, Poll poll)
		{
			Poll pollToUpdate = Get(id);

			if (pollToUpdate == null)
				return false;

			pollToUpdate.Title = poll.Title;
			pollToUpdate.Description = poll.Description;

			return true;
		}
		public bool Delete(int id)
		{
			Poll pollToDelete = Get(id);

			if (pollToDelete == null)
				return false;

			_polls.Remove(pollToDelete);

			return true;
		}
	}
}
