using DragonSpark.Application.Presentation.Configuration;
using DragonSpark.Extensions;
using DragonSpark.IoC;
using DragonSpark.Objects;
using DragonSpark.Serialization;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Threading;
using System.Windows;

namespace DragonSpark.Application.Presentation
{
    [Singleton( typeof(IResourceDictionaryLocator), Priority = Priority.Lowest )]
    public class ResourceDictionaryLocator : IResourceDictionaryLocator
    {
        public IEnumerable<ResourceDictionary> Locate( Assembly assembly )
        {
            var result = ResolveByType( assembly ).Union( ResolveByResources( assembly ) ).ToArray();
            return result;
        }

        IEnumerable<ResourceDictionary> ResolveByResources( Assembly assembly )
        {
            var manager = assembly.Transform( x => Enumerable.First<string>( x.GetManifestResourceNames() ).Transform( y => new ResourceManager( y.Replace( ".resources", string.Empty ), x ) ) );
            manager.GetStream( string.Empty );
            var set = manager.GetResourceSet( Thread.CurrentThread.CurrentUICulture, false, true );
            var enumerator = set.GetEnumerator();
            while ( enumerator.MoveNext() )
            {
                var key = enumerator.Key.To<string>();
                if ( key.EndsWith( ".xaml" ) )
                {
                    var existing = key.Replace( ".xaml", string.Empty ).Replace( Path.AltDirectorySeparatorChar, '.' ).Transform( x =>
                    {
                        var type = string.Format( "{0}.{1}", assembly.GetRootNamespace(), string.Join( ".", x.ToStringArray( '.' ).Select( y => y.Capitalized() ).ToArray() ) );
                        return assembly.GetType( type, false );
                    } );
                    if ( existing == null )
                    {
                        var item = enumerator.Value.AsTo<Stream, ResourceDictionary>( x => XamlSerializationHelper.Load( x ) as ResourceDictionary );
                        if ( item != null )
                        {
                            yield return item;
                        }
						
                    }
                }
            }
            manager.ReleaseAllResources();
        }

        IEnumerable<ResourceDictionary> ResolveByType( Assembly assembly )
        {
            var result = assembly.GetExportedTypes().FirstOrDefault().Transform( x => new ItemsFactory<ResourceDictionary>( x ).Create<IEnumerable<ResourceDictionary>>() );
            return result;
        }
    }
}