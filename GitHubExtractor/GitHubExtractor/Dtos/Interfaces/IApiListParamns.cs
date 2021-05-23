using Newtonsoft.Json;

namespace GitHubExtractor.Dtos.Interfaces
{
	public interface IApiListParamns : IPaginatedParamns
	{
		[JsonProperty("state")]
		string State { get; set; }
		[JsonProperty("sort")]
		string Sort { get; set; }

	}
}
