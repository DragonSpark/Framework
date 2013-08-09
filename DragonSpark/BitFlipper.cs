using System;
using DragonSpark.Extensions;

namespace DragonSpark
{
	public class BitFlipper
	{
		public bool Flipped { get; private set; }

		public void Reset()
		{
			Flipped = false;
		}

		public bool Check()
		{
			return Check( null );
		}

		public bool Flip()
		{
			var result = !Flipped && ( Flipped = true );
			return result;
		}

		public bool Check( Action action )
		{
			return CheckWith( null, action );
		}

		public bool Check( bool? condition, Action action )
		{
			var result = CheckWith( condition.HasValue ? new Func<bool>( () => condition.Value ) : null , action );
			return result;
		}

		public bool CheckWith( Func<bool> condition, Action action )
		{
			var original = Flipped;
			if ( !Flipped )
			{
				Flipped = condition.Transform( item => item(), () => true );
				Flipped.IsTrue( action );
			}
			var result = !original && Flipped;
			return result;
		}
	}
}