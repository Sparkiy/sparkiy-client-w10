using Microsoft.Practices.Unity;
using sparkiy.DI;
using sparkiy.ViewModels.Devices.IoT.Windows;

namespace sparkiy.ViewModels.Utilities
{
	/// <summary>
	/// View model resolver.
	/// </summary>
	public static class ViewModelResolver
	{
		/// <summary>
		/// Gets the home view model.
		/// </summary>
		/// <value>
		/// The home view model.
		/// </value>
		public static IHomeViewModel Home => Container.Instance.Resolve<IHomeViewModel>();

		/// <summary>
		/// Gets the windows device setup view model.
		/// </summary>
		/// <value>
		/// The windows device setup view model.
		/// </value>
		public static IDeviceSetupViewModel WindowsDeviceSetup => Container.Instance.Resolve<IDeviceSetupViewModel>();
	}
}
