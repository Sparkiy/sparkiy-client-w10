using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Threading;
using sparkiy.Connectors.Discourse;
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
		/// Gets a value indicating whether news items are loading.
		/// </summary>
		bool IsNewsItemsLoading { get; }

		/// <summary>
		/// Gets a value indicating whether news items are empty.
		/// </summary>
		bool AreNewsItemsEmpty { get; }

		/// <summary>
		/// Gets a value indicating whether discussions items are loading.
		/// </summary>
		bool IsDiscussionsItemsLoading { get; }

		/// <summary>
		/// Gets a value indicating whether discussions items are empty.
		/// </summary>
		bool AreDiscussionsItemsEmpty { get; }

		/// <summary>
		/// Gets the news items.
		/// </summary>
		/// <value>
		/// The news items.
		/// </value>
		ObservableCollection<INewsFeedItem> NewsItems { get; }

		/// <summary>
		/// Gets the discussion items.
		/// </summary>
		/// <value>
		/// The discussion items.
		/// </value>
		ObservableCollection<IDiscourseFeedItem> DiscussionItems { get; }
	}

	/// <summary>
	/// Home view model.
	/// </summary>
	public sealed class HomeViewModel : ViewModelBase, IHomeViewModel
	{
		// Services
		private readonly ITitleBarService titleBarService;
		private readonly ITumblrNewsService tumblrNewsService;
		private readonly IDiscourseService discourseService;

		// State variables
		private bool isNewsItemsLoading;
		private bool areNewsItemsEmpty;
        private bool isDiscussionsItemsLoading;
		private bool areDiscussionsItemsEmpty;


		/// <summary>
		/// Initializes a new instance of the <see cref="HomeViewModel"/> class.
		/// </summary>
		/// <param name="titleBarService">The title bar service.</param>
		/// <param name="tumblrNewsService">The tumblr news service.</param>
		/// <param name="discourseService">The discourse service.</param>
		/// <exception cref="System.ArgumentNullException">
		/// titleBarService
		/// or
		/// tumblrNewsService
		/// or
		/// discourseService
		/// </exception>
		public HomeViewModel(
			ITitleBarService titleBarService,
			ITumblrNewsService tumblrNewsService,
			IDiscourseService discourseService)
		{
			if (titleBarService == null) throw new ArgumentNullException(nameof(titleBarService));
			if (tumblrNewsService == null) throw new ArgumentNullException(nameof(tumblrNewsService));
			if (discourseService == null) throw new ArgumentNullException(nameof(discourseService));

			this.titleBarService = titleBarService;
			this.tumblrNewsService = tumblrNewsService;
			this.discourseService = discourseService;
		}


		/// <summary>
		/// Occurs on page loaded.
		/// </summary>
		public async Task LoadedAsync()
		{
			// Set title bar color
			this.titleBarService.SetAccentColor();
			
			// Create tasks list
			var tasks = new List<Func<Task>>()
			{
				this.LoadNewsAsync,
				this.LoadDiscussionsAsync
			};

			// Wait for all tasks to finish
			await Task.WhenAll(tasks.Select(t => t()));
		}

		/// <summary>
		/// Loads the news.
		/// </summary>
		/// <returns></returns>
		private async Task LoadNewsAsync()
		{
			this.IsNewsItemsLoading = true;
			
			var news = await this.tumblrNewsService.GetNewsAsync();
			foreach (var newsItem in news)
				this.NewsItems.Add(newsItem);

			this.AreNewsItemsEmpty = !this.NewsItems.Any();

			this.IsNewsItemsLoading = false;
		}

		/// <summary>
		/// Loads the discussions.
		/// </summary>
		/// <returns></returns>
		private async Task LoadDiscussionsAsync()
		{
			this.IsDiscussionsItemsLoading = true;

			var discussions = await this.discourseService.GetLatestDiscussionsAsync();
			foreach (var discussion in discussions)
				this.DiscussionItems.Add(discussion);

			this.AreDiscussionsItemsEmpty = !this.DiscussionItems.Any();

			this.IsDiscussionsItemsLoading = false;
		}


		/// <summary>
		/// Gets a value indicating whether news items are loading.
		/// </summary>
		public bool IsNewsItemsLoading
		{
			get { return this.isNewsItemsLoading; }
			set
			{
				this.isNewsItemsLoading = value;

				// ReSharper disable once ExplicitCallerInfoArgument
				this.RaisePropertyChanged(nameof(IsNewsItemsLoading));
			}
		}

		/// <summary>
		/// Gets a value indicating whether news items are empty.
		/// </summary>
		public bool AreNewsItemsEmpty
		{
			get { return this.areNewsItemsEmpty; }
			set
			{
				this.areNewsItemsEmpty = value;

				// ReSharper disable once ExplicitCallerInfoArgument
				this.RaisePropertyChanged(nameof(AreNewsItemsEmpty));
			}
		}

		/// <summary>
		/// Gets a value indicating whether discussions items are loading.
		/// </summary>
		public bool IsDiscussionsItemsLoading
		{
			get { return this.isDiscussionsItemsLoading; }
			set
			{
				this.isDiscussionsItemsLoading = value;

				// ReSharper disable once ExplicitCallerInfoArgument
				this.RaisePropertyChanged(nameof(IsDiscussionsItemsLoading));
			}
		}

		/// <summary>
		/// Gets a value indicating whether discussions items are empty.
		/// </summary>
		public bool AreDiscussionsItemsEmpty 
		{
			get { return this.areDiscussionsItemsEmpty; }
			set
			{
				this.areDiscussionsItemsEmpty = value;

				// ReSharper disable once ExplicitCallerInfoArgument
				this.RaisePropertyChanged(nameof(AreDiscussionsItemsEmpty));
			}
		}

		/// <summary>
		/// Gets the news items.
		/// </summary>
		/// <value>
		/// The news items.
		/// </value>
		public ObservableCollection<INewsFeedItem> NewsItems { get; } = new ObservableCollection<INewsFeedItem>();

		/// <summary>
		/// Gets the discussion items.
		/// </summary>
		/// <value>
		/// The discussion items.
		/// </value>
		public ObservableCollection<IDiscourseFeedItem> DiscussionItems { get; } = new ObservableCollection<IDiscourseFeedItem>();
	}
}
