using Newtonsoft.Json;

namespace GitHubExtractor.Models
{
	public class CommitInfo
	{
		[JsonProperty("author")]
		public Author Author { get; set; }

		[JsonProperty("committer")]
		public Committer Committer { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }
	}
}
