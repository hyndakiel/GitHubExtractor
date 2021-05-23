using GitHubExtractor.Dtos.Interfaces;

namespace GitHubExtractor.Dtos
{
	public class IssueCommentsRequestParamns : IPaginatedParamns
	{
		public int PerPage { get; set; }
		public int Page { get; set; }
	}
}
