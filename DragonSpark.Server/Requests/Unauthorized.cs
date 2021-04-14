using DragonSpark.Compose;
using DragonSpark.Diagnostics.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Server.Requests
{
	public class Unauthorized<T> : IRequesting<T>
	{
		readonly Template _template;
		readonly string   _type;

		public Unauthorized(Template template, string type)
		{
			_template = template;
			_type     = type;
		}

		public ValueTask<IActionResult> Get(Request<T> parameter)
		{
			var (owner, (userName, identity, _)) = parameter;
			_template.Execute(userName, _type, identity);
			var result = owner.Unauthorized().ToOperation<IActionResult>();
			return result;
		}

		public sealed class Template : LogWarning<string, string, Guid>
		{
			public Template(ILogger<Unauthorized<T>> logger)
				: base(logger, "User '{UserName}' does not have access to '{Type}' with identity #'{Identity}'.") {}
		}
	}
}