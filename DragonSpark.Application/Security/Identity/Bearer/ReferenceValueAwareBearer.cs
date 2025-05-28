using DragonSpark.Model.Selection.Stores;
using System.Security.Claims;

namespace DragonSpark.Application.Security.Identity.Bearer;

sealed class ReferenceValueAwareBearer : ReferenceValueStore<ClaimsIdentity, string>, IBearer
{
	public ReferenceValueAwareBearer(IBearer select) : base(select) {}
}