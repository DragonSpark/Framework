﻿using DragonSpark.Model.Operations.Selection.Conditions;

namespace DragonSpark.Application.Security.Identity.Profile;

public interface IUserSynchronizer<T> : IDepending<Login<T>> where T : IdentityUser;