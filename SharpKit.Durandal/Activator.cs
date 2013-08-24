using SharpKit.Durandal.Modules;
using SharpKit.JavaScript;
using SharpKit.jQuery;
using SharpKit.KnockoutJs;

namespace SharpKit.Durandal
{
	[JsType( JsMode.Json, Export = false )]
	public class ActivatorSettings
	{
		/// <summary>
		/// The default value passed to an object's deactivate function as its close parameter.
		/// </summary>
		[JsProperty( NativeField = true, Name = "closeOnDeactivate" )]
		public bool CloseOnDeactivate { get; set; }
				
		/// <summary>
		/// Lower-cased words which represent a truthy value.
		/// </summary>
		[JsProperty( NativeField = true, Name = "affirmations" )]
		public string[] Affirmations { get; set; }

		/// <summary>
		/// Interprets the response of a `canActivate` or `canDeactivate` call using the known affirmative values in the `affirmations` array.
		/// </summary>
		/// <param name="value"></param>
		/// <returns></returns>
		[JsMethod( Name = "interpretResponse" )]
		public bool InterpretResponse( object value )
		{
			return default(bool);
		}

		/// <summary>
		/// Determines whether or not the current item and the new item are the same.
		/// </summary>
		/// <param name="currentItem"></param>
		/// <param name="newItem"></param>
		/// <param name="currentActivationData"></param>
		/// <param name="newActivationData"></param>
		/// <returns></returns>
		[JsMethod( Name = "areSameItem" )]
		public bool AreSameItem( object currentItem, object newItem, object currentActivationData, object newActivationData )
		{
			return default(bool);
		}

		/// <summary>
		/// Called immediately before the new item is activated.
		/// </summary>
		/// <param name="newItem">newItem</param>
		[JsMethod( Name = "beforeActivate" )]
		public void BeforeActivate( object newItem )
		{}

		/// <summary>
		/// Called immediately after the old item is deactivated.
		/// </summary>
		/// <param name="oldItem">The previous item.</param>
		/// <param name="close">Whether or not the previous item was closed.</param>
		/// <param name="setter">The activate item setter function.</param>
		[JsMethod( Name = "afterDeactivate" )]
		public void AfterDeactivate( object oldItem, bool close, JsFunction setter )
		{}
	}

	[JsType( JsMode.Json, Export = false )]
	public class Activator : DependentObservable<JsObject>
	{
		/// <summary>
		/// The settings for this activator.
		/// </summary>
		[JsProperty( Name = "settings", NativeField =  true )]
		public ActivatorSettings Settings { get; set; }

		/// <summary>
		/// An observable which indicates whether or not the activator is currently in the process of activating an instance.
		/// </summary>
		[JsProperty( Name = "isActivating",  NativeField =  true )]
		public Observable<bool> IsActivating { get; set; }

		/// <summary>
		/// Determines whether or not the specified item can be deactivated.
		/// </summary>
		/// <param name="item">The item to check.</param>
		/// <param name="close">Whether or not to check if close is possible.</param>
		/// <returns></returns>
		[JsMethod( Name = "canDeactivateItem" )]
		public Promise CanDeactivateItem( object item, bool close )
		{
			return default(Promise);
		}

		/// <summary>
		/// Deactivates the specified item.
		/// </summary>
		/// <param name="item">The item to deactivate.</param>
		/// <param name="close">Whether or not to close the item.</param>
		/// <returns></returns>
		[JsMethod( Name = "deactivateItem" )]
		public Promise DeactivateItem( object item, bool close )
		{
			return default(Promise);
		}

		/// <summary>
		/// Determines whether or not the specified item can be activated.
		/// </summary>
		/// <param name="newItem">The item to check.</param>
		/// <param name="activationData">Data associated with the activation.</param>
		/// <returns></returns>
		[JsMethod( Name = "canActivateItem" )]
		public Promise CanActivateItem( object newItem, object activationData )
		{
			return default(Promise);
		}

		/// <summary>
		/// Determines whether or not the specified item can be activated.
		/// </summary>
		/// <param name="newItem">The item to check.</param>
		/// <param name="activationData">Data associated with the activation.</param>
		/// <returns></returns>
		[JsMethod( Name = "activateItem" )]
		public Promise ActivateItem( object newItem, object activationData )
		{
			return default(Promise);
		}

		/// <summary>
		/// Determines whether or not the activator, in its current state, can be activated.
		/// </summary>
		/// <returns>Promise</returns>
		[JsMethod( Name = "canActivate" )]
		public Promise CanActivate()
		{
			return default(Promise);
		}

		/// <summary>
		/// Activates the activator, in its current state.
		/// </summary>
		/// <returns></returns>
		[JsMethod( Name = "activate" )]
		public Promise Activate()
		{
			return default(Promise);
		}

		/// <summary>
		/// Determines whether or not the activator, in its current state, can be deactivated.
		/// </summary>
		/// <param name="close"></param>
		/// <returns></returns>
		[JsMethod( Name = "canDeactivate" )]
		public Promise CanDeactivate( bool close )
		{
			return default(Promise);
		}

		/// <summary>
		/// Deactivates the activator, in its current state.
		/// </summary>
		/// <param name="close"></param>
		/// <returns></returns>
		[JsMethod( Name = "deactivate" )]
		public Promise Deactivate( bool close )
		{
			return default(Promise);
		}
	}

	/// <summary>
	/// An activator is a read/write computed observable that enforces the activation lifecycle whenever changing values.
	/// </summary>
	[Module( Export = false )]
	public class ActivatorModule
	{
		[JsProperty( NativeField =  true )]
		public ActivatorSettings Defaults { get; set; }

		/// <summary>
		/// Creates a new activator.
		/// </summary>
		/// <param name="initialActiveItem">The item which should be immediately activated upon creation of the ativator.</param>
		/// <param name="settings">Per activator overrides of the default activator settings.</param>
		/// <returns></returns>
		public Activator Create( object initialActiveItem, ActivatorSettings settings )
		{
			return default(Activator);
		}

		/// <summary>
		/// Determines whether or not the provided object is an activator or not.
		/// </summary>
		/// <param name="object">Any object you wish to verify as an activator or not.</param>
		/// <returns>True if the object is an activator; false otherwise.</returns>
		public bool IsActivator( object @object )
		{
			return default(bool);
		}
	}
}