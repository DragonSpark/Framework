namespace DragonSpark.Application.AspNet.Entities.Migration;

sealed class AssignValue : IAssignValue
{
	public static AssignValue Default { get; } = new();

	AssignValue() {}

	public void Execute(AssignValueInput parameter)
	{
		var (source, destination) = parameter;
		destination.CurrentValue  = source;
	}
}