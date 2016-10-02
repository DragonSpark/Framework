using DragonSpark.Application.Setup;
using DragonSpark.Testing.Framework.Runtime;
using DragonSpark.TypeSystem;
using DragonSpark.Windows;
using System;

namespace DragonSpark.Testing.Framework.Application.Setup
{
	public sealed class FrameworkTypesAttribute : TypeProviderAttributeBase
	{
		public FrameworkTypesAttribute() : base( typeof(InitializationCommand), typeof(Configure), typeof(EnableServicesCommand), typeof(MetadataCommand) ) {}
	}

	public sealed class FormatterTypesAttribute : TypeProviderAttributeBase
	{
		public static Type[] Types { get; } = { typeof(MethodFormatter), typeof(TaskContextFormatter) };

		public FormatterTypesAttribute() : base( Types ) {}
	}
}