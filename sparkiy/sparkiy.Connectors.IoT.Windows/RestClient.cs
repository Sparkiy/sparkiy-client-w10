using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace sparkiy.Connectors.IoT.Windows
{
	public class RestClient : IRestClient
	{
		private string baseAddress;
		private string userName;
		private string password;


		public RestClient(string baseAddress, string userName, string password)
		{
			this.baseAddress = baseAddress;
			this.userName = userName;
			this.password = password;
		}


		public async Task<string> GetAsync(string path)
		{
			using (var client = GetClient())
			{
				try
				{
					var result = await client.GetAsync(path);
					var content = await result.Content.ReadAsStringAsync();
					return content;
				}
				catch (Exception ex)
				{
					// TODO Log the error
					return null;
				}
			}
		}

		private HttpClient GetClient()
		{
			// Obtain client credentials 
			var handler = new HttpClientHandler {Credentials = new NetworkCredential(this.userName, this.password)};

			// Instantiate new instance of http client and assign credentials and base address to it
			return new HttpClient(handler)
			{
				BaseAddress = new Uri(this.baseAddress, UriKind.Absolute)
			};
		}
	}
}