using SharpKit.Durandal.Modules;
using SharpKit.JavaScript;
using SharpKit.jQuery;

namespace SharpKit.Durandal
{
	[Module( Export = false )]
	public class System
	{
		/// <summary> Durandal's version.</summary>
		// [JsProperty( NativeField = true )]
		public JsString Version { get; set; }

		/// <summary>
		/// A noop function.
		/// </summary>
		public void Noop()
		{}

		/// <summary>
		/// Gets the module id for the specified object.
		/// </summary>
		/// <param name="module">The object whose module id you wish to determine.</param>
		/// <returns>The module id.</returns>
		public string GetModuleId( object module )
		{
			return default( string );
		}

		/// <summary>
		/// Sets the module id for the specified object.
		/// </summary>
		/// <param name="module">The object whose module id you wish to set.</param>
		/// <param name="id">The id to set for the specified object.</param>
		public void SetModuleId( object module, string id )
		{}

		/// <summary>
		/// Resolves the default object instance for a module. If the module is an object, the module is returned. If the module is a function, that function is called with `new` and it's result is returned.
		/// </summary>
		/// <param name="module">The module to use to get/create the default object for.</param>
		/// <returns>The default object for the module.</returns>
		public object ResolveObject( object module )
		{
			return default(object);
		}

		/// <summary>
		/// Gets/Sets whether or not Durandal is in debug mode.
		/// </summary>
		/// <param name="enable">Turns on/off debugging.</param>
		/// <returns>Whether or not Durandal is current debugging.</returns>
		public bool Debug( bool enable )
		{
			return default(bool);
		}

		/// <summary>
		/// Logs data to the console. Pass any number of parameters to be logged. Log output is not processed if the framework is not running in debug mode.
		/// </summary>
		/// <param name="info">The objects to log.</param>
		public void Log( params object[] info )
		{}

		/// <summary>
		/// Logs an error.
		/// </summary>
		/// <param name="obj">The error to report.</param>
		public void Error( JsString obj )
		{}

		/// <summary>
		/// Logs an error.
		/// </summary>
		/// <param name="obj">The error to report.</param>
		public void Error( JsError obj )
		{}

		/// <summary>
		/// Asserts a condition by throwing an error if the condition fails.
		/// </summary>
		/// <param name="condition">The condition to check.</param>
		/// <param name="message">The message to report in the error if the condition check fails.</param>
		public void Assert( bool condition, string message )
		{}

		/// <summary>
		/// Creates a deferred object which can be used to create a promise. Optionally pass a function action to perform which will be passed an object used in resolving the promise.
		/// </summary>
		/// <param name="function">The action to defer. You will be passed the deferred object as a paramter.</param>
		/// <returns>The deferred object.</returns>
		public Deferred Defer( JsFunction function )
		{
			return default(Deferred);
		}

		/// <summary>
		/// Creates a simple V4 UUID. This should not be used as a PK in your database. It can be used to generate internal, unique ids. For a more robust solution see [node-uuid](https://github.com/broofa/node-uuid).
		/// </summary>
		/// <returns>The guid.</returns>
		public JsString Guid()
		{
			return default(JsString);
		}

		/// <summary>
		/// Uses require.js to obtain a module. This function returns a promise which resolves with the module instance. You can pass more than one module id to this function or an array of ids. If more than one or an array is passed, then the promise will resolve with an array of module instances.
		/// </summary>
		/// <param name="moduleId">The id of the modules to load.</param>
		/// <returns>A promise for the loaded module(s).</returns>
		public Promise Acquire( string moduleId )
		{
			return default(Promise);
		}

		/// <summary>
		/// Uses require.js to obtain a module. This function returns a promise which resolves with the module instance. You can pass more than one module id to this function or an array of ids. If more than one or an array is passed, then the promise will resolve with an array of module instances.
		/// </summary>
		/// <param name="moduleIds">The ids of the modules to load.</param>
		/// <returns>A promise for the loaded module(s).</returns>
		public Promise Acquire( string[] moduleIds )
		{
			return default(Promise);
		}

