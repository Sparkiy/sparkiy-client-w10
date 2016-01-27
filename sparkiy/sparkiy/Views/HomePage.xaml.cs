using Windows.UI.Xaml;
using sparkiy.ViewModels;
using sparkiy.ViewModels.Utilities;

namespace sparkiy.Views
{
	/// <summary>
	/// Home page view.
	/// </summary>
	public sealed partial class HomePage
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HomePage"/> class.
		/// </summary>
		public HomePage()
		{
			// Retrieve view model
			this.ViewModel = ViewModelResolver.Home;

			// Trigger view model loaded on page loaded
			this.Loaded += async (sender, args) =>
			{
				await this.ViewModel.LoadedAsync();
				this.SparkiyPanel.StartRenderLoop();
			};

			// Initialize view and set data context
			this.InitializeComponent();
			this.DataContext = this;
		}
		
		/// <summary>
		/// Gets the view model.
		/// </summary>
		/// <value>
		/// The view model.
		/// </value>
		public IHomeViewModel ViewModel { get; }
	}
}
