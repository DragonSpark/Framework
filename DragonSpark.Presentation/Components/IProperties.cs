﻿using DragonSpark.Model.Operations;
using DragonSpark.Model.Selection;
using DragonSpark.Model.Sequences;
using System;

namespace DragonSpark.Presentation.Components
{
	public interface IProperties : ISelect<Type, Array<Func<ComponentBase, IOperation>>> {}
}