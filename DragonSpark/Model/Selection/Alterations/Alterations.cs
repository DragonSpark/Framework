namespace DragonSpark.Model.Selection.Alterations;

public class Alterations<T> : Aggregate<IAlteration<T>, T>
{
	public Alterations(params IAlteration<T>[] items) : base(items) {}
}