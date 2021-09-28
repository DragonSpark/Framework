namespace DragonSpark.Model.Results
{
	public readonly record struct Assignment<T>(T Value, bool Assigned);
}