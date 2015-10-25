using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Threading;
using sparkiy.Services.Devices;

namespace sparkiy.ViewModels.Devices.IoT.Windows
{
	/// <summary>
	/// Windows device setup view model.
	/// </summary>
	public interface IDeviceSetupViewModel : IPageViewModel
	{
		/// <summary>
		/// Occurs when progress is made.
		/// </summary>
		event DeviceSetupProgressEventHandler OnProgress;

		/// <summary>
		/// Gets the progress forward command.
		/// </summary>
		/// <value>
		/// The progress forward command.
		/// </value>
		RelayCommand ProgressForwardCommand { get; }

		/// <summary>
		/// Gets the progress backward command.
		/// </summary>
		/// <value>
		/// The progress backward command.
		/// </value>
		RelayCommand ProgressBackwardCommand { get; }
	}

	/// <summary>
	/// Windows device setup view model.
	/// </summary>
	public sealed class DeviceSetupViewModel : IDeviceSetupViewModel
	{
		private DeviceSetupSteps? currentStep;

		private IDeviceSetupService deviceSetupService;
		private DeviceInfo deviceInfo;


		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceSetupViewModel"/> class.
		/// </summary>
		public DeviceSetupViewModel()
		{
			this.deviceSetupService = new DeviceSetupService();

			// Initialize commands
			this.ProgressForwardCommand = new RelayCommand(this.ProgressForward);
			this.ProgressBackwardCommand = new RelayCommand(this.ProgressBackward);

			// Attach to progress changed event
			this.OnProgress += OnProgressChanged;
		}


		/// <summary>
		/// Occurs on page loaded.
		/// </summary>
		public async Task LoadedAsync()
		{
			// Go to initial step
			this.ProgressForward();
		}

		private void OnProgressChanged(object sender, DeviceSetupProgressEventArgs args)
		{
			if (args.NextStep == DeviceSetupSteps.FindingDevice)
			{
				// Start connecting to the device
				Task.Run(() => this.StartAutoConnect());
				return;
			}
		}

		private async Task StartAutoConnect()
		{
			// Try to connect to the device automatically
			var didConnect = await this.deviceSetupService.TryAutoConnect();
			if (!didConnect)
			{
				// Go back if connection fails
				this.ProgressBackward();
				return;
			}

			// Retrieve device information and go forward if connection is made
			this.deviceInfo = await this.deviceSetupService.GetDeviceInfo();
			this.ProgressForward();
		}

		/// <summary>
		/// Raises the on progress event.
		/// </summary>
		/// <param name="nextStep">The next step.</param>
		/// <remarks>
		/// Current step atribute is set to value of current step variable of this instance.
		/// </remarks>
		private void RaiseOnProgressEvent(DeviceSetupSteps nextStep)
		{
			DispatcherHelper.CheckBeginInvokeOnUI(() =>
				this.OnProgress?.Invoke(this, new DeviceSetupProgressEventArgs(this.currentStep, nextStep)));
		}

		/// <summary>
		/// Progresses forward.
		/// </summary>
		public void ProgressForward()
		{
			DeviceSetupSteps nextStep;

			// Determine whether we should start from beginning or 
			// move forward with next step.
			if (!this.currentStep.HasValue)
			{
				nextStep = DeviceSetupSteps.Start;
			}
			else
			{
				// Determine next step
				// ReSharper disable once SwitchStatementMissingSomeCases
				switch (this.currentStep.Value)
				{
					case DeviceSetupSteps.Start:
						nextStep = DeviceSetupSteps.FindingDevice;
						break;
					case DeviceSetupSteps.FindingDevice:
						nextStep = DeviceSetupSteps.ConfirmDevice;
						break;
					case DeviceSetupSteps.ConfirmDevice:
						nextStep = DeviceSetupSteps.CustomizeDevice;
						break;
					case DeviceSetupSteps.CustomizeDevice:
						nextStep = DeviceSetupSteps.Installation;
						break;
					case DeviceSetupSteps.Installation:
						nextStep = DeviceSetupSteps.Success;
						break;
					default:
						nextStep = DeviceSetupSteps.Start;
						break;
				}
			}

			// Raise changes event and set new step as current
			this.RaiseOnProgressEvent(nextStep);
			this.currentStep = nextStep;
		}

		/// <summary>
		/// Progresses backward.
		/// </summary>
		public void ProgressBackward()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Occurs when progress is made.
		/// </summary>
		public event DeviceSetupProgressEventHandler OnProgress;

		/// <summary>
		/// Gets the progress forward command.
		/// </summary>
		/// <value>
		/// The progress forward command.
		/// </value>
		public RelayCommand ProgressForwardCommand { get; private set; }

		/// <summary>
		/// Gets the progress backward command.
		/// </summary>
		/// <value>
		/// The progress backward command.
		/// </value>
		public RelayCommand ProgressBackwardCommand { get; private set; }
	}
}
