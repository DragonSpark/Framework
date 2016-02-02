using DragonSpark.Extensions;
using DragonSpark.Windows.Runtime;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using DragonSpark.Runtime;

namespace DragonSpark.Windows.Entity
{
	public class DbContextBuildingParameter
	{
		public DbContextBuildingParameter( DbContext context, DbModelBuilder builder )
		{
			Context = context;
			Builder = builder;
		}

		public DbContext Context { get; }
		public DbModelBuilder Builder { get; }
	}

	public class RegisterComplexTypesCommand : Command<DbContextBuildingParameter>
	{
		protected override void OnExecute( DbContextBuildingParameter parameter )
		{
			var method = parameter.Builder.GetType().GetMethod( nameof(parameter.Builder.ComplexType) );
			parameter.Context.GetDeclaredEntityTypes().First().Assembly.GetTypes().Where( x => x.Has<ComplexTypeAttribute>() ).Each( x =>
			{
				var info = method.MakeGenericMethod( x );
				info.Invoke( parameter.Builder, null );
			} );
		}
	}

	public class DefaultCommands : CompositeCommand<DbContextBuildingParameter>
	{
		public static DefaultCommands Instance { get; } = new DefaultCommands();

		public DefaultCommands() : base( new EnableLocalStoragePropertyCommand(), new RegisterComplexTypesCommand() ) {}
	}

	public class EnableLocalStoragePropertyCommand : Command<DbContextBuildingParameter>
	{
		readonly static MethodInfo ApplyMethod = typeof(EnableLocalStoragePropertyCommand).GetMethod( nameof(Apply), BindingOptions.AllProperties );

		readonly bool useConvention;

		public EnableLocalStoragePropertyCommand( bool useConvention = true )
		{
			this.useConvention = useConvention;
		}

		protected override void OnExecute( DbContextBuildingParameter parameter )
		{
			var types = parameter.Context.GetDeclaredEntityTypes().Select( x => x.Adapt().GetHierarchy( false ).Last() ).Distinct().SelectMany( x => x.Assembly.GetTypes().Where( y => x.Namespace == y.Namespace ) ).Distinct().ToArray();

			types.SelectMany( y => y.GetProperties( BindingOptions.AllProperties ).Where( z => z.Has<LocalStorageAttribute>() || ( useConvention && FollowsConvention( z ) )  ) ).Each( x =>
			{
				ApplyMethod.MakeGenericMethod( x.DeclaringType, x.PropertyType ).Invoke( this, new object[] { parameter.Builder, x } );
			} );
		}

		static bool FollowsConvention( PropertyInfo propertyInfo ) => 
			propertyInfo.Name.EndsWith( "Storage" ) && propertyInfo.DeclaringType.GetProperty( propertyInfo.Name.Replace( "Storage", string.Empty ) ).With( x => x.Has<NotMappedAttribute>() );

		static void Apply<TEntity, TProperty>( DbModelBuilder builder, PropertyInfo property ) where TEntity : class
		{
			var parameter = Expression.Parameter( typeof(TEntity), "x" );
			var expression = Expression.Property( parameter, property.Name );
			
			var result = Expression.Lambda<Func<TEntity, TProperty>>( expression, parameter );

			var configuration = builder.Entity<TEntity>();
			
			configuration.GetType().GetMethod( "Property", new[] { result.GetType() } ).With( y => y.Invoke( configuration, new object[] { result } ) );
		}
	}
}