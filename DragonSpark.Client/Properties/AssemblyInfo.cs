using DragonSpark;
using DragonSpark.Client;
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("DragonSpark.Client")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("DragonSpark.Client")]
[assembly: AssemblyCopyright("Copyright © DragonSpark Technologies Inc. 2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("f1561bb9-aec0-4454-a447-3670af957222")]

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

// [assembly: JsMergedFile( Filename = "MySite.js", Sources = new[] { "jQuery.js", "MyPage1.js", "MyPage2.js" } )]
// [assembly: JsMergedFile( Filename = "Resources/DragonSpark.Client.min.js", Sources = new[] { "res/DragonSpark.Client.js" }, Minify = true )]

[assembly: ClientResources( "dragonspark" )]
[assembly: ClientResource( "Core/jquery-2.0.2.min.js", Priority = Priority.Lowest )]
[assembly: ClientResource( "Core/knockout-2.3.0.js", Priority = Priority.Lower )]
[assembly: ClientResource( "Core/jquery.dateFormat-1.0.js" )]
[assembly: ClientResource( "Core/knockout.mapping-latest.js" )]
[assembly: ClientResource( "Core/linq.min.js" )]