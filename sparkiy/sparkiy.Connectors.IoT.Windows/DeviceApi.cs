using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using sparkiy.Connectors.IoT.Windows.Models;

namespace sparkiy.Connectors.IoT.Windows
{
	public class DeviceApi : IDeviceApi
	{
		private const string GetInstalledAppXPackagesApiPath = "/api/appx/packagemanager/packages";
		private const string GetIpConfigApiPath = "/api/networking/ipconfig";
		private const string GetComputerNameApiPath = "/api/os/machinename";
		private const string GetSoftwareInfoApiPath = "/api/os/info";

		private readonly RestClient client;


		public DeviceApi()
		{
			this.client = new RestClient("http://sparkiyhubaleks:8080", "Administrator", "lqYo9c6MvwvPjWD");
		}


		public async Task<MachineName> GetMachineNameAsync()
		{
			return await GetDeserializedAsync<MachineName>(GetComputerNameApiPath);
		}

		public async Task<SoftwareInfo> GetSoftwareInfo()
		{
			return await GetDeserializedAsync<SoftwareInfo>(GetSoftwareInfoApiPath);
		}

		public async Task<IpConfig> GetIpConfig()
		{
			return await GetDeserializedAsync<IpConfig>(GetIpConfigApiPath);
		}

		public async Task<AppXPackages> GetInstalledAppXPackages()
		{
			return await GetDeserializedAsync<AppXPackages>(GetInstalledAppXPackagesApiPath);
		}

		private async Task<T> GetDeserializedAsync<T>(string path)
		{
			// Retrieve JSON data
			var dataJson = await client.GetAsync(path);
			if (dataJson == null)
				return default(T);

			// Deserialize JSON data
			var data = TryDeserialize<T>(dataJson);

			return data;
		}

		private static T TryDeserialize<T>(string data)
		{
			try
			{
				return JsonConvert.DeserializeObject<T>(data);
			}
			catch (Exception)
			{
				return default(T);
			}
		}
	}
}