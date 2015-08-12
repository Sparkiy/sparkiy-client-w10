using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
	public interface IHomeViewModel : IPageViewModel
	{
	}

	/// <summary>
	/// Home view model.
	/// </summary>
	public sealed class HomeViewModel : IHomeViewModel
	{
		private readonly ITitleBarService titleBarService;


		public HomeViewModel(ITitleBarService titleBarService)
		{
			if (titleBarService == null) throw new ArgumentNullException(nameof(titleBarService));

			this.titleBarService = titleBarService;
		}


		/// <summary>
		/// Occurs on page loaded.
		/// </summary>
		public async Task LoadedAsync()
		{
			// Set title bar color
			this.titleBarService.SetAccentColor();
		}
	}
}
