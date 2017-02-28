using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DownloadArtifacts
{
	public class ConfigFileReader
	{
		private readonly string FileLocation;

		public ConfigFileReader(string fileLocation)
		{
			FileLocation = fileLocation;
		}

		public ConfigFileData ReadConfigFile()
		{
			ConfigFileData data = new ConfigFileData();
			if (File.Exists(FileLocation))
			{
				List<string> lines = new List<string>();
				string line; 
				StreamReader file = new StreamReader(FileLocation);
				while ((line = file.ReadLine()) != null)
				{
					lines.Add(line);
				}
				data.Url = lines.First();
				data.ProjectId = lines[1];
				data.BuildTypeId = data.Url.Substring(data.Url.IndexOf("=") + 1);
			}
			else
			{
				data = WriteConfigFile();
			}
			return data;
		}

		private ConfigFileData WriteConfigFile()
		{
			var data = new ConfigFileData();
			if (!Directory.Exists(StringConstants.DownloadArtifactsDirectory))
			{
				Directory.CreateDirectory(StringConstants.DownloadArtifactsDirectory);
			}
			Console.WriteLine(StringConstants.EnterTeamCityBuildUrl);
			var teamcityURL = Console.ReadLine();
			Console.WriteLine(StringConstants.EnterTeamCityProjectPrompt);
			var projectId = Console.ReadLine();
			var stringBuilder = new StringBuilder();
			stringBuilder.Append(teamcityURL);
			stringBuilder.Append(Environment.NewLine);
			stringBuilder.Append(projectId);
			data.Url = teamcityURL;
			data.ProjectId = projectId;
			data.BuildTypeId = teamcityURL.Substring(teamcityURL.LastIndexOf("=") + 1);
			
			File.WriteAllText(FileLocation, stringBuilder.ToString());
			return data;
		}
	}
}
