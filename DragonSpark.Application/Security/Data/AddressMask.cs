using DragonSpark.Model.Selection.Alterations;
using System.Text.RegularExpressions;

namespace DragonSpark.Application.Security.Data
{
	// ATTRIBUTION: https://stackoverflow.com/a/47637410/10340424
	public sealed class AddressMask : IAlteration<string>
	{
		public static AddressMask Default { get; } = new AddressMask();

		AddressMask() : this(@"(?:(?:^|(?<=@))([^.@])|\G(?!\A))[^.@](?:([^.@])(?=[.@]))?", @"$1*$2") {}

		readonly string _pattern, _substitution;

		public AddressMask(string pattern, string substitution)
		{
			_pattern      = pattern;
			_substitution = substitution;
		}

		public string Get(string parameter)
			=> parameter.Contains("@")
				   ? parameter.Split('@')[0].Length < 4
					     ? @"*@*.*"
					     : Regex.Replace(parameter, _pattern, _substitution)
				   : new string('*', parameter.Length);
	}
}