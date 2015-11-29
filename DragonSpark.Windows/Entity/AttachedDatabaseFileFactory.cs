using System.Data.Entity;
using System.Data.Entity.Core.Common;
using System.Data.SqlClient;
using System.IO;
using DragonSpark.Activation.Build;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Entity
{
	public class AttachedDatabaseFileFactory : FactoryBase<FileInfo>
	{
		readonly DbContext context;

		public AttachedDatabaseFileFactory( DbContext context )
		{
			this.context = context;
		}

		protected override FileInfo CreateItem()
		{
			var result = new SqlConnectionStringBuilder( context.Database.Connection.ConnectionString ).AttachDBFilename.NullIfEmpty().Transform( DbProviderServices.ExpandDataDirectory ).Transform( s => new FileInfo( s ) );
			return result;
		}
	}
}