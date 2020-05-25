﻿using DragonSpark.Model.Operations;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Model {
	interface IExternalSignin : ISelecting<ExternalLoginInfo, SignInResult> {}
}