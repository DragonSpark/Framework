using System;
using System.Linq;
using System.ServiceModel;
using System.Windows.Markup;

namespace DragonSpark.Application.Presentation.Configuration
{
    public static class XamlTypeResolverExtensions
    {
        public static System.Type ResolveType( this IXamlTypeResolver target, string typeName )
        {
            try
            {
                var result = target.Resolve( typeName );
                return result;
            }
            catch ( NotSupportedException )
            {
                var assemblies = new[] { typeof(Uri).Assembly, typeof(System.Type).Assembly, typeof(IClientChannel).Assembly };
                var name = typeName.Split( ':' )[1];
                var result = assemblies.SelectMany( x => x.GetExportedTypes() ).FirstOrDefault( y => y.Name == name );
                return result;
            }
        }
    }
}