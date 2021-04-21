using GitHubExtractor.Models;
using GitHubExtractor.Services.Interfaces;
using NLog;
using System.Collections.Generic;
using System.Linq;

namespace GitHubExtractor.Services
{
	public class GitHubService : IGitHubService
	{
		public IGitHubPullRequestService GitHubPullRequestService { get; set; }

		public IGitHubIssuesRequestService GitHubIssuesRequestService { get; set; }

		public static readonly Logger LOG = LogManager.GetCurrentClassLogger();

		public GitHubService(IGitHubPullRequestService gitHubPullRequestService, IGitHubIssuesRequestService gitHubissuesRequestService)//, IFileCreator fileCreator)
		{

			GitHubPullRequestService = gitHubPullRequestService;
			GitHubIssuesRequestService = gitHubissuesRequestService;
			//FileCreator = fileCreator;
		}

		public void CreatePullRequestCSVFile()
		{
			LOG.Info("INIT - GET PULL REQUESTS");
			IGitHubPullRequestService gitHubPullRequestService = GitHubPullRequestService;
			IEnumerable<PullRequestResponse> pullRequests = gitHubPullRequestService.List();
			LOG.Info("END - GET PULL REQUESTS - Result {0} pull requests", pullRequests.Count());

			LOG.Info("INIT - GET COMMENTS FROM PULL REQUESTS");
			foreach (PullRequestResponse pullRequestResponse in pullRequests)
			{
				IssueResponse issue = GitHubIssuesRequestService.Get(pullRequestResponse.IssueUrl);

				//Get commits too?
			}
			LOG.Info("END - GET COMMENTS FROM PULL REQUESTS - Result {0}");

			//Create CSV file
		}
	}
}
