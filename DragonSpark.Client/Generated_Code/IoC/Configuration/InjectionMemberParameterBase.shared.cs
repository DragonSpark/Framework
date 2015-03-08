using System.Windows.Markup;

namespace DragonSpark.IoC.Configuration
{
	[ContentProperty( "Parameters" )]
	public abstract class InjectionMemberParameterBase : InjectionMember
	{
		public InjectionParameterValueCollection Parameters
		{
			get { return parameters ?? ( parameters = new InjectionParameterValueCollection() ); }
		}	InjectionParameterValueCollection parameters;
	}
}