using System;
using sparkiy.Views.Devices.IoT.Windows;

namespace sparkiy.ViewModels.Devices.IoT.Windows
{
	/// <summary>
	/// Device setup progress event arguments.
	/// </summary>
	public class DeviceSetupProgressEventArgs : EventArgs
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceSetupProgressEventArgs"/> class.
		/// </summary>
		/// <param name="nextStep">The next step.</param>
		public DeviceSetupProgressEventArgs(DeviceSetupSteps nextStep) : this(null, nextStep)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="DeviceSetupProgressEventArgs"/> class.
		/// </summary>
		/// <param name="currentStep">The current step.</param>
		/// <param name="nextStep">The next step.</param>
		public DeviceSetupProgressEventArgs(DeviceSetupSteps? currentStep, DeviceSetupSteps nextStep)
		{
			this.CurrentStep = currentStep;
			this.NextStep = nextStep;
		}


		/// <summary>
		/// Gets or sets the current step.
		/// </summary>
		/// <value>
		/// The current step.
		/// </value>
		public DeviceSetupSteps? CurrentStep { get; set; }

		/// <summary>
		/// Gets or sets the next step.
		/// </summary>
		/// <value>
		/// The next step.
		/// </value>
		public DeviceSetupSteps NextStep { get; set; }
	}
}