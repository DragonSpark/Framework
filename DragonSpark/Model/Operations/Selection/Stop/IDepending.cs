using DragonSpark.Model.Selection;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Model.Operations.Selection.Stop;

public interface IDepending<T> : Conditions.IDepending<Stop<T>>;

public class Depending<T> : Conditions.Depending<Stop<T>>, IDepending<T>
{
	public Depending(ISelect<Stop<T>, ValueTask<bool>> select) : base(select) {}

	public Depending(Func<Stop<T>, ValueTask<bool>> select) : base(select) {}
}

public interface IDepending : IDepending<None>;
