using DragonSpark.Model.Results;
using DragonSpark.Model.Selection.Alterations;

namespace DragonSpark.Presentation.Security.Identity
{
	public class CurrentExternalLogin : DelegatedSelection<string, string>
	{
		protected CurrentExternalLogin(IAlteration<string> select, CurrentAuthenticationMethod current)
			: base(select, current) {}
	}
}