using System;
using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Aspects.Validation
{
	sealed class AutoValidationControllerFactory : ParameterizedSourceBase<IAutoValidationController>
	{
		public static IParameterizedSource<IAutoValidationController> Default { get; } = new AutoValidationControllerFactory().ToCache();
		AutoValidationControllerFactory() : this( AdapterLocator.Default.Get, AspectHub.Default.Set ) {}

		readonly Func<object, IParameterValidationAdapter> adapterSource;
		readonly Action<object, IAspectHub> set;

		AutoValidationControllerFactory( Func<object, IParameterValidationAdapter> adapterSource, Action<object, IAspectHub> set )
		{
			this.adapterSource = adapterSource;
			this.set = set;
		}

		public override IAutoValidationController Get( object parameter )
		{
			var adapter = adapterSource( parameter );
			var result = new AutoValidationController( adapter );
			set( parameter, result );
			return result;
		}
	}
}