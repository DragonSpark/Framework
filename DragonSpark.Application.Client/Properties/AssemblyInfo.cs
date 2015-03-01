using DragonSpark;
using DragonSpark.Activation.IoC.Commands;
using DragonSpark.Application.Client.Presentation;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Markup;

[assembly: AssemblyTitle("DragonSpark.Client.Windows")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("DragonSpark.Client.Windows")]
[assembly: AssemblyCopyright("Copyright ©  2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: Guid("5ff3f7b6-75ff-4c2f-b837-909268889d96")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]

[assembly:ThemeInfo( ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly )]

// [assembly: IgnoreNamespaceDuringRegistration( typeof(DockingManager) )]
[assembly: NeutralResourcesLanguage("en-US")]
[assembly: InternalsVisibleTo("DragonSpark.Client.Windows.Testing")]
[assembly: InternalsVisibleTo("DragonSpark.Testing")]
[assembly: XmlnsPrefix( "http://framework.dragonspark.us", "ds" )]
// [assembly: XmlnsDefinition( "http://framework.dragonspark.us", "DragonSpark.Client" )]
// [assembly: XmlnsDefinition( "http://framework.dragonspark.us", "DragonSpark.Client.Forms" )]
[assembly: XmlnsDefinition( "http://framework.dragonspark.us", "DragonSpark.Application.Client.Interaction" )]
[assembly: XmlnsDefinition( "http://framework.dragonspark.us", "DragonSpark.Application.Client.Markup" )]
[assembly: XmlnsDefinition( "http://framework.dragonspark.us", "DragonSpark.Application.Client.Controls" )]
[assembly: XmlnsDefinition( "http://framework.dragonspark.us", "DragonSpark.Application.Client.Converters" )]
[assembly: Registration( Priority.Lower, typeof(ViewObject) )]