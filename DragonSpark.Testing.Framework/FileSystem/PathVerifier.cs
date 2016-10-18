using System;
using System.Linq;

namespace DragonSpark.Testing.Framework.FileSystem
{
	/// <summary>
	/// Attribution: https://github.com/tathamoddie/System.IO.Abstractions
	/// </summary>
	public class PathVerifier
	{
		readonly private IFileSystemAccessor fileSystemAccessor;

		internal PathVerifier(IFileSystemAccessor fileSystemAccessor)
		{
			this.fileSystemAccessor = fileSystemAccessor;
		}

		public void IsLegalAbsoluteOrRelative(string path, string paramName)
		{
			if (path.Trim() == string.Empty)
			{
				throw new ArgumentException(Properties.Resources.THE_PATH_IS_NOT_OF_A_LEGAL_FORM, paramName);
			}

			if (ExtractFileName(path).IndexOfAny(fileSystemAccessor.Path.GetInvalidFileNameChars()) > -1)
			{
				throw new ArgumentException(Properties.Resources.ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION);
			}

			var filePath = ExtractFilePath(path);
			if (MockPath.HasIllegalCharacters(filePath, false))
			{
				throw new ArgumentException(Properties.Resources.ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION);
			}
		}

		private string ExtractFileName(string fullFileName)
		{
			return fullFileName.Split(fileSystemAccessor.Path.DirectorySeparatorChar).Last();
		}

		private string ExtractFilePath(string fullFileName)
		{
			var extractFilePath = fullFileName.Split(fileSystemAccessor.Path.DirectorySeparatorChar);
			return string.Join(fileSystemAccessor.Path.DirectorySeparatorChar.ToString(), extractFilePath.Take(extractFilePath.Length - 1));
		}
	}
}
