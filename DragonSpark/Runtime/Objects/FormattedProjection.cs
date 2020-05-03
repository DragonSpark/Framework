using DragonSpark.Model;
using DragonSpark.Model.Selection;
using DragonSpark.Text;
using System;

namespace DragonSpark.Runtime.Objects
{
	class FormattedProjection<T> : Selection<T, IProjection>, IFormattedProjection<T> where T : notnull
	{
		public FormattedProjection(ISelect<T, IProjection> @default, params Pair<string, Func<T, IProjection>>[] pairs)
			: base(@default, pairs) {}
	}
}