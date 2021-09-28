using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Application.Entities.Editing
{
	public class Save<TIn, T> : IOperation<TIn> where T : class
	{
		readonly ISelecting<TIn, T> _selecting;
		readonly Action<T>          _configure;
		readonly Save<T>            _save;

		protected Save(ISelecting<TIn, T> selecting, Action<T> configure, Save<T> save)
		{
			_selecting = selecting;
			_configure = configure;
			_save      = save;
		}

		public async ValueTask Get(TIn parameter)
		{
			var subject = await _selecting.Await(parameter);
			_configure(subject);
			await _save.Await(subject);
		}
	}

	public class Save<T> : Modify<T> where T : class
	{
		public Save(IStandardScopes scopes) : base(scopes, UpdateLocal<T>.Default.Then().Operation()) {}
	}
}