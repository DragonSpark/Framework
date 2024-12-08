﻿using DragonSpark.Model.Selection;
using System.Security.Claims;

namespace DragonSpark.Application.AspNet.Security.Identity.Claims.Access;

public interface IReadClaim : ISelect<ClaimsPrincipal, Accessed>;