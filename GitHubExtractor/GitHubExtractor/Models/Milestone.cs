using Newtonsoft.Json;

namespace GitHubExtractor.Models
{
	public class Milestone
	{
		[JsonProperty("creator")]
		public Creator Creator { get; set; }
	}
}
