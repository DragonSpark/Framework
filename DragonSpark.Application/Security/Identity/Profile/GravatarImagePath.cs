using DragonSpark.Model.Selection.Alterations;
using System;

namespace DragonSpark.Application.Security.Identity.Profile
{
	public sealed class GravatarImagePath : IAlteration<string>
	{
		public static GravatarImagePath Default { get; } = new GravatarImagePath();

		GravatarImagePath() : this(IdentifierHash.Default.Get) {}

		readonly Func<string, string> _hash;
		readonly string _template;

		public GravatarImagePath(Func<string, string> hash,
		                         string template = "https://www.gravatar.com/avatar/{0}.jpg")
		{
			_hash = hash;
			_template = template;
		}

		public string Get(string parameter) => string.Format(_template, _hash(parameter.Trim().ToLower()));
	}
}
