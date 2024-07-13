using SixLabors.Fonts;

namespace DragonSpark.Drawing;

public sealed record FontDefinition(string Family, byte Size, byte Dpi = 72, KerningMode Mode = KerningMode.Auto);