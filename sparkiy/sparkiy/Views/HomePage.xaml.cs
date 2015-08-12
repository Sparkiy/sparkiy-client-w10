using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using sparkiy.ViewModels;
using sparkiy.ViewModels.Utilities;

namespace sparkiy.Views
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
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
			this.Loaded += (sender, args) => this.ViewModel.LoadedAsync();

			this.InitializeComponent();
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
