using Newtonsoft.Json;

namespace GitHubExtractor.Dtos.Interfaces
{
	public interface IApiListParamns
	{
		[JsonProperty("state")]
		string State { get; set; }
		[JsonProperty("sort")]
		string Sort { get; set; }
		[JsonProperty("per_page")]
		int PerPage { get; set; }
		[JsonProperty("page")]
		int Page { get; set; }
	}
}
