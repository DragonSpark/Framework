using SharpKit.Durandal.Modules;
using SharpKit.JavaScript;

namespace SharpKit.Durandal.Plugins
{
	[Module( Export = false )]
	public class Serializer
	{
		/// <summary>
		/// The name of the attribute that the serializer should use to identify an object's type.
		/// </summary>
		[JsProperty( NativeField = true )]
		public string TypeAttribute { get; set; }

		/// <summary>
		/// The route configs that are registered.
		/// </summary>
		[JsProperty( NativeField = true )]
		public object Space { get; set; }

		/// <summary>
		/// The default replacer function used during serialization. By default properties starting with '_' or '$' are removed from the serialized object.
		/// </summary>
		/// <param name="key">The object key to check.</param>
		/// <param name="value">value to check.</param>
		/// <returns>The value to serialize.</returns>
		public object Replacer( string key, object value )
		{
			return default(object);
		}

		/// <summary>
		/// Serializes the object.
		/// </summary>
		/// <param name="object">The object to serialize.</param>
		/// <param name="settings">Settings can specify a replacer or space to override the serializer defaults.</param>
		/// <returns>The JSON string.</returns>
		public string Serialize( object @object, object settings )
		{
			return default(string);
		}

		/// <summary>
		/// Gets the type id for an object instance, using the configured `typeAttribute`.
		/// </summary>
		/// <param name="object">The object to serialize.</param>
		/// <returns>The type.</returns>
		public string GetTypeId( object @object )
		{
			return default(string);
		}

		/// <summary>
		/// Maps type ids to object constructor functions. Keys are type ids and values are functions.
		/// </summary>
		[JsProperty( NativeField = true )]
		public JsObject TypeMap { get; set; }

		/// <summary>
		/// Adds a type id/constructor function mampping to the `typeMap`.
		/// </summary>
		/// <param name="typeId">The type id.</param>
		/// <param name="constructor">The constructor.</param>
		public void RegisterType( string typeId, JsFunction constructor )
		{}

		/// <summary>
		/// The default reviver function used during deserialization. By default is detects type properties on objects and uses them to re-construct the correct object using the provided constructor mapping.
		/// </summary>
		/// <param name="key">The attribute key.</param>
		/// <param name="value">The object value associated with the key.</param>
		/// <param name="getTypeId">A custom function used to get the type id from a value.</param>
		/// <param name="getConstructor">A custom function used to get the constructor function associated with a type id.</param>
		/// <returns></returns>
		public object Reviver( string key, object value, JsFunction getTypeId, object getConstructor )
		{
			return default(object);
		}

		/// <summary>
		/// Deserialize the JSON.
		/// </summary>
		/// <param name="string">The JSON string.</param>
		/// <param name="settings">Settings can specify a reviver, getTypeId function or getConstructor function.</param>
		/// <returns>The JSON string.</returns>
		public string Deserialize( string @string, JsFunction settings )
		{
			return default(string);
		}
	}
}