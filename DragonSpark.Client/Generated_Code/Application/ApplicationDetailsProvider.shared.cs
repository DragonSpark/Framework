using System;
using System.Reflection;
using DragonSpark.Application.Assemblies;
using DragonSpark.Extensions;

namespace DragonSpark.Application
{
	public class ApplicationDetailsProvider
	{
		readonly Assembly assembly;

		public ApplicationDetailsProvider() : this( Assembly.GetExecutingAssembly() )
		{}

		public ApplicationDetailsProvider( string typeName ) : this( Type.GetType( typeName ) )
		{}

		public ApplicationDetailsProvider( Type targetType ) : this( targetType.Assembly )
		{}

		public ApplicationDetailsProvider( Assembly assembly )
		{
			this.assembly = assembly;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
		public ApplicationDetails RetrieveApplicationDetails()
		{
			var result = new ApplicationDetails
			{
				Title = assembly.FromMetadata<AssemblyTitleAttribute, string>( item => item.Title ),
				Product = assembly.FromMetadata<AssemblyProductAttribute, string>( item => item.Product ),
				Company = assembly.FromMetadata<AssemblyCompanyAttribute, string>( item => item.Company ),
				CompanyUri = assembly.FromMetadata<CompanyUriAttribute, Uri>( item => item.Uri ),
				Description = assembly.FromMetadata<AssemblyDescriptionAttribute, string>( item => item.Description ),
				Configuration = assembly.FromMetadata<AssemblyConfigurationAttribute, string>( item => item.Configuration ),
				IssueTrackingUri = assembly.FromMetadata<IssueTrackingUriAttribute, Uri>( item => item.Uri ),
				SupportUri = assembly.FromMetadata<SupportUriAttribute, Uri>( item => item.Uri ),
				OriginalLaunchDate = assembly.FromMetadata<OriginalLaunchDateAttribute, DateTime?>( item => item.LaunchDate, () => (DateTime?)null ),
				VersionInformation = new VersionInformation( new Version( assembly.FromMetadata<AssemblyVersionAttribute,string>( item => item.Version, ResolveVersion ) ) ),
				SupportDescription = assembly.FromMetadata<SupportDescriptionAttribute, string>( item => item.Description ),
				Copyright = assembly.FromMetadata<AssemblyCopyrightAttribute, string>( item => item.Copyright )
			};
			return result;
		}

		string ResolveVersion()
		{
			var result = assembly.FullName.Transform( item => item.Split( ',' )[1].Split( '=' )[1], () => "1.0.0.0" );
			return result;
		}
	}
}