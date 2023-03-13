namespace DragonSpark.Application.Entities.Editing;

public class AttachMany<T> : ModifyMany<T> where T : class
{
	protected AttachMany(SessionEditors editors) : base(editors, AttachLocal<T>.Default) {}
}

public class AttachMany : AttachMany<object>
{
	public AttachMany(SessionEditors editors) : base(editors) {}
}