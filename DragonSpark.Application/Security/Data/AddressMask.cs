using DragonSpark.Model.Selection.Alterations;
using System.Text.RegularExpressions;

namespace DragonSpark.Application.Security.Data
{
	public sealed class AddressMask : IAlteration<string>
	{
		public static AddressMask Default { get; } = new AddressMask();

		AddressMask() : this(@"(?<=[\w]{1})[\w-\._\+%\\]*(?=[\w]{1}@)|(?<=@[\w]{1})[\w-_\+%]*(?=\.)") {}

		readonly string _pattern;

		public AddressMask(string pattern) => _pattern = pattern;

		public string Get(string parameter)
			=> parameter.Contains("@")
				   ? parameter.Split('@')[0].Length < 4
					     ? @"*@*.*"
					     : Regex.Replace(parameter, _pattern, m => new string('*', m.Length))
				   : new string('*', parameter.Length);
	}
}