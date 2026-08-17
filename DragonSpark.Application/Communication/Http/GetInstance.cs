using System.Reflection;
using System.Web;
using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Runtime.Activation;
using DragonSpark.Text;

namespace DragonSpark.Application.Communication.Http;

sealed class GetInstance<T> : IStopAware<HttpContent, T>
{
    public static GetInstance<T> Default { get; } = new();

    GetInstance() : this(New<T>.Default.Get, SnakeCase.Default) {}

    readonly Func<T>                          _new;
    readonly IFormatter<ReadOnlyMemory<char>> _name;

    public GetInstance(Func<T> @new, IFormatter<ReadOnlyMemory<char>> name)
    {
        _new  = @new;
        _name = name;
    }

    // ReSharper disable once CognitiveComplexity
    // ReSharper disable once ExcessiveIndentation
    // ReSharper disable once MethodTooLong
    public async ValueTask<T> Get(Stop<HttpContent> parameter)
    {
	    var (subject, stop) = parameter;
	    var text   = await subject.ReadAsStringAsync(stop).Off();
	    var parsed = HttpUtility.ParseQueryString(text);

	    if (typeof(T).IsValueType || typeof(T) == typeof(string))
	    {
		    return parsed.Count > 0 ? (T)(object)parsed.Get(0)! : default!;
	    }

	    var result = _new();

	    foreach (var property in typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
	                                      .Where(p => p.CanWrite))
	    {
		    var name  = _name.Get(property.Name.AsMemory());
		    var value = parsed.Get(name);

		    if (!string.IsNullOrEmpty(value))
		    {
			    if (property.PropertyType == typeof(string))
			    {
				    property.SetValue(result, value);
			    }
			    else if (property.PropertyType == typeof(int) && int.TryParse(value, out var intVal))
			    {
				    property.SetValue(result, intVal);
			    }
		    }
	    }

	    return result;
    }}