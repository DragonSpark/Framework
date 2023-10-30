namespace DragonSpark.Application.Connections;

sealed class AssignSignedContent : AssignHeader, IAssignSignedContent
{
	public AssignSignedContent(SignToken value) : base(SignedTokenHeaderName.Default, value) {}
}