using SharpKit.Durandal.Modules;
using SharpKit.Html;
using SharpKit.JavaScript;
using SharpKit.jQuery;

namespace SharpKit.Durandal
{
	[JsType( JsMode.Prototype, Export = false )]
	public class CompositionTransaction
	{
		[JsMethod( Name = "complete" )]
		public void Complete( JsFunction callback )
		{}
	}

	[Module( Export = false )]
	public class Composition
	{
		/// <summary>
		/// Converts a transition name to its moduleId.
		/// </summary>
		/// <param name="name">The name of the transtion.</param>
		/// <returns>The moduleId.</returns>
		public string ConvertTransitionToModuleId( string name )
		{
			return default(string);
		}

		/// <summary>
		/// Represents the currently executing composition transaction.
		/// </summary>
		[JsProperty( NativeField = true )]
		public CompositionTransaction Current { get; set; }

		/// <summary>
		/// Registers a binding handler that will be invoked when the current composition transaction is complete.
		/// </summary>
		/// <param name="name">The name of the binding handler.</param>
		/// <param name="config">The binding handler instance. If none is provided, the name will be used to look up an existing handler which will then be converted to a composition handler.</param>
		/// <param name="initOptionsFactory">If the registered binding needs to return options from its init call back to knockout, this function will server as a factory for those options. It will receive the same parameters that the init function does.</param>
		public void AddBindingHandler( string name, object config, JsFunction initOptionsFactory )
		{}

		/// <summary>
		/// Gets an object keyed with all the elements that are replacable parts, found within the supplied elements. The key will be the part name and the value will be the element itself.
		/// </summary>
		/// <param name="element">The element to search for parts.</param>
		/// <returns>An object keyed by part.</returns>
		public object GetParts( HtmlElement element )
		{
			return default(object);
		}

		/// <summary>
		/// Gets an object keyed with all the elements that are replacable parts, found within the supplied elements. The key will be the part name and the value will be the element itself.
		/// </summary>
		/// <param name="elements">The elements to search for parts.</param>
		/// <returns>An object keyed by part.</returns>
		public object GetParts( HtmlElement[] elements )
		{
			return default(object);
		}

		/// <summary>
		/// Executes the default view location strategy.
		/// </summary>
		/// <param name="context">The composition context containing the model and possibly existing viewElements.</param>
		/// <returns>A promise for the view.</returns>
		public Promise DefaultStrategy( object context )
		{
			return default(Promise);
		}

		/// <summary>
		/// Initiates a composition.
		/// </summary>
		/// <param name="element">The DOMElement or knockout virtual element that serves as the parent for the composition.</param>
		/// <param name="settings">The composition settings.</param>
		/// <param name="bindingContext">The current binding context.</param>
		/// <param name="fromBinding"></param>
		public void Compose( HtmlElement element, object settings, object bindingContext, object fromBinding )
		{}
	}
}