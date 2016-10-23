using DragonSpark.Commands;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using FileInfo = System.IO.FileInfo;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public class InitializePartsAssemblyCommand : CommandBase<IEnumerable<Type>>
	{
		public static IScope<InitializePartsAssemblyCommand> Current { get; } = new Scope<InitializePartsAssemblyCommand>( Factory.GlobalCache( () => new InitializePartsAssemblyCommand() ) );
		InitializePartsAssemblyCommand() : this( ProvisionFromSystemFileCommand.Current.Get().Execute ) {}

		readonly Action<FileInfo> provision;

		[UsedImplicitly]
		public InitializePartsAssemblyCommand( Action<FileInfo> provision )
		{
			this.provision = provision;
		}

		public override void Execute( IEnumerable<Type> parameter )
		{
			var items = parameter
				.Assemblies()
				.Where( assembly => assembly.Location != null )
				.Select( assembly => new FileInfo( assembly.Location ) );
			foreach ( var item in items )
			{
				provision( item );
			}
		}
	}

	/*public sealed class AssemblyDictionaryFactory : ParameterizedSourceBase<IEnumerable<Type>, IDictionary<string, Assembly>>
	{
		public static ISource<AssemblyDictionaryFactory> Current { get; } = new Scope<AssemblyDictionaryFactory>( Sources.Factory.GlobalCache( () => new AssemblyDictionaryFactory() ) );
		AssemblyDictionaryFactory() : this( FileSystemRepository.Current.Get(), Path.Current.Get() ) {}

		readonly IFileSystemRepository repository;
		readonly IPath path;

		[UsedImplicitly]
		public AssemblyDictionaryFactory( IFileSystemRepository repository, IPath path )
		{
			this.repository = repository;
			this.path = path;
		}

		public override IDictionary<string, Assembly> Get( IEnumerable<Type> parameter )
		{
			var result = new Dictionary<string, Assembly>();
			var items = parameter.Assemblies().Introduce( path, tuple => new KeyValuePair<string, Assembly>( tuple.Item2.GetFileName( tuple.Item1.Location ), tuple.Item1 ) );
			foreach ( var item in items )
			{
				repository.Set( item.Key, new FileElement( System.IO.File.ReadAllBytes( item.Key ) ) );
				result.Add( item.Key, item.Value );
			}
			return result;
		}
	}*/
}