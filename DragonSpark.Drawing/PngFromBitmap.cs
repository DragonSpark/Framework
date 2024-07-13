using System.Drawing;
using System.Drawing.Imaging;

namespace DragonSpark.Drawing;

public abstract class PngFromBitmap(Bitmap source) : LoadImage(source, ImageFormat.Png);