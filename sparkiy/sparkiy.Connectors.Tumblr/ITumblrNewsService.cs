using System.Collections.Generic;
using System.Threading.Tasks;

namespace sparkiy.Connectors.Tumblr
{
	public interface ITumblrNewsService
	{
		/// <summary>
		/// Gets the news feed items.
		/// </summary>
		/// <returns>Returns collection of feed items.</returns>
		Task<IEnumerable<INewsFeedItem>> GetNewsAsync();
	}
}