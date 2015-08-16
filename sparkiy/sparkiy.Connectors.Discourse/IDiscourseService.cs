using System.Collections.Generic;
using System.Threading.Tasks;

namespace sparkiy.Connectors.Discourse
{
	/// <summary>
	/// Discourse service.
	/// </summary>
	public interface IDiscourseService
	{
		/// <summary>
		/// Gets the news feed items.
		/// </summary>
		/// <returns>Returns collection of feed items.</returns>
		Task<IEnumerable<IDiscourseFeedItem>> GetLatestDiscussionsAsync();
	}
}