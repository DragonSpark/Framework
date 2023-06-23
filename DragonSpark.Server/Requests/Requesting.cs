﻿using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Selection;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Server.Requests;

public class Requesting<T> : Selecting<Request<T>, IActionResult>, IRequesting<T>
{
	public Requesting(ISelect<Request<T>, ValueTask<IActionResult>> @select) : base(@select) {}

	public Requesting(Func<Request<T>, ValueTask<IActionResult>> @select) : base(@select) {}
}