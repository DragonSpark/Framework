
using DragonSpark.Application.Communication.Entity;

namespace DragonSpark.Application.Presentation.Entity
{
	public class EntityFieldView : IEntityFieldView
	{
		public EntityFieldView( IEntitySetProfile profile, object model, /*object view,*/ bool isVisible, bool isEditable )
		{
			Profile = profile;
			// View = view;
			Model = model;
			IsVisible = isVisible;
			IsEditable = isEditable;
		}

		public IEntitySetProfile Profile { get; private set; }

		public object Model { get; private set; }

		/*public object View { get; private set; }*/

		public bool IsVisible { get; private set; }
		
		public bool IsEditable { get; private set; }
		
		
		

		
	}
}