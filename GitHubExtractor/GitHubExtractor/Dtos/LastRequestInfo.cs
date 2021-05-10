using System.Net;

namespace GitHubExtractor.Dtos
{
	public class LastRequestInfo
	{
		public WebHeaderCollection Headers { get; set; }
		public string Response { get; set; }
	}
}
