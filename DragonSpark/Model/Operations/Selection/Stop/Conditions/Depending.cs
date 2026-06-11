using System;
using System.Threading.Tasks;
using DragonSpark.Model.Selection;

namespace DragonSpark.Model.Operations.Selection.Stop;

public class Depending<T> : Selection.Conditions.Depending<Stop<T>>, IDepending<T>
{
    public Depending(ISelect<Stop<T>, ValueTask<bool>> select) : base(select) {}

    public Depending(Func<Stop<T>, ValueTask<bool>> select) : base(select) {}
}