﻿using DragonSpark.Web.Security;
using Microsoft.Web.WebPages.OAuth;
using System.Web.Mvc;

namespace DragonSpark.Application.Controllers
{
    public class SecurityController : Controller
    {
	    readonly ApplicationDetails applicationDetails;
	    readonly IAuthenticationResultProcessor processor;

	    public SecurityController( ApplicationDetails applicationDetails, IAuthenticationResultProcessor processor )
	    {
		    this.applicationDetails = applicationDetails;
		    this.processor = processor;
	    }

	    [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
		public ActionResult Login( string provider, string returnUrl )
		{
			return new ExternalLoginResult( provider, Url.Action( "ExternalLoginCallback", new { ReturnUrl = returnUrl } ) );
		}

		public ActionResult Default()
		{
			var result = View( applicationDetails );
			return result;
		}

		public ActionResult Complete()
        {
			var result = View( applicationDetails );
			return result;
        }

		[AllowAnonymous]
		public ActionResult ExternalLoginCallback( string returnUrl )
		{
			var authentication = OAuthWebSecurity.VerifyAuthentication( Url.Action( "ExternalLoginCallback", new { ReturnUrl = returnUrl } ) );
			var process = processor.Process( authentication );
			var result = RedirectToAction( process ? "Complete" : "Error" );
			return result;
		}

		[AllowAnonymous, ChildActionOnly]
		public ActionResult ExternalLoginsList( string returnUrl )
		{
			ViewBag.ReturnUrl = returnUrl;
			return PartialView( "_ExternalLoginsListPartial", OAuthWebSecurity.RegisteredClientData );
		}

        [AllowAnonymous]
        public ActionResult Error()
        {
	        var result = View( applicationDetails );
	        return result;
        }

		internal class ExternalLoginResult : ActionResult
		{
			public ExternalLoginResult( string provider, string returnUrl )
			{
				Provider = provider;
				ReturnUrl = returnUrl;
			}

			public string Provider { get; private set; }
			public string ReturnUrl { get; private set; }

			public override void ExecuteResult( ControllerContext context )
			{
				OAuthWebSecurity.RequestAuthentication( Provider, ReturnUrl );
			}
		}
    }
}
