using System.Web.Mvc;

namespace DragonSpark.Application.Server.Controllers
{
	public class WelcomeController : Controller
	{
		readonly ApplicationDetails applicationDetails;

		public WelcomeController( ApplicationDetails applicationDetails )
		{
			this.applicationDetails = applicationDetails;
		}

		public ActionResult Default()
		{
			var result = View( applicationDetails );
			return result;
		}
	}
}