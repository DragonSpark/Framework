using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Commands;
using DragonSpark.Sources;
using DragonSpark.Specifications;
using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Testing.Parts;
using System;
using System.Collections.Generic;

namespace DragonSpark.Testing.Objects.FileSystem
{
	[ApplyAutoValidation, ApplySpecification( typeof(OnlyOnceSpecification) )]
	public sealed class InitializePartsCommand : CompositeCommand
	{
		public InitializePartsCommand() : base( 
			InitializePartsAssemblyCommand.Current.Get(),
			TypeSystem.Configuration.AssemblyLoader.Configured( AssemblyLoader.Current.Delegate().Self )
			) {}

		public sealed class Attribute : ExecutedRunCommandAttributeBase
		{
			public Attribute() : base( new InitializePartsCommand() ) {}
		}
	}

	public sealed class InitializePartsAssemblyCommand : SuppliedEnumerableComand<Type>
	{
		public static IScope<InitializePartsAssemblyCommand> Current { get; } = new Scope<InitializePartsAssemblyCommand>( Factory.GlobalCache( () => new InitializePartsAssemblyCommand() ) );
		InitializePartsAssemblyCommand() : base( new SpecificationCommand<IEnumerable<Type>>( new OnlyOnceSpecification<IEnumerable<Type>>(), Framework.FileSystem.InitializePartsAssemblyCommand.Current.Get().Execute ), typeof(PublicClass) ) {}

		/*public sealed class Attribute : ExecutedRunCommandAttributeBase
		{
			public Attribute() : base( Current.Get() ) {}
		}*/
	}
}
