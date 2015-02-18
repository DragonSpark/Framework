using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Expressions;
using System.Reflection;
using DragonSpark.Extensions;

namespace DragonSpark.Server.Legacy.Entity
{
	public class LocalStoragePropertyProcessor
	{
		static readonly MethodInfo ApplyMethod = typeof(LocalStoragePropertyProcessor).GetMethod( "Apply", BindingOptions.AllProperties );

		public static LocalStoragePropertyProcessor Instance
		{
			get { return InstanceField; }
		}	static readonly LocalStoragePropertyProcessor InstanceField = new LocalStoragePropertyProcessor();

		public void Process( DbContext context, DbModelBuilder modelBuilder, bool useConvention = true )
		{
			var types = context.GetDeclaredEntityTypes().Select( x => x.GetHierarchy( false ).Last() ).Distinct().SelectMany( x => x.Assembly.GetValidTypes().Where( y => x.Namespace == y.Namespace ) ).Distinct().ToArray();

			types.SelectMany( y => y.GetProperties( BindingOptions.AllProperties ).Where( z => z.IsDecoratedWith<LocalStorageAttribute>() || ( useConvention && FollowsConvention( z ) )  ) ).Apply( x =>
			{
				ApplyMethod.MakeGenericMethod( x.DeclaringType, x.PropertyType ).Invoke( this, new object[] { modelBuilder, x } );
			} );
		}

		static bool FollowsConvention( PropertyInfo propertyInfo )
		{
			var result = propertyInfo.Name.EndsWith( "Storage" ) && propertyInfo.DeclaringType.GetProperty( propertyInfo.Name.Replace( "Storage", string.Empty ) ).Transform( x => x.IsDecoratedWith<NotMappedAttribute>() );
			return result;
		}

		void Apply<TEntity, TProperty>( DbModelBuilder builder, PropertyInfo property ) where TEntity : class
		{
			var parameter = Expression.Parameter( typeof(TEntity), "x" );
			var expression = Expression.Property( parameter, property.Name );
			
			var result = Expression.Lambda<Func<TEntity, TProperty>>( expression, parameter );

			var configuration = builder.Entity<TEntity>();
			
			configuration.GetType().GetMethod( "Property", new[] { result.GetType() } ).NotNull( y => y.Invoke( configuration, new object[] { result } ) );
		}
	}
}