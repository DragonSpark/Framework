using DragonSpark.Compose;
using DragonSpark.Model.Properties;
using DragonSpark.Model.Selection.Stores;
using System.Security.Claims;

namespace DragonSpark.Application.Security
{
	public class IdentityProperty<T> : DecoratedTable<ClaimsPrincipal, T>, IProperty<ClaimsPrincipal, T>
		where T : class
	{
		public IdentityProperty() : base(Name.Default.Then().Intern().Table<T>().Get()) {}
	}
}