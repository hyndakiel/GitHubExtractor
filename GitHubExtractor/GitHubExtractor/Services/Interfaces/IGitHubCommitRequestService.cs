using GitHubExtractor.Models;

namespace GitHubExtractor.Services.Interfaces
{
	public interface IGitHubCommitRequestService
	{
		public Commit GetCommits(string sha);
	}
}
