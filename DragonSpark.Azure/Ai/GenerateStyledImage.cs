using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection.Stop;
using DragonSpark.Model.Selection.Alterations;
using OpenAI.Images;

namespace DragonSpark.Azure.Ai;

public sealed class GenerateStyledImage : IStopAware<GenerateImageInput, GeneratedImage>
{
	readonly IStopAware<GenerateImageInput, GeneratedImage> _previous;
	readonly IAlteration<string>                            _prompt;

	public GenerateStyledImage(GenerateImage previous) : this(previous, StylizedPrompt.Default) {}

	public GenerateStyledImage(IStopAware<GenerateImageInput, GeneratedImage> previous, IAlteration<string> prompt)
	{
		_previous = previous;
		_prompt   = prompt;
	}

	public ValueTask<GeneratedImage> Get(Stop<GenerateImageInput> parameter)
	{
		var (subject, _) = parameter;
		return _previous.Get(parameter with { Subject = subject with { Prompt = _prompt.Get(subject.Prompt) } });
	}
}