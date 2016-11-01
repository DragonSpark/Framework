﻿using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Commands;
using DragonSpark.Sources;
using DragonSpark.Specifications;
using DragonSpark.Testing.Framework.Application;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Testing.Parts;
using System;

namespace DragonSpark.Testing.Objects.FileSystem
{
	[ApplyAutoValidation, ApplySpecification( typeof(OnlyOnceSpecification) )]
	public sealed class InitializePartsCommand : CompositeCommand
	{
		public InitializePartsCommand() : base( 
			InitializePartsAssemblyCommand.Current.Get(),
			TypeSystem.Configuration.AssemblyLoader.ToCommand( AssemblyLoader.Current.GetValueDelegate() )
			) {}

		public sealed class Public : ApplicationPublicPartsAttribute
		{
			public Public() : base( new InitializePartsCommand().Execute ) {}
		}

		public sealed class All : ApplicationPartsAttribute
		{
			public All() : base( new InitializePartsCommand().Execute ) {}
		}
	}

	public sealed class InitializePartsAssemblyCommand : SuppliedEnumerableComand<Type>
	{
		public static IScope<InitializePartsAssemblyCommand> Current { get; } = Scopes.Create( () => new InitializePartsAssemblyCommand() );
		InitializePartsAssemblyCommand() : base( Framework.FileSystem.InitializePartsAssemblyCommand.Current.Get(), typeof(PublicClass) ) {}
	}
}
