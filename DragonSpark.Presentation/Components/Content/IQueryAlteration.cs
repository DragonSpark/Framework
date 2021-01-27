using DragonSpark.Model.Selection;
using System.Linq;

namespace DragonSpark.Presentation.Components.Content
{
	public interface IQueryAlteration<T> : ISelect<QueryParameter<T>, IQueryable<T>> {}
}