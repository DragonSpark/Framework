using System.ComponentModel;
using System.Windows.Controls;
using DragonSpark.Application.Presentation.Commands;
using DragonSpark.Extensions;

namespace DragonSpark.Application.Presentation.Entity.Fields
{


	public class CheckFormForChangesCommand : CommandBase<DataForm>
	{
		public static CheckFormForChangesCommand Instance
		{
			get { return InstanceField; }
		}	static readonly CheckFormForChangesCommand InstanceField = new CheckFormForChangesCommand();

		protected override void Execute( DataForm parameter )
		{
			switch ( parameter.Mode )
			{
				case DataFormMode.Edit:
					parameter.CurrentItem.As<IChangeTracking>( x => x.IsChanged.IsFalse( () =>
					{
						parameter.CancelEdit();
						parameter.BeginEdit();
					} ) );
					break;
			}
		}
	}
}