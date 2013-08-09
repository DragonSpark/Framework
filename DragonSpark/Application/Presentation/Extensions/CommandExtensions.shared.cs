using System.Windows.Input;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Extensions
{
	public static class CommandExtensions
	{
		public static void ExecuteWith( this ICommand target, object parameter )
		{
			target.NotNull( x => x.CanExecute( parameter ).IsTrue( () => x.Execute( parameter ) ) );
		}
	}
}