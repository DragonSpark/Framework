using DragonSpark.Setup.Registration;
using DragonSpark.TypeSystem;
using System.Reflection;
using System.Runtime.InteropServices;
using DragonSpark.Testing.Objects;
using DragonSpark.Windows.Testing.Setup;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle( "DragonSpark.Windows.Testing" )]
[assembly: AssemblyDescription( "" )]
[assembly: AssemblyConfiguration( "" )]
[assembly: AssemblyCompany( "DragonSpark Technologies Incorporated" )]
[assembly: AssemblyProduct( "DragonSpark.Windows.Testing" )]
[assembly: AssemblyCopyright( "Copyright © The DragonSpark Framework 2016" )]
[assembly: AssemblyTrademark( "" )]
[assembly: AssemblyCulture( "" )]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible( false )]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid( "b21e892a-be56-4024-bd63-aa332279e7c5" )]

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
[assembly: Application]
[assembly: Registration( typeof(Command) )]

[assembly: AssemblyProvider.Factory] // Default assembly factory.