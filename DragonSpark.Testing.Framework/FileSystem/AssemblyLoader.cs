﻿using DragonSpark.Aspects.Alteration;
using DragonSpark.Sources;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Windows.FileSystem;
using JetBrains.Annotations;
using System;
using System.Reflection;

namespace DragonSpark.Testing.Framework.FileSystem
{
	[ApplyAlteration( typeof(FileNameAlteration) )]
	public class AssemblyLoader : ParameterizedSourceBase<string, Assembly>
	{
		public static ISource<AssemblyLoader> Current { get; } = new Scope<AssemblyLoader>( Factory.GlobalCache( () => new AssemblyLoader() ) );
		AssemblyLoader() : this( FileSystemRepository.Current.Get(), AppDomain.CurrentDomain ) {}

		readonly IFileSystemRepository repository;
		readonly AppDomain domain;

		[UsedImplicitly]
		public AssemblyLoader( IFileSystemRepository repository, AppDomain domain )
		{
			this.repository = repository;
			this.domain = domain;
		}

		public override Assembly Get( string parameter ) => domain.Load( repository.GetFile( parameter ).Unwrap() );
	}
}