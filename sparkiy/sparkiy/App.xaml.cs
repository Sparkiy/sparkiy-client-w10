using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using GalaSoft.MvvmLight.Threading;
using Microsoft.Practices.Unity;
using Mindscape.Raygun4Net;
using sparkiy.Connectors.Discourse;
using sparkiy.Connectors.Tumblr;
using sparkiy.DI;
using sparkiy.Services.UI;
using sparkiy.ViewModels;
using sparkiy.Views;
using Serilog;
using InjectionConstructor = Microsoft.Practices.Unity.InjectionConstructor;

namespace sparkiy
{
	public class DebugTextWriter : TextWriter
	{
		/// <summary>
		/// Writes a character to the text string or stream.
		/// </summary>
		/// <param name="value">The character to write to the text stream.</param>
		/// <exception cref="System.NotImplementedException"></exception>
		public override void Write(char value)
		{
			Debug.Write(value);
		}

		public override void Flush()
		{
		}

		public override async Task FlushAsync()
		{
		}

		public override void Write(char[] buffer)
		{
			Debug.Write(buffer);
        }

		public override void Write(bool value)
		{
			Debug.Write(value);
		}

		public override void Write(char[] buffer, int index, int count)
		{
			Debug.Write(buffer.Skip(index).Take(count));
		}

		public override void Write(string value)
		{
			Debug.Write(value);
		}

		public override void Write(object value)
		{
			Debug.Write(value);
		}

		public override void Write(string format, params object[] arg)
		{
			Debug.Write(string.Format(this.FormatProvider, format, arg));
		}

		public override void WriteLine()
		{
			Debug.WriteLine(string.Empty);
		}

		public override void WriteLine(string value)
		{
			Debug.WriteLine(value);
		}

		public override void WriteLine(object value)
		{
			Debug.WriteLine(value);
		}


		/// <summary>
		/// When overridden in a derived class, returns the character encoding in which the output is written.
		/// </summary>
		public override Encoding Encoding { get; }
	}

    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
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
			this.InitializeTracking();
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
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
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
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}
