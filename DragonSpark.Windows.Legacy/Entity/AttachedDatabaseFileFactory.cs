using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using System.Composition;
using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.SqlClient;
using System.IO;
using System.IO.Abstractions;

namespace DragonSpark.Windows.Legacy.Entity
{
	public class AttachedDatabaseFileFactory : ParameterizedSourceBase<DbContext, FileInfoBase>
	{
		[Export]
		public static AttachedDatabaseFileFactory Default { get; } = new AttachedDatabaseFileFactory();
		AttachedDatabaseFileFactory() {}

		public override FileInfoBase Get( DbContext parameter ) => 
			new SqlConnectionStringBuilder( parameter.Database.Connection.ConnectionString ).AttachDBFilename.NullIfEmpty().With( DbProviderServices.ExpandDataDirectory ).With( s => new FileInfo( s ) );
	}
}