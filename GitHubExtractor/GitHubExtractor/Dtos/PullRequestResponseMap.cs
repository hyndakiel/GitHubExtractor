using CsvHelper.Configuration;
using System.Collections.Generic;
using System.Globalization;

namespace GitHubExtractor.Dtos
{
	public class PullRequestCsvFileDataMap : ClassMap<PullRequestCsvFileData>
	{
		public readonly IEnumerable<string> HEADERS = new List<string>()
		{
			"PR_Number", "Issue_Closed_Date", "Issue_Author", "Issue_Title", "Issue_Body", "Issue_Comments", "PR_Closed_Date", "PR_Author", "PR_Title", "PR_Body", "PR_Comments", "Commit_Author", "Commit_Date", "Commit_Message", "isPR",
		};

		public PullRequestCsvFileDataMap()
		{
			AutoMap(CultureInfo.InvariantCulture);
			Map(m => m.PrNumber).Name("PR_Number");
			Map(m => m.IssueClosedDate).Name("Issue_Closed_Date");
			Map(m => m.IssueAuthor).Name("Issue_Author");
			Map(m => m.IssueTitle).Name("Issue_Title");
			Map(m => m.IssueBody).Name("Issue_Body");
			Map(m => m.IssueComments).Name("Issue_Comments");
			Map(m => m.PrCloseData).Name("PR_Closed_Date");
			Map(m => m.PrAuthor).Name("PR_Author");
			Map(m => m.PrTitle).Name("PR_Title");
			Map(m => m.PrBody).Name("PR_Body");
			Map(m => m.PrComments).Name("PR_Comments");
			Map(m => m.CommitAuthor).Name("Commit_Author");
			Map(m => m.CommitDate).Name("Commit_Date");
			Map(m => m.CommitMessage).Name("Commit_Message");
			//Map(m => m.).Name("isPR");
		}
	}
}
