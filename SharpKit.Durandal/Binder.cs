using SharpKit.Durandal.Modules;
using SharpKit.Html;
using SharpKit.JavaScript;

namespace SharpKit.Durandal
{
	[Module( Export = false )]
	public class Binder
	{
		/// <summary>
		/// Called before every binding operation. Does nothing by default.
		/// </summary>
		/// <param name="data">The data that is about to be bound.</param>
		/// <param name="view">The view that is about to be bound.</param>
		/// <param name="instruction">The object that carries the binding instructions.</param>
		public void BeforeBind( object data, HtmlElement view, object instruction )
		{}

		/// <summary>
		/// Called after every binding operation. Does nothing by default.
		/// </summary>
		/// <param name="data">The data that has just been bound.</param>
		/// <param name="view">The view that has just been bound.</param>
		/// <param name="instruction">The object that carries the binding instructions.</param>
		public void AfterBind( object data, HtmlElement view, object instruction )
		{}

		/// <summary>
		/// Indicates whether or not the binding system should throw errors or not.
		/// 
		/// The binding system will not throw errors by default. Instead it will log them.
		/// </summary>
		[JsProperty( NativeField = false )]
		public bool ThrowOnErrors { get; set; }

		/// <summary>
		/// Gets the binding instruction that was associated with a view when it was bound.
		/// </summary>
		/// <param name="view">The view that was previously bound.</param>
		/// <returns>The object that carries the binding instructions.</returns>
		public object GetBindingInstruction( HtmlElement view )
		{
			return default(object);
		}

		/// <summary>
		/// Binds the view, preserving the existing binding context. Optionally, a new context can be created, parented to the previous context.
		/// </summary>
		/// <param name="bindingContext">The current binding context.</param>
		/// <param name="view">The view to bind.</param>
		/// <param name="obj">The data to bind to, causing the creation of a child binding context if present.</param>
		/// <returns></returns>
		public object BindContext( object bindingContext, HtmlElement view, object obj )
		{
			return default(object);
		}

		/// <summary>
		/// Binds the view, preserving the existing binding context. Optionally, a new context can be created, parented to the previous context.
		/// </summary>
		/// <param name="obj">The data to bind to.</param>
		/// <param name="view">The view to bind.</param>
		/// <returns></returns>
		public object Bind( object obj, HtmlElement view )
		{
			return default(object);
		}
	}
}