using System;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using sparkiy.Connectors.IoT.Windows;
using sparkiy.Connectors.IoT.Windows.Models;

namespace sparkiy.Services.Devices
{
	public interface IDeviceSetupService
	{
		Task<bool> TryAutoConnect();

		Task<DeviceInfo> TryGetDeviceInfo();

		Task<DeviceInfo> TryGetDeviceInfo(Connection connection, Credentials credentials);
	}

	public class DeviceSetupService : IDeviceSetupService
	{
		const string DefaultComputerName = "minwinpc";
		const string DefaultApiPort = "8080";
		const string DefaultUserName = "Administrator";
		const string DefaultPassword = "p@ssw0rd";

		private Connection ConnectedWith { get; set; }


		public async Task<bool> TryAutoConnect()
		{
			bool hasConnected = false;

			// Try connecting with host name
			hasConnected = await this.AutoConnectHostName();
			if (hasConnected) return true;

			// Brute-force local network
			hasConnected = await this.AutoConnectInNetwork();

			return false;
		}

		private async Task<bool> AutoConnectInNetwork()
		{
			var lanIdentifiers = NetworkInformation.GetLanIdentifiers();

			// TODO Implement

			return false;
		}

		private async Task<bool> AutoConnectHostName()
		{
			const int numberOfRetries = 3;

			// Instantiate client
			var credentials = new Credentials(DefaultUserName, DefaultPassword);
			var connection = new Connection(DefaultComputerName, DefaultApiPort);
			var client = new DeviceApi(connection, credentials);

			// Try to connect to device 
			for (var retryCount = 0; retryCount < numberOfRetries; retryCount++)
			{
				try
				{
					// Try to get response from device
					var response = await client.GetMachineNameAsync();

					// Check if response is valid return if it is equal
					var didConnect = response.ComputerName.Equals(DefaultComputerName);
					if (!didConnect) continue;

					// Confirm connection
					this.ConnectedWith = connection;
					return true;
				}
				catch (Exception)
				{
					// TODO Log: failed to connect to the device
				}
			}

			return false;
		}

		public async Task<DeviceInfo> TryGetDeviceInfo()
		{
			var credentials = new Credentials(DefaultUserName, DefaultPassword);
			var connection = new Connection(DefaultComputerName, DefaultApiPort);
			return await this.TryGetDeviceInfo(connection, credentials);
		}

		public async Task<DeviceInfo> TryGetDeviceInfo(Connection connection, Credentials credentials)
		{
			// Instantiate client
			var client = new DeviceApi(connection, credentials);

			try
			{
				// Get software and network information
				var softwareInfo = await client.GetSoftwareInfo();
				var networkInfo = await client.GetIpConfig();

				// Instantiate device information object
				return new DeviceInfo()
				{
					SoftwareInfo = softwareInfo,
					NetworkInfo = networkInfo
				};
			}
			catch (Exception)
			{
				// TODO Log: failed to connect to the device
			}

			return null;
		}
	}

	public class DeviceInfo
	{
		public SoftwareInfo SoftwareInfo { get; set; }

		public IpConfig NetworkInfo { get; set; }
	}
}
