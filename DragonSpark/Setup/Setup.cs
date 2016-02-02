using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.Runtime.Values;

namespace DragonSpark.Setup
{
	/*public class RegisterItemsCommand : Command<IEnumerable<object>>
	{
		readonly RegisterAllClassesCommand<OnlyIfNotRegistered> register;

		public RegisterItemsCommand( [Required]RegisterAllClassesCommand<OnlyIfNotRegistered> register )
		{
			this.register = register;
		}

		protected override void OnExecute( IEnumerable<object> parameter ) => parameter.Each( register.ExecuteWith );
	}*/

	public abstract class Setup<T> : CompositeCommand<T>, ISetup<T>
	{
		public Collection<object> Items { get; } = new Collection<object>();

		protected override void OnExecute( T parameter )
		{
			using ( new AmbientContextCommand<ITaskMonitor>().ExecuteWith( new TaskMonitor() ) )
			{
				using ( new AmbientContextCommand<ISetup>().ExecuteWith( this ) )
				{
					base.OnExecute( parameter );
				}
			}
		}
	}
}
