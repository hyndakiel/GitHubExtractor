using Newtonsoft.Json;

namespace GitHubExtractor.Models
{
	public class CommitInfo
	{
		[JsonProperty("author")]
		public Author Author { get; internal set; }

		[JsonProperty("message")]
		public string Message { get; internal set; }
	}
}
