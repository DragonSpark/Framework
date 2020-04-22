using DragonSpark.Compose;
using JetBrains.Annotations;
using Radzen;
using Radzen.Blazor;
using System.Collections.Generic;
using System.Reflection;

namespace DragonSpark.Presentation.Components.Forms
{
	public class Form<TItem> : RadzenTemplateForm<TItem>, IRadzenForm
	{
		readonly static FieldInfo FieldInfo = typeof(RadzenTemplateForm<TItem>)
			.GetField("components", BindingFlags.Instance | BindingFlags.NonPublic);

		readonly FieldInfo _field;

		[UsedImplicitly]
		public Form() : this(FieldInfo) {}

		public Form(FieldInfo field) => _field = field;

		public new void AddComponent(IRadzenFormComponent component)
		{
			var components = _field.GetValue(this).To<List<IRadzenFormComponent>>();
			components.RemoveAll(x => x.Name == component.Name);
			components.Add(component);
		}


		protected override void OnParametersSet()
		{
			var context = EditContext;
			base.OnParametersSet();
			if (context != null && context != EditContext)
			{
				_field.GetValue(this)
				      .To<List<IRadzenFormComponent>>()
				      .Clear();
			}
		}
	}
}