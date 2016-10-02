using DragonSpark.Extensions;
using DragonSpark.Runtime;
using DragonSpark.TypeSystem;
using PostSharp.Patterns.Contracts;
using System;
using System.Linq;
using System.Windows.Markup;

namespace DragonSpark.Configuration
{
	[ContentProperty( nameof(Aliases) )]
	public class Registration : IEquatable<string>
	{
		readonly Func<string, bool> equalsReference;

		public Registration()
		{
			equalsReference = Equals;
		}

		public Registration( string key, object value ) : this( key, value, Items<string>.Default ) {}

		public Registration( string key, object value, params string[] aliases )
		{
			Key = key;
			Aliases = new DeclarativeCollection<string>( aliases );
			Value = value;
			equalsReference = Equals;
		}

		public DeclarativeCollection<string> Aliases { get; } = new DeclarativeCollection<string>();

		[NotEmpty]
		public string Key { [return: NotEmpty]get; set; }

		[NotNull]
		public object Value { [return: NotNull]get; set; }

		public override bool Equals( object obj ) => obj.AsTo( equalsReference );

		public virtual bool Equals( string other ) => Key.Append( Aliases ).Contains( other );

		public override int GetHashCode() => 0;
	}
}
