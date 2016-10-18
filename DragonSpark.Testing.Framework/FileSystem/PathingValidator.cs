using System;
using System.IO;
using System.Linq;

namespace DragonSpark.Testing.Framework.FileSystem
{
	sealed class PathingValidator : IPathingValidator
	{
		readonly static char[] InvalidChars = Path.GetInvalidFileNameChars();
		public static PathingValidator Default { get; } = new PathingValidator();
		PathingValidator() {}

		public void IsLegalAbsoluteOrRelative(string pathToValidate, string paramName)
		{
			if (pathToValidate.Trim() == string.Empty)
			{
				throw new ArgumentException(Properties.Resources.THE_PATH_IS_NOT_OF_A_LEGAL_FORM, paramName);
			}

			if (ExtractFileName(pathToValidate).IndexOfAny(InvalidChars) > -1)
			{
				throw new ArgumentException(Properties.Resources.ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION);
			}

			var filePath = ExtractFilePath(pathToValidate);
			if (MockPath.HasIllegalCharacters(filePath, false))
			{
				throw new ArgumentException(Properties.Resources.ILLEGAL_CHARACTERS_IN_PATH_EXCEPTION);
			}
		}

		static string ExtractFileName(string fullFileName) => fullFileName.Split(Path.DirectorySeparatorChar).Last();

		static string ExtractFilePath(string fullFileName)
		{
			var extractFilePath = fullFileName.Split(Path.DirectorySeparatorChar);
			return string.Join(Path.DirectorySeparatorChar.ToString(), extractFilePath.Take(extractFilePath.Length - 1));
		}
	}
}