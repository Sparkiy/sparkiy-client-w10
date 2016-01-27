using System;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using sparkiy.Connectors.IoT.Windows;
using sparkiy.Connectors.IoT.Windows.Models;

namespace sparkiy.Services.Devices
{
	public interface IDeviceSetupService
	{
		Task<bool> Connect(Connection connection, Credentials credentials);

		Task<bool> TryAutoConnect();

		Task<DeviceInfo> GetDeviceInfo();

		Task<bool> SetDeviceName(string deviceName);
	}

	public class DeviceSetupService : IDeviceSetupService
	{
		const string DefaultComputerName = "minwinpc";
		const string DefaultApiPort = "8080";
		const string DefaultUserName = "Administrator";
		const string DefaultPassword = "p@ssw0rd";

		public Credentials EstablishedConnectionCredentials { get; set; }

		public Connection EstablishedConnection { get; set; }


		public async Task<bool> Connect(Connection connection, Credentials credentials)
		{
			const int numberOfRetries = 3;

			// Instantiate client
			var client = this.GetDeviceApi(connection, credentials);

			// Try to connect to device 
			for (var retryCount = 0; retryCount < numberOfRetries; retryCount++)
			{
				try
				{
					// Try to get response from device
					var response = await client.GetMachineNameAsync();

					// Check if response is valid return if it is equal
					var isValidName = !string.IsNullOrWhiteSpace(response.ComputerName);
					if (!isValidName) throw new InvalidOperationException("Invalid device name received.");

					// Confirm connection
					this.SetEstablishedConnection(connection, credentials);
					return true;
				}
				catch (Exception)
				{
					// TODO Log: failed to connect to the device
				}
			}

			// If we reached this, we failed to connect to the device or verify its name
			return false;
		}


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

			// TODO Implement in-network auto-discovery. 
			// This should scan all local ip addresses one-by one and check 
			// if we can retrieve device name from that address. Port is default, 
			// user name and password is default. We take first device that responds
			// to the request and stop the discovery.

			return false;
		}

		private async Task<bool> AutoConnectHostName()
		{
			// Instantiate client and connect
			var credentials = new Credentials(DefaultUserName, DefaultPassword);
			var connection = new Connection(DefaultComputerName, DefaultApiPort);
			return await this.Connect(connection, credentials);
		}

		public async Task<DeviceInfo> GetDeviceInfo()
		{
			// Check connection
			this.CheckEstablishedConnection();

			// Retrieve device information
			return await this.GetDeviceInfo(this.EstablishedConnection, this.EstablishedConnectionCredentials);
		}

		private async Task<DeviceInfo> GetDeviceInfo(Connection connection, Credentials credentials)
		{
			if (connection == null) throw new ArgumentNullException(nameof(connection));
			if (credentials == null) throw new ArgumentNullException(nameof(credentials));

			// Instantiate client
			var client = this.GetDeviceApi(connection, credentials);

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

		public Task<bool> SetDeviceName(string deviceName)
		{
			throw new NotImplementedException();
		}

		private async Task<bool> SetDeviceName(Connection connection, Credentials credentials, string deviceName)
		{
			if (connection == null) throw new ArgumentNullException(nameof(connection));
			if (credentials == null) throw new ArgumentNullException(nameof(credentials));

			// Instantiate client
			var client = this.GetDeviceApi(connection, credentials);

			try
			{
				throw new NotImplementedException();

				return true;
			}
			catch (Exception)
			{
				// TODO Log: failed to set device name

				return false;
			}
		}

		private void SetEstablishedConnection(Connection connection, Credentials credentials)
		{
			if (connection == null) throw new ArgumentNullException(nameof(connection));
			if (credentials == null) throw new ArgumentNullException(nameof(credentials));

			// Assign arguments
			this.EstablishedConnection = connection;
			this.EstablishedConnectionCredentials = credentials;
		}

		private DeviceApi GetDeviceApi(Connection connection, Credentials credentials, bool saveConnection = false)
		{
			if (connection == null) throw new ArgumentNullException(nameof(connection));
			if (credentials == null) throw new ArgumentNullException(nameof(credentials));

			// Set connection data if requested
			if (saveConnection)
				this.SetEstablishedConnection(connection, credentials);

			// Instantiate new device api
			return new DeviceApi(connection, credentials);
		}

		private DeviceApi GetDeviceApi()
		{
			// Check connection
			this.CheckEstablishedConnection();

			// Retrieve device api from already established connection
			return this.GetDeviceApi(this.EstablishedConnection, this.EstablishedConnectionCredentials);
		}

		private void CheckEstablishedConnection()
		{
			// Make sure connection was established already
			if (this.EstablishedConnection == null || this.EstablishedConnectionCredentials == null)
				throw new InvalidOperationException("Establish connection with device before calling this method. You can do that by first connecting or auto-connecting to the device.");
		}
	}

	public class DeviceInfo
	{
		public SoftwareInfo SoftwareInfo { get; set; }

		public IpConfig NetworkInfo { get; set; }
	}
}
