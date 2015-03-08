using DragonSpark.Application.Presentation.Navigation;

namespace DragonSpark.Application.Presentation.Infrastructure
{
	public interface IViewValidator
	{
		void Validate( ViewValidationContext context );
	}
}