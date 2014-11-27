using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
using System.Windows;
using System.Windows.Controls;
using DragonSpark.Stationed.IoC.Commands;
using Xceed.Wpf.AvalonDock;

[assembly: AssemblyTitle("DragonSpark.Client.Stationed")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("DragonSpark.Client.Stationed")]
[assembly: AssemblyCopyright("Copyright ©  2014")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("5ff3f7b6-75ff-4c2f-b837-909268889d96")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
// You can specify all the values or you can default the Build and Revision Numbers 
// by using the '*' as shown below:
// [assembly: AssemblyVersion("1.0.*")]
[assembly: AssemblyVersion("1.0.0.0")]
[assembly: AssemblyFileVersion("1.0.0.0")]


[assembly:ThemeInfo( ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly )]

[assembly: IgnoreNamespaceDuringRegistration( typeof(DockingManager) )]
[assembly: IgnoreNamespaceDuringRegistration( typeof(RichTextBox) )]