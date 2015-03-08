using System;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Client;
using System.Windows.Markup;
using DragonSpark.Extensions;
using DragonSpark.Objects;
using Microsoft.Windows.Data.DomainServices;

namespace DragonSpark.Application.Presentation.Entity.Operations
{
	[ContentProperty( "QueryDefinition" )]
	public class LoadSourceOperation : LoadSourceOperation<DomainCollectionViewSource>
	{
		protected override EntityQuery<TEntity> DetermineQuery<TEntity>()
		{
			var result = QueryDefinition.Transform( x => x.CreateQuery<TEntity>( ContextChecked.DomainContext ).Transform( y => ContextChecked.View.SortDescriptions.Any() ? y.SortAndPageBy( ContextChecked.View ) : y ) );
			return result;
		}

		public QueryDefinition QueryDefinition
		{
			get { return queryDefinition; }
			set { SetProperty( ref queryDefinition, value, () => QueryDefinition ); }
		}	QueryDefinition queryDefinition;

		/*public bool EnablePagingAndSorting
		{
			get { return enablePagingAndSorting; }
			set { SetProperty( ref enablePagingAndSorting, value, () => EnablePagingAndSorting ); }
		}	bool enablePagingAndSorting = true;*/
	}

	public abstract class LoadSourceOperation<TSource> : EntityOperationCommmandBase<TSource> where TSource : DomainCollectionViewSource
	{
		readonly static MethodInfo CreateMethod = typeof(LoadSourceOperation<TSource>).GetMethod( "CreateOperation", DragonSparkBindingOptions.AllProperties );

		protected override void OnAttached()
		{
			base.OnAttached();

			ContextChecked.With( x =>
			{
				x.Loading += ParameterOnLoading;
				x.LoadComplete += ParameterOnLoadComplete;
			} );
			
		}

		protected override void OnDetached()
		{
			base.OnDetached();
			ContextChecked.With( x =>
			{
				x.Loading -= ParameterOnLoading;
				x.LoadComplete -= ParameterOnLoadComplete;
			} );
		}

		void ParameterOnLoading( object sender, LoadOperationEventArgs args )
		{
			args.Operation = LoadOperation;
		}

		protected override OperationBase ResolveOperation()
		{
			return LoadOperation;
		}

		LoadOperation LoadOperation
		{
			get { return loadOperation ?? ( loadOperation = ResolveLoadOperation() ); }
		}	LoadOperation loadOperation;

		LoadOperation ResolveLoadOperation()
		{
			var result = CreateMethod.MakeGenericMethod( ContextChecked.EntityType ).Invoke( this, null ).To<LoadOperation>();
			return result;
		}

		LoadOperation CreateOperation<TEntity>() where TEntity : System.ServiceModel.DomainServices.Client.Entity
		{
			var query = DetermineQuery<TEntity>();
			var result = ContextChecked.DomainContext.Load( query, LoadBehavior.MergeIntoCurrent, true );
			return result;
		}

		protected abstract EntityQuery<TEntity> DetermineQuery<TEntity>() where TEntity : System.ServiceModel.DomainServices.Client.Entity;

		void ParameterOnLoadComplete( object sender, EventArgs eventArgs )
		{
			loadOperation = null;
		}

		protected override void OnCancel( EventArgs args )
		{
			base.OnCancel( args );
			ContextChecked.View.SetTotalItemCount( 0 );
		}

		[DefaultPropertyValue( "Loading Items" )]
		public override string Title
		{
			get { return base.Title; }
			set { base.Title = value; }
		}
	}
}