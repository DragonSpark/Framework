using System.Collections.Generic;
using System.Linq;
using DragonSpark.Runtime;
using ICommand = System.Windows.Input.ICommand;

namespace DragonSpark.Extensions
{
	public static class CommandExtensions
	{
		public static IEnumerable<T> Apply<T>( this IEnumerable<T> @this, object parameter ) where T : ICommand
		{
			var result = @this.Select( x => x.Apply( parameter ) ).NotNull().ToArray();
			return result;
		}

		public static void Apply<T, TParameter>( this T @this, TParameter parameter ) where T : ICommand<TParameter> => Apply<T>( @this, parameter );

		public static T Apply<T>( this T @this, object parameter ) where T : ICommand
		{
			var result = @this.CanExecute( parameter ) ? @this : default(T);
			result.With( x => x.Execute( parameter ) );
			return result;
		}
	}
}
