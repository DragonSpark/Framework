using DragonSpark.Application;
using DragonSpark.Application.Presentation;
using Microsoft.Practices.Unity;

namespace DragonSpark.Features.Modules.ViewProfiles
{
	public class Module : ApplicationModule<Configuration>
	{
		readonly IResourceDictionaryProcessor processor;

		public Module( IUnityContainer container, IModuleMonitor monitor, IResourceDictionaryProcessor processor ) : base( container, monitor )
		{
			this.processor = processor;
		}

        protected override void Initialize()
        {
            processor.Process( this );
            base.Initialize();
        }
	}
}
