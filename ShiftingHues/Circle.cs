using System;
using Microsoft.Xna.Framework;

namespace ShiftingHues
{
	/// <summary>
	/// Describes a 2D-circle. - J Mor
	/// </summary>
	public struct Circle
	{
		#region Fields & Properties
		#region Fields
		/// <summary>
		/// The x coordinate of the center of this <see cref="Circle"/>.
		/// </summary>
		public float X;
		/// <summary>
		/// The y coordinate of the center of this <see cref="Circle"/>.
		/// </summary>
		public float Y;
		/// <summary>
		/// The radius of this <see cref="Circle"/>.
		/// </summary>
		public float Radius;

		private FRectangle hitbox;
		#endregion
		#region Properties
		/// <summary>
		/// The center of this <see cref="Circle"/>. - J Mor
		/// </summary>
		public FPoint Center
		{
			get => new FPoint(X, Y);
			set
			{
				//hitbox.Offset(value - Center);
				X = value.X;
				Y = value.Y;
				hitbox = new FRectangle(X, Y, Radius * 2);
			}
		}

		/// <summary>
		/// Returns the area of this <see cref="Circle"/>. - J Mor
		/// </summary>
		public float Area { get => (float)Math.PI * Radius * Radius; }

		public FRectangle Hitbox { get => hitbox; }
		#endregion
		#endregion
		#region Operators
		/// <summary>
		/// Compares whether two <see cref="FRectangle"/> instances are equal.
		/// </summary>
		/// <param name="a"><see cref="FRectangle"/> instance on the left of the equal sign.</param>
		/// <param name="b"><see cref="FRectangle"/> instance on the right of the equal sign.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public static bool operator ==(Circle a, Circle b) => ((a.X == b.X) && (a.Y == b.Y) && (a.Radius == b.Radius));
		/// <summary>
		/// Compares whether two <see cref="FRectangle"/> instances are not equal.
		/// </summary>
		/// <param name="a"><see cref="FRectangle"/> instance on the left of the not equal sign.</param>
		/// <param name="b"><see cref="FRectangle"/> instance on the right of the not equal sign.</param>
		/// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
		public static bool operator !=(Circle a, Circle b) => !(a == b);
		/// <summary>
		/// Casts the specified <see cref="Circle"/> as a <see cref="FRectangle"/>. - J Mor
		/// </summary>
		/// <param name="c">The <see cref="Circle"/> to cast.</param>
		/// <remarks>Intended for quick rough collision checks.</remarks>
		public static explicit operator FRectangle(Circle c) => new FRectangle(c.Center, c.Radius);
		#endregion
		#region Constructors

		public Circle(float x, float y, float radius)
		{
			X = x;
			Y = y;
			this.Radius = radius;
			this.hitbox = new FRectangle(x, y, radius * 2);
		}

		public Circle(FPoint center, float radius)
		{
			X = center.X;
			Y = center.Y;
			this.Radius = radius;
			this.hitbox = new FRectangle(center, radius * 2);
		}

