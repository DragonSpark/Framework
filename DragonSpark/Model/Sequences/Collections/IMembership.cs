using DragonSpark.Model.Commands;

namespace DragonSpark.Model.Sequences.Collections;

public interface IMembership<in T>
{
	ICommand<T> Add { get; }

	ICommand<T> Remove { get; }
}