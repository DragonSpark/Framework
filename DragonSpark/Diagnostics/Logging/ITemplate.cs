using DragonSpark.Model.Selection;

namespace DragonSpark.Diagnostics.Logging
{
	public interface ITemplate<T> : ISelect<ExceptionParameter<T>, TemplateException> {}
}