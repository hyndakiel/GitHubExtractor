using GitHubExtractor.Models;
using System.Collections.Generic;

namespace GitHubExtractor.Services.Interfaces
{
	public interface IGitHubPullRequestService
	{
		public IList<PullRequestResponse> List();
		public IEnumerable<PullRequestComment> Comments(int pullRequestNumber);
	}
}
