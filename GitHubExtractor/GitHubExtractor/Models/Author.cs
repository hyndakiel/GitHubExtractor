using Newtonsoft.Json;
using System;

namespace GitHubExtractor.Models
{
	public class Author
	{
		[JsonProperty("name")]
		public string Name { get; set; }

		[JsonProperty("date")]
		public DateTime Date { get; set; }
	}
}
