using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.Content;

public interface IActiveContents<T> : ISelect<Func<ValueTask<T>>, IActiveContent<T>> {}