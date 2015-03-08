namespace DragonSpark.Application.Presentation.ComponentModel
{
	public class BindableEnumerationItem
	{
		public int UnderlyingValue { get; set; }
		public string DisplayName { get; set; }
		public object Value { get; set; }

		public override string ToString()
		{
			return DisplayName;
		}

		public override bool Equals(object obj)
		{
			var otherBindable = obj as BindableEnumerationItem;

			if(otherBindable != null) 
				return UnderlyingValue == otherBindable.UnderlyingValue;

			return obj != null && UnderlyingValue.Equals((int)obj);
		}

		public override int GetHashCode()
		{
			return UnderlyingValue.GetHashCode();
		}
	}
}