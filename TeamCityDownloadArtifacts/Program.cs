using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Mime;
using System.Xml;
using DownloadDlls;
using Newtonsoft.Json.Serialization;

namespace TeamCityDownloadArtifacts
{
	public class Program
	{
		private const string teamCityUrl = "https://teamcity.com/";
		private const string BaseUrl = "https://teamcity.com/httpAuth/app/rest";
		private static string _downloadPath;
		private static string _zipExtractionDirectory;
		private static bool _cleanDirectory = true;

		public static void Main(string[] args)
		{
			ConfigFileReader configFileReader = new ConfigFileReader();
			var data = configFileReader.ReadConfigFile();

			if (args.Any() && args[0].Equals("--help"))
			{
				//display help
				Console.Write(StringConstants.HelpResponse);
				Environment.Exit(0);
			}
			Setup(args);

			var builds = GetBuilds(data);

			var buildDocuments = XmlHelper.GetBuildDocs(builds);
			if (!buildDocuments.Any())
			{
				Console.WriteLine(StringConstants.NoBuildsFound + args[0]);
				Environment.Exit(1);
			}
			var orderedBuildDocuments = buildDocuments.OrderByDescending(x => x.Id);

			Console.WriteLine(StringConstants.Downloading);
			if (GetFileResponse(orderedBuildDocuments.First(), data) || _cleanDirectory)
			{
				//now we got to unzip the files.
				Console.WriteLine(StringConstants.Unzipping);
				ZipFile.ExtractToDirectory(_downloadPath, _zipExtractionDirectory);
			}
			Console.WriteLine(StringConstants.MissionAccomplished);
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
						FileHelper.WriteToFile(StringConstants.ExtractionDirectory, _zipExtractionDirectory, false);
						if (!Directory.Exists(_zipExtractionDirectory))
						{
							SetDefaultExtractionDirectory();
						}
					}
					else
					{
						string zipExtractionDirectory;
						if ((zipExtractionDirectory = FileHelper.GetFileContents(StringConstants.ExtractionDirectory, false)) != null && Directory.Exists(zipExtractionDirectory))
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
				Console.WriteLine(StringConstants.Cleaning);
				FileHelper.CleanPackages(_zipExtractionDirectory);
			}
		}

		private static void SetDefaultExtractionDirectory()
		{
			Console.WriteLine(StringConstants.DirectoryNotFound + StringConstants.DefaultExtractionDirectory);
			_zipExtractionDirectory = StringConstants.DefaultExtractionDirectory; //use a Default place where we can store these artifacts
		}

		private static XmlNodeList GetBuilds(ConfigFileData data)
		{
			var url = BaseUrl + "/projects/" + data.ProjectId + "/buildTypes/id:" + data.BuildTypeId + "/builds?status=SUCCESS";
			var xmlDocument = XmlHelper.GetXml(url);
			var builds = xmlDocument.GetElementsByTagName("build");
			return builds;
		}

		private static bool GetFileResponse(BuildDoc buildDoc, ConfigFileData data)
		{
			bool newCopy = false;
			var url = teamCityUrl + "repository/downloadAll/" + data.BuildTypeId + "/" + buildDoc.Id + ":id/artifacts.zip";

			var bytesProcessed = 0;

			var response = ResponseHelper.GetWebResponse(url);

			_downloadPath = StringConstants.DownloadArtifactsDirectory + "\\Build" + buildDoc.Id + ".zip";
			if (!File.Exists(_downloadPath))
			{
				FileHelper.DeleteOldZips();
				var stream = response.GetResponseStream();
				var localStream = File.Create(_downloadPath);

				byte[] buffer = new byte[1048576];
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
			
			Console.WriteLine(StringConstants.DownloadComplete);
			return newCopy;
		}
	}
}
