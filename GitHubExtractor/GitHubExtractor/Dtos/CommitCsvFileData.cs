namespace GitHubExtractor.Dtos
{
	public class CommitCsvFileData
	{

		public string AuthorLogin { get; set; }
		public string CommiterLogin { get; set; }
		public string PrNumber { get; set; }
		public string SHA { get; set; }
		public string CommitMessage { get; set; }
		public string FileName { get; set; }
		public string PatchText { get; set; }
		public string Additions { get; set; }
		public string Deletions { get; set; }
		public string StatusChanges { get; set; }
	}
}
