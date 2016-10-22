using DragonSpark.Commands;
using DragonSpark.Extensions;
using DragonSpark.Sources;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Reflection;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public class InitializePartsAssemblyCommand : CommandBase<IEnumerable<Type>>
	{
		public static IScope<InitializePartsAssemblyCommand> Current { get; } = new Scope<InitializePartsAssemblyCommand>( Factory.GlobalCache( () => new InitializePartsAssemblyCommand() ) );
		InitializePartsAssemblyCommand() : this( FileSystemRepository.Current.Get(), Path.Current.Get(), ByteReader.Default.Get ) {}

		readonly IFileSystemRepository repository;
		readonly IPath path;
		readonly Func<string, ImmutableArray<byte>> reader;

		[UsedImplicitly]
		public InitializePartsAssemblyCommand( IFileSystemRepository repository, IPath path, Func<string, ImmutableArray<byte>> reader )
		{
			this.repository = repository;
			this.path = path;
			this.reader = reader;
		}

		public override void Execute( IEnumerable<Type> parameter )
		{
			var items = parameter.Assemblies().Introduce( path, tuple => new KeyValuePair<string, Assembly>( tuple.Item2.GetFileName( tuple.Item1.Location ), tuple.Item1 ) );
			foreach ( var item in items )
			{
				repository.Set( item.Key, new FileElement( reader( item.Key ) ) );
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