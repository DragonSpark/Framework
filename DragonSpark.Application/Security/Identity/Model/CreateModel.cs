using DragonSpark.Model.Selection;

namespace DragonSpark.Application.Security.Identity.Model;

public sealed class CreateModel : ISelect<CreateModelInput, CreateModelView>
{
	public static CreateModel Default { get; } = new();
	
	CreateModel() {}

	public CreateModelView Get(CreateModelInput parameter)
	{
		var (principal, address) = parameter;
		return new (principal.AuthenticatedIdentity(), principal.UserName(), address);
	}
}