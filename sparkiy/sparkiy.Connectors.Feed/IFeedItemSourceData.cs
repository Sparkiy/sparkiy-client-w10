using System;

namespace sparkiy.Connectors.Feed
{
	/// <summary>
	/// Feed item contract containing all source data.
	/// </summary>
	public interface IFeedItemSourceData
	{
		/// <summary>
		/// Gets or sets the content.
		/// </summary>
		/// <value>
		/// The content.
		/// </value>
		string Content { get; set; }

		/// <summary>
		/// Gets or sets the link.
		/// </summary>
		/// <value>
		/// The link.
		/// </value>
		Uri Link { get; set; }

		/// <summary>
		/// Gets or sets the publishing date.
		/// </summary>
		/// <value>
		/// The publishing date.
		/// </value>
		DateTime PublishingDate { get; set; }

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>
		/// The title.
		/// </value>
		string Title { get; set; }
	}
}