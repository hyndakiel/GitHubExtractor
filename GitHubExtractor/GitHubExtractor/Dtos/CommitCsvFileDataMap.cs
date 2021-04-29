using CsvHelper.Configuration;
using System.Globalization;

namespace GitHubExtractor.Dtos
{
	public class CommitCsvFileDataMap : ClassMap<CommitCsvFileData>
	{
		public CommitCsvFileDataMap()
		{
			AutoMap(CultureInfo.InvariantCulture);
			Map(m => m.AuthorLogin).Name("Author login");
			Map(m => m.CommiterLogin).Name("Committer login");
			Map(m => m.PrNumber).Name("PR number");
			Map(m => m.SHA).Name("SHA");
			Map(m => m.CommitMessage).Name("Commit Message");
			Map(m => m.FileName).Name("file name");
			Map(m => m.PatchText).Name("Patch text");
			Map(m => m.Additions).Name("Additions");
			Map(m => m.Deletions).Name("Deletions");
			Map(m => m.StatusChanges).Name("status");
			Map(m => m.Changes).Name("changes");
		}
	}
}
