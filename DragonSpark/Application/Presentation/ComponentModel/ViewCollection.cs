
namespace DragonSpark.Application.Presentation.ComponentModel
{
	partial class ViewCollection<T>
	{
		/// <summary>
		/// Moves the item within the collection.
		/// </summary>
		/// <param name="oldIndex">The old position of the item.</param>
		/// <param name="newIndex">The new position of the item.</param>
		protected override sealed void MoveItem( int oldIndex, int newIndex )
		{
			Threading.Application.Execute( () => MoveItemBase( oldIndex, newIndex ) );
		}

		/// <summary>
		/// Exposes the base implementation fo the <see cref="MoveItem"/> function.
		/// </summary>
		/// <param name="oldIndex">The old index.</param>
		/// <param name="newIndex">The new index.</param>
		/// <remarks>Used to avoid compiler warning regarding unverificable code.</remarks>
		protected virtual void MoveItemBase( int oldIndex, int newIndex )
		{
			base.MoveItem( oldIndex, newIndex );
		}

	}
}
