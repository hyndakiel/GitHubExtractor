using System;

namespace GitHubExtractor.Dtos
{
	public class PullRequestCsvFileData : AbstractCsvFileData
	{
		public string PrNumber { get; internal set; }
		public DateTime? IssueClosedDate { get; internal set; }
		public string IssueAuthor { get; internal set; }
		public string IssueTitle { get; internal set; }
		public string IssueBody { get; internal set; }
		public DateTime? PrCloseData { get; internal set; }
		public string PrTitle { get; internal set; }
		public string PrBody { get; internal set; }
		public string PrComments { get; internal set; }
		public string IssueComments { get; internal set; }
		public string PrAuthor { get; internal set; }
		public string CommitAuthor { get; internal set; }
		public DateTime? CommitDate { get; internal set; }
		public string CommitMessage { get; internal set; }
	}
}
