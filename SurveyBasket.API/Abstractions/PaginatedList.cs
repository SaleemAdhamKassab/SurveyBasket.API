using Microsoft.EntityFrameworkCore;

namespace SurveyBasket.API.Abstractions
{
	public class PaginatedList<T>(int pageNumber, int count, int pageSize, List<T> items)
	{
		public int PageNumber { get; set; } = pageNumber;
		public int TotalPages { get; set; } = (int)Math.Ceiling(count / (double)pageSize);
		public List<T> Items { get; private set; } = items;


		public bool HasPreviousPage => PageNumber > 1;
		public bool HasNextPage => PageNumber < TotalPages;

		public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> query, int pageNumber, int pageSize)
		{
			var count = await query.CountAsync();
			var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

			return new PaginatedList<T>(pageNumber, count, pageSize, items);
		}
	}
}
