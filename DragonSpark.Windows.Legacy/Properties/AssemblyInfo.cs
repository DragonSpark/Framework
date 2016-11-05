// ReSharper disable once FilesNotPartOfProjectWarning

using DragonSpark;
using DragonSpark.Application;
using System;
using System.Reflection;
using System.Resources;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Windows.Markup;

[assembly: AssemblyTitle( "DragonSpark.Windows.Legacy" )]
[assembly: AssemblyDescription( "" )]
[assembly: AssemblyConfiguration( "" )]
[assembly: AssemblyCompany( "DragonSpark Technologies Incorporated" )]
[assembly: AssemblyProduct( "DragonSpark.Windows.Legacy" )]
[assembly: AssemblyCopyright( "Copyright © The DragonSpark Framework 2016" )]
[assembly: AssemblyTrademark( "" )]
[assembly: AssemblyCulture( "" )]

[assembly: NeutralResourcesLanguage( "en-US" )]

[assembly: ComVisible( false )]
[assembly: CLSCompliant( false )]

[assembly: Guid( "4e4cf3dc-1c0e-4a37-9871-71124aef7cb9" )]

[assembly: AssemblyVersion( "0.1.2.0" )]
[assembly: AssemblyFileVersion( "0.1.2.0" )]
[assembly: XmlnsPrefix( "http://framework.dragonspark.us", "ds" )]
[assembly: XmlnsDefinition( "http://framework.dragonspark.us", "DragonSpark.Windows.Legacy.Markup" )]
[assembly: Registration( Priority.AfterLower )]
[assembly: InternalsVisibleTo( "DragonSpark.Windows.Testing" )]
