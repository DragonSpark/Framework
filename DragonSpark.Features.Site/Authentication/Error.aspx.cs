using System;
using System.Linq;
using System.Web.Script.Serialization;

namespace DragonSpark.Features.Site.Authentication
{
	public partial class Error : System.Web.UI.Page
	{
		protected void Page_Load( object sender, EventArgs e )
		{
			DataBind();
		}

		protected ErrorDetails Details
		{
			get { return details ?? ( details = new JavaScriptSerializer().Deserialize<ErrorDetails>( Request[ "ErrorDetails" ] ) ); }
		}	ErrorDetails details;

		protected string Message
		{
			get { return message ?? ( message = string.Join( "<br/>", Details.errors.Select( er => string.Format( "Error Code {0}: {1}", er.errorCode, er.errorMessage ) ).ToArray() ) ); }
		}	string message;
	}

	public class ErrorContext
	{
		public string errorCode { get; set; }
		public string errorMessage { get; set; }
	}

	public class ErrorDetails
	{
		public string context { get; set; }
		public int httpReturnCode { get; set; }
		public string identityProvider { get; set; }
		public ErrorContext[] errors { get; set; }
	}

}