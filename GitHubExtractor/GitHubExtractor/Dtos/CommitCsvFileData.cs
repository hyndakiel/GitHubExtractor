using GitHubExtractor.Models;
using System.Linq;
using System.Text;

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
		public int Additions { get; set; }
		public int Deletions { get; set; }
		public string StatusChanges { get; set; }

		public void PrepareChangesInfo(Commit commit)
		{
			CreateFilesField(commit);
			this.Additions = commit.Files.Sum(file => file.Additions);
			this.Deletions = commit.Files.Sum(file => file.Deletions);
		}

		private void CreateFilesField(Commit commit)
		{
			StringBuilder stringBuilderFileNames = new StringBuilder();
			StringBuilder stringBuilderPatchText = new StringBuilder();

			stringBuilderFileNames.Append("[");
			foreach (File file in commit.Files)
			{
				stringBuilderFileNames.Append(file.FilePath);
				stringBuilderFileNames.Append(", ");

				stringBuilderPatchText.Append(file.Patch);
				stringBuilderPatchText.Append(", ");
			}
			stringBuilderFileNames.Append("]");

			this.FileName = stringBuilderFileNames.ToString();
			this.PatchText = stringBuilderPatchText.ToString();
		}
	}
}
