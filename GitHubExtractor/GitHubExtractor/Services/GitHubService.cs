using CsvHelper;
using CsvHelper.Configuration;
using GitHubExtractor.Configs;
using GitHubExtractor.Dtos;
using GitHubExtractor.Models;
using GitHubExtractor.Services.Interfaces;
using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace GitHubExtractor.Services
{
	public class GitHubService : IGitHubService
	{
		public IGitHubPullRequestService GitHubPullRequestService { get; set; }

		public IGitHubIssuesRequestService GitHubIssuesRequestService { get; set; }

		public static readonly Logger LOG = LogManager.GetCurrentClassLogger();

		private readonly string DELIMETER = "\a";

		private readonly IEnumerable<string> HEADERS = new List<string>()
		{
			"PR_Number", "Issue_Closed_Date", "Issue_Author", "Issue_Title", "Issue_Body", "Issue_Comments", "PR_Closed_Date", "PR_Author", "PR_Title", "PR_Body", "PR_Comments", "Commit_Author", "Commit_Date", "Commit_Message", "isPR",
		};
		private readonly string FILE_PATH_KEY = "PullRequestFilePathKey";

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

			List<PullRequestCsvFileData> data = new List<PullRequestCsvFileData>();
			LOG.Info("INIT - GET COMMENTS FROM PULL REQUESTS");
			foreach (PullRequestResponse pullRequestResponse in pullRequests)
			{
				IssueResponse issue = GitHubIssuesRequestService.Get(pullRequestResponse.IssueUrl);

				//Get Comments too?
				//Get commits too?

				TransformIntoCsvFormat(pullRequestResponse, issue, data);
			}
			LOG.Info("END - GET COMMENTS FROM PULL REQUESTS - Result {0}");

			LOG.Info("INIT - CREATING CSV");
			CreateCsvFile<PullRequestCsvFileData, PullRequestCsvFileDataMap>(data);
			LOG.Info("END - CREATING CSV");
		}

		private void TransformIntoCsvFormat(PullRequestResponse pullRequestResponse, IssueResponse issue, List<PullRequestCsvFileData> data)
		{
			PullRequestCsvFileData item = new PullRequestCsvFileData();
			item.PrNumber = pullRequestResponse.Id;
			item.IssueClosedDate = issue.ClosedAt;
			item.IssueAuthor = issue.Milestone?.Creator?.Login;
			item.IssueTitle = issue.Title;
			item.IssueBody = issue.Body;
			//item.IssueComments = CreateIssueCommentsField(comments);
			item.PrCloseData = pullRequestResponse.CloseDate;
			//item.PrAuthor = pullRequestResponse.
			item.PrTitle = pullRequestResponse.Title;
			item.PrBody = pullRequestResponse.Body;
			//item.PrComments = pullRequestResponse.
			//item.CommitAuthor = pullRequestResponse.
			//item.ComitDate = pullRequestResponse.
			//item.CommitMessage = pullRequestResponse.
			//item.IsPr = pullRequestResponse.

			data.Add(item);
		}

		private void CreateCsvFile<T, TClassMap>(IEnumerable<T> data) where TClassMap : ClassMap
		{
			string filePath = GetFilePath();

			Directory.CreateDirectory(filePath);

			string fileName = GetFileName();

			string completePath = filePath + fileName;
			StreamWriter writer = new StreamWriter(completePath);

			CsvConfiguration configuration = new CsvConfiguration(new CultureInfo("en-us"));

			configuration.Delimiter = DELIMETER;
			configuration.HasHeaderRecord = true;

			CsvWriter csvWriter = new CsvWriter(writer, configuration);
			csvWriter.Context.RegisterClassMap<TClassMap>();
			SetupHeader(csvWriter);
			SetupData(csvWriter, data);

			LOG.Info("INIT - SAVING CSV TO PATH: {0} WITH NAME", filePath, fileName);
			writer.Flush();
			LOG.Info("INIT - SAVING CSV TO PATH: {0} WITH NAME", filePath, fileName);
		}

		private string GetFileName()
		{
			string fileName = String.Format("{0:yyyy-MM-dd}-pull_request.csv", DateTime.Now);
			return fileName;
		}

		private string GetFilePath()
		{
			AppConfig appConfig = AppConfig.Instance;
			string filePath = appConfig.GetConfig(FILE_PATH_KEY);
			return filePath;
		}

		private void SetupHeader(CsvWriter csvWriter)
		{
			foreach (string header in HEADERS)
			{
				csvWriter.WriteField(header);
			}
			csvWriter.NextRecord();
		}

		private void SetupData<T>(CsvWriter csvWriter, IEnumerable<T> data)
		{
			csvWriter.WriteRecords(data);
		}
	}
}
