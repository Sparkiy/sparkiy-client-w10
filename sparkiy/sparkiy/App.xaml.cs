using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Practices.Unity;
using Mindscape.Raygun4Net;
using sparkiy.Connectors.Discourse;
using sparkiy.Connectors.Tumblr;
using sparkiy.DI;
using sparkiy.Logging;
using sparkiy.Services.UI;
using sparkiy.ViewModels;
using sparkiy.ViewModels.Devices.IoT.Windows;
using sparkiy.Views;
using Serilog;

namespace sparkiy
{
	/// <summary>
	/// Provides application-specific behavior to supplement the default Application class.
	/// </summary>
	// ReSharper disable once RedundantExtendsListEntry
	// ReSharper disable once ArrangeTypeModifiers
	sealed partial class App : Application
	{
		private ILogger logger;


		/// <summary>
		/// Initializes the singleton application object.  This is the first line of authored code
		/// executed, and as such is the logical equivalent of main() or WinMain().
		/// </summary>
		public App()
		{
			this.InitializeLogging();
			this.InitializeContainer();
			this.InitializeComponent();
			this.Suspending += OnSuspending;
		}

		/// <summary>
		/// Initializes the container.
		/// </summary>
		private void InitializeContainer()
		{
			// Initialize the container
			Container.Initialize();

			// Register logger
			Container.Instance.RegisterInstance(this.logger);

			// Register services
			Container.Instance.RegisterType<ITitleBarService, TitleBarService>();
			Container.Instance.RegisterType<ITumblrNewsService, TumblrNewsService>();
			Container.Instance.RegisterType<IDiscourseService, DiscourseService>();
			
			// Register view models
			Container.Instance.RegisterType<IHomeViewModel, HomeViewModel>();
			Container.Instance.RegisterType<IDeviceSetupViewModel, DeviceSetupViewModel>();
		}

		/// <summary>
		/// Initializes the logging.
		/// </summary>
		private void InitializeLogging()
		{
			// Configure logger
			var loggerConfiguration = new LoggerConfiguration()
#if DEBUG
				.WriteTo.TextWriter(new DebugTextWriter())
#endif
				;
			var loggerInstance = loggerConfiguration.CreateLogger();

			// Assign logger 
			this.logger = loggerInstance;
		}

		/// <summary>
		/// Initializes the tracking.
		/// </summary>
		private void InitializeTracking()
		{
			Task.Run(() =>
			{
				try
				{
					RaygunClient.Attach("Hfj6rtvPJb46JAtPAeC+hA==");
					Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync();
				}
				catch (Exception ex)
				{
					// Ignore, these are not needed for application to work properly.
					this.logger.Warning(ex, "Failed to initialize Raygun or Application Insights.");
#if DEBUG
					throw;
#endif
				}
			});
		}

		/// <summary>
		/// Invoked when the application is launched normally by the end user.  Other entry points
		/// will be used such as when the application is launched to open a specific file.
		/// </summary>
		/// <param name="e">Details about the launch request and process.</param>
		protected override void OnLaunched(LaunchActivatedEventArgs e)
		{
			// Initialize dispatcher
			DispatcherHelper.Initialize();

#if DEBUG
			if (System.Diagnostics.Debugger.IsAttached)
			{
				//this.DebugSettings.EnableFrameRateCounter = true;
			}
#endif

			Frame rootFrame = Window.Current.Content as Frame;

			// Do not repeat app initialization when the Window already has content,
			// just ensure that the window is active
			if (rootFrame == null)
			{
				// Create a Frame to act as the navigation context and navigate to the first page
				rootFrame = new Frame();

				rootFrame.NavigationFailed += OnNavigationFailed;

				if (e.PreviousExecutionState == ApplicationExecutionState.Terminated)
				{
					//TODO: Load state from previously suspended application
				}

				// Place the frame in the current Window
				Window.Current.Content = rootFrame;
			}

			if (rootFrame.Content == null)
			{
				// When the navigation stack isn't restored navigate to the first page,
				// configuring the new page by passing required information as a navigation
				// parameter
				rootFrame.Navigate(typeof(HomePage), e.Arguments);
			}
			// Ensure the current window is active
			Window.Current.Activate();
		}

		/// <summary>
		/// Handles the <see cref="E:Activated" /> event.
		/// </summary>
		/// <param name="args">The <see cref="IActivatedEventArgs"/> instance containing the event data.</param>
		protected override void OnActivated(IActivatedEventArgs args)
		{
			// Start tracking
			this.InitializeTracking();

			base.OnActivated(args);
		}

		/// <summary>
		/// Invoked when Navigation to a certain page fails
		/// </summary>
		/// <param name="sender">The Frame which failed navigation</param>
		/// <param name="e">Details about the navigation failure</param>
		// ReSharper disable once MemberCanBeMadeStatic.Local
		private void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
		}

		/// <summary>
		/// Invoked when application execution is being suspended.  Application state is saved
		/// without knowing whether the application will be terminated or resumed with the contents
		/// of memory still intact.
		/// </summary>
		/// <param name="sender">The source of the suspend request.</param>
		/// <param name="e">Details about the suspend request.</param>
		// ReSharper disable once MemberCanBeMadeStatic.Local
		private void OnSuspending(object sender, SuspendingEventArgs e)
		{
			var deferral = e.SuspendingOperation.GetDeferral();
			deferral.Complete();
		}
	}
}
