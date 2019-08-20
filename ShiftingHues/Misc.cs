using System;
using Microsoft.Xna.Framework;
using JMMGExt;

namespace ShiftingHues
{
	public static class _Misc
	{
		#region Round
		public static int RoundToInt(float value)
		{
			return (value - (int)value > .4) ? (int)Math.Ceiling(value) : (int)Math.Floor(value);
		}
		public static int RoundToInt(double value)
		{
			return (value - (int)value > .4) ? (int)Math.Ceiling(value) : (int)Math.Floor(value);
		}
		public static Point RoundToPoint(Vector2 value)
		{
			return new Point(((value.X - (int)value.X > .4) ? (int)Math.Ceiling(value.X) : (int)Math.Floor(value.X)), ((value.Y - (int)value.Y > .4) ? (int)Math.Ceiling(value.Y) : (int)Math.Floor(value.Y)));
			//int x = (value.X - (int)value.X > .4) ? (int)Math.Ceiling(value.X) : (int)Math.Floor(value.X);
			//int y = (value.Y - (int)value.Y > .4) ? (int)Math.Ceiling(value.Y) : (int)Math.Floor(value.Y);
			//return new Point(x, y);
		}
		public static Point RoundToPoint(float value1, float value2)
		{
			return new Point(((value1 - (int)value1 > .4) ? (int)Math.Ceiling(value1) : (int)Math.Floor(value1)), ((value2 - (int)value2 > .4) ? (int)Math.Ceiling(value2) : (int)Math.Floor(value2)));
			//int x = (value1 - (int)value1 > .4) ? (int)Math.Ceiling(value1) : (int)Math.Floor(value1);
			//int y = (value2 - (int)value2 > .4) ? (int)Math.Ceiling(value2) : (int)Math.Floor(value2);
			//return new Point(x, y);
		}
		public static Vector2 RoundToVector(Vector2 value)
		{
			return new Vector2(((value.X - (int)value.X > .4) ? (int)Math.Ceiling(value.X) : (int)Math.Floor(value.X)), ((value.Y - (int)value.Y > .4) ? (int)Math.Ceiling(value.Y) : (int)Math.Floor(value.Y)));
			//int x = (value.X - (int)value.X > .4) ? (int)Math.Ceiling(value.X) : (int)Math.Floor(value.X);
			//int y = (value.Y - (int)value.Y > .4) ? (int)Math.Ceiling(value.Y) : (int)Math.Floor(value.Y);
			//return new Vector2(x, y);
		}
		#endregion
		///// <summary>
		///// Checks if the <see cref="Rectangle"/> is outside of the bounds of the screen's drawing area. - J Mor
		///// </summary>
		///// <param name="rect">The <see cref="Rectangle"/> to check.</param>
		///// <returns>true if the field is out of bounds, false otherwise.</returns>
		//public static bool IsOutOfScreenBounds(Rectangle rect)
		//{
		//	return ((rect.Right < 0 || rect.Left > _Cnsts.PLAY_AREA_WIDTH) || (rect.Bottom < 0 || rect.Top > _Cnsts.PLAY_AREA_HEIGHT));
		//}
		///// <summary>
		///// Checks if the <see cref="FRectangle"/> is outside of the bounds of the screen's drawing area. - J Mor
		///// </summary>
		///// <param name="rect">The <see cref="FRectangle"/> to check.</param>
		///// <returns>true if the field is out of bounds, false otherwise.</returns>
		//public static bool IsOutOfScreenBounds(FRectangle rect)
		//{
		//	return ((rect.Right < 0 || rect.Left > _Cnsts.PLAY_AREA_WIDTH) || (rect.Bottom < 0 || rect.Top > _Cnsts.PLAY_AREA_HEIGHT));
		//}
	}
	public static class _Cast
	{
		#region Primitives
		//public static int DToInt(double d) { return (int)Math.Round(d); }
		//public static float DToF(double d) { return (float)Math.Round(d); }
		//public static int FToInt(float f) { return (int)Math.Round(f); }
		#endregion
		#region Structs To Structs
		public static Vector2 Vect2ToIntVect2(Vector2 v) { return new Vector2((int)Math.Round(v.X), (int)Math.Round(v.Y)); }
		public static Point Vect2ToPoint(Vector2 v) { return new Point((int)Math.Round(v.X), (int)Math.Round(v.Y)); }
		public static FPoint Vect2ToIntFPoint(Vector2 v) { return new FPoint((int)Math.Round(v.X), (int)Math.Round(v.Y)); }
		public static FPoint Vect2ToFPoint(Vector2 v) { return new FPoint(v.X, v.Y); }
		#endregion
	}
}