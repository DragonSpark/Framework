using SixLabors.ImageSharp;

namespace DragonSpark.Drawing;

public readonly record struct ResizeToRatioInput(Image Subject, Size Size);