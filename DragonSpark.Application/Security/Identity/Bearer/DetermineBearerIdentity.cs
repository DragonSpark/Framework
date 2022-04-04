using DragonSpark.Model.Selection.Stores;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class DetermineBearerIdentity : ReferenceValueStore<ClaimsIdentity, ClaimsIdentity>
{
	public DetermineBearerIdentity(BearerIdentity source) : base(source) {}
}