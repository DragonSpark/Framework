using System;
using System.Reflection;
using System.Windows.Markup;
using System.Xaml;

namespace DragonSpark.Application.Presentation.Configuration
{
	public class Static : IMarkupExtension<object>
	{
		public object ProvideValue( IServiceProvider serviceProvider )
		{
			Object ret = null;
			var typeResolveFailed = true;
			var type = MemberType;
			String fieldMemberName = null;
			String fullFieldMemberName = null;

			if ( Member != null )
			{
				if ( MemberType != null ) //on a le type et le membre
				{
					fieldMemberName = Member;
					fullFieldMemberName = String.Format( "{0}.{1}", type.FullName, Member );
				}
				else
				{
					var index = Member.IndexOf( '.' );

					if ( index >= 0 )
					{
						var typeName = Member.Substring( 0, index );

						if ( !String.IsNullOrEmpty( typeName ) )
						{
							var xamlTypeResolver = serviceProvider.GetService( typeof(IXamlTypeResolver) ) as IXamlTypeResolver;

							if ( xamlTypeResolver != null )
							{
								type = xamlTypeResolver.Resolve( typeName );
								fieldMemberName = Member.Substring( index + 1 ); //, Member.Length - index - 1
								typeResolveFailed = String.IsNullOrEmpty( fieldMemberName );
							}
						}
					}
				}

				if ( typeResolveFailed )
				{
					throw new InvalidOperationException( "Member" );
				}
				if ( type.IsEnum )
				{
					ret = Enum.Parse( type, fieldMemberName, true );
				}
				else
				{
					var fail = true;

					var field = type.GetField( fieldMemberName, BindingFlags.Public | BindingFlags.FlattenHierarchy | BindingFlags.Static );

					if ( field != null )
					{
						fail = false;
						ret = field.GetValue( null );
					}
					else
					{
						var property = type.GetProperty( fieldMemberName, BindingFlags.Public | BindingFlags.Static );
						if ( property != null )
						{
							fail = false;
							ret = property.GetValue( null, null );
						}
					}

					if ( fail )
					{
						throw new ArgumentException( fullFieldMemberName );
					}
				}
			}
			else
			{
				throw new InvalidOperationException();
			}

			return ret;
		}

		public string Member { get; set; }

		public System.Type MemberType { get; set; }
	}
}
