using DragonSpark.Model.Selection.Conditions;
using DragonSpark.Model.Sequences.Collections;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace DragonSpark.Composition.Compose
{
	sealed class CanRegister : Condition<Type>
	{
		public CanRegister(IServiceCollection services)
			: base(new NotHave<Type>(services.Select(x => x.ServiceType))) {}
	}
}