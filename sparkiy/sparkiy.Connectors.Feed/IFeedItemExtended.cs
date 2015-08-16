namespace sparkiy.Connectors.Feed
{
	/// <summary>
	/// Feed item contract containing extended properties.
	/// </summary>
	public interface IFeedItemExtended
	{
		/// <summary>
		/// Gets the text only from content.
		/// </summary>
		/// <value>
		/// The text from content.
		/// </value>
		string Text { get; }

		/// <summary>
		/// Gets the formated date.
		/// </summary>
		/// <value>
		/// The formated date.
		/// </value>
		string FormatedDate { get; }
	}
}