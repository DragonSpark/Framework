using DragonSpark.Sources.Parameterized;

namespace DragonSpark.Windows.Markup
{
	public class MemberInfoKeyFactory : ParameterizedSourceBase<PropertyReference, string>
	{
		public static MemberInfoKeyFactory Default { get; } = new MemberInfoKeyFactory();

		public override string Get( PropertyReference parameter ) => $"{parameter.DeclaringType.FullName}::{parameter.PropertyName}";
	}
}