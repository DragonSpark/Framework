using DragonSpark.Aspects.Specifications;
using DragonSpark.Aspects.Validation;
using DragonSpark.Commands;
using DragonSpark.Sources;
using DragonSpark.Specifications;
using DragonSpark.Testing.Framework.Application.Setup;
using DragonSpark.Testing.Framework.FileSystem;
using DragonSpark.Testing.Parts;
using System;

namespace DragonSpark.Testing.Objects.FileSystem
{
	/*public sealed class InitializeUserSettingsCommand : SuppliedCommand<System.IO.FileInfo>
	{
		public InitializeUserSettingsCommand() : base( Framework.Application.Setup.InitializeUserSettingsCommand.Current.Get(), Windows.Setup.Defaults.UserSettingsPath ) {}
	}*/

	[ApplyAutoValidation, ApplySpecification( typeof(OnlyOnceSpecification) )]
	public sealed class InitializePartsCommand : CompositeCommand
	{
		public InitializePartsCommand() : base( 
			InitializePartsAssemblyCommand.Current.Get(),
			TypeSystem.Configuration.AssemblyLoader.Configured( AssemblyLoader.Current.GetCurrentDelegate() )
			) {}

		public sealed class Attribute : ExecutedRunCommandAttributeBase
		{
			public Attribute() : base( new InitializePartsCommand() ) {}
		}
	}

	public sealed class InitializePartsAssemblyCommand : SuppliedEnumerableComand<Type>
	{
		public static IScope<InitializePartsAssemblyCommand> Current { get; } = new Scope<InitializePartsAssemblyCommand>( Factory.GlobalCache( () => new InitializePartsAssemblyCommand() ) );
		InitializePartsAssemblyCommand() : base( Framework.FileSystem.InitializePartsAssemblyCommand.Current.Get(), typeof(PublicClass) ) {}
	}
}
