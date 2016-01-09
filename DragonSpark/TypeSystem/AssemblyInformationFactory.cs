using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Extensions;
using DragonSpark.Setup.Registration;
using PostSharp.Patterns.Contracts;

namespace DragonSpark.TypeSystem
{
	[RegisterFactory]
	public class AssemblyInformationFactory : FactoryBase<Assembly, AssemblyInformation>
	{
		readonly IAttributeProvider provider;

		readonly static Type[] Attributes =
		{
			typeof(AssemblyTitleAttribute),
			typeof(AssemblyProductAttribute),
			typeof(AssemblyCompanyAttribute),
			typeof(AssemblyDescriptionAttribute),
			typeof(AssemblyConfigurationAttribute),
			typeof(AssemblyCopyrightAttribute)
		};

		public AssemblyInformationFactory() : this( AttributeProvider.Instance ) {}

		public AssemblyInformationFactory( [Required]IAttributeProvider provider ) : base( new FactoryParameterCoercer<Assembly>() )
		{
			this.provider = provider;
		}

		protected override AssemblyInformation CreateItem( Assembly parameter )
		{
			var result = new AssemblyInformation { Version = parameter.GetName().Version };
			Attributes.Select( parameter.GetCustomAttribute ).Cast<object>().NotNull().Each( item => item.MapInto( result ) );
			result.Configuration = result.Configuration.NullIfEmpty() ?? provider.FromMetadata<DebuggableAttribute, string>( parameter, attribute => "DEBUG" );
			return result;
		}
	}
}