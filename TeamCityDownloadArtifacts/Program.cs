using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Xml;
using DownloadArtifacts.Helpers;
using static DownloadArtifacts.ApiStringConstants;
using static DownloadArtifacts.StringConstants;

namespace DownloadArtifacts
{
	public class Program
	{
		private const string teamCityUrl = "https://teamcity.com/";
		private const string BaseUrl = "https://teamcity.com/httpAuth/app/rest";
		private static string _downloadPath;
		private static string _zipExtractionDirectory;
		private static bool _cleanDirectory = true;
		private const int _131072MBs = 1048576;

		public static void Main(string[] args)
		{
			ConfigFileReader configFileReader = new ConfigFileReader();
			var data = configFileReader.ReadConfigFile();

			if (args.Any() && args[0].Equals("--help"))
			{
				//display help
				Console.Write(HelpResponse);
				Environment.Exit(0);
			}
			Setup(args);

			var builds = GetBuilds(data);

			var buildDocuments = XmlHelper.GetBuildDocs(builds);
			if (!buildDocuments.Any())
			{
				Console.WriteLine(NoBuildsFound + args[0]);
				Environment.Exit(1);
			}
			var orderedBuildDocuments = buildDocuments.OrderByDescending(x => x.Id);

			Console.WriteLine(Downloading);
			if (GetFileResponse(orderedBuildDocuments.First(), data) || _cleanDirectory)
			{
				//now we got to unzip the files.
				Console.WriteLine(Unzipping);
				ZipFile.ExtractToDirectory(_downloadPath, _zipExtractionDirectory);
			}
			Console.WriteLine(MissionAccomplished);
		}

		private static void Setup(string[] args)
		{
			FileHelper.CreateDirectoryIfNotExists();
			if (args.Any())
			{
				if (args[0].Contains("true"))
				{
					if (args.Length > 1)
					{
						_zipExtractionDirectory = args[1];
						FileHelper.WriteToFile(ExtractionDirectory, _zipExtractionDirectory, false);
						if (!Directory.Exists(_zipExtractionDirectory))
						{
							SetDefaultExtractionDirectory();
						}
					}
					else
					{
						string zipExtractionDirectory;
						if ((zipExtractionDirectory = FileHelper.GetFileContents(ExtractionDirectory, false)) != null && Directory.Exists(zipExtractionDirectory))
						{
							_zipExtractionDirectory = zipExtractionDirectory;
						}
						else
						{
							SetDefaultExtractionDirectory();
						}
					}
					if (args.Length > 2)
					{
						_cleanDirectory = args[2].Contains("false") ? false : true;
					}
				}
				
			}
			if (_cleanDirectory)
			{
				Console.WriteLine(Cleaning);
				FileHelper.CleanPackages(_zipExtractionDirectory);
			}
		}

		private static void SetDefaultExtractionDirectory()
		{
			Console.WriteLine(DirectoryNotFound + DefaultExtractionDirectory);
			_zipExtractionDirectory = DefaultExtractionDirectory; //use a Default place where we can store these artifacts
		}

		private static XmlNodeList GetBuilds(ConfigFileData data)
		{
			var url = BaseUrl + Projects + data.ProjectId + BuildTypesId + data.BuildTypeId + BuildsStatusSuccess;
			var xmlDocument = XmlHelper.GetXml(url);
			var builds = xmlDocument.GetElementsByTagName(Build);
			return builds;
		}

		private static bool GetFileResponse(BuildDoc buildDoc, ConfigFileData data)
		{
			bool newCopy = false;
			var url = teamCityUrl + RepositoryDownloadAll + data.BuildTypeId + Slash + buildDoc.Id + IdArtifactsZip;

			var bytesProcessed = 0;

			var response = ResponseHelper.GetWebResponse(url);

			_downloadPath = DownloadArtifactsDirectory + BackSlash + Build + buildDoc.Id + ZipExtension;
			if (!File.Exists(_downloadPath))
			{
				FileHelper.DeleteOldZips();
				var stream = response.GetResponseStream();
				var localStream = File.Create(_downloadPath);

				byte[] buffer = new byte[_131072MBs];
				var bytesRead = 0;

				do
				{
					bytesRead = stream.Read(buffer, 0, buffer.Length);

					localStream.Write(buffer, 0, bytesRead);

					bytesProcessed += bytesRead;
				}
				while (bytesRead > 0);
				localStream.Close();
				stream.Close();
				newCopy = true;
			}
			
			Console.WriteLine(DownloadComplete);
			return newCopy;
		}
	}
}
