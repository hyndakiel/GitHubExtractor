using Newtonsoft.Json;

namespace GitHubExtractor.Models
{
	public class Committer
	{
		[JsonProperty("name")]
		public string Name { get; set; }
	}
}
