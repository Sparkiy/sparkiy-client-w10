using System.Diagnostics;

namespace sparkiy.Connectors.IoT.Windows.Models
{
	[DebuggerDisplay("AppXPackageInfo: {Name}")]
	public class AppXPackageInfo
	{
		public string Name { get; set; }
		public string PackageFamilyName { get; set; }
		public string PackageFullName { get; set; }
		public string PackageRelativeId { get; set; }
	}
}