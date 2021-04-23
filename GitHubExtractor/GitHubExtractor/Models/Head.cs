using Newtonsoft.Json;

namespace GitHubExtractor.Models
{
	public class Head
	{
		[JsonProperty("sha")]
		public string Sha { get; set; }
	}
}
