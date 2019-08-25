using System;
using Microsoft.Xna.Framework;

namespace JMMGExt
{
	/// <summary>
	/// Describes a 2D-point using float values. - J Mor
	/// </summary>
	public struct FPoint : IEquatable<FPoint>
	{
		#region Fields
		#region Private Fields
		private static readonly FPoint zeroFPoint = new FPoint();
		#endregion
		#region Public Fields
		/// <summary>
		/// The x coordinate of this <see cref="FPoint"/>.
		/// </summary>
		public float X;
		/// <summary>
		/// The y coordinate of this <see cref="FPoint"/>.
		/// </summary>
		public float Y;
		#endregion
		#endregion
		#region Properties
		/// <summary>
		/// Returns a <see cref="FPoint"/> with coordinates 0, 0.
		/// </summary>
		public static FPoint Zero
		{
			get { return zeroFPoint; }
		}
		#region Internal Properties
		internal string DebugDisplayString
		{
			get
			{
				return string.Concat(
					this.X.ToString(), "  ",
					this.Y.ToString()
				);
			}
		}
		#endregion
		#endregion
		#region Constructors
		/// <summary>
		/// Constructs a point with X and Y from two values.
		/// </summary>
		/// <param name="x">The x coordinate in 2d-space.</param>
		/// <param name="y">The y coordinate in 2d-space.</param>
		public FPoint(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}
		/// <summary>
		/// Constructs a point with X and Y set to the same value.
		/// </summary>
		/// <param name="value">The x and y coordinates in 2d-space.</param>
		public FPoint(int value)
		{
			this.X = value;
			this.Y = value;
		}
		/// <summary>
		/// Constructs a <see cref="FPoint"/> from the given <see cref="Point"/>. - J Mor
		/// </summary>
		/// <param name="p">The <see cref="Point"/> to construct from.</param>
		public FPoint(Point p)
		{
			this.X = p.X;
			this.Y = p.Y;
		}
		/// <summary>
		/// Constructs a point with X and Y from two values.
		/// </summary>
		/// <param name="x">The x coordinate in 2d-space.</param>
		/// <param name="y">The y coordinate in 2d-space.</param>
		public FPoint(float x, float y)
		{
			this.X = x;
			this.Y = y;
		}
		/// <summary>
		/// Constructs a point with X and Y set to the same value.
		/// </summary>
		/// <param name="value">The x and y coordinates in 2d-space.</param>
		public FPoint(float value)
		{
			this.X = value;
			this.Y = value;
		}
		/// <summary>
		/// Constructs a <see cref="FPoint"/> from the given <see cref="Vector2"/>. - J Mor
		/// </summary>
		/// <param name="v">The <see cref="Vector2"/> to construct from.</param>
		public FPoint(Vector2 v)
		{
			this.X = v.X;
			this.Y = v.Y;
		}
		#endregion
		#region Operators
		#region Operations
		/// <summary>
		/// Adds two points.
		/// </summary>
		/// <param name="value1">Source <see cref="FPoint"/> on the left of the add sign.</param>
		/// <param name="value2">Source <see cref="FPoint"/> on the right of the add sign.</param>
		/// <returns>Sum of the points.</returns>
		public static FPoint operator +(FPoint value1, FPoint value2)
		{
			return new FPoint(value1.X + value2.X, value1.Y + value2.Y);
		}
		/// <summary>
		/// Adds a <see cref="FPoint"/> and a <see cref="Vector2"/>.
		/// </summary>
		/// <param name="value1">Source <see cref="FPoint"/> on the left of the add sign.</param>
		/// <param name="value2">Source <see cref="Vector2"/> on the right of the add sign.</param>
		/// <returns>Sum of the points.</returns>
		public static FPoint operator +(FPoint value1, Vector2 value2)
		{
			return new FPoint(value1.X + value2.X, value1.Y + value2.Y);
		}
		/// <summary>
		/// Subtracts a <see cref="FPoint"/> from a <see cref="FPoint"/>.
		/// </summary>
		/// <param name="value1">Source <see cref="FPoint"/> on the left of the sub sign.</param>
		/// <param name="value2">Source <see cref="FPoint"/> on the right of the sub sign.</param>
		/// <returns>Result of the subtraction.</returns>
		public static FPoint operator -(FPoint value1, FPoint value2)
		{
			return new FPoint(value1.X - value2.X, value1.Y - value2.Y);
		}
		/// <summary>
		/// Subtracts a <see cref="FPoint"/> from a <see cref="Vector2"/>.
		/// </summary>
		/// <param name="value1">Source <see cref="Vector2"/> on the left of the sub sign.</param>
		/// <param name="value2">Source <see cref="FPoint"/> on the right of the sub sign.</param>
		/// <returns>Result of the subtraction.</returns>
		public static Vector2 operator -(Vector2 value1, FPoint value2)
		{
			return new Vector2(value1.X - value2.X, value1.Y - value2.Y);
		}
		/// <summary>
		/// Multiplies the components of two points by each other.
		/// </summary>
		/// <param name="value1">Source <see cref="FPoint"/> on the left of the mul sign.</param>
		/// <param name="value2">Source <see cref="FPoint"/> on the right of the mul sign.</param>
		/// <returns>Result of the multiplication.</returns>
		public static FPoint operator *(FPoint value1, FPoint value2)
		{
			return new FPoint(value1.X * value2.X, value1.Y * value2.Y);
		}
		/// <summary>
		/// Divides the components of a <see cref="FPoint"/> by the components of another <see cref="FPoint"/>.
		/// </summary>
		/// <param name="source">Source <see cref="FPoint"/> on the left of the div sign.</param>
		/// <param name="divisor">Divisor <see cref="FPoint"/> on the right of the div sign.</param>
		/// <returns>The result of dividing the points.</returns>
		public static FPoint operator /(FPoint source, FPoint divisor)
		{
			return new FPoint(source.X / divisor.X, source.Y / divisor.Y);
		}
		#endregion
		#region Comparisons
		/// <summary>
		/// Compares whether two <see cref="FPoint"/> instances are equal.
		/// </summary>
		/// <param name="a"><see cref="FPoint"/> instance on the left of the equal sign.</param>
		/// <param name="b"><see cref="FPoint"/> instance on the right of the equal sign.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public static bool operator ==(FPoint a, FPoint b)
		{
			return a.Equals(b);
		}
		/// <summary>
		/// Compares whether two <see cref="FPoint"/> instances are not equal.
		/// </summary>
		/// <param name="a"><see cref="FPoint"/> instance on the left of the not equal sign.</param>
		/// <param name="b"><see cref="FPoint"/> instance on the right of the not equal sign.</param>
		/// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>	
		public static bool operator !=(FPoint a, FPoint b)
		{
			return !a.Equals(b);
		}
		#endregion
		#region Casts
		/// <summary>
		/// Casts the <see cref="FPoint"/> as a <see cref="Vector2"/>.
		/// </summary>
		/// <param name="p">The <see cref="FPoint"/> to cast.</param>
		public static implicit operator Vector2(FPoint p) { return new Vector2(p.X, p.Y); }
		/// <summary>
		/// Casts the <see cref="Vector2"/> as a <see cref="FPoint"/>.
		/// </summary>
		/// <param name="p">The <see cref="Vector2"/> to cast.</param>
		/// <remarks>Currently implict due to <see cref="FPoint"/>s and <see cref="Vector2"/>s being different interpertations of the same data (2 float values representing an x and y value) with no loss of precision.</remarks>
		public static implicit operator FPoint(Vector2 p) { return new FPoint(p.X, p.Y); }
		/// <summary>
		/// Cast the <see cref="FPoint"/> as a <see cref="Point"/>.
		/// </summary>
		/// <param name="p">The <see cref="FPoint"/> to cast.</param>
		public static explicit operator Point(FPoint p) { return new Point((int)Math.Round(p.X), (int)Math.Round(p.Y)); }
		/// <summary>
		/// Cast the <see cref="Point"/> as a <see cref="FPoint"/>.
		/// </summary>
		/// <param name="p">The <see cref="Point"/> to cast.</param>
		public static explicit operator FPoint(Point p) => new FPoint(p.X, p.Y);
		#endregion
		#endregion
		#region Public methods

