using DragonSpark.Activation;
using DragonSpark.ComponentModel;
using DragonSpark.Extensions;
using System;
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

	public class AssemblyInformationFactory : FactoryBase<Assembly, AssemblyInformation>
	{
		static readonly Type[] Attributes = { typeof(AssemblyTitleAttribute), typeof(AssemblyProductAttribute), typeof(AssemblyCompanyAttribute), typeof(AssemblyDescriptionAttribute), typeof(AssemblyConfigurationAttribute), typeof(AssemblyCompanyAttribute) };

		protected override AssemblyInformation CreateFrom( Type resultType, Assembly parameter )
		{
			var result = new AssemblyInformation { Version = new Version( parameter.GetCustomAttribute<AssemblyVersionAttribute>().Transform( attribute => attribute.Version ) ?? "1.0.0.0" ) };

			Attributes.Select( parameter.GetCustomAttribute ).NotNull().Apply( attribute => attribute.MapInto( result ) );
			
			return result;
		}
	}
}