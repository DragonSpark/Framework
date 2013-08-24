using SharpKit.Durandal.Modules;
using SharpKit.JavaScript;

namespace DragonSpark.Client
{
	/* [JsType( JsMode.Global )]
	public class DefaultClient
	{
		readonly static jQuery JQuery = new jQuery( HtmlContext.document.body );

		static void Load()
		{
			HtmlContext.alert( string.Format( "Hello {0}!", 6776 ) );
		}
	}*/

	[Module]
	class TestModule : JsContext
	{
		readonly SharpKit.Durandal.System system;
		readonly int testesAgain;
		readonly SharpKit.Durandal.System second;

		public TestModule( SharpKit.Durandal.System system, int testesAgain, SharpKit.Durandal.System second ) 
		{
			this.system = system; 
			this.testesAgain = testesAgain;
			this.second = second;
		}

		public void TestMethod()  
		{ 
			var version = system.Version; 

			var temp = system.GetModuleId( this ); 
			
			system.Noop();
		}
	}

	/*[JsType( JsMode.Prototype )]
	public class Grid : HtmlContext   
	{
		public Grid()
		{
			Rows = new JsArray<GridRow>();  
		}
		public HtmlElement Element { get; set; }
		public HtmlElement GridBody { get; set; }
		public JsArray<GridRow> Rows { get; set; }
		public void Render() 
		{
			if ( Element != null )
			{ 
				// Element.setAttribute( "_Grid", this ); 
				if ( GridBody == null || GridBody.nodeName != "TBODY" )
				{ 
					var gridBody = document.createElement( "TBODY" ).As<HtmlElement>();
					GridBody = gridBody; 
					Element.appendChild( GridBody );
				}
			}
		}
	}

	[JsType(JsMode.Json)]
	public class GridRow
	{
		public HtmlElement Element { get; set; }
		public object Data { get; set; }
	}*/

}
