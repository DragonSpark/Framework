namespace DragonSpark.Model.Commands;

public interface ICommand : ICommand<None> {}

public interface ICommand<in T>
{
	void Execute(T parameter);
}