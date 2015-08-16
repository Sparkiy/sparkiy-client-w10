using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace sparkiy.Connectors.Feed
{
	/// <summary>
	/// Feed item.
	/// </summary>
	public class FeedItem : IFeedItem
	{
		/// <summary>
		/// Compiled regular expression for performance.
		/// </summary>
		protected static readonly Regex HtmlTagRegex = new Regex("<.*?>", RegexOptions.Compiled);

		/// <summary>
		/// Gets the formated date.
		/// </summary>
		/// <param name="date">The date.</param>
		/// <returns>Returns string containing formated short date.</returns>
		protected static string GetDateFormated(DateTime date)
		{
			return Windows.Globalization.DateTimeFormatting.DateTimeFormatter.ShortDate.Format(date);
		}

		/// <summary>
		/// Gets the text from html string.
		/// </summary>
		/// <param name="content">The html string.</param>
		/// <returns>Returns string containing only text from html string, this will remove all tags and only leave text.</returns>
		protected string GetText(string content)
		{
			// Remove tags
			content = HtmlTagRegex.Replace(content, string.Empty);

			// Remove title from start if exists
			if (content.StartsWith(this.Title))
				content = content.Skip(this.Title.Length).Aggregate(string.Empty, (s, c) => s += c);

			// Trim start and end
			content = content.Trim(' ', ':', '-');

			return content;
		}

		/// <summary>
		/// Gets or sets the content.
		/// </summary>
		/// <value>
		/// The content.
		/// </value>
		public string Content { get; set; }

		/// <summary>
		/// The text
		/// </summary>
		public string Text => GetText(this.Content);

		/// <summary>
		/// Gets or sets the link.
		/// </summary>
		/// <value>
		/// The link.
		/// </value>
		public Uri Link { get; set; }

		/// <summary>
		/// Gets or sets the publishing date.
		/// </summary>
		/// <value>
		/// The publishing date.
		/// </value>
		public DateTime PublishingDate { get; set; }

		/// <summary>
		/// Gets or sets the formated date.
		/// </summary>
		/// <value>
		/// The formated date.
		/// </value>
		public string FormatedDate => GetDateFormated(this.PublishingDate);

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		public string Title { get; set; }
	}
}