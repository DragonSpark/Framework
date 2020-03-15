using DragonSpark.Model.Selection.Alterations;
using Microsoft.AspNetCore.Identity;

namespace DragonSpark.Application.Security.Identity.Model {
	interface IAppliedAuthentication : IAlteration<ExternalLoginInfo> {}
}