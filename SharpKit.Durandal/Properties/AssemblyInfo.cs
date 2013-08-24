// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.

using SharpKit.Durandal.Modules;
using SharpKit.JavaScript;
using SharpKit.jQuery;
using SharpKit.KnockoutJs;
using System.Reflection;
using System.Runtime.InteropServices;

[assembly: AssemblyTitle("SharpKit.Durandal")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("")]
[assembly: AssemblyProduct("SharpKit.Durandal")]
[assembly: AssemblyCopyright("Copyright ©  2013")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("8c6e3007-b65a-49fa-bf46-87f535fe6164")]

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

[assembly: ModuleRegistration( typeof(jQuery), "Resources/Modules/jquery-2.0.2.min.js" )]
[assembly: ModuleRegistration( typeof(Knockout), "Resources/Modules/knockout-2.3.0.js" )]
[assembly: JsEmbeddedResource( "Resources/Modules/require.js" )]
[assembly: JsEmbeddedResource( "Resources/Modules/text.js" )]

[assembly: JsEmbeddedResource( "Resources/activator.js" )]
[assembly: JsEmbeddedResource( "Resources/app.js" )]
[assembly: JsEmbeddedResource( "Resources/binder.js" )]
[assembly: JsEmbeddedResource( "Resources/composition.js" )]
[assembly: JsEmbeddedResource( "Resources/events.js" )]
[assembly: JsEmbeddedResource( "Resources/system.js" )]
[assembly: JsEmbeddedResource( "Resources/viewEngine.js" )]
[assembly: JsEmbeddedResource( "Resources/viewLocator.js" )]

[assembly: JsEmbeddedResource( "Resources/Plugins/dialog.js" )]
[assembly: JsEmbeddedResource( "Resources/Plugins/history.js" )]
[assembly: JsEmbeddedResource( "Resources/Plugins/http.js" )]
[assembly: JsEmbeddedResource( "Resources/Plugins/observable.js" )]
[assembly: JsEmbeddedResource( "Resources/Plugins/router.js" )]
[assembly: JsEmbeddedResource( "Resources/Plugins/serializer.js" )]
[assembly: JsEmbeddedResource( "Resources/Plugins/widget.js" )]

[assembly: JsEmbeddedResource( "Resources/Transitions/entrance.js" )]

[assembly: JsEmbeddedResource( "Resources/Widgets/Expander/view.html" )]
[assembly: JsEmbeddedResource( "Resources/Widgets/Expander/viewmodel.js" )]