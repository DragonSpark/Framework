using System.Collections.Generic;
using DragonSpark.IoC;
using DragonSpark.Runtime;

namespace DragonSpark.Application
{
	[Singleton( typeof(IApplicationParameterSource), Priority = Priority.Lowest )]
	public class ApplicationParameterSource : IApplicationParameterSource
	{
		readonly IDictionary<string, string> applicationParameters;

		public ApplicationParameterSource( IDictionary<string, string> applicationParameters )
		{
			this.applicationParameters = applicationParameters;
		}

		public object Retrieve( ApplicationParameter parameter )
		{
			var name = parameter.ToString();
			var result = applicationParameters.ContainsKey( name ) ? applicationParameters[ name ] : null;
			return result;
		}
	}

	public static class ApplicationParameterSourceExtensions
	{
        [System.Diagnostics.CodeAnalysis.SuppressMessage( "Microsoft.Design", "CA1004:GenericMethodsShouldProvideTypeParameter", Justification = "Used to determine the key for retrieval." )]
        public static TResult Retrieve<TOwner,TResult>( this IApplicationParameterSource target, string name )
		{
			var result = target.Retrieve( new ApplicationParameter( typeof(TOwner), name ) ).ConvertTo<TResult>();
			return result;
		}
	}
}