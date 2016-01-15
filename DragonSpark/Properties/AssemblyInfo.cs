using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using DragonSpark;
using DragonSpark.Activation;
using DragonSpark.Activation.FactoryModel;
using DragonSpark.Activation.IoC;
using DragonSpark.Aspects;
using DragonSpark.Modularity;
using DragonSpark.Runtime;
using DragonSpark.Setup;
using DragonSpark.Setup.Registration;
using Microsoft.Practices.ServiceLocation;

[assembly: CLSCompliant( false )]

[assembly: AssemblyTitle( "DragonSpark" )]
[assembly: AssemblyDescription( "" )]
[assembly: AssemblyConfiguration( "" )]

[assembly: AssemblyProduct( "The DragonSpark Framework" )]
[assembly: AssemblyCompany( "DragonSpark Technologies Inc." )]
[assembly: AssemblyCopyright( "Copyright © DragonSpark Technologies Inc. 2016" )]
[assembly: NeutralResourcesLanguage( "en-US" )]
[assembly: InternalsVisibleTo( "DragonSpark.Testing" )]

[assembly: AssemblyTrademark( "" )]
[assembly: AssemblyCulture( "" )]

[assembly: ComVisible( false )]

[assembly: AssemblyVersion( "2016.2.1.1" )]
[assembly: AssemblyFileVersion( "2016.2.1.1" )]
[assembly: InternalsVisibleTo( "DragonSpark.Testing" )]
/*
[assembly: XmlnsPrefix("http://framework.dragonspark.us", "ds")]
[assembly: XmlnsDefinition("http://framework.dragonspark.us", "DragonSpark.Configuration")]
[assembly: XmlnsDefinition("http://framework.dragonspark.us", "DragonSpark.IoC.Configuration")]
[assembly: XmlnsDefinition("http://framework.dragonspark.us", "DragonSpark.Logging.Configuration")]
*/

[assembly: Registration( Priority.AboveLowest, typeof(IServiceLocator), typeof(IModule), typeof(IExecutionContext), typeof(ISetupParameter), typeof(Collection), typeof(Collection<>), typeof(FactoryParameterCoercer<>), Namespaces = "DragonSpark.Aspects" )]
// [assembly: AddAspect( AttributeTargetAssemblies = "regex:^mscorlib", AttributeTargetTypes = "System.Collections.Collection`1", AttributeTargetMembers = "Add" )]

[assembly: DefaultValueAspectProvider]