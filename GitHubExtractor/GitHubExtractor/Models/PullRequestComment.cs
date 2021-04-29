using Newtonsoft.Json;

namespace GitHubExtractor.Models
{
	public class PullRequestComment
	{
		[JsonProperty("body")]
		public string Body { get; set; }
	}
}
