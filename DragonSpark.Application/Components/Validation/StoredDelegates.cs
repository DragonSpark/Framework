﻿using DragonSpark.Compose;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Components.Forms;

namespace DragonSpark.Application.Components.Validation;

sealed class StoredDelegates : Select<FieldIdentifier, object>, IDelegates
{
	public StoredDelegates() : base(Start.A.Selection<FieldIdentifier>()
	                                     .By.Calling(x => x.Key())
	                                     .Select(Delegates.Default.ToStandardTable())
	                                     .Introduce()
	                                     .Select(x => x.Item2(x.Item1.Model))) {}
}