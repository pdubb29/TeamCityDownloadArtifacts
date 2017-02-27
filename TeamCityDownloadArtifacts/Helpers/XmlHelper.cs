using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TeamCityDownloadArtifacts;

namespace DownloadDlls
{
	public class XmlHelper
	{
		public static List<BuildDoc> GetBuildDocs(XmlNodeList xmlBuilds)
		{
			var projects = new List<BuildDoc>();

			for (int i = 0; i < xmlBuilds.Count; i++)
			{
				var attributes = xmlBuilds.Item(i).Attributes;

				if (attributes != null)
				{
					var project = new BuildDoc()
					{
						Number = Convert.ToInt32(GetAttribute(attributes, "number")),
						Id = Convert.ToInt32(GetAttribute(attributes, "id")),
						Href = GetAttribute(attributes, "href"),
						BuildTypeId = GetAttribute(attributes, "buildTypeId"),
						WebUrl = GetAttribute(attributes, "webUrl"),
						BranchName = GetAttribute(attributes, "branchName"),
						DefaultBranch = GetAttribute(attributes, "defaultBranch"),
						State = GetAttribute(attributes, "state"),
						Status = GetAttribute(attributes, "status")
					};

					projects.Add(project);
				}
			}

			return projects;
		}

		public static string GetAttribute(XmlAttributeCollection xmlAttributes, string key)
		{
			return xmlAttributes[key] != null ? xmlAttributes[key].Value : "";
		}

		public static List<RestApiProjectDoc> GetProjectDocs(XmlNodeList xmlProjects)
		{
			var projects = new List<RestApiProjectDoc>();

			for (int i = 0; i < xmlProjects.Count; i++)
			{
				var attributes = xmlProjects.Item(i).Attributes;

				if (attributes != null)
				{
					var project = new RestApiProjectDoc()
					{
						Description = GetAttribute(attributes, "description"),
						Id = GetAttribute(attributes, "id"),
						Href = GetAttribute(attributes, "href"),
						Name = GetAttribute(attributes, "name"),
						WebUrl = GetAttribute(attributes, "webUrl")
					};

					projects.Add(project);
				}
			}

			return projects;
		}

		public static XmlDocument GetXml(string url)
		{
			var response = ResponseHelper.GetWebResponse(url);

			var stream = response.GetResponseStream();

			var streamReader = new StreamReader(stream);

			var stringLine = "";
			var xml = "";

			while (stringLine != null)
			{
				stringLine = streamReader.ReadLine();
				if (stringLine != null)
				{
					xml += stringLine;
				}
			}

			XmlDocument xmlDocument = new XmlDocument();
			xmlDocument.LoadXml(xml);

			return xmlDocument;

		}
	}
}
