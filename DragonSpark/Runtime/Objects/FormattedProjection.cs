using System;
using DragonSpark.Model.Selection;
using DragonSpark.Text;

namespace DragonSpark.Runtime.Objects
{
	class FormattedProjection<T> : Selection<T, IProjection>, IFormattedProjection<T>
	{
		public FormattedProjection(ISelect<T, IProjection> @default, params Pair<string, Func<T, IProjection>>[] pairs)
			: base(@default, pairs) {}
	}
}