using Microsoft.AspNetCore.Mvc;
using System;

namespace DragonSpark.Server.Requests;

public readonly record struct Query(ControllerBase Owner, Guid Subject);

public readonly record struct Query<T>(ControllerBase Owner, T Subject);