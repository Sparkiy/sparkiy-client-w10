namespace sparkiy.Connectors.IoT.Windows.Models
{
	public class Dhcp
	{
		public int LeaseExpires { get; set; }
		public int LeaseObtained { get; set; }
		public Address Address { get; set; }
	}
}