		public Circle(Vector2 center, float radius)
		{
			X = center.X;
			Y = center.Y;
			this.Radius = radius;
			this.hitbox = new FRectangle(center, radius * 2);
		}
		#endregion
		#region Methods
		#region Contains
		#region Shapes
		/// <summary>
		/// Checks if the object is completely contained by this <see cref="Circle"/>'s perimeter. - J Mor
		/// </summary>
		/// <param name="obj">The <see cref="Circle"/> to check.</param>
		/// <returns><c>true</c> if the <see cref="Circle"/>is contained; <c>false</c> otherwise.</returns>
		/// <remarks>If this radius is the biggest && the smaller radius(theirs) + the dist between centers is smaller than the larger radius (mine), true</remarks>
		public bool Contains(Circle obj) =>
			(Radius > obj.Radius) &&
			(DistanceFromCenter(obj.Center) + obj.Radius < Radius); // TODO: Check logic & test. - J Mor
																	/// <summary>
																	/// Checks if the object is completely contained by this <see cref="Circle"/>'s perimeter. - J Mor
																	/// </summary>
																	/// <param name="obj">The <see cref="Circle"/> to check.</param>
																	/// <param name="result"><c>true</c> if the <see cref="Circle"/>is contained; <c>false</c> otherwise.</param>
																	/// <remarks>If this radius is the biggest && the smaller radius(theirs) + the dist between centers is smaller than the larger radius (mine), true</remarks>
		public void Contains(ref Circle value, out bool result) => result =
			(Radius > value.Radius) &&
			(DistanceFromCenter(value.Center) + value.Radius < Radius);
		/// <summary>
		/// Checks if the object is completely contained by this <see cref="Circle"/>'s perimeter. - J Mor
		/// </summary>
		/// <param name="obj">The <see cref="FRectangle"/> to check.</param>
		/// <returns><c>true</c> if the <see cref="FRectangle"/>is contained; <c>false</c> otherwise.</returns>
		/// <remarks>If all corners are inside, the whole thing is inside.</remarks>
		public bool Contains(FRectangle obj) =>
			(Contains(obj.TopLeft) &&
			Contains(obj.TopRight) &&
			Contains(obj.BottomRight) &&
			Contains(obj.BottomLeft)); // TODO: Check logic & test. - J Mor

		public void Contains(FRectangle obj, out bool result) => result =
			(Contains(obj.TopLeft) &&
			Contains(obj.TopRight) &&
			Contains(obj.BottomRight) &&
			Contains(obj.BottomLeft)); // TODO: Check logic & test. - J Mor

		public bool Contains(Rectangle value) => Contains(new FRectangle(value));

		public void Contains(ref Rectangle value, out bool result) => result = Contains(new FRectangle(value));
		#endregion
		#region Points

		public bool Contains(int x, int y) => (DistanceFromCenter(x, y) < Radius);

		public bool Contains(Point value) => (DistanceFromCenter(value) < Radius);

		public void Contains(ref Point value, out bool result) => result = (DistanceFromCenter(value) < Radius);

		public bool Contains(float x, float y) => (DistanceFromCenter(x, y) < Radius);

		public bool Contains(FPoint value) => (DistanceFromCenter(value) < Radius);

		public void Contains(ref FPoint value, out bool result) => result = (DistanceFromCenter(value) < Radius);

		public bool Contains(Vector2 value) => (DistanceFromCenter(value) < Radius);

		public void Contains(ref Vector2 value, out bool result) => result = (DistanceFromCenter(value) < Radius);
		#endregion
		#endregion
		#region Intersects
		#region Shapes
		/// <summary>
		/// Checks if the object touches this <see cref="Circle"/>'s perimeter. - J Mor
		/// </summary>
		/// <param name="value">The <see cref="Circle"/> to check.</param>
		/// <returns><c>true</c> if they intersect; <c>false</c> otherwise.</returns>
		/// <remarks>If the dist between centers is = or less than the sum of the radii & the distance between centers + the smaller radii is > or = to the larger radii, true</remarks>
		public bool Intersects(Circle value) =>
			(DistanceFromCenter(value.Center) <= Radius + value.Radius) &&
			((Radius > value.Radius) ?
				(DistanceFromCenter(value.Center) + value.Radius >= Radius) :
				(DistanceFromCenter(value.Center) + Radius >= value.Radius)); // TODO: Check logic & test. - J Mor

		public void Intersects(Circle value, out bool result) => result =
			(DistanceFromCenter(value.Center) <= (Radius + value.Radius)) &&
			((Radius > value.Radius) ?
				((DistanceFromCenter(value.Center) + value.Radius) >= Radius) :
				((DistanceFromCenter(value.Center) + Radius) >= value.Radius)); // TODO: Check logic & test. - J Mor
																				/// <summary>
																				/// Checks if the object touches this <see cref="Circle"/>'s perimeter. - J Mor
																				/// </summary>
																				/// <param name="value">The <see cref="FRectangle"/> to check.</param>
																				/// <returns><c>true</c> if they intersect; <c>false</c> otherwise.</returns>
		public bool Intersects(FRectangle value) =>
			(DistanceFromCenter(value.TopLeft, value.TopRight) == Radius) ||
			(DistanceFromCenter(value.TopRight, value.BottomRight) == Radius) ||
			(DistanceFromCenter(value.BottomRight, value.BottomLeft) == Radius) ||
			(DistanceFromCenter(value.BottomLeft, value.TopLeft) == Radius);

