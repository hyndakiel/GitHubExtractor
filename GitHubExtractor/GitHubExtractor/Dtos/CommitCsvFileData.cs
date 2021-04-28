using GitHubExtractor.Models;
using System;

namespace GitHubExtractor.Dtos
{
	public class CommitCsvFileData
	{

		public string AuthorLogin { get; set; }
		public string CommiterLogin { get; set; }
		public int PrNumber { get; set; }
		public string SHA { get; set; }
		public string CommitMessage { get; set; }
		public string FileName { get; set; }
		public string PatchText { get; set; }
		public string Additions { get; set; }
		public string Deletions { get; set; }
		public string StatusChanges { get; set; }

		public void PrepareChangesInfo(Commit commit)
		{
			throw new NotImplementedException();
		}
	}
}
