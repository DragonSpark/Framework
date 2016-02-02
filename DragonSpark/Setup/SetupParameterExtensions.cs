using System.Threading.Tasks;

namespace DragonSpark.Setup
{
	public static class SetupParameterExtensions
	{
		/*public static TItem AsRegistered<TItem>( this ISetupParameter @this, TItem item )
		{
			@this.Register( item );
			return item;
		}*/

		/*public static TItem AsRegisteredDisposal<TItem>( this ISetupParameter @this, TItem item ) where TItem : IDisposable
		{
			@this.RegisterForDispose( item );
			return item;
		}*/

		// public static TItem Item<TItem>( this ISetupParameter @this ) => @this.Items.OfType<TItem>().SingleOrDefault();

		/*public static T GetArguments<T>( this ISetupParameter @this )
		{
			var arguments = @this.GetArguments();
			arguments.GetType().Adapt().GuardAsAssignable<T>( nameof(arguments) );

			var result = (T)arguments;
			return result;
		}*/

		/*public static T Monitor<T>( this ISetupParameter @this, T task ) where T : Task
		{
			@this.Monitor( task );
			return task;
		}*/
	}
}