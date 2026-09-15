using System.Drawing;
using System.Drawing.Imaging;

namespace DragonSpark.Drawing;

public readonly record struct BitmapAsDataInput(Bitmap Subject, ImageFormat format);