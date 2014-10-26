// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
using System.Reflection;
using System.Runtime.InteropServices;
using DragonSpark;
using DragonSpark.IoC.Configuration;
using DragonSpark.Server.ClientHosting;

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
[assembly: AssemblyVersion("2013.11.12.1")]
[assembly: AssemblyFileVersion("2013.11.12.1")]

[assembly: ClientResources( "dragonspark" )]
/*[assembly: ClientResource( "Core/stacktrace.js", Priority = Priority.Lowest )]
[assembly: ClientResource( "Core/jquery-2.0.3.min.js", Priority = Priority.Lowest )]
[assembly: ClientResource( "Core/knockout-3.0.0.js", Priority = Priority.Lower )]
[assembly: ClientResource( "Core/q.min.js", Priority = Priority.Lower )]
[assembly: ClientResource( "Core/breeze.min.js" )]
[assembly: ClientResource( "Core/jquery.signalR-2.0.0.min.js" )]
[assembly: ClientResource( "Core/jquery.ba-resize.min.js" )]
[assembly: ClientResource( "Core/jquery.dateFormat-1.0.js" )]
[assembly: ClientResource( "Core/knockout.mapping-latest.js" )]
[assembly: ClientResource( "Core/knockout.validation.js" )]
[assembly: ClientResource( "Core/linq.min.js" )]*/
[assembly: Registration( Priority.Lower )]