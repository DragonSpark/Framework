using System;
using System.Linq;
using System.Reflection;

namespace DragonSpark.Windows.Legacy.Markup
{
	public class MemberInfoKeyExtension : ConfigurationKeyExtension
	{
		public MemberInfoKeyExtension( Type type, string member ) : this( type.GetMember( member ).First() ) {}

		public MemberInfoKeyExtension( MemberInfo member ) : base( MemberInfoKeyFactory.Default.Get( PropertyReference.New( member ) ) ) {}
	}
}