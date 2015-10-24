using System.Threading.Tasks;

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
}