using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Web.Syndication;
using Serilog;

namespace sparkiy.Connectors.Discourse
{
	/// <summary>
	/// Discourse service.
	/// </summary>
	public sealed class DiscourseService : IDiscourseService
	{
		private readonly ILogger logger;
		private readonly Uri newsUrl = new Uri("http://talk.sparkiy.com/latest.rss");


		/// <summary>
		/// Initializes a new instance of the <see cref="DiscourseService"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <exception cref="System.ArgumentNullException"></exception>
		public DiscourseService(ILogger logger)
		{
			if (logger == null) throw new ArgumentNullException(nameof(logger));

			this.logger = logger;
		}


		/// <summary>
		/// Gets the news feed items.
		/// </summary>
		/// <returns>Returns collection of feed items.</returns>
		public async Task<IEnumerable<IDiscourseFeedItem>> GetLatestDiscussionsAsync()
		{
			try
			{
				// The web object that will retrieve our feeds
				var client = new SyndicationClient();

				// Retrieve the feeds
				var feed = await client.RetrieveFeedAsync(this.newsUrl);

				// Construct feed collection
				var feedData = new List<IDiscourseFeedItem>(
					feed.Items.Select(item =>
						new DiscourseFeedItem
						{
							Content = item.Summary?.Text ?? string.Empty,
							Link = item.Links[0]?.Uri,
							PublishingDate = item.PublishedDate.DateTime,
							Title = item.Title?.Text ?? string.Empty
						}));

				return feedData;
			}
			catch (Exception ex)
			{
				this.logger.Error(ex, "Failed to retrieve Discourse latest feed.");
				return new List<IDiscourseFeedItem>();
			}
		}
	}
}
