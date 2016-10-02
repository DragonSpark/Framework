using DragonSpark.Application;
using DragonSpark.Extensions;
using DragonSpark.Sources.Parameterized;
using DragonSpark.Sources.Parameterized.Caching;
using System.Composition;
using System.Diagnostics;
using System.Reflection;

namespace DragonSpark.TypeSystem
{
	public sealed class AssemblyInformationSource : FactoryCache<Assembly, AssemblyInformation>
	{
		[Export]
		public static IParameterizedSource<Assembly, AssemblyInformation> Default { get; } = new AssemblyInformationSource();
		AssemblyInformationSource() {}

		protected override AssemblyInformation Create( Assembly parameter )
		{
			var result = new AssemblyInformation
						 {
							 Version = parameter.GetName().Version,
							 Title = parameter.From<AssemblyTitleAttribute, string>( attribute => attribute.Title ),
							 Product = parameter.From<AssemblyProductAttribute, string>( attribute => attribute.Product ),
							 Company = parameter.From<AssemblyCompanyAttribute, string>( attribute => attribute.Company ),
							 Description = parameter.From<AssemblyDescriptionAttribute, string>( attribute => attribute.Description ),
							 Copyright = parameter.From<AssemblyCopyrightAttribute, string>( attribute => attribute.Copyright )
						 };
			result.Configuration = result.Configuration.NullIfEmpty() ?? parameter.From<DebuggableAttribute, string>( attribute => "DEBUG" );
			return result;
		}
	}
}