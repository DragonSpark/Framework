using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Authentication.OAuth;

namespace DragonSpark.Application.Security.Identity.Authentication
{
	public class SetScopes<T> : ICommand<T> where T : OAuthOptions
	{
		readonly Array<string> _scopes;

		protected SetScopes(params string[] scopes) => _scopes = scopes;

		public void Execute(T parameter)
		{
			parameter.Scope.Clear();

			foreach (var field in _scopes.Open())
			{
				parameter.Scope.Add(field);
			}
		}
	}

}