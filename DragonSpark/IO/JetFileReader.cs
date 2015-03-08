using System;
using System.Data;
using System.Data.OleDb;
using System.IO;
using DragonSpark;
using DragonSpark.Runtime;

namespace DragonSpark.Io
{
	public class JetFileReader
	{
		readonly JetReaderOptions options;

		public JetFileReader( JetReaderOptions options )
		{
			this.options = options;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "The result is disposed elsewhere.")]
		public DataTable Create()
		{
			var schemaPath = Path.Combine( options.Directory, "Schema.ini" );
			var schema = string.Format( new NamedTokenFormatter(), Properties.Resources.SchemaTemplate, options );
			File.WriteAllText( schemaPath, schema );

			var connectionString =
				string.Format( @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=""text;HDR={1};FMT={2}""",
				               options.Directory, options.FirstRowContainsHeaderInfo ? "YES" : "NO", options.Delimiter );
			using ( var connection = new OleDbConnection( connectionString ) )
			{
				var command = connection.CreateCommand();
				command.CommandText = String.Format( "SELECT * FROM [{0}]", options.FileName );
				using ( var adapter = new OleDbDataAdapter( command ) )
				{
					var result = new DataTable( Path.GetFileNameWithoutExtension( options.FilePath ) );
					adapter.Fill( result );
					result.PrimaryKey = new[] { result.Columns[ 0 ] };
					return result;
				}
			}
		}
	}
}