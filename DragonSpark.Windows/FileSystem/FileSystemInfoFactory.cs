using DragonSpark.Activation;
using DragonSpark.Sources.Parameterized;
using JetBrains.Annotations;
using System;
using System.IO.Abstractions;

namespace DragonSpark.Windows.FileSystem
{
	public abstract class FileSystemInfoFactory<TAbstraction, TImplementation, TResult> : ParameterizedSourceBase<string, TResult> 
		where TAbstraction : FileSystemInfoBase
		where TResult : IFileSystemInfo
		where TImplementation : TResult
	{
		readonly Func<string, TAbstraction> source;
		readonly Func<TAbstraction, TImplementation> abstractionSource;

		protected FileSystemInfoFactory( Func<string, TAbstraction> source ) : this( source, ParameterConstructor<TAbstraction, TImplementation>.Default ) {}

		[UsedImplicitly]
		protected FileSystemInfoFactory( Func<string, TAbstraction> source, Func<TAbstraction, TImplementation> abstractionSource )
		{
			this.source = source;
			this.abstractionSource = abstractionSource;
		}

		public override TResult Get( string parameter ) => abstractionSource( source( parameter ) );
	}

	public class FileSystemImplementationFactory<TFileSystemInfo, TImplementation, TAbstraction> : ParameterizedSourceBase<string, TAbstraction> 
		where TFileSystemInfo : System.IO.FileSystemInfo 
		where TImplementation : TAbstraction
	{
		readonly Func<string, TFileSystemInfo> infoSource;
		readonly Func<TFileSystemInfo, TImplementation> wrapperSource;

		public FileSystemImplementationFactory() : this( ParameterConstructor<string, TFileSystemInfo>.Default, ParameterConstructor<TFileSystemInfo, TImplementation>.Default ) {}

		[UsedImplicitly]
		public FileSystemImplementationFactory( Func<string, TFileSystemInfo> infoSource, Func<TFileSystemInfo, TImplementation> wrapperSource )
		{
			this.infoSource = infoSource;
			this.wrapperSource = wrapperSource;
		}

		public override TAbstraction Get( string parameter ) => wrapperSource( infoSource( parameter ) );
	}
}