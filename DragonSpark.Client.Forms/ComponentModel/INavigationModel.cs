using System.Collections.Generic;
using DragonSpark.Activation.IoC.Commands;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms.ComponentModel
{
	public interface INavigationModel
	{
		void PopToRoot( Page ancestralNav );
		void PushModal( Page page );
		Page PopModal();
		bool RemovePage( Page page );
		void InsertPageBefore( Page page, Page before );
		Page CurrentPage { get; }
		IReadOnlyList<IReadOnlyList<Page>> Tree { get; }
		IEnumerable<Page> Roots { get; }
		void Push( Page root, Page ancester );
		Page Pop( Page ancestor );
	}

	[RegisterAs( typeof(INavigationModel) )]
	class NavigationModel : Xamarin.Forms.NavigationModel, INavigationModel
	{}
}