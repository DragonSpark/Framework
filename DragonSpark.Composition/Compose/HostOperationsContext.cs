﻿using DragonSpark.Model.Operations;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace DragonSpark.Composition.Compose
{
	public sealed class HostOperationsContext
	{
		readonly IOperationResult<HostBuilder, IHost> _select;

		public HostOperationsContext(IOperationResult<HostBuilder, IHost> select) => _select = select;

		public ValueTask<IHost> Run() => _select.Get(new HostBuilder());
	}
}