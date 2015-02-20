using DragonSpark.Common.IoC.Commands;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
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
[assembly: IgnoreNamespaceDuringRegistration( typeof(RichTextBox) )]

[assembly: NeutralResourcesLanguage("en-US")]
[assembly: InternalsVisibleTo("DragonSpark.Client.Windows.Testing")]
[assembly: InternalsVisibleTo("DragonSpark.Testing")]
[assembly: XmlnsPrefix( "http://framework.dragonspark.us", "ds" )]
[assembly: XmlnsDefinition( "http://framework.dragonspark.us", "DragonSpark.Client.Windows" )]
[assembly: XmlnsDefinition( "http://framework.dragonspark.us", "DragonSpark.Client.Windows.Forms" )]
[assembly: XmlnsDefinition( "http://framework.dragonspark.us", "DragonSpark.Client.Windows.Interaction" )]
[assembly: XmlnsDefinition( "http://framework.dragonspark.us", "DragonSpark.Client.Windows.Markup" )]
[assembly: XmlnsDefinition( "http://framework.dragonspark.us", "DragonSpark.Client.Windows.Controls" )]