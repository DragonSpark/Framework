using DragonSpark.Model.Commands;
using System.Data.Common;
using System.Text.RegularExpressions;

namespace DragonSpark.Application.AspNet.Entities.Search;

sealed class RewriteCommand : ICommand<DbCommand>
{
	public static RewriteCommand Default { get; } = new();

	RewriteCommand() : this(new(@"\[(?<column>\w+)\]\s+LIKE\s+N'%(?<term>[^%]+)%'",
	                            RegexOptions.Compiled | RegexOptions.IgnoreCase)) {}

	readonly Regex _expression;

	public RewriteCommand(Regex expression) => _expression = expression;

	public void Execute(DbCommand parameter)
	{
		var input = parameter.CommandText;
		if (input.Contains("LIKE"))
		{
			parameter.CommandText
				= _expression.Replace(input,
				                      x =>
				                      {
					                      var column = x.Groups["column"].Value;
					                      var term   = x.Groups["term"].Value;
					                      return $"CONTAINS([{column}], '\"{term}*\"')";
				                      });
		}
	}
}