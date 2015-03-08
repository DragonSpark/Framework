using DragonSpark.Runtime;
using DragonSpark.Extensions;
using DragonSpark.Application.Presentation.Extensions;

namespace DragonSpark.Application.Presentation.Interaction
{
	public class SetProperty : Expression.Samples.Interactivity.SetProperty
	{
		protected override void Invoke(object parameter)
		{
			this.RefreshValue( TargetObjectProperty );
			if ( Value != null )
			{
				var target = TargetObject ?? Target;
				target.Transform( x => x.GetType().GetProperty( PropertyName ).Transform( y => y.PropertyType ) ).NotNull( x =>
				{
					Value = Value.ConvertTo( x );
				} );

				base.Invoke( parameter );
			}
		}
	}
}