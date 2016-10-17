using DragonSpark.Sources.Parameterized;
using System;
using System.Security.Policy;

namespace DragonSpark.Windows.Runtime
{
	public sealed class AppDomainSource : ParameterizedSourceBase<string, AppDomain>
	{
		public static AppDomainSource Default { get; } = new AppDomainSource();
		AppDomainSource() : this( AppDomain.CurrentDomain ) {}

		readonly AppDomainSetup setup;
		readonly Evidence evidence;

		public AppDomainSource( AppDomain domain ) : this( domain.SetupInformation, domain.Evidence ) {}

		public AppDomainSource( AppDomainSetup setup, Evidence evidence )
		{
			this.setup = setup;
			this.evidence = evidence;
		}

		public override AppDomain Get( string parameter ) => AppDomain.CreateDomain( parameter, evidence, setup );
	}
}