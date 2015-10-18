using System.Collections.Generic;
using Windows.UI.Xaml.Controls;
using sparkiy.ViewModels.Devices.IoT.Windows;
using sparkiy.ViewModels.Utilities;

namespace sparkiy.Views.Devices.IoT.Windows
{
	/// <summary>
	/// Windows device setup view.
	/// </summary>
	public sealed partial class DeviceSetupView
	{
		private readonly Dictionary<DeviceSetupSteps, PivotItem> steps = 
			new Dictionary<DeviceSetupSteps, PivotItem>();


		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceSetupView"/> class.
		/// </summary>
		public DeviceSetupView()
		{
			// Initialize view and set data context
			this.InitializeViewModel();
			this.InitializeComponent();
			this.InitializeStepsCollection();
			this.DataContext = this;
		}

		private void InitializeViewModel()
		{
			// Retrieve view model
			this.ViewModel = ViewModelResolver.WindowsDeviceSetup;

			// Handle next step on progress changed
			this.ViewModel.OnProgress += (sender, e) => this.MoveToStep(e.NextStep);

			// Trigger view model loaded on page loaded
			this.Loaded += async (sender, args) => await this.ViewModel.LoadedAsync();
		}

		/// <summary>
		/// Initializes steps collection with mapping between 
		/// <see cref="DeviceSetupSteps"/> and <see cref="PivotItem"/> of cooresponding step.
		/// </summary>
		private void InitializeStepsCollection()
		{
			this.steps.Add(DeviceSetupSteps.Start, this.StartStep);
			this.steps.Add(DeviceSetupSteps.FindingDevice, this.FindingDeviceStep);
			this.steps.Add(DeviceSetupSteps.ConfirmDevice, this.FoundDeviceStep);
			this.steps.Add(DeviceSetupSteps.CustomizeDevice, this.CustomizeStep);
			this.steps.Add(DeviceSetupSteps.Installation, this.InstallStep);
			this.steps.Add(DeviceSetupSteps.Success, this.DoneStep);
		}

		/// <summary>
		/// Moves to the given step of the guide.
		/// </summary>
		/// <param name="step">The destination step.</param>
		private void MoveToStep(DeviceSetupSteps step)
		{
			this.GuideStepsContainer.SelectedIndex = this.GuideStepsContainer.Items?.IndexOf(this.steps[step]) ?? 0;
		}

		/// <summary>
		/// Gets the view model.
		/// </summary>
		/// <value>
		/// The view model.
		/// </value>
		public IDeviceSetupViewModel ViewModel { get; private set; }
	}
}
