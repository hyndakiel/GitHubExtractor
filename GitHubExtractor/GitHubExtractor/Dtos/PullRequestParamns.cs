using Newtonsoft.Json;

namespace GitHubExtractor.Dtos
{
	public class PullRequestParamns
	{
		public PullRequestParamns()
		{
		}

		public PullRequestParamns(string state, string sort)
		{
			State = state;
			Sort = sort;
		}

		[JsonProperty("state")]
		public string State { get; set; }
		[JsonProperty("sort")]
		public string Sort { get; set; }
	}
}
