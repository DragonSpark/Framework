using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.Setup.Registration;

namespace DragonSpark.TypeSystem
{
	[RegisterFactoryForResult]
	public class AssemblyInformationFactory : FactoryBase<Assembly, AssemblyInformation>
	{
		readonly static Type[] Attributes =
		{
			typeof(AssemblyTitleAttribute),
			typeof(AssemblyProductAttribute),
			typeof(AssemblyCompanyAttribute),
			typeof(AssemblyDescriptionAttribute),
			typeof(AssemblyConfigurationAttribute),
			typeof(AssemblyCopyrightAttribute)
		};

		public AssemblyInformationFactory() : base( new FactoryParameterCoercer<Assembly>() )
		{}

		protected override AssemblyInformation CreateItem( Assembly parameter )
		{
			var result = new AssemblyInformation { Version = parameter.GetName().Version };
			Attributes.Select( parameter.GetCustomAttribute ).Cast<object>().NotNull().Each( item => item.MapInto( result ) );
			result.Configuration = result.Configuration.NullIfEmpty() ?? parameter.FromMetadata<DebuggableAttribute, string>( attribute => "DEBUG" );
			return result;
		}
	}
}