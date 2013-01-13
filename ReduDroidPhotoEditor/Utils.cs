using System.IO;
using Android.Content;
using Android.Graphics;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;
using Matrix = Android.Graphics.Matrix;
using Point = Android.Graphics.Point;

namespace ReduDroidPhotoEditor
{
	public static class Utils
	{
		public static Stream ByteArray2Stream (byte[] bytes)
		{
			MemoryStream stream = new MemoryStream (bytes);

			return stream;
		}

		public static Point GetScreenResolution (Context context)
		{
			DisplayMetrics displayMetrics = new DisplayMetrics ();

			IWindowManager windowManager = context.GetSystemService (Context.WindowService).JavaCast<IWindowManager> ();
			windowManager.DefaultDisplay.GetMetrics (displayMetrics);

			Point screenResolution = new Point (displayMetrics.WidthPixels, displayMetrics.HeightPixels);

			return screenResolution;
		}

		private static Bitmap Texture2D2Bitmap (Texture2D image)
		{
			Bitmap bitmap = Bitmap.CreateBitmap (image.Width, image.Height, Bitmap.Config.Argb8888);

			Color[] data = new Color[image.Width * image.Height];

			image.GetData (data);
			for (int x = 0; x < image.Width; x++) {
				for (int y = 0; y < image.Height; y++) {
					Color c = data [x + (y * image.Width)];

					bitmap.SetPixel (x, y, new Android.Graphics.Color (c.R, c.G, c.B, c.A));
				}
			}

			return bitmap;
		}

		public static Bitmap CreateBitmap (Texture2D image, Vector2 scale, float rotation, Vector2 origin)
		{
			Matrix matrix = new Matrix ();
			matrix.PostScale (scale.X, scale.Y);
			matrix.PostRotate (rotation, origin.X, origin.Y);

			Bitmap source = Texture2D2Bitmap (image);
			Bitmap newBitmap = Bitmap.CreateBitmap (source, 0, 0, source.Width, source.Height, matrix, true);

			return newBitmap;
		}
	}
}