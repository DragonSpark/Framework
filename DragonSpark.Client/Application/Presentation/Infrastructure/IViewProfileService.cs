namespace DragonSpark.Application.Presentation.Infrastructure
{
    public interface IViewProfileService
    {
        ViewProfile SelectedProfile { get; }
        void Select( ViewProfile profile );
        void Register( ViewProfile profile );
    }
}