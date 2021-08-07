using AspNet.Security.OAuth.Patreon;
using DragonSpark.Model.Commands;
using DragonSpark.Model.Sequences;

namespace DragonSpark.Identity.Patreon
{
	public class SetPatreonFields : ICommand<PatreonAuthenticationOptions>
	{
		readonly Array<string> _fields;

		protected SetPatreonFields(params string[] fields) => _fields = fields;

		public void Execute(PatreonAuthenticationOptions parameter)
		{
			parameter.Fields.Clear();
			foreach (var field in _fields.Open())
			{
				parameter.Fields.Add(field);
			}
		}
	}
}