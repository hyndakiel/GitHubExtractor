using Newtonsoft.Json;

namespace GitHubExtractor.Models
{
	public class Creator
	{
		[JsonProperty("login")]
		public string Login { get; set; }
	}
}
