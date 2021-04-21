using GitHubExtractor.Models;

namespace GitHubExtractor.Services.Interfaces
{
	public interface IGitHubIssuesRequestService
	{
		public IssueResponse Get(string url);
	}
}
