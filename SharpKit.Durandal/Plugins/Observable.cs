using SharpKit.Durandal.Modules;
using SharpKit.JavaScript;
using SharpKit.KnockoutJs;

namespace SharpKit.Durandal.Plugins
{
	/// <summary>
	/// Enables automatic observability of plain javascript object for ES5 compatible browsers. Also, converts promise properties into observables that are updated when the promise resolves.
	/// You can call observable(obj, propertyName) to get the observable function for the specified property on the object.
	/// </summary>
	[Module( Export = false )]
	public class Observable<T> : KnockoutJs.Observable<T>
	{
		public Observable( object obj, string propertyName )
		{}

		/// <summary>
		/// Converts a normal property into an observable property using ES5 getters and setters.
		/// </summary>
		/// <param name="obj">The target object on which the property to convert lives.</param>
		/// <param name="propertyName">The name of the property to convert.</param>
		/// <param name="original">The original value of the property. If not specified, it will be retrieved from the object.</param>
		/// <returns>The underlying observable.</returns>
		public Observable<T> ConvertProperty( object obj, string propertyName, object original )
		{
			return default(Observable<T>);
		}

		/// <summary>
		/// Defines a computed property using ES5 getters and setters.
		/// </summary>
		/// <param name="obj">The target object on which to create the property.</param>
		/// <param name="propertyName">The name of the property to define.</param>
		/// <param name="evaluator">The Knockout computed function.</param>
		/// <returns>The underlying computed observable.</returns>
		public Observable<T> DefineProperty( object obj, string propertyName, JsFunc<T> evaluator )
		{
			return default(Observable<T>);
		}
		
		/// <summary>
		/// Defines a computed property using ES5 getters and setters.
		/// </summary>
		/// <param name="obj">The target object on which to create the property.</param>
		/// <param name="propertyName">The name of the property to define.</param>
		/// <param name="options">The computed options object.</param>
		/// <returns>The underlying computed observable.</returns>
		public DependentObservable<T> DefineProperty( object obj, string propertyName, DependentObservableOptions<T> options )
		{
			return default(DependentObservable<T>);
		}
	}
}