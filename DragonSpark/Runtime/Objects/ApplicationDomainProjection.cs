using DragonSpark.Compose;
using DragonSpark.Text.Formatting;
using System;

namespace DragonSpark.Runtime.Objects
{
	sealed class ApplicationDomainProjection : FormattedProjection<AppDomain>
	{
		public static ApplicationDomainProjection Default { get; } = new ApplicationDomainProjection();

		ApplicationDomainProjection()
			: base(DefaultApplicationDomainFormatter.Default.Project(x => x.FriendlyName, x => x.Id),
			       ApplicationDomainName.Default.Entry(x => x.FriendlyName, x => x.Id, x => x.IsFullyTrusted),
			       ApplicationDomainIdentifier.Default.Entry(x => x.FriendlyName, x => x.Id, x => x.BaseDirectory,
			                                                 x => x.RelativeSearchPath!)) {}
	}
}