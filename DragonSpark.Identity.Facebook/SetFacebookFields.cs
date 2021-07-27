using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;
using Microsoft.AspNetCore.Authentication.Facebook;

namespace DragonSpark.Identity.Facebook
{
	public class SetFacebookFields : ICommand<FacebookOptions>
	{
		readonly Array<string> _fields;

		protected SetFacebookFields(params string[] fields) => _fields = fields;

		public void Execute(FacebookOptions parameter)
		{
			parameter.Fields.Clear();
			foreach (var field in _fields.Open())
			{
				parameter.Fields.Add(field);
			}
		}
	}
}