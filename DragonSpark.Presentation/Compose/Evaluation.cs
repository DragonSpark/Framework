using DragonSpark.Compose;
using Microsoft.AspNetCore.Components;
using System;

namespace DragonSpark.Presentation.Compose
{
	public sealed class Evaluation<T> where T : ComponentBase
	{
		public static Evaluation<T> Default { get; } = new Evaluation<T>();

		Evaluation() {}

		public Func<T, TValue> Using<TValue>(Func<T, TValue> select)
			=> Start.A.Selection<T>().By.Calling(select).Stores().New().Get;
	}
}