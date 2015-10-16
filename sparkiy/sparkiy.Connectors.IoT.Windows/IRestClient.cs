using System.Threading.Tasks;

namespace sparkiy.Connectors.IoT.Windows
{
	public interface IRestClient
	{
		Task<string> GetAsync(string path);
	}
}