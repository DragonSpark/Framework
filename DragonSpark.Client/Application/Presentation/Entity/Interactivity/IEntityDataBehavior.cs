namespace DragonSpark.Application.Presentation.Entity.Interactivity
{
    public interface IEntityDataBehavior
	{
		void Attach( object entity );
		void Detach();
	}
}