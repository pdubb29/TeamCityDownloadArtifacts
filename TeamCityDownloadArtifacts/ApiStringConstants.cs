using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownloadArtifacts
{
	public static class ApiStringConstants
	{
		public const string Projects = "/projects/";
		public const string RepositoryDownloadAll = "repository/downloadAll/";
		public const string Slash = "/";
		public const string BackSlash = "\\";
		public const string Build = "build";
		public const string ZipExtension = ".zip";
		public const string BuildTypesId = "/buildTypes/id:";
		public const string BuildsStatusSuccess = "/builds?status=SUCCESS";
		public const string IdArtifactsZip = ":id/artifacts.zip";
	}
}
