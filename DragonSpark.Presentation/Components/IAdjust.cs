using DragonSpark.Model.Selection.Alterations;
using System.Linq;

namespace DragonSpark.Presentation.Components
{
	public interface IAdjust<T> : IAlteration<IQueryable<T>> {}
}