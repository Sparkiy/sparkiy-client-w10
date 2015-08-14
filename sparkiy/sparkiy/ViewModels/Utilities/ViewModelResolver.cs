using Microsoft.Practices.Unity;
using sparkiy.DI;

namespace sparkiy.ViewModels.Utilities
{
	/// <summary>
	/// View model resolver.
	/// </summary>
	public static class ViewModelResolver
	{
		/// <summary>
		/// The home view model.
		/// </summary>
		public static IHomeViewModel Home => Container.Instance.Resolve<IHomeViewModel>();
	}
}
