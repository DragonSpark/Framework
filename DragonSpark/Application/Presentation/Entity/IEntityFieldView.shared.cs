using DragonSpark.Application.Communication.Entity;

namespace DragonSpark.Application.Presentation.Entity
{
	public interface IEntityFieldView
	{
		IEntitySetProfile Profile { get; }

		bool IsEditable { get; }

		bool IsVisible { get; }
		// object View { get; }
		object Model { get; }
	}
}