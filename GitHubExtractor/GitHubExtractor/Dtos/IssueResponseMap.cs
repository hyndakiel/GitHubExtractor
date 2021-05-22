using CsvHelper.Configuration;
using System.Globalization;

namespace GitHubExtractor.Dtos
{
	public class IssueResponseMap : ClassMap<IssueFileCsvData>
	{
		public IssueResponseMap()
		{
			AutoMap(CultureInfo.InvariantCulture);
			Map(m => m.IssueNumber).Name("Issue Number");
			Map(m => m.IssueClosedDate).Name("Issue_Closed_Date");
			Map(m => m.IssueAuthor).Name("Issue_Author");
			Map(m => m.IssueTitle).Name("Issue_Title");
			Map(m => m.IssueBody).Name("Issue_Body");
			Map(m => m.IssueComments).Name("Issue Comments");
		}
	}
}
