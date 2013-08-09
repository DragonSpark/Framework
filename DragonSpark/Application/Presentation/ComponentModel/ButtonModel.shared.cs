namespace DragonSpark.Application.Presentation.ComponentModel
{
	public class ButtonModel
	{
		public ButtonModel() {}

		public ButtonModel(string name)
		{
			Content = ToolTip = Action = name;
		}

		public object Content { get; set; }
		public object ToolTip { get; set; }
		public string Action { get; set; }
	}
}