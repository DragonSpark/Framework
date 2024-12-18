﻿namespace DragonSpark.Azure.Ai;

public sealed class AiServicesConfiguration
{
	public string Address { get; set; } = default!;
	public string Key { get; set; } = default!;
	public string ImageModel { get; set; } = "dall-e-3";
}