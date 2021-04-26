namespace GitHubExtractor.Services.Interfaces
{
	interface IGitHubService
	{
		public IGitHubPullRequestService GitHubPullRequestService { get; set; }
		void CreatePullRequestCSVFile();
		void CreateCommitsCSVFile();
	}
}
