using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Resources.Core;
using Windows.Web.Syndication;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.OAuth;

namespace sparkiy.Connectors.Tumblr
{
	public interface ITumblrNewsService
	{
		Task<List<IFeedItem>> GetNewsAsync();
	}

	public sealed class TumblrNewsService : ITumblrNewsService
	{
		private const string NewsUrl = "http://blog.sparkiy.com/rss";


		public async Task<List<IFeedItem>> GetNewsAsync()
		{
			//The web object that will retrieve our feeds..
			SyndicationClient client = new SyndicationClient();

			//The URL of our feeds..
			Uri feedUri = new Uri(NewsUrl);

			//Retrieve async the feeds..
			var feed = await client.RetrieveFeedAsync(feedUri);

			//The list of our feeds..
			List<IFeedItem> feedData = new List<IFeedItem>();

			//Fill up the list with each feed content..
			foreach (SyndicationItem item in feed.Items)
			{
				var feedItem = new FeedItem();
				feedItem.Content = item.Summary.Text;
				feedItem.Link = item.Links[0].Uri;
				feedItem.PubDate = item.PublishedDate.DateTime;
				feedItem.Title = item.Title.Text;

				feedData.Add(feedItem);
			}

			return feedData;
		}
	}


	public class FeedItem : IFeedItem
	{
		public string Content { get; set; }
		public Uri Link { get; set; }
		public DateTime PubDate { get; set; }
		public string Title { get; set; }
	}

	public interface IFeedItem
	{
		string Content { get; set; }
		Uri Link { get; set; }
		DateTime PubDate { get; set; }
		string Title { get; set; }
	}
}
