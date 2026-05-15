using DragonSpark.Model.Sequences;

namespace DragonSpark.Azure.Ai;

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
	                "overgrown post-apocalyptic nature, cinematic sunlight rays, lush vegetation on ruins") {}
}