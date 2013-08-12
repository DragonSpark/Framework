using SharpKit.Html;
using SharpKit.JavaScript;
using SharpKit.jQuery;

namespace DragonSpark.Client
{
    [JsType( JsMode.Global )]
	public class DefaultClient
	{
		readonly static jQuery JQuery = new jQuery( HtmlContext.document.body );

		static void DefaultClient_Load()
		{
			JQuery.append( "Ready<br/>" );
		}

		static void btnTest_click( DOMEvent e )
		{
			new jQuery( HtmlContext.document.body ).append( "Hello world<br/>" );
		}
	}

	[JsType( JsMode.Global )]
	class MyPage : HtmlContext
	{
		public static void Main()
		{
			var input = document.getElementById("input1").As<HtmlInputElement>();
			input.value = "MyValue";
		}
	}

	[JsType( JsMode.Prototype )]
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
	}

}
