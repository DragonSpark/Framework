using DragonSpark.Commands;
using DragonSpark.ComponentModel;
using JetBrains.Annotations;
using System.Windows.Input;

namespace DragonSpark.Application.Setup
{
	public class ApplyExportsCommand<T> : CommandBase<object> where T : class, ICommand
	{
		[PostSharp.Patterns.Contracts.NotNull, Service, UsedImplicitly]
		public IExportProvider Exports { [return: PostSharp.Patterns.Contracts.NotNull]get; set; }

		[UsedImplicitly]
		public string ContractName { get; set; }

		public override void Execute( object parameter )
		{
			var exports = Exports.GetExports<T>( ContractName );
			
			foreach ( var export in exports )
			{
				export.Execute( parameter );
			}
		}
	}
}