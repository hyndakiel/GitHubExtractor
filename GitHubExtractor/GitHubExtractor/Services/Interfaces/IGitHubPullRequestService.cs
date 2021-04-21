using GitHubExtractor.Models;
using System.Collections.Generic;

namespace GitHubExtractor.Services.Interfaces
{
	public interface IGitHubPullRequestService
	{
		public IEnumerable<PullRequestResponse> List();
	}
}
