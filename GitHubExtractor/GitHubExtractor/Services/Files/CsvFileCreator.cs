using CsvHelper;
using CsvHelper.Configuration;
using NLog;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace GitHubExtractor.Services.Files
{
	public class CsvFileCreator : IFileCreator
	{
		public string FilePath { get; set; }
		public string FileName { get; set; }

		public static readonly Logger LOG = LogManager.GetCurrentClassLogger();

		private readonly string DELIMETER = "\a";

		public CsvFileCreator() { }

		public CsvFileCreator(string filePath, string fileName)
		{
			FilePath = filePath;
			FileName = fileName;
		}

		public void CreateFile<T, TClassMap>(List<T> data) where TClassMap : ClassMap
		{
			string filePath = FilePath;
			string fileName = FileName;

			Directory.CreateDirectory(filePath);

			string completePath = filePath + fileName;
			StreamWriter writer = new StreamWriter(completePath);

			CsvConfiguration configuration = new CsvConfiguration(new CultureInfo("en-us"));

			configuration.Delimiter = DELIMETER;
			configuration.HasHeaderRecord = true;

			CsvWriter csvWriter = new CsvWriter(writer, configuration);
			csvWriter.Context.RegisterClassMap<TClassMap>();
			SetupData(csvWriter, data);

			LOG.Info("INIT - SAVING CSV TO PATH: {0} WITH NAME {1}", filePath, fileName);
			writer.Flush();
			LOG.Info("END - SAVING CSV TO PATH: {0} WITH NAME {1}", filePath, fileName);
		}

		private void SetupData<T>(CsvWriter csvWriter, IEnumerable<T> data)
		{
			csvWriter.WriteRecords(data);
		}
	}
}
