namespace DragonSpark.Application.Components.Validation.Expressions;

public sealed class StandardCharactersPattern : Expression
{
	public static StandardCharactersPattern Default { get; } = new();

	StandardCharactersPattern() :
		base(@"([a-zA-Z0-9- _.;'"":!+*&%$#^@=[\]()~`<>\\/,?{}\|]|\n|\u00a9|\u00ae|[\u2000-\u3300]|[\u0080-\uFFFF]|\ud83c[\ud000-\udfff]|\ud83d[\ud000-\udfff]|\ud83e[\ud000-\udfff])") {}
}