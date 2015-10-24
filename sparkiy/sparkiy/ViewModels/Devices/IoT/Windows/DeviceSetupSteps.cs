namespace sparkiy.ViewModels.Devices.IoT.Windows
{
	/// <summary>
	/// Device setup steps.
	/// </summary>
	public enum DeviceSetupSteps
	{
		/// <summary>
		/// The start step/
		/// </summary>
		Start,

		/// <summary>
		/// The finding device step.
		/// </summary>
		FindingDevice,

		/// <summary>
		/// The manual connection step.
		/// </summary>
		ManualConnection,

		/// <summary>
		/// The confirm device step.
		/// </summary>
		ConfirmDevice,

		/// <summary>
		/// The customize device.
		/// </summary>
		CustomizeDevice,

		/// <summary>
		/// The installation step.
		/// </summary>
		Installation,

		/// <summary>
		/// The setup succeded step.
		/// </summary>
		Success
	}
}