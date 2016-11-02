using DragonSpark.Commands;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Specifications;
using JetBrains.Annotations;
using System;
using System.Reflection;
using FileInfo = System.IO.FileInfo;

namespace DragonSpark.Testing.Framework.FileSystem
{
	public class InitializePartsAssemblyCommand : CommandBase<Type>
	{
		public static InitializePartsAssemblyCommand Default { get; } = new InitializePartsAssemblyCommand();
		InitializePartsAssemblyCommand() : this( AssemblySystemFileSource.Default.Get, ProvisionFromSystemFileCommand.Default.Execute ) {}

		readonly Func<Assembly, FileInfo> source;
		readonly Action<FileInfo> provision;

		[UsedImplicitly]
		public InitializePartsAssemblyCommand( Func<Assembly, FileInfo> source, Action<FileInfo> provision )
		{
			this.source = source;
			this.provision = provision;
		}

		public override void Execute( Type parameter )
		{
			var file = source( parameter.Assembly );
			if ( file != null )
			{
				provision( file );
			}
		}
	}

	public class AssemblySystemFileSource : ParameterizedSourceBase<Assembly, FileInfo>
	{
		public static IParameterizedSource<Assembly, FileInfo> Default { get; } = new AssemblySystemFileSource().Apply( Specification.Implementation );
		AssemblySystemFileSource() {}

		// ReSharper disable once AssignNullToNotNullAttribute
		public override FileInfo Get( Assembly parameter ) => new FileInfo( parameter.Location );

		public sealed class Specification : SpecificationBase<Assembly>
		{
			public static Specification Implementation { get; } = new Specification();
			Specification() {}

			public override bool IsSatisfiedBy( Assembly parameter ) => parameter.Location != null;
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