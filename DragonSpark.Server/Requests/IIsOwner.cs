﻿using DragonSpark.Model.Operations;

namespace DragonSpark.Server.Requests
{
	public interface IIsOwner : ISelecting<Query, bool?> {}
}