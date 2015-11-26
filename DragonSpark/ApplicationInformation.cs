using DragonSpark.Activation;
using DragonSpark.Extensions;
using DragonSpark.Setup;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace DragonSpark
{
	public class ApplicationInformation
	{
		public AssemblyInformation AssemblyInformation { get; set; }

		public Uri CompanyUri { get; set; }
		
		public DateTimeOffset? DeploymentDate { get; set; }
	}

	public class AssemblyInformation
	{
		public string Title { get; set; }

		public string Product { get; set; }

		public string Company { get; set; }

		public string Description { get; set; }

		public string Configuration { get; set; }

		public string Copyright { get; set; }

		public Version Version { get; set; }
	}

	[RegisterFactoryForResult]
	public class AssemblyInformationFactory : FactoryBase<Assembly, AssemblyInformation>
	{
		static readonly Type[] Attributes =
		{
			typeof(AssemblyTitleAttribute),
			typeof(AssemblyProductAttribute),
			typeof(AssemblyCompanyAttribute),
			typeof(AssemblyDescriptionAttribute),
			typeof(AssemblyConfigurationAttribute),
			typeof(AssemblyCopyrightAttribute)
		};

		protected override AssemblyInformation CreateItem( Assembly parameter )
		{
			var result = new AssemblyInformation();
			parameter.GetName().Append(  Attributes.Select( parameter.GetCustomAttribute ).Cast<object>() ).NotNull().Apply( attribute => attribute.MapInto( result ) );
			result.Configuration = result.Configuration.NullIfEmpty() ?? parameter.FromMetadata<DebuggableAttribute, string>( attribute => "DEBUG" );
			return result;
		}
	}
}