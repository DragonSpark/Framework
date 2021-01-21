using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;
using System.Threading.Tasks;

namespace DragonSpark.Presentation.Components.State
{
	public sealed class Remove : IRemove
	{
		readonly ProtectedSessionStorage _store;

		public Remove(ProtectedSessionStorage store) => _store = store;

		public ValueTask Get(string parameter) => _store.DeleteAsync(parameter);
	}
}