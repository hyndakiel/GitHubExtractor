using Newtonsoft.Json;

namespace GitHubExtractor.Dtos.Interfaces
{
	public interface IPaginatedParamns
	{
		[JsonProperty("per_page")]
		int PerPage { get; set; }
		[JsonProperty("page")]
		int Page { get; set; }
	}
}
