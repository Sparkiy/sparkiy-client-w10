using System.Threading.Tasks;
using sparkiy.Connectors.IoT.Windows.Models;


namespace sparkiy.Connectors.IoT.Windows
{
	public interface IDeviceApi
	{
		Task<MachineName> GetMachineNameAsync();

		Task<SoftwareInfo> GetSoftwareInfo();

		Task<IpConfig> GetIpConfig();

		Task<AppXPackages> GetInstalledAppXPackages();
	}
}
