using Newtonsoft.Json;

namespace GitHubExtractor.Dtos
{
	public class PullRequestParamns
	{
		public PullRequestParamns()
		{
		}

		public PullRequestParamns(string state, string @base, string sort)
		{
			State = state;
			Base = @base;
			Sort = sort;
		}

		[JsonProperty("state")]
		public string State { get; set; }
		[JsonProperty("base")]
		public string Base { get; set; }
		[JsonProperty("sort")]
		public string Sort { get; set; }
	}
}
