using System.Windows;
using System.Windows.Markup;
using DragonSpark.IoC.Configuration;
using Microsoft.Practices.Unity;

namespace DragonSpark.Application.Presentation.Configuration
{
    [ContentProperty( "ResourceDictionary" )]
    public class RegisterResourceDictionaryCommand : IContainerConfigurationCommand
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Intended to be set through Xaml." )]
        public ResourceDictionary ResourceDictionary { get; set; }

        public void Configure( IUnityContainer container )
        {
            System.Windows.Application.Current.Resources.MergedDictionaries.Add( ResourceDictionary );
        }
    }
}