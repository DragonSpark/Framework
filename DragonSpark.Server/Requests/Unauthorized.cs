using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Server.Requests;

public class Unauthorized<T> : IRequesting<T>
{
	readonly Template _template;
	readonly string   _type;

	protected Unauthorized(Template template, string type)
	{
		_template = template;
		_type     = type;
	}

	public ValueTask<IActionResult> Get(Request<T> parameter)
	{
		var (owner, (name, identity, _)) = parameter;
		_template.Execute(name, _type, identity);
		var result = owner.Unauthorized().ToOperation<IActionResult>();
		return result;
	}

	public sealed class Template : LogWarning<uint?, string, Guid>
	{
		public Template(ILogger<Template> logger)
			: base(logger, "User '{User}' does not have access to '{Type}' with identity #'{Identity}'.") {}
	}
}