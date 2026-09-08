using DragonSpark.Model.Selection.Stores;
using DragonSpark.Model.Sequences;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;

namespace DragonSpark.Application.AspNet.Entities.Migration.Identity;

sealed class ModelWhere<T> : ReferenceValueStore<IEntityType, Expression<Func<T, bool>>>, IWhere<T>
{
	public ModelWhere(Array<object> keys) : base(new ComposeModelWhere<T>(keys)) {}
}