using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Threading;
using sparkiy.Connectors.Tumblr;
using sparkiy.Services.UI;

namespace sparkiy.ViewModels
{
	/// <summary>
	/// Page view mode contract.
	/// </summary>
	public interface IPageViewModel
	{
		/// <summary>
		/// Occurs on page loaded.
		/// </summary>
		Task LoadedAsync();
	}

	/// <summary>
	/// Home view model contract.
	/// </summary>
	public interface  IHomeViewModel : IPageViewModel
	{
		/// <summary>
		/// Gets a value indicating whether this instance are news items loading.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance are news items loading; otherwise, <c>false</c>.
		/// </value>
		bool IsNewsItemsLoading { get; }

		/// <summary>
		/// Gets the news items.
		/// </summary>
		/// <value>
		/// The news items.
		/// </value>
		ObservableCollection<IFeedItem> NewsItems { get; }
	}

	/// <summary>
	/// Home view model.
	/// </summary>
	public sealed class HomeViewModel : IHomeViewModel
	{
		private readonly ITitleBarService titleBarService;


		public HomeViewModel(ITitleBarService titleBarService)
		{
			if (titleBarService == null) throw new ArgumentNullException(nameof(titleBarService));

			this.titleBarService = titleBarService;
		}


		/// <summary>
		/// Occurs on page loaded.
		/// </summary>
		public async Task LoadedAsync()
		{
			// Set title bar color
			this.titleBarService.SetAccentColor();
			
			// Retrieve news items
			var news = await new TumblrNewsService().GetNewsAsync();
			foreach (var newsItem in news)
				this.NewsItems.Add(newsItem);
		}


		/// <summary>
		/// Gets a value indicating whether this instance are news items loading.
		/// </summary>
		/// <value>
		/// <c>true</c> if this instance are news items loading; otherwise, <c>false</c>.
		/// </value>
		public bool IsNewsItemsLoading { get; private set; }

		/// <summary>
		/// Gets the news items.
		/// </summary>
		/// <value>
		/// The news items.
		/// </value>
		public ObservableCollection<IFeedItem> NewsItems { get; } = new ObservableCollection<IFeedItem>();
    }
}
