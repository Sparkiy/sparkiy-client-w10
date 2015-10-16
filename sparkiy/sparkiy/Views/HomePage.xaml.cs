using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using sparkiy.Connectors.IoT.Windows;
using sparkiy.Extensions;
using sparkiy.ViewModels;
using sparkiy.ViewModels.Utilities;

namespace sparkiy.Views
{
	/// <summary>
	/// Home page view.
	/// </summary>
	public sealed partial class HomePage : Page
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HomePage"/> class.
		/// </summary>
		public HomePage()
		{
			// Retrieve view model
			this.ViewModel = ViewModelResolver.Home;

			// Trigger view model loaded on page loaded
			this.Loaded += async (sender, args) => await this.ViewModel.LoadedAsync();

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
