using System.IO;

namespace DragonSpark.Io
{
	public class JetReaderOptions
	{
		readonly string filePath;
		readonly TextDelimiter delimiter;
		readonly CharacterSet characterSet;
		readonly bool firstRowContainsHeaderInfo;

		public JetReaderOptions( string filePath ) : this( filePath, TextDelimiter.CsvDelimited )
		{}

		public JetReaderOptions( string filePath, TextDelimiter delimiter ) : this( filePath, delimiter, CharacterSet.Ansi )
		{}

		public JetReaderOptions( string filePath, TextDelimiter delimiter, CharacterSet characterSet ) : this( filePath, delimiter, characterSet, true )
		{}

		public JetReaderOptions( string filePath, TextDelimiter delimiter, bool firstRowContainsHeaderInfo ) : this( filePath, delimiter, CharacterSet.Ansi, firstRowContainsHeaderInfo )
		{}

		public JetReaderOptions( string filePath, TextDelimiter delimiter, CharacterSet characterSet, bool firstRowContainsHeaderInfo )
		{
			this.filePath = filePath;
			this.delimiter = delimiter;
			this.characterSet = characterSet;
			this.firstRowContainsHeaderInfo = firstRowContainsHeaderInfo;
		}

		public bool FirstRowContainsHeaderInfo
		{
			get { return firstRowContainsHeaderInfo; }
		}

		public CharacterSet CharacterSet
		{
			get { return characterSet; }
		}

		public TextDelimiter Delimiter
		{
			get { return delimiter; }
		}

		public string FilePath
		{
			get { return filePath; }
		}

		public string Directory
		{
			get { return Path.GetDirectoryName( filePath ); }
		}
	
		public string FileName
		{
			get { return Path.GetFileName( filePath ); }
		}
	}
}