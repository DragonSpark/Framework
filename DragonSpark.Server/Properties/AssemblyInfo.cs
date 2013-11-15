using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
using System.Windows.Markup;
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
using DragonSpark;
using DragonSpark.IoC.Configuration;
using DragonSpark.Server.Configuration;
using Microsoft.Owin;

[assembly: AssemblyTitle("DragonSpark.Server")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("DragonSpark.Server")]
[assembly: AssemblyCopyright("Copyright ©  2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("fdc73486-ab1f-48e2-aeeb-f5f93cc75445")]

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

[assembly: XmlnsPrefix("http://framework.dragonspark.us/web", "dsweb")]

[assembly: XmlnsDefinition("http://framework.dragonspark.us/web", "DragonSpark.Server")]
[assembly: XmlnsDefinition("http://framework.dragonspark.us/web", "DragonSpark.Server.Configuration")]
[assembly: XmlnsDefinition("http://framework.dragonspark.us/web", "DragonSpark.Server.Security")]

[assembly: OwinStartup( typeof(EnableSignalR) )]

[assembly: Registration( Priority.Low )]