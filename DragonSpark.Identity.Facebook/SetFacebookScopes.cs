using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Authentication.Facebook;

namespace DragonSpark.Identity.Facebook
{
	public class SetFacebookScopes : ICommand<FacebookOptions>
	{
		readonly Array<string> _scopes;

		protected SetFacebookScopes(params string[] scopes) => _scopes = scopes;

		public void Execute(FacebookOptions parameter)
		{
			parameter.Scope.Clear();

			foreach (var field in _scopes.Open())
			{
				parameter.Scope.Add(field);
			}
		}
	}
}