		public float Length(FPoint point)
		{
			FPoint p = this - point;
			return (float)Math.Sqrt((p.X * p.X) + (p.Y * p.Y));
		}
		#region Comparisons
		/// <summary>
		/// Compares whether current instance is equal to specified <see cref="Object"/>.
		/// </summary>
		/// <param name="obj">The <see cref="Object"/> to compare.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public override bool Equals(object obj)
		{
			return (obj is FPoint) && Equals((FPoint)obj);
		}
		/// <summary>
		/// Compares whether current instance is equal to specified <see cref="FPoint"/>.
		/// </summary>
		/// <param name="other">The <see cref="FPoint"/> to compare.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public bool Equals(FPoint other)
		{
			return ((X == other.X) && (Y == other.Y));
		}
		#endregion
		/// <summary>
		/// Gets the hash code of this <see cref="FPoint"/>.
		/// </summary>
		/// <returns>Hash code of this <see cref="FPoint"/>.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				var hash = 17;
				hash = hash * 23 + X.GetHashCode();
				hash = hash * 23 + Y.GetHashCode();
				return hash;
			}

		}
		/// <summary>
		/// Returns a <see cref="String"/> representation of this <see cref="FPoint"/> in the format:
		/// {X:[<see cref="X"/>] Y:[<see cref="Y"/>]}
		/// </summary>
		/// <returns><see cref="String"/> representation of this <see cref="FPoint"/>.</returns>
		public override string ToString()
		{
			return "{X:" + X + " Y:" + Y + "}";
		}
		#region Conversions
		/// <summary>
		/// Gets a <see cref="Vector2"/> representation for this object. - J Mor
		/// </summary>
		/// <returns>A <see cref="Vector2"/> representation for this object.</returns>
		public Vector2 ToVector2()
		{
			return new Vector2(X, Y);
		}
		/// <summary>
		/// Gets a <see cref="Point"/> representation for this object. - J Mor
		/// </summary>
		/// <returns>A <see cref="Point"/> representation for this object.</returns>
		public Point ToPoint()
		{
			return new Point((int)Math.Round(X), (int)Math.Round(Y));
		}
		/// <summary>
		/// Deconstruction method for <see cref="FPoint"/>.
		/// </summary>
		/// <param name="x">The x-coordinate as an int.</param>
		/// <param name="y">The y-coordinate as an int.</param>
		public void Deconstruct(out int x, out int y)
		{
			x = (int)Math.Round(X);
			y = (int)Math.Round(Y);
		}
		/// <summary>
		/// Deconstruction method for <see cref="FPoint"/>.
		/// </summary>
		/// <param name="x">The x-coordinate as a float.</param>
		/// <param name="y">The y-coordinate as a float.</param>
		public void Deconstruct(out float x, out float y)
		{
			x = X;
			y = Y;
		}
		#endregion
		#endregion
	}
}