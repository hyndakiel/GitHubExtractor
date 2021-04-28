using Newtonsoft.Json;
using System.Collections.Generic;

namespace GitHubExtractor.Models
{
	public class Commit
	{
		[JsonProperty("commit")]
		public CommitInfo CommitInfo { get; set; }

		[JsonProperty("files")]
		public IList<File> Files { get; set; }

	}
}
