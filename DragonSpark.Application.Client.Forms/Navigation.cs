using DragonSpark.Application.Client.Eventing;
using DragonSpark.Application.Client.Forms.ComponentModel;
using DragonSpark.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DragonSpark.Application.Client.Forms
{
	public class Navigation : INavigation
	{
		readonly IPlatform platform;
		readonly INavigationModel model;

		public Navigation( IPlatform platform, INavigationModel model )
		{
			this.platform = platform;
			this.model = model;

			this.Event<ReturningEvent>().Subscribe( OnReturning );
		}

		void OnReturning( CancelEventArgs args )
		{
			args.Cancel |= ShouldCancel();
		}

		protected virtual bool ShouldCancel()
		{
			var page = model.Roots.Last();
			var result = page.SendBackButtonPressed();
			/*if ( !cancel && model.Tree.Count > 1 )
			{
				var page2 = model.PopModal();
				if ( page2 != null )
				{
					SetCurrent( model.CurrentPage, true, true );
					cancel = true;
				}
			}*/
			return result;
		}

		public IReadOnlyList<Page> NavigationStack
		{
			get { return model.Tree.Last(); }
		}

		public IReadOnlyList<Page> ModalStack
		{
			get { return model.Roots.ToList(); }
		}

		public void RemovePage( Page page )
		{
			if ( model.CurrentPage != page )
			{
				model.RemovePage( page );
			}
			else
			{
				PopAsync();
			}
		}

		public void InsertPageBefore( Page page, Page before )
		{
			model.InsertPageBefore( page, before );
		}

		public Task PushModalAsync( Page modal )
		{
			return PushModalAsync( modal, true );
		}

		public Task PushModalAsync( Page modal, bool animated )
		{
			// modal.Parent = platform.Page.Parent;
			var tcs = new TaskCompletionSource<object>();
			model.PushModal( modal );
			SetCurrent( model.CurrentPage, animated, false, () => tcs.SetResult( null ) );
			// modal.NavigationProxy.Inner = this;
			return tcs.Task;
		}

		public Task<Page> PopModalAsync()
		{
			return PopModalAsync( true );
		}

		public Task<Page> PopModalAsync( bool animated )
		{
			var source = new TaskCompletionSource<Page>();
			var result = model.PopModal();
			SetCurrent( model.CurrentPage, animated, true, () => source.SetResult( result ) );
			return source.Task;
		}

		public Task PushAsync( Page root )
		{
			return this.PushAsync(root, true);
		}

		public async Task PushAsync( Page root, bool animated )
		{
			await Push( root, platform.Page, animated );
		}

		public async Task Push( Page root, Page ancester, bool animated )
		{
			model.Push( root, ancester );
			await SetCurrent( model.CurrentPage, animated );

			new[] { root.NavigationProxy, root.Parent.AsTo<Xamarin.Forms.Application, NavigationProxy>( application => application.NavigationProxy ) }.NotNull().Where( proxy => proxy.Inner == null ).Apply( proxy =>
			{
				proxy.Inner = this;
			} );
		}

		public Task<Page> PopAsync()
		{
			return PopAsync( true );
		}

		public Task<Page> PopAsync( bool animated )
		{
			return Pop( platform.Page, animated );
		}

		public async Task<Page> Pop( Page ancestor, bool animated )
		{
			var result = model.Pop( ancestor );
			await SetCurrent( model.CurrentPage, animated, true );
			return result;
		}

		public Task PopToRootAsync()
		{
			return PopToRootAsync( true );
		}

		public async Task PopToRootAsync( bool animated )
		{
			await PopToRoot( platform.Page, animated );
		}

		public async Task PopToRoot( Page ancestor, bool animated )
		{
			model.PopToRoot( ancestor );
			await SetCurrent( model.CurrentPage, animated, true );
		}

		Task SetCurrent( Page page, bool animated, bool popping = false, Action completedCallback = null )
		{
			return Task.Factory.StartNew( () =>
			{
				platform.SetPage( page );
				completedCallback.With( x => x() );
			}, CancellationToken.None, TaskCreationOptions.None, TaskScheduler.FromCurrentSynchronizationContext() );
		}
	}
}