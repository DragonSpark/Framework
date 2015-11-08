using System;
using System.Data.Entity;
using System.IO;
using System.Transactions;
using DragonSpark.Extensions;
using DragonSpark.Io;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DragonSpark.Testing.Framework
{
	public class EnsureStorageAttribute : TestMethodProcessorAttribute
	{
		public EnsureStorageAttribute()
		{
			Priority = Priority.Highest;
		}

		protected internal override void Process( TestMethodProcessingContext context )
		{
			var database = context.Locator.GetInstance<DbContext>().Database;
			database.Initialize( false );
		}
	}

	public class TransactionScopeAttribute : TestMethodProcessorAttribute, IDisposable
	{
		TransactionScope TransactionScope { get; set; }

		protected internal override void Process( TestMethodProcessingContext context )
		{
			TransactionScope = new TransactionScope();
		}

		public void Dispose() 
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		protected virtual void Dispose( bool disposing )
		{
			TransactionScope.NotNull( x => x.Dispose() );
		}
	}

	public class PrepareWorkspaceAttribute : TestMethodProcessorAttribute
	{
		readonly bool move;

		public PrepareWorkspaceAttribute( bool move = false )
		{
			this.move = move;
		}

		protected internal override void Process( TestMethodProcessingContext context )
		{
			context.Locator.TryGetInstance<IWorkspace>().NotNull( x =>
			{
				context.TestMethod.GetAttributes<DeploymentItemAttribute>().Apply( item =>
				{
					var fileName = Path.GetFileName( item.Path );
				    var combine = Path.Combine( item.OutputDirectory, fileName );
				    var fileInfo = new FileInfo( combine );
					if ( fileInfo.Exists )
					{
					    Process( x, fileInfo );
					}
					else
					{
						var directory = new DirectoryInfo( fileName );
						if ( directory.Exists )
						{
							var path = Path.Combine( x.Directory.FullName, directory.Name );
							if ( move )
							{
								directory.MoveTo( path );
							}
							else
							{
								directory.CopyTo( path );
							}
						}
					}
				} );
			} );
		}

		void Process( IWorkspace workspace, FileInfo fileInfo )
		{
			var path = Path.Combine( workspace.Directory.FullName, fileInfo.Directory.Name, fileInfo.Name );
			Path.GetDirectoryName( path ).NotNull( item => Directory.CreateDirectory( item ) );
			if ( move )
			{
				fileInfo.MoveTo( path );
			}
			else
			{
				fileInfo.CopyTo( path, true );
			}
		}
	}
}