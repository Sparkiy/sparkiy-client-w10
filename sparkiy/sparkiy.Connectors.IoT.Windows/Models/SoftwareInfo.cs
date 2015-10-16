namespace sparkiy.Connectors.IoT.Windows.Models
{
	public class SoftwareInfo : MachineName
	{
		public string Language { get; set; }

		public string MacAddress { get; set; }

		public string OsEdition { get; set; }

		public string OsVersion { get; set; }

		public string Platform { get; set; }

		public string SqmMachineId { get; set; }
	}
}