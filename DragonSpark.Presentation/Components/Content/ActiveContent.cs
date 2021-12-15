using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

sealed class ActiveContent<T> : Resulting<T?>, IActiveContent<T>
{
	public ActiveContent(Func<ValueTask<T?>> content) : base(content) {}
}