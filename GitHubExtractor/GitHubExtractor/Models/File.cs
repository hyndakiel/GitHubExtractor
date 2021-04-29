using Newtonsoft.Json;

namespace GitHubExtractor.Models
{
	public class File
	{
		[JsonProperty("filename")]
		public string FilePath { get; set; }
		[JsonProperty("additions")]
		public int Additions { get; set; }
		[JsonProperty("deletions")]
		public int Deletions { get; set; }
		[JsonProperty("changes")]
		public int Changes { get; set; }
		[JsonProperty("patch")]
		public string Patch { get; set; }
		[JsonProperty("status")]
		public string Status { get; set; }
	}
}
