using GitHubExtractor.Dtos.Interfaces;

namespace GitHubExtractor.Dtos
{
	class IssuesRequestParamns : IApiListParamns
	{
		public IssuesRequestParamns() { }

		public IssuesRequestParamns(string state, string sort)
		{
			State = state;
			Sort = sort;
		}

		public string State { get; set; }
		public string Sort { get; set; }
		public int PerPage { get; set; }
		public int Page { get; set; }
	}
}
