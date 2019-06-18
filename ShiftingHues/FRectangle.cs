using System;
using Microsoft.Xna.Framework;

namespace ShiftingHues
{
	/// <summary>
	/// Describes a 2D-rectangle using floating point numbers.
	/// </summary>
	public struct FRectangle
	{
		#region Fields & Properties
		#region Fields
		#region Private Fields
		private static FRectangle emptyFRectangle = new FRectangle();
		#endregion
		#region Public Fields
		/// <summary>
		/// The x coordinate of the top-left corner of this <see cref="FRectangle"/>.
		/// </summary>
		public float X;
		/// <summary>
		/// The y coordinate of the top-left corner of this <see cref="FRectangle"/>.
		/// </summary>
		public float Y;
		/// <summary>
		/// The width of this <see cref="FRectangle"/>.
		/// </summary>
		public float Width;
		/// <summary>
		/// The height of this <see cref="FRectangle"/>.
		/// </summary>
		public float Height;
		#endregion
		#endregion
		#region Properties
		#region Public Properties
		#region Float Values
		/// <summary>
		/// Returns the x coordinate of the left edge of this <see cref="FRectangle"/>.
		/// </summary>
		public float Left { get => this.X; }
		/// <summary>
		/// Returns the x coordinate of the right edge of this <see cref="FRectangle"/>.
		/// </summary>
		public float Right { get => (this.X + this.Width); }
		/// <summary>
		/// Returns the y coordinate of the top edge of this <see cref="FRectangle"/>.
		/// </summary>
		public float Top { get => this.Y; }
		/// <summary>
		/// Returns the y coordinate of the bottom edge of this <see cref="FRectangle"/>.
		/// </summary>
		public float Bottom { get => (this.Y + this.Height); }
		/// <summary>
		/// Returns the area of this <see cref="FRectangle"/>. - J Mor
		/// </summary>
		public float Area { get => Width * Height; }
		#endregion
		#region FPoints
		/// <summary>
		/// Returns the coordinates of the top-left corner of this <see cref="FRectangle"/>.
		/// </summary>
		public FPoint TopLeft { get => new FPoint(X, Y); }
		/// <summary>
		/// Returns the coordinates of the bottom-right corner of this <see cref="FRectangle"/>.
		/// </summary>
		public FPoint BottomRight { get => new FPoint(X + Width, Y + Height); }
		/// <summary>
		/// Returns the coordinates of the top-right corner of this <see cref="FRectangle"/>.
		/// </summary>
		public FPoint TopRight { get => new FPoint(X + Width, Y); }
		/// <summary>
		/// Returns the coordinates of the bottom-left corner of this <see cref="FRectangle"/>.
		/// </summary>
		public FPoint BottomLeft { get => new FPoint(X, Y + Height); }
		/// <summary>
		/// The top-left coordinates of this <see cref="FRectangle"/>.
		/// </summary>
		public FPoint Location
		{
			get => new FPoint(this.X, this.Y);
			set
			{
				X = value.X;
				Y = value.Y;
			}
		}
		/// <summary>
		/// The width-height coordinates of this <see cref="FRectangle"/>.
		/// </summary>
		public FPoint Size
		{
			get => new FPoint(this.Width, this.Height);
			set
			{
				Width = value.X;
				Height = value.Y;
			}
		}
		/// <summary>
		/// A <see cref="FPoint"/> located in the center of this <see cref="FRectangle"/>.
		/// </summary>
		public FPoint Center
		{
			get => new FPoint(this.X + (this.Width / 2), this.Y + (this.Height / 2));
			set => Location = new FPoint(value.X - Center.X, value.Y - Center.Y);
		}
		#endregion
		#region Int-Based
		/// <summary>
		/// The top-left coordinates of this <see cref="FRectangle"/> as a <see cref="Point"/>. - J Mor
		/// </summary>
		public Point LocationAsPoint
		{
			get => new Point((int)Math.Round(X), (int)Math.Round(Y));
			set
			{
				X = value.X;
				Y = value.Y;
			}
		}
		/// <summary>
		/// The width-height coordinates of this <see cref="FRectangle"/> as integers. - J Mor
		/// </summary>
		public Point SizeAsPoint
		{
			get => new Point((int)Math.Round(Width), (int)Math.Round(Height));
			set
			{
				Width = value.X;
				Height = value.Y;
			}
		}
		/// <summary>
		/// A <see cref="Point"/> located in the center of this <see cref="FRectangle"/>. - J Mor
		/// </summary>
		/// <remarks>
		/// If <see cref="Width"/> or <see cref="Height"/> is an odd number,
		/// the center point will be rounded down.
		/// </remarks>
		public Point CenterAsPoint
		{
			get => new Point((int)(Math.Round(X + (Width / 2))), (int)(Math.Round(Y + (Height / 2))));
		}
		/// <summary>
		/// Returns this <see cref="FRectangle"/> as a <see cref="Rectangle"/>. - J Mor
		/// </summary>
		public Rectangle Rectangle
		{
			get => new Rectangle((int)Math.Round(X), (int)Math.Round(Y), (int)Math.Round(Width), (int)Math.Round(Height));
		}
		#endregion
		#region Misc
		/// <summary>
		/// Whether or not this <see cref="FRectangle"/> has an equal <see cref="Width"/> and
		/// <see cref="Height"/>.
		/// </summary>
		public bool IsSquare { get => (this.Width == this.Height); }
		/// <summary>
		/// Whether or not this <see cref="FRectangle"/> has a <see cref="Width"/> and
		/// <see cref="Height"/> of 0, and a <see cref="Location"/> of (0, 0).
		/// </summary>
		public bool IsEmpty
		{
			get => ((this.Width == 0) && (this.Height == 0) && (this.X == 0) && (this.Y == 0));
		}
		/// <summary>
		/// Returns a <see cref="FRectangle"/> with X = 0, Y = 0, Width = 0, Height = 0.
		/// </summary>
		public static FRectangle Empty { get => emptyFRectangle; }
		#endregion
		#endregion
		#region Internal Properties
		internal string DebugDisplayString
		{
			get => string.Concat(this.X, "  ", this.Y, "  ", this.Width, "  ", this.Height);
		}
		#endregion
		#endregion
		#endregion
		#region Constructors
		#region Int-Based Constructors
		/// <summary>
		/// Creates a new instance of <see cref="FRectangle"/> struct, with the specified
		/// position, width, and height.
		/// </summary>
		/// <param name="x">The x coordinate of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="y">The y coordinate of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="width">The width of the created <see cref="FRectangle"/>.</param>
		/// <param name="height">The height of the created <see cref="FRectangle"/>.</param>
		/// <remarks>An original constructor of <see cref="Rectangle"/>.</remarks>
		public FRectangle(int x, int y, int width, int height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}
		/// <summary>
		/// Creates a new instance of <see cref="FRectangle"/> struct, with the specified
		/// position and size.
		/// </summary>
		/// <param name="x">The x coordinate of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="y">The y coordinate of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="size">The size of the created <see cref="FRectangle"/>.</param>
		public FRectangle(int x, int y, int size)
		{
			this.X = x;
			this.Y = y;
			this.Width = size;
			this.Height = size;
		}
		/// <summary>
		/// Creates a new instance of <see cref="FRectangle"/> struct, with the specified
		/// location and size.
		/// </summary>
		/// <param name="location">The x and y coordinates of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="size">The width and height of the created <see cref="FRectangle"/>.</param>
		/// <remarks>An original constructor of <see cref="Rectangle"/>.</remarks>
		public FRectangle(Point location, Point size)
		{
			this.X = location.X;
			this.Y = location.Y;
			this.Width = size.X;
			this.Height = size.Y;
		}
		#endregion
		#region Float-Based Constructors
		/// <summary>
		/// Creates a new instance of <see cref="FRectangle"/> struct, with the specified
		/// position, width, and height.
		/// </summary>
		/// <param name="x">The x coordinate of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="y">The y coordinate of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="width">The width of the created <see cref="FRectangle"/>.</param>
		/// <param name="height">The height of the created <see cref="FRectangle"/>.</param>
		public FRectangle(float x, float y, float width, float height)
		{
			this.X = x;
			this.Y = y;
			this.Width = width;
			this.Height = height;
		}
		/// <summary>
		/// Creates a new instance of <see cref="FRectangle"/> struct, with the specified
		/// position, and size.
		/// </summary>
		/// <param name="x">The x coordinate of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="y">The y coordinate of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="size">The size of the created <see cref="FRectangle"/>.</param>
		public FRectangle(float x, float y, float size)
		{
			this.X = x;
			this.Y = y;
			this.Width = size;
			this.Height = size;
		}
		/// <summary>
		/// Creates a new instance of <see cref="FRectangle"/> struct, with the specified
		/// position, width, and height.
		/// </summary>
		/// <param name="x">The x coordinate of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="y">The y coordinate of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="size">The width and height of the created <see cref="FRectangle"/>.</param>
		public FRectangle(float x, float y, FPoint size)
		{
			this.X = x;
			this.Y = y;
			this.Width = size.X;
			this.Height = size.Y;
		}
		/// <summary>
		/// Creates a new instance of <see cref="FRectangle"/> struct, with the specified
		/// position, width, and height.
		/// </summary>
		/// <param name="x">The x coordinate of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="y">The y coordinate of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="size">The width and height of the created <see cref="FRectangle"/>.</param>
		public FRectangle(float x, float y, Vector2 size)
		{
			this.X = x;
			this.Y = y;
			this.Width = size.X;
			this.Height = size.Y;
		}
		/// <summary>
		/// Creates a new instance of <see cref="FRectangle"/> struct, with the specified
		/// position, width, and height.
		/// </summary>
		/// <param name="location">The x and y coordinates of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="width">The width of the created <see cref="FRectangle"/>.</param>
		/// <param name="height">The height of the created <see cref="FRectangle"/>.</param>
		public FRectangle(FPoint location, float width, float height)
		{
			this.X = location.X;
			this.Y = location.Y;
			this.Width = width;
			this.Height = height;
		}
		/// <summary>
		/// Creates a new instance of <see cref="FRectangle"/> struct, with the specified
		/// location and size.
		/// </summary>
		/// <param name="location">The x and y coordinates of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="size">The size of the created <see cref="FRectangle"/>.</param>
		public FRectangle(FPoint location, float size)
		{
			this.X = location.X;
			this.Y = location.Y;
			this.Width = size;
			this.Height = size;
		}
		/// <summary>
		/// Creates a new instance of <see cref="FRectangle"/> struct, with the specified
		/// location and size.
		/// </summary>
		/// <param name="location">The x and y coordinates of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="size">The size of the created <see cref="FRectangle"/>.</param>
		public FRectangle(Vector2 location, float size)
		{
			this.X = location.X;
			this.Y = location.Y;
			this.Width = size;
			this.Height = size;
		}
		#region Float-Based Points
		/// <summary>
		/// Creates a new instance of <see cref="FRectangle"/> struct, with the specified
		/// location and size.
		/// </summary>
		/// <param name="location">The x and y coordinates of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="size">The width and height of the created <see cref="FRectangle"/>.</param>
		public FRectangle(FPoint location, FPoint size)
		{
			this.X = location.X;
			this.Y = location.Y;
			this.Width = size.X;
			this.Height = size.Y;
		}
		/// <summary>
		/// Creates a new instance of <see cref="FRectangle"/> struct, with the specified
		/// location and size.
		/// </summary>
		/// <param name="location">The x and y coordinates of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="size">The width and height of the created <see cref="FRectangle"/>.</param>
		public FRectangle(Vector2 location, Vector2 size)
		{
			this.X = location.X;
			this.Y = location.Y;
			this.Width = size.X;
			this.Height = size.Y;
		}
		/// <summary>
		/// Creates a new instance of <see cref="FRectangle"/> struct, with the specified
		/// location and size.
		/// </summary>
		/// <param name="location">The x and y coordinates of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="size">The width and height of the created <see cref="FRectangle"/>.</param>
		public FRectangle(Vector2 location, FPoint size)
		{
			this.X = location.X;
			this.Y = location.Y;
			this.Width = size.X;
			this.Height = size.Y;
		}
		/// <summary>
		/// Creates a new instance of <see cref="FRectangle"/> struct, with the specified
		/// location and size.
		/// </summary>
		/// <param name="location">The x and y coordinates of the top-left corner of the created <see cref="FRectangle"/>.</param>
		/// <param name="size">The width and height of the created <see cref="FRectangle"/>.</param>
		public FRectangle(FPoint location, Vector2 size)
		{
			this.X = location.X;
			this.Y = location.Y;
			this.Width = size.X;
			this.Height = size.Y;
		}
		#endregion
		#endregion
		/// <summary>
		/// Creates a new instance of <see cref="FRectangle"/> struct, with the specified <see cref="Rectangle"/>.
		/// </summary>
		/// <param name="rectangle">The "casted" <see cref="Rectangle"/>.</param>
		public FRectangle(Rectangle rectangle)
		{
			this.X = rectangle.X;
			this.Y = rectangle.Y;
			this.Width = rectangle.X;
			this.Height = rectangle.Y;
		}
		#endregion
		#region Operators
		/// <summary>
		/// Compares whether two <see cref="FRectangle"/> instances are equal.
		/// </summary>
		/// <param name="a"><see cref="FRectangle"/> instance on the left of the equal sign.</param>
		/// <param name="b"><see cref="FRectangle"/> instance on the right of the equal sign.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public static bool operator ==(FRectangle a, FRectangle b)
		{
			return ((a.X == b.X) && (a.Y == b.Y) && (a.Width == b.Width) && (a.Height == b.Height));
		}
		/// <summary>
		/// Compares whether two <see cref="FRectangle"/> instances are not equal.
		/// </summary>
		/// <param name="a"><see cref="FRectangle"/> instance on the left of the not equal sign.</param>
		/// <param name="b"><see cref="FRectangle"/> instance on the right of the not equal sign.</param>
		/// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
		public static bool operator !=(FRectangle a, FRectangle b) => !(a == b);
		/// <summary>
		/// Casts a <see cref="FRectangle"/> as a <see cref="Rectangle"/>. - J Mor
		/// </summary>
		/// <param name="fr">The <see cref="FRectangle"/> to cast.</param>
		public static explicit operator Rectangle(FRectangle fr) => new Rectangle((int)Math.Round(fr.X), (int)Math.Round(fr.Y),
																				  (int)Math.Round(fr.Width), (int)Math.Round(fr.Height));
		#endregion
		#region Public Methods
		#region Contains
		#region Int-Based
		/// <summary>
		/// Gets whether or not the provided coordinates lie within the bounds of this <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="x">The x coordinate of the point to check for containment.</param>
		/// <param name="y">The y coordinate of the point to check for containment.</param>
		/// <returns><c>true</c> if the provided coordinates lie inside this <see cref="FRectangle"/>; <c>false</c> otherwise.</returns>
		public bool Contains(int x, int y)
		{
			return ((((this.X <= x) && (x < (this.X + this.Width))) && (this.Y <= y)) && (y < (this.Y + this.Height)));
		}
		/// <summary>
		/// Gets whether or not the provided <see cref="FPoint"/> lies within the bounds of this <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="value">The coordinates to check for inclusion in this <see cref="FRectangle"/>.</param>
		/// <returns><c>true</c> if the provided <see cref="FPoint"/> lies inside this <see cref="FRectangle"/>; <c>false</c> otherwise.</returns>
		public bool Contains(Point value)
		{
			return ((((this.X <= value.X) && (value.X < (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y < (this.Y + this.Height)));
		}
		/// <summary>
		/// Gets whether or not the provided <see cref="FPoint"/> lies within the bounds of this <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="value">The coordinates to check for inclusion in this <see cref="FRectangle"/>.</param>
		/// <param name="result"><c>true</c> if the provided <see cref="FPoint"/> lies inside this <see cref="FRectangle"/>; <c>false</c> otherwise. As an output parameter.</param>
		public void Contains(ref Point value, out bool result)
		{
			result = ((((this.X <= value.X) && (value.X < (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y < (this.Y + this.Height)));
		}
		/// <summary>
		/// Gets whether or not the provided <see cref="FRectangle"/> lies within the bounds of this <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="value">The <see cref="FRectangle"/> to check for inclusion in this <see cref="FRectangle"/>.</param>
		/// <returns><c>true</c> if the provided <see cref="FRectangle"/>'s bounds lie entirely inside this <see cref="FRectangle"/>; <c>false</c> otherwise.</returns>
		public bool Contains(Rectangle value)
		{
			return ((((this.X <= value.X) && ((value.X + value.Width) <= (this.X + this.Width))) && (this.Y <= value.Y)) && ((value.Y + value.Height) <= (this.Y + this.Height)));
		}
		/// <summary>
		/// Gets whether or not the provided <see cref="FRectangle"/> lies within the bounds of this <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="value">The <see cref="FRectangle"/> to check for inclusion in this <see cref="FRectangle"/>.</param>
		/// <param name="result"><c>true</c> if the provided <see cref="FRectangle"/>'s bounds lie entirely inside this <see cref="FRectangle"/>; <c>false</c> otherwise. As an output parameter.</param>
		public void Contains(ref Rectangle value, out bool result)
		{
			result = ((((this.X <= value.X) && ((value.X + value.Width) <= (this.X + this.Width))) && (this.Y <= value.Y)) && ((value.Y + value.Height) <= (this.Y + this.Height)));
		}
		#endregion
		#region Float-Based
		/// <summary>
		/// Gets whether or not the provided coordinates lie within the bounds of this <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="x">The x coordinate of the point to check for containment.</param>
		/// <param name="y">The y coordinate of the point to check for containment.</param>
		/// <returns><c>true</c> if the provided coordinates lie inside this <see cref="FRectangle"/>; <c>false</c> otherwise.</returns>
		public bool Contains(float x, float y)
		{
			return ((((this.X <= x) && (x < (this.X + this.Width))) && (this.Y <= y)) && (y < (this.Y + this.Height)));
		}
		/// <summary>
		/// Gets whether or not the provided <see cref="FPoint"/> lies within the bounds of this <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="value">The coordinates to check for inclusion in this <see cref="FRectangle"/>.</param>
		/// <returns><c>true</c> if the provided <see cref="FPoint"/> lies inside this <see cref="FRectangle"/>; <c>false</c> otherwise.</returns>
		public bool Contains(FPoint value)
		{
			return ((((this.X <= value.X) && (value.X < (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y < (this.Y + this.Height)));
		}
		/// <summary>
		/// Gets whether or not the provided <see cref="FPoint"/> lies within the bounds of this <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="value">The coordinates to check for inclusion in this <see cref="FRectangle"/>.</param>
		/// <param name="result"><c>true</c> if the provided <see cref="FPoint"/> lies inside this <see cref="FRectangle"/>; <c>false</c> otherwise. As an output parameter.</param>
		public void Contains(ref FPoint value, out bool result)
		{
			result = ((((this.X <= value.X) && (value.X < (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y < (this.Y + this.Height)));
		}
		/// <summary>
		/// Gets whether or not the provided <see cref="Vector2"/> lies within the bounds of this <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="value">The coordinates to check for inclusion in this <see cref="FRectangle"/>.</param>
		/// <returns><c>true</c> if the provided <see cref="Vector2"/> lies inside this <see cref="FRectangle"/>; <c>false</c> otherwise.</returns>
		public bool Contains(Vector2 value)
		{
			return ((((this.X <= value.X) && (value.X < (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y < (this.Y + this.Height)));
		}
		/// <summary>
		/// Gets whether or not the provided <see cref="Vector2"/> lies within the bounds of this <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="value">The coordinates to check for inclusion in this <see cref="FRectangle"/>.</param>
		/// <param name="result"><c>true</c> if the provided <see cref="Vector2"/> lies inside this <see cref="FRectangle"/>; <c>false</c> otherwise. As an output parameter.</param>
		public void Contains(ref Vector2 value, out bool result)
		{
			result = ((((this.X <= value.X) && (value.X < (this.X + this.Width))) && (this.Y <= value.Y)) && (value.Y < (this.Y + this.Height)));
		}
		/// <summary>
		/// Gets whether or not the provided <see cref="FRectangle"/> lies within the bounds of this <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="value">The <see cref="FRectangle"/> to check for inclusion in this <see cref="FRectangle"/>.</param>
		/// <returns><c>true</c> if the provided <see cref="FRectangle"/>'s bounds lie entirely inside this <see cref="FRectangle"/>; <c>false</c> otherwise.</returns>
		public bool Contains(FRectangle value)
		{
			return ((((this.X <= value.X) && ((value.X + value.Width) <= (this.X + this.Width))) && (this.Y <= value.Y)) && ((value.Y + value.Height) <= (this.Y + this.Height)));
		}
		/// <summary>
		/// Gets whether or not the provided <see cref="FRectangle"/> lies within the bounds of this <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="value">The <see cref="FRectangle"/> to check for inclusion in this <see cref="FRectangle"/>.</param>
		/// <param name="result"><c>true</c> if the provided <see cref="FRectangle"/>'s bounds lie entirely inside this <see cref="FRectangle"/>; <c>false</c> otherwise. As an output parameter.</param>
		public void Contains(ref FRectangle value, out bool result)
		{
			result = ((((this.X <= value.X) && ((value.X + value.Width) <= (this.X + this.Width))) && (this.Y <= value.Y)) && ((value.Y + value.Height) <= (this.Y + this.Height)));
		}
		///// <summary>
		///// Gets whether or not the provided <see cref="FLineSegment"/> lies within the bounds of this <see cref="FRectangle"/>.
		///// </summary>
		///// <param name="value">The <see cref="FLineSegment"/> to check for inclusion in this <see cref="FRectangle"/>.</param>
		///// <returns><c>true</c> if the provided <see cref="FLineSegment"/>'s bounds lie entirely inside this <see cref="FRectangle"/>; <c>false</c> otherwise.</returns>
		//public bool Contains(FLineSegment value)
		//{
		//	return Contains(value.P1) && Contains(value.P2);
		//}
		///// <summary>
		///// Gets whether or not the provided <see cref="FLineSegment"/> lies within the bounds of this <see cref="FRectangle"/>.
		///// </summary>
		///// <param name="value">The <see cref="FLineSegment"/> to check for inclusion in this <see cref="FRectangle"/>.</param>
		///// <param name="result"><c>true</c> if the provided <see cref="FLineSegment"/>'s bounds lie entirely inside this <see cref="FRectangle"/>; <c>false</c> otherwise. As an output parameter.</param>
		//public void Contains(FLineSegment value, out bool result)
		//{
		//	result = Contains(value.P1) && Contains(value.P2);
		//}
		#endregion
		#endregion
		#region Equals
		/// <summary>
		/// Compares whether current instance is equal to specified <see cref="Object"/>.
		/// </summary>
		/// <param name="obj">The <see cref="Object"/> to compare.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public override bool Equals(object obj) => (obj is FRectangle) && this == ((FRectangle)obj);
		/// <summary>
		/// Compares whether current instance is equal to specified <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="other">The <see cref="FRectangle"/> to compare.</param>
		/// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
		public bool Equals(FRectangle other) => this == other;
		#endregion
		#region Inflation
		/// <summary>
		/// Adjusts the edges of this <see cref="FRectangle"/> by specified horizontal and vertical amounts. 
		/// </summary>
		/// <param name="horizontalAmount">Value to adjust the left and right edges.</param>
		/// <param name="verticalAmount">Value to adjust the top and bottom edges.</param>
		public void Inflate(int horizontalAmount, int verticalAmount)
		{
			X -= horizontalAmount;
			Y -= verticalAmount;
			Width += horizontalAmount * 2;
			Height += verticalAmount * 2;
		}
		/// <summary>
		/// Adjusts the edges of this <see cref="FRectangle"/> by specified horizontal and vertical amounts. 
		/// </summary>
		/// <param name="horizontalAmount">Value to adjust the left and right edges.</param>
		/// <param name="verticalAmount">Value to adjust the top and bottom edges.</param>
		public void Inflate(float horizontalAmount, float verticalAmount)
		{
			X -= horizontalAmount;
			Y -= verticalAmount;
			Width += horizontalAmount * 2;
			Height += verticalAmount * 2;
		}
		#endregion
		#region Intersection
		/// <summary>
		/// Gets whether or not the other <see cref="FRectangle"/> intersects with this rectangle.
		/// </summary>
		/// <param name="value">The other rectangle for testing.</param>
		/// <returns><c>true</c> if other <see cref="FRectangle"/> intersects with this <see cref="FRectangle"/>; <c>false</c> otherwise.</returns>
		public bool Intersects(FRectangle value)
		{
			return (!(value.Contains(TopLeft) && value.Contains(TopRight) && value.Contains(BottomLeft) && value.Contains(BottomRight)) && ((value.Contains(TopLeft) || value.Contains(TopRight) || value.Contains(BottomLeft) || value.Contains(BottomRight))));
			//return value.Left < Right &&
			//	   Left < value.Right &&
			//	   value.Top < Bottom &&
			//	   Top < value.Bottom;
		}
		/// <summary>
		/// Gets whether or not the other <see cref="FRectangle"/> intersects with this rectangle.
		/// </summary>
		/// <param name="value">The other rectangle for testing.</param>
		/// <param name="result"><c>true</c> if other <see cref="FRectangle"/> intersects with this rectangle; <c>false</c> otherwise. As an output parameter.</param>
		public void Intersects(ref FRectangle value, out bool result)
		{
			result = (!(value.Contains(TopLeft) && value.Contains(TopRight) && value.Contains(BottomLeft) && value.Contains(BottomRight)) && ((value.Contains(TopLeft) || value.Contains(TopRight) || value.Contains(BottomLeft) || value.Contains(BottomRight))));
			//result = value.Left < Right &&
			//	   Left < value.Right &&
			//	   value.Top < Bottom &&
			//	   Top < value.Bottom;
		}
		/// <summary>
		/// Creates a new <see cref="FRectangle"/> that contains overlapping region of two other rectangles.
		/// </summary>
		/// <param name="value1">The first <see cref="FRectangle"/>.</param>
		/// <param name="value2">The second <see cref="FRectangle"/>.</param>
		/// <returns>Overlapping region of the two rectangles.</returns>
		public static FRectangle Intersect(FRectangle value1, FRectangle value2)
		{
			if (value1.Intersects(value2))
			{
				float left_side = Math.Max(value1.X, value2.X);
				float right_side = Math.Min(value1.X + value1.Width, value2.X + value2.Width);
				float top_side = Math.Max(value1.Y, value2.Y);
				float bottom_side = Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);
				return new FRectangle(left_side, top_side, right_side - left_side, bottom_side - top_side);
			}
			else return new FRectangle(0, 0, 0, 0);
		}
		/// <summary>
		/// Creates a new <see cref="FRectangle"/> that contains overlapping region of two other rectangles.
		/// </summary>
		/// <param name="value1">The first <see cref="FRectangle"/>.</param>
		/// <param name="value2">The second <see cref="FRectangle"/>.</param>
		/// <param name="result">Overlapping region of the two rectangles as an output parameter.</param>
		public static void Intersect(ref FRectangle value1, ref FRectangle value2, out FRectangle result) => result = Intersect(value1, value2);
		#endregion
		#region Offset
		/// <summary>
		/// Changes the <see cref="Location"/> of this <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="offsetX">The x coordinate to add to this <see cref="FRectangle"/>.</param>
		/// <param name="offsetY">The y coordinate to add to this <see cref="FRectangle"/>.</param>
		public void Offset(int offsetX, int offsetY)
		{
			X += offsetX;
			Y += offsetY;
		}
		/// <summary>
		/// Changes the <see cref="Location"/> of this <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="offsetX">The x coordinate to add to this <see cref="FRectangle"/>.</param>
		/// <param name="offsetY">The y coordinate to add to this <see cref="FRectangle"/>.</param>
		public void Offset(float offsetX, float offsetY)
		{
			X += offsetX;
			Y += offsetY;
		}
		/// <summary>
		/// Changes the <see cref="Location"/> of this <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="amount">The x and y components to add to this <see cref="FRectangle"/>.</param>
		public void Offset(FPoint amount)
		{
			X += amount.X;
			Y += amount.Y;
		}
		/// <summary>
		/// Changes the <see cref="Location"/> of this <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="amount">The x and y components to add to this <see cref="FRectangle"/>.</param>
		public void Offset(Point amount)
		{
			X += amount.X;
			Y += amount.Y;
		}
		/// <summary>
		/// Changes the <see cref="Location"/> of this <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="amount">The x and y components to add to this <see cref="FRectangle"/>.</param>
		public void Offset(Vector2 amount)
		{
			X += amount.X;
			Y += amount.Y;
		}
		#endregion
		#region Union
		/// <summary>
		/// Creates a new <see cref="FRectangle"/> that completely contains two other rectangles.
		/// </summary>
		/// <param name="value1">The first <see cref="FRectangle"/>.</param>
		/// <param name="value2">The second <see cref="FRectangle"/>.</param>
		/// <returns>The union of the two rectangles.</returns>
		public static FRectangle Union(FRectangle value1, FRectangle value2)
		{
			float x = Math.Min(value1.X, value2.X);
			float y = Math.Min(value1.Y, value2.Y);
			return new FRectangle(x, y, Math.Max(value1.Right, value2.Right) - x, Math.Max(value1.Bottom, value2.Bottom) - y);
		}
		/// <summary>
		/// Creates a new <see cref="FRectangle"/> that completely contains two other rectangles.
		/// </summary>
		/// <param name="value1">The first <see cref="FRectangle"/>.</param>
		/// <param name="value2">The second <see cref="FRectangle"/>.</param>
		/// <param name="result">The union of the two rectangles as an output parameter.</param>
		public static void Union(ref FRectangle value1, ref FRectangle value2, out FRectangle result)
		{
			result.X = Math.Min(value1.X, value2.X);
			result.Y = Math.Min(value1.Y, value2.Y);
			result.Width = Math.Max(value1.Right, value2.Right) - result.X;
			result.Height = Math.Max(value1.Bottom, value2.Bottom) - result.Y;
		}
		#endregion
		#region Deconstructions
		/// <summary>
		/// Deconstruction method for <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void Deconstruct(out int x, out int y, out int width, out int height)
		{
			x = (int)Math.Round(X);
			y = (int)Math.Round(Y);
			width = (int)Math.Round(Width);
			height = (int)Math.Round(Height);
		}
		/// <summary>
		/// Deconstruction method for <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="width"></param>
		/// <param name="height"></param>
		public void Deconstruct(out float x, out float y, out float width, out float height)
		{
			x = X;
			y = Y;
			width = Width;
			height = Height;
		}
		#endregion
		#region GetExtremes
		/// <summary>
		/// Deconstruction method for <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="topLeft"></param>
		/// <param name="topRight"></param>
		/// <param name="bottomRight"></param>
		/// <param name="bottomLeft"></param>
		public void GetExtremes(out Point topLeft, out Point topRight, out Point bottomRight, out Point bottomLeft)
		{
			topLeft = TopLeft.ToPoint();
			topRight = TopRight.ToPoint();
			bottomRight = BottomRight.ToPoint();
			bottomLeft = BottomLeft.ToPoint();
		}
		/// <summary>
		/// Deconstruction method for <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="topLeft"></param>
		/// <param name="topRight"></param>
		/// <param name="bottomRight"></param>
		/// <param name="bottomLeft"></param>
		public void GetExtremes(out FPoint topLeft, out FPoint topRight, out FPoint bottomRight, out FPoint bottomLeft)
		{
			topLeft = TopLeft;
			topRight = TopRight;
			bottomRight = BottomRight;
			bottomLeft = BottomLeft;
		}
		/// <summary>
		/// Deconstruction method for <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="topLeft"></param>
		/// <param name="topRight"></param>
		/// <param name="bottomRight"></param>
		/// <param name="bottomLeft"></param>
		public void GetExtremes(out Vector2 topLeft, out Vector2 topRight, out Vector2 bottomRight, out Vector2 bottomLeft)
		{
			topLeft = TopLeft;
			topRight = TopRight;
			bottomRight = BottomRight;
			bottomLeft = BottomLeft;
		}
		/// <summary>
		/// Deconstruction method for <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="points"></param>
		public void GetExtremes(out Point[] points)
		{
			points = new Point[] { TopLeft.ToPoint(), TopRight.ToPoint(), BottomRight.ToPoint(), BottomLeft.ToPoint() };
		}
		/// <summary>
		/// Deconstruction method for <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="points"></param>
		public void GetExtremes(out FPoint[] points)
		{
			points = new FPoint[] { TopLeft, TopRight, BottomRight, BottomLeft };
		}
		/// <summary>
		/// Deconstruction method for <see cref="FRectangle"/>.
		/// </summary>
		/// <param name="points"></param>
		public void GetExtremes(out Vector2[] points)
		{
			points = new Vector2[] { TopLeft, TopRight, BottomRight, BottomLeft };
		}
		#endregion
		#region Misc. Overloads
		/// <summary>
		/// Gets the hash code of this <see cref="FRectangle"/>.
		/// </summary>
		/// <returns>Hash code of this <see cref="FRectangle"/>.</returns>
		public override int GetHashCode()
		{
			unchecked
			{
				var hash = 17;
				hash = hash * 23 + X.GetHashCode();
				hash = hash * 23 + Y.GetHashCode();
				hash = hash * 23 + Width.GetHashCode();
				hash = hash * 23 + Height.GetHashCode();
				return hash;
			}
		}
		/// <summary>
		/// Returns a <see cref="String"/> representation of this <see cref="FRectangle"/> in the format:
		/// {X:[<see cref="X"/>] Y:[<see cref="Y"/>] Width:[<see cref="Width"/>] Height:[<see cref="Height"/>]}
		/// </summary>
		/// <returns><see cref="String"/> representation of this <see cref="FRectangle"/>.</returns>
		public override string ToString()
		{
			return "{X:" + X + " Y:" + Y + " Width:" + Width + " Height:" + Height + "}";
		}
		#endregion
		#endregion
	}
}