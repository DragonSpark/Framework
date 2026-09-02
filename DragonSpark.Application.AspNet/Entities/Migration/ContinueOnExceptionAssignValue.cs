namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class ContinueOnExceptionAssignValue : IAssignValue
{
	public static ContinueOnExceptionAssignValue Default { get; } = new();

	ContinueOnExceptionAssignValue() : this(AssignValue.Default) {}

	readonly IAssignValue _previous;

	public ContinueOnExceptionAssignValue(IAssignValue previous) => _previous = previous;

	public void Execute(AssignValueInput parameter)
	{
		try
		{
			_previous.Execute(parameter);
		}
		catch
		{
			// ignored
		}
	}
}