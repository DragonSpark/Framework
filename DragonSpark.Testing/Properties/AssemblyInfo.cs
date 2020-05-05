using DragonSpark.Application.Hosting.xUnit;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Xunit;

// General Information about an assembly is controlled through the following
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyCopyright("Copyright © 2020 DragonSpark Technologies Incorporated")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible
// to COM components.  If you need to access a type in this assembly from
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("0475e50e-41d2-496c-9a99-1828e85400ba")]

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
[assembly: InternalsVisibleTo("DynamicProxyGenAssembly2")]
[assembly: XunitTestingApplication]

[assembly:
	TestFramework("DragonSpark.Application.Hosting.xUnit.TestFramework", "DragonSpark.Application.Hosting.xUnit")]