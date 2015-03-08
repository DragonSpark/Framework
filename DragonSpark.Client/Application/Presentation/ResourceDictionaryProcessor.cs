using DragonSpark.Extensions;
using DragonSpark.IoC;

namespace DragonSpark.Application.Presentation
{
    [Singleton( typeof(IResourceDictionaryProcessor), Priority = Priority.Lowest )]
    public class ResourceDictionaryProcessor : IResourceDictionaryProcessor
    {
        readonly IResourceDictionaryLocator locator;

        public ResourceDictionaryProcessor( IResourceDictionaryLocator locator )
        {
            this.locator = locator;
        }

        public void Process( IModule container )
        {
            var items = locator.Locate( container.GetType().Assembly );
            items.Apply( System.Windows.Application.Current.Resources.MergedDictionaries.Add );
        }
    }
}