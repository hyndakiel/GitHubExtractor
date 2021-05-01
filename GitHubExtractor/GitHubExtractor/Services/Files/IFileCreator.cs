using CsvHelper.Configuration;
using System.Collections.Generic;

namespace GitHubExtractor.Services.Files
{
	public interface IFileCreator
	{
		string FilePath { get; set; }
		string FileName { get; set; }

		void CreateFile<T, TClassMap>(List<T> data) where TClassMap : ClassMap;
	}
}