		public void Intersects(FRectangle value, out bool result) => result =
			(DistanceFromCenter(value.TopLeft, value.TopRight) == Radius) ||
			(DistanceFromCenter(value.TopRight, value.BottomRight) == Radius) ||
			(DistanceFromCenter(value.BottomRight, value.BottomLeft) == Radius) ||
			(DistanceFromCenter(value.BottomLeft, value.TopLeft) == Radius);

		public bool Intersects(Rectangle value) => Intersects(new FRectangle(value));

		public void Intersects(ref Rectangle value, out bool result) => result = Intersects(new FRectangle(value));
		#endregion
		#region Points
		#region Int-Based

		public bool Intersects(int x, int y) => (DistanceFromCenter(x, y) == Radius);

		public void Intersects(ref int x, ref int y, out bool result) => result = (DistanceFromCenter(x, y) == Radius);

		public bool Intersects(Point value) => (DistanceFromCenter(value) == Radius);

		public void Intersects(ref Point value, out bool result) => result = (DistanceFromCenter(value) == Radius);
		#endregion
		#region Float-Based

		public bool Intersects(float x, float y) => (DistanceFromCenter(x, y) == Radius);

		public void Intersects(ref float x, ref float y, out bool result) => result = (DistanceFromCenter(x, y) == Radius);

		public bool Intersects(FPoint value) => (DistanceFromCenter(value) == Radius);

		public void Intersects(ref FPoint value, out bool result) => result = (DistanceFromCenter(value) == Radius);

		public bool Intersects(Vector2 value) => (DistanceFromCenter(value) == Radius);

		public void Intersects(ref Vector2 value, out bool result) => result = (DistanceFromCenter(value) == Radius);
		#endregion
		#endregion
		#endregion
		#region Equals
		/// <summary>
		/// Compares whether current instance is equal to specified <see cref="Object"/>.
		/// </summary>
		/// <param name="obj">The <see cref="Object"/> to compare.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public override bool Equals(object obj) => (obj is Circle) && this == ((Circle)obj);
		/// <summary>
		/// Compares whether current instance is equal to specified <see cref="Circle"/>.
		/// </summary>
		/// <param name="other">The <see cref="Circle"/> to compare.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public bool Equals(Circle other) => this == other;
		#endregion
		#region Inflate

		public void Inflate(int horizontalAmount, int verticalAmount)
		{
			throw new NotImplementedException();
		}

		public void Inflate(float horizontalAmount, float verticalAmount)
		{
			throw new NotImplementedException();
		}
		#endregion
		#region Offset

		public void Offset(int offsetX, int offsetY)
		{
			X += offsetX;
			Y += offsetY;
			hitbox.Offset(offsetX, offsetY);
		}

		public void Offset(float offsetX, float offsetY)
		{
			X += offsetX;
			Y += offsetY;
			hitbox.Offset(offsetX, offsetY);
		}

		public void Offset(FPoint offset)
		{
			X += offset.X;
			Y += offset.Y;
			hitbox.Offset(offset);
		}

		public void Offset(Point offset)
		{
			X += offset.X;
			Y += offset.Y;
			hitbox.Offset(offset);
		}

