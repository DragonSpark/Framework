using System;
using System.Collections.ObjectModel;
using System.Windows.Data;

namespace DragonSpark.Application.Presentation.Configuration
{
	public class BindingCollection : Collection<BindingBase>
	{
		protected override void InsertItem( int index, BindingBase item )
		{
			if ( item == null )
			{
				throw new ArgumentNullException( "item" );
			}
			ValidateItem( item );
			base.InsertItem( index, item );
		}

		protected override void SetItem( int index, BindingBase item )
		{
			if ( item == null )
			{
				throw new ArgumentNullException( "item" );
			}
			ValidateItem( item );
			base.SetItem( index, item );
		}

		static void ValidateItem( BindingBase binding )
		{
			if ( !( binding is Binding ) )
			{
				throw new NotSupportedException( "BindingCollectionContainsNonBinding" );
			}
		}
	}
}