using System;
using System.Collections;
using System.Linq;
using System.Windows.Markup;
using DragonSpark.Extensions;

namespace DragonSpark.Windows.Markup
{
	public abstract class CollectionExtensionBase<TCollection> : MarkupExtension where TCollection : class
	{
		protected CollectionExtensionBase() : this( typeof(object) )
		{}

		protected CollectionExtensionBase( Type typeArgument )
		{
			this.typeArgument = typeArgument;
		}

		// Items is the actual collection we'll return from ProvideValue.
		public TCollection Items
		{
			get
			{
				if ( items == null )
				{
					var collectionType = GetCollectionType( TypeArgument );
					items = Activator.CreateInstance( collectionType ) as TCollection;
				}
				return items;
			}			
		}	TCollection items;

		public Type TypeArgument
		{
			get { return typeArgument; }
			set
			{
				typeArgument = value;
				if ( items != null )
				{
					object oldItems = items;
					items = null;
					CopyItems( oldItems );
				}
			}
		}	Type typeArgument;


		protected virtual void CopyItems( object oldItems )
		{
			Items.As<IList>( x => oldItems.As<IList>().Cast<object>().Each( y => x.Add( y ) ) );
		}

		protected abstract Type GetCollectionType( Type type );

		public override object ProvideValue( System.IServiceProvider serviceProvider )
		{
			return Items;
		}
	}
}