﻿using DragonSpark.Model.Commands;

namespace DragonSpark.Application.AspNet.Entities.Editing;

public sealed class AttachLocal<T> : Command<Edit<T>>, IModify<T> where T : class
{
	public static AttachLocal<T> Default { get; } = new();

	AttachLocal() : base(x => x.Attach(x.Subject)) {}
}