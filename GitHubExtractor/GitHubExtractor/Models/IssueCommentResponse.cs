using Newtonsoft.Json;

namespace GitHubExtractor.Models
{
	public class IssueCommentResponse
	{
		[JsonProperty("body")]
		public string Body { get; set; }
	}
}
