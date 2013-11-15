using DragonSpark.Extensions;
using DragonSpark.IoC.Configuration;
using DragonSpark.Logging;
using MefContrib.Integration.Unity;
using Microsoft.Practices.Unity;
using System.ComponentModel.Composition.Hosting;
using System.Linq;

namespace DragonSpark.IoC
{
	public class CompositionExtension : UnityContainerExtension
	{
	    public CompositionExtension()
	    {
	        EnableRegistration = true;
	    }

	    protected override void Initialize()
		{}

		public CompositionContainer CompositionContainer
		{
			get { return compositionContainer ?? ( compositionContainer = ResolveCompositionContainer() ); }
		}	CompositionContainer compositionContainer;

	    public bool EnableRegistration { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Catalogs are disposed when the Container is disposed." )]
		CompositionContainer ResolveCompositionContainer()
		{
		    var result = Container.EnableCompositionIntegration().CompositionContainer;
			return result;
		}
	}
}