		/// <summary>
		/// Extends the first object with the properties of the following objects.
		/// </summary>
		/// <param name="obj">The target object to extend.</param>
		/// <param name="extensions">The target object to extend.</param>
		public void Extend( object obj, params object[] extensions )
		{}

		/// <summary>
		/// Uses a setTimeout to wait the specified milliseconds.
		/// </summary>
		/// <param name="milliseconds">The number of milliseconds to wait.</param>
		/// <returns>The promise.</returns>
		public Promise Wait( int milliseconds )
		{
			return default(Promise);
		}

		/// <summary>
		/// Gets all the owned keys of the specified object.
		/// </summary>
		/// <param name="object">The object whose owned keys should be returned.</param>
		/// <returns>The keys.</returns>
		public JsString[] Keys( object @object )
		{
			return default(JsString[]);
		}

		/// <summary>
		/// Determines if the specified object is an html element.
		/// </summary>
		/// <param name="object">The object to check.</param>
		/// <returns>True if matches the type, false otherwise.</returns>
		public bool IsElement( object @object )
		{
			return default(bool);
		}

		/// <summary>
		/// Determines if the specified object is an array.
		/// </summary>
		/// <param name="object">The object to check.</param>
		/// <returns>True if matches the type, false otherwise.</returns>
		public bool IsArray( object @object )
		{
			return default(bool);
		}

		/// <summary>
		/// Determines if the specified object is...an object. ie. Not an array, string, etc.
		/// </summary>
		/// <param name="object">The object to check.</param>
		/// <returns>True if matches the type, false otherwise.</returns>
		public bool IsObject( object @object )
		{
			return default(bool);
		}

		/// <summary>
		/// Determines if the specified object is a boolean.
		/// </summary>
		/// <param name="object">The object to check.</param>
		/// <returns>True if matches the type, false otherwise.</returns>
		public bool IsBoolean( object @object )
		{
			return default(bool);
		}

		/// <summary>
		/// Determines if the specified object is a promise.
		/// </summary>
		/// <param name="object">The object to check.</param>
		/// <returns>True if matches the type, false otherwise.</returns>
		public bool IsPromise( object @object )
		{
			return default(bool);
		}

		/// <summary>
		/// Determines if the specified object is a function arguments object.
		/// </summary>
		/// <param name="object">The object to check.</param>
		/// <returns>True if matches the type, false otherwise.</returns>
		public bool IsArguments( object @object )
		{
			return default(bool);
		}

		/// <summary>
		/// Determines if the specified object is a function.
		/// </summary>
		/// <param name="object">The object to check.</param>
		/// <returns>True if matches the type, false otherwise.</returns>
		public bool IsFunction( object @object )
		{
			return default(bool);
		}

		/// <summary>
		/// Determines if the specified object is a string.
		/// </summary>
		/// <param name="object">The object to check.</param>
		/// <returns>True if matches the type, false otherwise.</returns>
		public bool IsString( object @object )
		{
			return default(bool);
		}

		/// <summary>
		/// Determines if the specified object is a number.
		/// </summary>
		/// <param name="object">The object to check.</param>
		/// <returns>True if matches the type, false otherwise.</returns>
		public bool IsNumber( object @object )
		{
			return default(bool);
		}

		/// <summary>
		/// Determines if the specified object is a date.
		/// </summary>
		/// <param name="object">The object to check.</param>
		/// <returns>True if matches the type, false otherwise.</returns>
		public bool IsDate( object @object )
		{
			return default(bool);
		}

		/// <summary>
		/// Determines if the specified object is a regular expression.
		/// </summary>
		/// <param name="object">The object to check.</param>
		/// <returns>True if matches the type, false otherwise.</returns>
		public bool IsRegExp( object @object )
		{
			return default(bool);
		}
	}
}