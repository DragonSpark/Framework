using SharpKit.Durandal.Modules;
using SharpKit.Html;
using SharpKit.JavaScript;
using SharpKit.jQuery;

namespace SharpKit.Durandal.Plugins
{
	[Module( Export = false )]
	public class Dialog
	{
		[JsType( JsMode.Prototype, Export = false )]
		public class DialogContext
		{
			[JsProperty( Name = "removeDelay", NativeField = true )]
			public int RemoveDelay { get; set; }

			[JsProperty( Name = "blockoutOpacity", NativeField = true )]
			public double BlockoutOpacity { get; set; } 

			/// <summary>
			/// In this function, you are expected to add a DOM element to the tree which will serve as the "host" for the modal's composed view. You must add a property called host to the modalWindow object which references the dom element. It is this host which is passed to the composition module.
			/// </summary>
			/// <param name="theDialog">The dialog model.</param>
			[JsMethod( Name = "addHost" )]
			public void AddHost( Dialog theDialog )
			{}

			/// <summary>
			/// This function is expected to remove any DOM machinery associated with the specified dialog and do any other necessary cleanup.
			/// </summary>
			/// <param name="theDialog">The dialog model.</param>
			[JsMethod( Name = "removeHost" )]
			public void RemoveHost( Dialog theDialog )
			{}

			/// <summary>
			/// This function is called after the modal is fully composed into the DOM, allowing your implementation to do any final modifications, such as positioning or animation. You can obtain the original dialog object by using `getDialog` on context.model.
			/// </summary>
			/// <param name="child">The dialog view.</param>
			/// <param name="parent">The parent view.</param>
			/// <param name="context">The composition context.</param>
			[JsMethod( Name = "compositionComplete" )]
			public void CompositionComplete( HtmlElement child, HtmlElement parent, object context )
			{}
		}

		[JsType( JsMode.Prototype, Export = false )]
		public class MessageBox
		{
			/// <summary>
			/// Models a message box's message, title and options.
			/// </summary>
			/// <param name="message"></param>
			/// <param name="title"></param>
			/// <param name="options"></param>
			public MessageBox( string message, string title, string[] options )
			{}

			/// <summary>
			/// Selects an option and closes the message box, returning the selected option through the dialog system's promise.
			/// </summary>
			/// <param name="dialogResult">The result to select.</param>
			[JsMethod( Name = "selectOption" )]
			public void SelectOption( string dialogResult )
			{}

			/// <summary>
			/// Provides the view to the composition system.
			/// </summary>
			/// <returns>The view of the message box.</returns>
			[JsMethod( Name = "getView" )]
			public HtmlElement GetView()
			{
				return default(HtmlElement);
			}

			/// <summary>
			/// The title to be used for the message box if one is not provided.
			/// </summary>
			[JsProperty( NativeField = true, Name = "defaultTitle" )]
			public static string DefaultTitle { get { return default (string); } }

			/// <summary>
			/// The options to display in the message box of none are specified.
			/// </summary>
			[JsProperty( NativeField = true, Name = "defaultOptions" )]
			public static string[] DefaultOptions { get { return default(string[]); } }

			/// <summary>
			/// The markup for the message box's view.
			/// </summary>
			[JsProperty( NativeField = true, Name = "defaultViewMarkup" )]
			public static string DefaultViewMarkup { get { return default(string); } }
		}

		/// <summary>
		/// The constructor function used to create message boxes.
		/// </summary>
		[JsProperty( Name = "MessageBox" )]
		public JsFunc<MessageBox> CreateMessageBox { get { return null; } }

		/// <summary>
		/// The css zIndex that the last dialog was displayed at.
		/// </summary>
		[JsProperty( NativeField = true )]
		public int CurrentZIndex { get { return default(int); } }

		/// <summary>
		/// Gets the next css zIndex at which a dialog should be displayed.
		/// </summary>
		[JsProperty( NativeField = true )]
		public int GetNextZIndex { get { return default(int); } }

		/// <summary>
		/// Determines whether or not there are any dialogs open.
		/// </summary>
		/// <returns>True if a dialog is open. false otherwise.</returns>
		public bool IsOpen()
		{
			return default(bool);
		}

		/// <summary>
		/// Gets the dialog context by name or returns the default context if no name is specified.
		/// </summary>
		/// <param name="name">The name of the context to retrieve.</param>
		/// <returns>The context.</returns>
		public DialogContext GetContext( string name )
		{
			return default(DialogContext);
		}

		/// <summary>
		/// Adds (or replaces) a dialog context.
		/// </summary>
		/// <param name="name">The name of the context to add.</param>
		/// <param name="dialogContext">The context to add.</param>
		public void AddContext( string name, DialogContext dialogContext )
		{}

		/// <summary>
		/// Gets the dialog model that is associated with the specified object.
		/// </summary>
		/// <param name="obj">The object for whom to retrieve the dialog.</param>
		/// <returns>The dialog model.</returns>
		public Dialog GetDialog( object obj )
		{
			return default(Dialog);
		}

		/// <summary>
		/// Closes the dialog associated with the specified object.
		/// </summary>
		/// <param name="obj">The object whose dialog should be closed.</param>
		public void Close( object obj )
		{}

		/// <summary>
		/// Shows a dialog.
		/// </summary>
		/// <param name="obj">The object (or moduleId) to display as a dialog.</param>
		/// <param name="activationData">The data that should be passed to the object upon activation.</param>
		/// <param name="context">The name of the dialog context to use. Uses the default context if none is specified.</param>
		/// <returns>A promise that resolves when the dialog is closed and returns any data passed at the time of closing.</returns>
		public Promise Show( object obj, object activationData, string context )
		{
			return default(Promise);
		}

		/// <summary>
		/// Shows a dialog.
		/// </summary>
		/// <param name="moduleId">The moduleId to display as a dialog.</param>
		/// <param name="activationData">The data that should be passed to the object upon activation.</param>
		/// <param name="context">The name of the dialog context to use. Uses the default context if none is specified.</param>
		/// <returns>A promise that resolves when the dialog is closed and returns any data passed at the time of closing.</returns>
		public Promise Show( string moduleId, object activationData, string context )
		{
			return default(Promise);
		}

		/// <summary>
		/// Shows a message box.
		/// </summary>
		/// <param name="message">The message to display in the dialog.</param>
		/// <param name="title">The title message.</param>
		/// <param name="options">The options to provide to the user.</param>
		/// <returns>A promise that resolves when the message box is closed and returns the selected option.</returns>
		public Promise ShowMessage( string message, string title, string[] options )
		{
			return default(Promise);
		}

		/// <summary>
		/// Installs this module into Durandal. Adds `app.showDialog` and `app.showMessage` convenience methods.
		/// </summary>
		/// <param name="config">Add a `messageBox` property to supply a custom message box constructor. Add a `messageBoxView` property to supply custom view markup for the built-in message box.</param>
		public Promise Install( object config )
		{
			return default(Promise);
		}
	}
}
