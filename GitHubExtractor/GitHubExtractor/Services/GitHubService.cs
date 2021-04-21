using GitHubExtractor.Services.Interfaces;
using System.Collections.Generic;

namespace GitHubExtractor.Services
{
	public class GitHubService : IGitHubService
	{
		public IGitHubPullRequestService GitHubPullRequestService { get; set; }


		public GitHubService(IGitHubPullRequestService gitHubPullRequestService)//, IFileCreator fileCreator)
		{

			GitHubPullRequestService = gitHubPullRequestService;
			//FileCreator = fileCreator;
		}

		public void CreatePullRequestCSVFile()
		{
			IGitHubPullRequestService gitHubPullRequestService = GitHubPullRequestService;
			IEnumerable<object> pullRequests = gitHubPullRequestService.List();
		}
	}
}
