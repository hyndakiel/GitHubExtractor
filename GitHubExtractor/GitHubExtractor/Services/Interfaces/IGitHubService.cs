using GitHubExtractor.Models;
using System.Collections.Generic;

namespace GitHubExtractor.Services.Interfaces
{
	interface IGitHubService
	{
		public IGitHubPullRequestService GitHubPullRequestService { get; set; }
		void CreatePullRequestCSVFile(IList<PullRequestResponse> pullRequests);
		void CreateCommitsCSVFile(IList<PullRequestResponse> pullRequests);
		void CreateIssuesCSVFile(IList<IssueResponse> issues);
		void CreateFiles();
	}
}
