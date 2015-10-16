using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Web.Syndication;
using Serilog;

namespace sparkiy.Connectors.Tumblr
{
	/// <summary>
	/// Tumblr news service.
	/// </summary>
	public sealed class TumblrNewsService : ITumblrNewsService
	{
		private readonly ILogger logger;
		private readonly Uri newsUrl = new Uri("http://blog.sparkiy.com/rss");


		/// <summary>
		/// Initializes a new instance of the <see cref="TumblrNewsService"/> class.
		/// </summary>
		/// <param name="logger">The logger.</param>
		/// <exception cref="System.ArgumentNullException">
		/// logger
		/// </exception>
		public TumblrNewsService(ILogger logger)
		{
			if (logger == null) throw new ArgumentNullException(nameof(logger));

			this.logger = logger;
		}


		/// <summary>
		/// Gets the news feed items.
		/// </summary>
		/// <returns>Returns collection of feed items.</returns>
		public async Task<IEnumerable<INewsFeedItem>> GetNewsAsync()
		{
			try
			{
				// The web object that will retrieve our feeds
				var client = new SyndicationClient();

				// Retrieve the feeds
				var feed = await client.RetrieveFeedAsync(this.newsUrl);

				// Construct feed collection
				var feedData = new List<INewsFeedItem>(
					feed.Items.Select(item =>
						new NewsFeedItem
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
				this.logger.Error(ex, "Failed to retrieve Tumblr news feed.");
				return new List<INewsFeedItem>();
			}
		}
	}
}
