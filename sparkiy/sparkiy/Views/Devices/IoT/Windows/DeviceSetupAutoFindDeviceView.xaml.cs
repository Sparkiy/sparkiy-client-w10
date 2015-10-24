using System;
using Windows.UI.Xaml;

namespace sparkiy.Views.Devices.IoT.Windows
{
	/// <summary>
	/// Windows device setup auto find device view.
	/// </summary>
	public sealed partial class DeviceSetupAutoFindDeviceView
	{
		private DispatcherTimer prepareGuideTimer;


		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceSetupAutoFindDeviceView"/> class.
		/// </summary>
		public DeviceSetupAutoFindDeviceView()
		{
			this.InitializeComponent();
			this.InitializeFlipView();
		}


		/// <summary>
		/// Initializes the flip view.
		/// </summary>
		private void InitializeFlipView()
		{
			// Prepare timer and set interval
			this.prepareGuideTimer = new DispatcherTimer()
			{
				Interval = TimeSpan.FromSeconds(3)
			};

			// Flip to next item on timer tick
			this.prepareGuideTimer.Tick += (sender, o) =>
			{
				// Retrieve items
				var guideItems = this.PrepareGuideFlipView.Items;
				if (guideItems == null) return;

				// Select next item or first item if selected item is last
				if (this.PrepareGuideFlipView.SelectedIndex >= (guideItems.Count - 1))
					this.PrepareGuideFlipView.SelectedIndex = 0;
				else this.PrepareGuideFlipView.SelectedIndex = this.PrepareGuideFlipView.SelectedIndex + 1;
			};

			// Start timer
			this.prepareGuideTimer.Start();
		}
	}
}
