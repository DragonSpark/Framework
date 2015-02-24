using System;
using DragonSpark;
using DragonSpark.Activation.IoC.Commands;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Practices.ServiceLocation;

[assembly: CLSCompliant( false )]

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("DragonSpark")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]

[assembly: AssemblyProduct( "The DragonSpark Framework" )]
[assembly: AssemblyCompany( "DragonSpark Technologies Inc." )]
[assembly: AssemblyCopyright( "Copyright © DragonSpark Technologies Inc. 2015" )]
[assembly: NeutralResourcesLanguage( "en-US" )]
[assembly: InternalsVisibleTo( "DragonSpark.Testing" )]

[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
// [assembly: Guid("497a96a0-d0ce-4d7b-9d3e-1731259466ac")]

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
[assembly: AssemblyVersion("2015.10.1.1")]
[assembly: AssemblyFileVersion("2015.10.1.1")]
// [assembly: InternalsVisibleTo( "DragonSpark.Testing, PublicKey=002400000480000094000000060200000024000052534131000400000100010015b5cbe04089f7b0a29ed23a95e0b6601a65a2b27191460d819f3523802925f16d210ed7fbd6bee26e4a3d3d33832ab05182cc7157c3d66977b0d274dec3096a23c56e9e06c6c132e246a6ad283305b3670af7f101b3b3f4628813555ce3448b6cdafc3ceb2982ae79704e7b7763c03ca58ec2000bed3814cbea63c25c3a36b1" )]
[assembly: InternalsVisibleTo( "DragonSpark.Testing" )]
/*
[assembly: XmlnsPrefix("http://framework.dragonspark.us", "ds")]
[assembly: XmlnsDefinition("http://framework.dragonspark.us", "DragonSpark.Configuration")]
[assembly: XmlnsDefinition("http://framework.dragonspark.us", "DragonSpark.IoC.Configuration")]
[assembly: XmlnsDefinition("http://framework.dragonspark.us", "DragonSpark.Logging.Configuration")]
*/
[assembly: Registration( Priority.AboveLowest, typeof(UnityContainerTypeConfiguration), typeof(IServiceLocator) )]