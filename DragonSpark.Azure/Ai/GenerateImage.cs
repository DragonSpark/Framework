using DragonSpark.Compose;
using DragonSpark.Model.Operations;
using DragonSpark.Model.Operations.Selection;
using DragonSpark.Model.Selection.Alterations;
using DragonSpark.Model.Sequences;
using DragonSpark.Text;
using OpenAI.Images;
using System;
using System.Threading.Tasks;

namespace DragonSpark.Azure.Ai;

public sealed class GenerateImage : ISelecting<Stop<GenerateImageInput>, GeneratedImage>
{
	readonly ImageClient         _client;
	readonly IAlteration<string> _prompt;

	public GenerateImage(ImageClient client) : this(client, Prompt.Default) {}

	public GenerateImage(ImageClient client, IAlteration<string> prompt)
	{
		_client = client;
		_prompt = prompt;
	}

	public async ValueTask<GeneratedImage> Get(Stop<GenerateImageInput> parameter)
	{
		var ((prompt, size), item) = parameter;
		var input = _prompt.Get(prompt);
		var response = await _client
		                     .GenerateImageAsync(input, new()
		                                         {
			                                         Size = size,
													 Quality = GeneratedImageQuality.LowQuality
			                                         /*Style = GeneratedImageStyle.Natural*/
		                                         }, // TODO V2
		                                         item)
		                     .Off();
		return response.Value;
	}
}

// TODO
sealed class Prompt : IAlteration<string>
{
	public static Prompt Default { get; } = new();

	Prompt() : this(Style.Default) {}

	readonly IText _style;

	public Prompt(IText style) => _style = style;

	public string Get(string parameter) => $"{parameter}, {_style.Get()}";
}

sealed class Styles : Instances<string>
{
	public static Styles Default { get; } = new();

	Styles() : base("hyper-realistic digital oil painting, thick brushstrokes, dramatic chiaroscuro",
	                "futuristic synthwave aesthetic, vibrant neon glow, retro-futurism",
	                "dark cinematic noir, high contrast, atmospheric grit and smoke",
	                "ethereal dreamscape, soft focus, pastel iridescent lighting",
	                "industrial brutalist design, sharp angles, metallic textures",
	                "stunning 3D isometric render, octane render, soft clay textures, vibrant colors",
	                "classic 1970s psychedelic rock poster, bold flowing lines, trippy saturated colors",
	                "minimalist charcoal sketch, rough textured paper, deep shadows, expressive lines",
	                "luxury gold and marble architectural design, elegant lighting, clean sharp focus",
	                "vibrant pop art style, bold halftone dots, high energy comic book aesthetic",
	                "atmospheric watercolor wash, bleeding ink edges, dreamy soft textures",
	                "cybernetic bioluminescence, glowing organic veins, deep ocean abyss lighting",
	                "vintage kodachrome photography, 1950s aesthetic, warm film grain, nostalgic light",
	                "glitch art aesthetic, digital distortion, chromatic aberration, high-tech error style",
	                "overgrown post-apocalyptic nature, cinematic sunlight rays, lush vegetation on ruins"
	               ) {}
}

sealed class Style : IText
{
	public static Style Default { get; } = new();

	Style() : this(Styles.Default, Random.Shared) {}

	readonly Array<string> _styles;
	readonly uint          _length;
	readonly Random        _random;

	public Style(Array<string> styles, Random random) : this(styles, styles.Length, random) {}

	public Style(Array<string> styles, uint length, Random random)
	{
		_styles = styles;
		_length = length;
		_random = random;
	}

	public string Get() => string.Empty; // _styles[_random.Next((int)_length)];
}