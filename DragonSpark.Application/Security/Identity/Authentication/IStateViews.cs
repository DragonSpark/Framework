﻿using DragonSpark.Model.Operations.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Authentication;

public interface IStateViews<T> : ISelecting<ClaimsPrincipal, StateView<T>> where T : IdentityUser;