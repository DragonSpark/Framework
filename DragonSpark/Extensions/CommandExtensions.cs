using DragonSpark.Runtime;
using System.Collections.Generic;
using System.Linq;
using ICommand = System.Windows.Input.ICommand;

namespace DragonSpark.Extensions
{
	public static class CommandExtensions
	{
		public static IEnumerable<T> ExecuteWith<T>( this IEnumerable<T> @this, object parameter ) where T : ICommand
		{
			var result = @this.Select( x => x.ExecuteWith( parameter ) ).NotNull().ToArray();
			return result;
		}

		public static void Run<T, TParameter>( this T @this, TParameter parameter ) where T : ICommand<TParameter> => ExecuteWith<T>( @this, parameter );

		public static T ExecuteWith<T, TParameter>( this T @this, TParameter parameter ) where T : ICommand<TParameter> 
			=> ExecuteWith<T>( @this, parameter );

		public static T ExecuteWith<T>( this T @this, object parameter ) where T : ICommand
		{
			var result = @this.CanExecute( parameter ) ? @this : default(T);
			result.With( x => x.Execute( parameter ) );
			return result;
		}
	}
}