		public void Offset(Vector2 offset)
		{
			X += offset.X;
			Y += offset.Y;
			hitbox.Offset(offset);
		}
		#endregion
		#region DistanceFromCenter
		public float DistanceFromCenter(Circle c) => Math.Abs(Center.Length(c.Center) - (Radius + c.Radius));
		#region Single Points
		/// <summary>
		/// Returns the distance of the specified point from the center of the circle
		/// </summary>
		/// <param name="point">The point to check</param>
		/// <returns>The distance of the point from the center.</returns>
		public float DistanceFromCenter(Vector2 point) => Math.Abs(((point - Center).Length()));
		/// <summary>
		/// Returns the distance of the specified point from the center of the circle
		/// </summary>
		/// <param name="point">The point to check</param>
		/// <returns>The distance of the point from the center.</returns>
		public float DistanceFromCenter(FPoint point) => Math.Abs(Center.Length(point));
		/// <summary>
		/// Returns the distance of the specified point from the center of the circle
		/// </summary>
		/// <param name="point">The point to check</param>
		/// <returns>The distance of the point from the center.</returns>
		public float DistanceFromCenter(Point point) => Math.Abs(((new Vector2(point.X - Center.X, point.Y - Center.Y)).Length()));
		/// <summary>
		/// Returns the distance of the specified point from the center of the circle
		/// </summary>
		/// <param name="x">The x value of the point to check.</param>
		/// <param name="y">The y value of the point to check.</param>
		/// <returns>The distance of the point from the center.</returns>
		public float DistanceFromCenter(float x, float y) => Math.Abs(((new Vector2(x, y) - Center).Length()));
		/// <summary>
		/// Returns the distance of the specified point from the center of the circle
		/// </summary>
		/// <param name="x">The x value of the point to check.</param>
		/// <param name="y">The y value of the point to check.</param>
		/// <returns>The distance of the point from the center.</returns>
		public float DistanceFromCenter(int x, int y) => Math.Abs(((new Vector2(x, y) - Center).Length()));
		#endregion
		#region Line Segment of 2 Points
		/// <summary>
		/// This gets the shortest distance from the line defined by these 2 points and the center of this circle - J Mor
		/// </summary>
		/// <param name="p1">One point on the line</param>
		/// <param name="p2">Another point on the line</param>
		/// <returns>The double value of the shortest distance from the line to the center of the circle</returns>
		public float DistanceFromCenter(Vector2 p1, Vector2 p2)
		{
			float top = Math.Abs((p2.Y - p1.Y) * Center.X - (p2.X - p1.X) * Center.Y + p2.X * p1.Y - p2.Y * p1.X);
			float bottom = (float)Math.Sqrt(Math.Pow((p2.Y - p1.Y), 2.0) + Math.Pow((p2.X - p1.X), 2.0));
			return top / bottom;
		}
		/// <summary>
		/// This gets the shortest distance from the line defined by these 2 points and the center of this circle - J Mor
		/// </summary>
		/// <param name="p1">One point on the line</param>
		/// <param name="p2">Another point on the line</param>
		/// <returns>The double value of the shortest distance from the line to the center of the circle</returns>
		public float DistanceFromCenter(FPoint p1, FPoint p2)
		{
			float top = Math.Abs((p2.Y - p1.Y) * Center.X - (p2.X - p1.X) * Center.Y + p2.X * p1.Y - p2.Y * p1.X);
			float bottom = (float)Math.Sqrt(Math.Pow((p2.Y - p1.Y), 2.0) + Math.Pow((p2.X - p1.X), 2.0));
			return top / bottom;
		}
		/// <summary>
		/// This gets the shortest distance from the line defined by these 2 points and the center of this circle - J Mor
		/// </summary>
		/// <param name="p1">One point on the line</param>
		/// <param name="p2">Another point on the line</param>
		/// <returns>The double value of the shortest distance from the line to the center of the circle</returns>
		public float DistanceFromCenter(Point p1, Point p2)
		{
			float top = Math.Abs((p2.Y - p1.Y) * Center.X - (p2.X - p1.X) * Center.Y + p2.X * p1.Y - p2.Y * p1.X);
			float bottom = (float)Math.Sqrt(Math.Pow((p2.Y - p1.Y), 2.0) + Math.Pow((p2.X - p1.X), 2.0));
			return top / bottom;
		}
		#endregion
		#endregion
		#region Get Extremes
		/// <summary>
		/// Gets an array of the four extreme points from the x and y axes. - J Mor
		/// </summary>
		/// <returns>An array of the four extreme points from the x and y axes.</returns>
		public FPoint[] GetExtremes()
		{
			hitbox.GetExtremes(out FPoint[] fPoints);
			return fPoints;
		}
		//public FPoint[] GetExtremes()
		//{
		//	return new FPoint[]
		//	{
		//		new FPoint(Center.X - Radius, Center.Y),
		//		new FPoint(Center.X, Center.Y - Radius),
		//		new FPoint(Center.X + Radius, Center.Y),
		//		new FPoint(Center.X, Center.Y + Radius)
		//	};
		//}
		/// <summary>
		/// Gets an array of the four extreme points from the x and y axes. - J Mor
		/// </summary>
		/// <param name="result">An array of the four extreme points from the x and y axes.</param>
		public void GetExtremes(out FPoint[] result) => hitbox.GetExtremes(out result);
		//public void GetExtremes(out FPoint[] result)
		//{
		//	result = new FPoint[]
		//	{
		//		new FPoint(Center.X - Radius, Center.Y),
		//		new FPoint(Center.X, Center.Y - Radius),
		//		new FPoint(Center.X + Radius, Center.Y),
		//		new FPoint(Center.X, Center.Y + Radius)
		//	};
		//}
		/// <summary>
		/// Gets an array of the four extreme points from the x and y axes. - J Mor
		/// </summary>
		/// <param name="result">An array of the four extreme points from the x and y axes.</param>
		public void GetExtremes(out Vector2[] result) => hitbox.GetExtremes(out result);
		//public void GetExtremes(out Vector2[] result)
		//{
		//	result = new Vector2[]
		//	{
		//		new Vector2(Center.X - Radius, Center.Y),
		//		new Vector2(Center.X, Center.Y - Radius),
		//		new Vector2(Center.X + Radius, Center.Y),
		//		new Vector2(Center.X, Center.Y + Radius)
		//	};
		//}
		#endregion
		#region IsContainedBy
		/// <summary>
		/// Checks if this <see cref="Circle"/> is inside of a given <see cref="FRectangle"/>. - J Mor
		/// </summary>
		/// <param name="rect">The <see cref="FRectangle"/> that may contain this circle</param>
		/// <returns><c>true</c> if all four points are inside; <c>false</c> otherwise</returns>
		public bool IsContainedBy(FRectangle rect)
		{
			// If the rectangle contains all 4 of the extremes, it contains the whole circle, so check for each extreme
			FPoint[] points = GetExtremes();
			foreach (FPoint v in points) if (!rect.Contains(v)) return false;
			return true;
		}
		/// <summary>
		/// Checks if this <see cref="Circle"/> is inside of a given <see cref="Rectangle"/>. - J Mor
		/// </summary>
		/// <param name="rect">The <see cref="Rectangle"/> that may contain this circle</param>
		/// <returns><c>true</c> if all four points are inside; <c>false</c> otherwise</returns>
		public bool IsContainedBy(Rectangle rect)
		{
			// If the rectangle contains all 4 of the extremes, it contains the whole circle, so check for each extreme
			GetExtremes(out Vector2[] points);
			foreach (Vector2 v in points) if (!rect.Contains(v)) return false;
			return true;
		}
		#endregion
		#region Misc. Overloads
		/// <summary>
		/// Gets the hash code of this <see cref="Circle"/>. - J Mor
		/// </summary>
		/// <returns>Hash code of this <see cref="Circle"/>.</returns>
		/// <remarks>Research how hash code generation works.</remarks>
		public override int GetHashCode() // TODO: Research how hash code generation works. - J Mor
		{
			unchecked
			{
				var hash = 17;
				hash = hash * 23 + X.GetHashCode();
				hash = hash * 23 + Y.GetHashCode();
				hash = hash * 23 + Radius.GetHashCode();
				return hash;
			}
		}
		/// <summary>
		/// Returns a <see cref="String"/> representation of this <see cref="FRectangle"/> in the format:
		/// {X:[<see cref="X"/>] Y:[<see cref="Y"/>] Width:[<see cref="Width"/>] Height:[<see cref="Height"/>]}
		/// </summary>
		/// <returns><see cref="String"/> representation of this <see cref="FRectangle"/>.</returns>
		public override string ToString() => "{X:" + X + " Y:" + Y + " Radius:" + Radius + "}";
		#endregion

		public void ResetHitbox()
		{
			hitbox = new FRectangle(X, Y, Radius * 2);
		}
		#endregion
	}
}