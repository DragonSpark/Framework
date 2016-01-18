using DragonSpark;
using DragonSpark.ComponentModel;
using DragonSpark.Setup.Registration;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Markup;
using DragonSpark.Aspects;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle( "DragonSpark.Testing.Framework" )]
[assembly: AssemblyDescription( "" )]
[assembly: AssemblyConfiguration( "" )]
[assembly: AssemblyCompany( "The DragonSpark Framework" )]
[assembly: AssemblyProduct( "DragonSpark.Testing.Framework" )]
[assembly: AssemblyCopyright( "Copyright © The DragonSpark Framework 2011" )]
[assembly: AssemblyTrademark( "" )]
[assembly: AssemblyCulture( "" )]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible( false )]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid( "0d3d9c01-5c46-4489-98cb-455604dabe02" )]

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
[assembly: AssemblyVersion( "2016.2.1.1" )]
[assembly: AssemblyFileVersion( "2016.2.1.1" )]

[assembly: XmlnsDefinition( "http://framework.dragonspark.us", "DragonSpark.Testing.Framework" )]
[assembly: Registration( Priority.Lower )]
[assembly: ApplyDefaultValues]