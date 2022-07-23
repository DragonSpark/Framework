namespace DragonSpark.Application.Connections;

sealed class AssignSignedContent : AssignHeader
{
	public AssignSignedContent(SignToken value) : base(SignedTokenHeaderName.Default, value) {}
}