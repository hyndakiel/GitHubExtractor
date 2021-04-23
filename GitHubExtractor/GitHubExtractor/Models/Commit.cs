using Newtonsoft.Json;

namespace GitHubExtractor.Models
{
	public class Commit
	{
		[JsonProperty("commit")]
		public CommitInfo CommitInfo { get; internal set; }
	}
}
