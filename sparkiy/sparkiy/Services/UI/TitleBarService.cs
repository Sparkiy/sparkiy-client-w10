using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using sparkiy.Extensions;

namespace sparkiy.Services.UI
{
	/// <summary>
	/// Title bar service contract.
	/// </summary>
	public interface ITitleBarService
	{
		/// <summary>
		/// Sets the accent color of title bar.
		/// </summary>
		void SetAccentColor();
	}

	/// <summary>
	/// Title bar service.
	/// </summary>
	public sealed class TitleBarService : ITitleBarService
	{
		private const string TitleBarAccentColorKey = "TitleBarAccentColor";
		private const string TitleBarBackgroundInactiveColorKey = "TitleBarBackgroundInactiveColor";
		private const string TitleBarForegroundColorKey = "TitleBarForegroundColor";
		private const string TitleBarForegroundInactiveColorKey = "TitleBarForegroundInactiveColor";
		private const string TitleBarButtonBackgroundColorKey = "TitleBarButtonBackgroundColor";
		private const string TitleBarButtonBackgroundHoverColorKey = "TitleBarButtonBackgroundHoverColor";
		private const string TitleBarButtonBackgroundPressedColorKey = "TitleBarButtonBackgroundPressedColor";
		private const string TitleBarButtonBackgroundInactiveColorKey = "TitleBarButtonBackgroundInactiveColor";
		private const string TitleBarButtonForegroundColorKey = "TitleBarButtonForegroundColor";
		private const string TitleBarButtonForegroundHoverColorKey = "TitleBarButtonForegroundHoverColor";
		private const string TitleBarButtonForegroundPressedColorKey = "TitleBarButtonForegroundPressedColor";
		private const string TitleBarButtonForegroundInactiveColorKey = "TitleBarButtonForegroundInactiveColor";

		/// <summary>
		/// Sets the accent color of title bar.
		/// </summary>
		public void SetAccentColor()
		{
			// Retrieve title bar
			var titleBar = ApplicationView.GetForCurrentView().TitleBar;

			// Colors
			Color titleBarAccentColor;
			Color titleBarBackgroundInactiveColor;
			Color titleBarForegroundColor;
			Color titleBarForegroundInactiveColor;
			Color titleBarButtonBackgroundColor;
			Color titleBarButtonBackgroundHoverColor;
			Color titleBarButtonBackgroundPressedColor;
			Color titleBarButtonBackgroundInactiveColor;
			Color titleBarButtonForegroundColor;
			Color titleBarButtonForegroundHoverColor;
			Color titleBarButtonForegroundPressedColor;
			Color titleBarButtonForegroundInactiveColor;

			// Retrieve colors
			Application.Current.Resources.TryGetValue(TitleBarAccentColorKey, out titleBarAccentColor);
			Application.Current.Resources.TryGetValue(TitleBarBackgroundInactiveColorKey, out titleBarBackgroundInactiveColor);
			Application.Current.Resources.TryGetValue(TitleBarForegroundColorKey, out titleBarForegroundColor);
			Application.Current.Resources.TryGetValue(TitleBarForegroundInactiveColorKey, out titleBarForegroundInactiveColor);
			Application.Current.Resources.TryGetValue(TitleBarButtonBackgroundColorKey, out titleBarButtonBackgroundColor);
			Application.Current.Resources.TryGetValue(TitleBarButtonBackgroundHoverColorKey, out titleBarButtonBackgroundHoverColor);
			Application.Current.Resources.TryGetValue(TitleBarButtonBackgroundPressedColorKey, out titleBarButtonBackgroundPressedColor);
			Application.Current.Resources.TryGetValue(TitleBarButtonBackgroundInactiveColorKey, out titleBarButtonBackgroundInactiveColor);
			Application.Current.Resources.TryGetValue(TitleBarButtonForegroundColorKey, out titleBarButtonForegroundColor);
			Application.Current.Resources.TryGetValue(TitleBarButtonForegroundHoverColorKey, out titleBarButtonForegroundHoverColor);
			Application.Current.Resources.TryGetValue(TitleBarButtonForegroundPressedColorKey, out titleBarButtonForegroundPressedColor);
			Application.Current.Resources.TryGetValue(TitleBarButtonForegroundInactiveColorKey, out titleBarButtonForegroundInactiveColor);

			// Set values
			titleBar.BackgroundColor = titleBarAccentColor;
			titleBar.ForegroundColor = titleBarForegroundColor;
			titleBar.InactiveForegroundColor = titleBarForegroundInactiveColor;
			titleBar.InactiveBackgroundColor = titleBarBackgroundInactiveColor;
			titleBar.ButtonBackgroundColor = titleBarButtonBackgroundColor;
			titleBar.ButtonHoverBackgroundColor = titleBarButtonBackgroundHoverColor;
			titleBar.ButtonPressedBackgroundColor = titleBarButtonBackgroundPressedColor;
			titleBar.ButtonInactiveBackgroundColor = titleBarButtonBackgroundInactiveColor;
			titleBar.ButtonForegroundColor = titleBarButtonForegroundColor;
			titleBar.ButtonHoverForegroundColor = titleBarButtonForegroundHoverColor;
			titleBar.ButtonPressedForegroundColor = titleBarButtonForegroundPressedColor;
			titleBar.ButtonInactiveForegroundColor = titleBarButtonForegroundInactiveColor;
		}
	}
}
