using System.Collections.Generic;

namespace sparkiy.Connectors.IoT.Windows.Models
{
	public class Adapter
	{
		public string Description { get; set; }
		public string HardwareAddress { get; set; }
		public int Index { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public Dhcp Dhcp { get; set; }
		public IList<Address> Gateways { get; set; }
		public IList<Address> IpAddresses { get; set; }
	}
}