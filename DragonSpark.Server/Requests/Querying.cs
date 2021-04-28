using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Server.Requests
{
	public class Querying<T> : Selecting<Query<T>, IActionResult>, IQuerying<T>
	{
		public Querying(ISelect<Query<T>, ValueTask<IActionResult>> select) : base(select) {}

		public Querying(Func<Query<T>, ValueTask<IActionResult>> select) : base(select) {}
	}
}