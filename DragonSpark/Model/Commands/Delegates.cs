﻿using System;
using DragonSpark.Model.Selection.Stores;

namespace DragonSpark.Model.Commands
{
	sealed class Delegates<T> : ReferenceValueStore<ICommand<T>, Action<T>>
	{
		public static Delegates<T> Default { get; } = new Delegates<T>();

		Delegates() : base(x => x.Execute) {}
	}
}