using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.Setup.Registration;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	[Register.Factory]
	public class AssemblyInformationFactory : FactoryBase<Assembly, AssemblyInformation>
	{
		public static AssemblyInformationFactory Instance { get; } = new AssemblyInformationFactory();

		readonly static Type[] Attributes =
		{
			typeof(AssemblyTitleAttribute),
			typeof(AssemblyProductAttribute),
			typeof(AssemblyCompanyAttribute),
			typeof(AssemblyDescriptionAttribute),
			typeof(AssemblyConfigurationAttribute),
			typeof(AssemblyCopyrightAttribute)
		};

		AssemblyInformationFactory() : base( new FactoryParameterCoercer<Assembly>() )
		{}

		protected override AssemblyInformation CreateItem( Assembly parameter )
		{
			var result = new AssemblyInformation { Version = parameter.GetName().Version };
			Attributes.Select( parameter.GetCustomAttribute ).Cast<object>().NotNull().Each( item => item.MapInto( result ) );
			result.Configuration = result.Configuration.NullIfEmpty() ?? TypeSystem.Attributes.Get( parameter ).From<DebuggableAttribute, string>( attribute => "DEBUG" );
			return result;
		}
	}
}