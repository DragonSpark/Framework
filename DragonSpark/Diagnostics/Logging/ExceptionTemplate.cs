using DragonSpark.Model.Sequences;

namespace DragonSpark.Diagnostics.Logging;

public class ExceptionTemplate<T> : ITemplate<T>
{
	readonly string _template;

	public ExceptionTemplate(string template) => _template = template;

	public TemplateException Get(ExceptionParameter<T> parameter)
	{
		var (exception, argument) = parameter;
		var result = new TemplateException(_template, exception, argument!);
		return result;
	}
}

public class ExceptionTemplate<T1, T2> : ITemplate<(T1, T2)>
{
	readonly string _template;

	protected ExceptionTemplate(string template) => _template = template;

	public TemplateException Get(ExceptionParameter<(T1, T2)> parameter)
	{
		var (exception, (first, second)) = parameter;
		return new(_template, exception, first!, second!);
	}
}

public class ExceptionTemplate<T1, T2, T3> : ITemplate<(T1, T2, T3)>
{
	readonly string _template;

	public ExceptionTemplate(string template) => _template = template;

	public TemplateException Get(ExceptionParameter<(T1, T2, T3)> parameter)
	{
		var (exception, (first, second, third)) = parameter;
		return new (_template, exception, first!, second!, third!);
	}
}

public class ExceptionTemplate : ITemplate<Array<object>>
{
	readonly string _template;

	public ExceptionTemplate(string template) => _template = template;

	public TemplateException Get(ExceptionParameter<Array<object>> parameter)
	{
		var (exception, argument) = parameter;
		return new (_template, exception, argument.Open());
	}
}