using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShiftingHues.Graphics
{
	/// <summary>
	/// A container for advanced information about how to draw a <see cref="Texture2D"/>. 
	/// A structure containing the extra information the <see cref="SpriteBatch.Draw(Texture2D, Rectangle, Rectangle?, Color, float, Vector2, SpriteEffects, float)"/> method can accept.
	/// </summary>
	public struct DrawEffects2D
	{
		#region Fields and Properties
		/// <summary>
		/// The top-left of the <see cref="DestinationRectangle"/>.
		/// </summary>
		public Point Position
		{
			get => DestinationRectangle.Location;
			set => DestinationRectangle.Location = value;
		}

		public Rectangle DestinationRectangle/* { get; set; }*/;

		public Color Tint /*{ get; private set; }*/;

		public float Rotation /*{ get; private set; }*/;

		public Vector2 Origin /*{ get; private set; }*/;

		public SpriteEffects SpriteEffectOrientation /*{ get; private set; }*/;

		/// <summary>
		/// The layer to draw to. Must be between <c>0.0f</c> & <c>1.0f</c>. <c>0.0f</c> is the bottom layer.
		/// </summary>
		/// <remarks>https://www.dreamincode.net/forums/topic/395207-draw-textures-in-certain-orderlayer-depth/</remarks>
		public float LayerDepth /*{ get; private set; }*/;

		public bool WasInitialized { get; private set; }
		#endregion

		#region Constructors

		public DrawEffects2D(
			Rectangle DestinationRectangle/* = Rectangle.Empty*/,
			Color Tint/* = Color.White*/,
			Vector2 Origin/* = Vector2.Zero*/,
			float Rotation = 0f,
			SpriteEffects SpriteEffectOrientation = SpriteEffects.None,
			float LayerDepth = 0f)
		{
			this.DestinationRectangle = DestinationRectangle;
			this.Tint = Tint;
			this.Rotation = Rotation;
			this.Origin = Origin;
			this.SpriteEffectOrientation = SpriteEffectOrientation;
			this.LayerDepth = LayerDepth;

			this.WasInitialized = true;
		}

		#region Overloads

		public DrawEffects2D(
			//Rectangle DestinationRectangle/* = Rectangle.Empty*/,
			Color Tint/* = Color.White*/,
			Vector2 Origin/* = Vector2.Zero*/,
			float Rotation = 0f,
			SpriteEffects SpriteEffectOrientation = SpriteEffects.None,
			float LayerDepth = 0f)
			: this(Rectangle.Empty, Tint, Origin, Rotation, SpriteEffectOrientation, LayerDepth) { }

		public DrawEffects2D(
			Rectangle DestinationRectangle/* = Rectangle.Empty*/,
			//Color Tint/* = Color.White*/,
			Vector2 Origin/* = Vector2.Zero*/,
			float Rotation = 0f,
			SpriteEffects SpriteEffectOrientation = SpriteEffects.None,
			float LayerDepth = 0f)
			: this(DestinationRectangle, Color.White, Origin, Rotation, SpriteEffectOrientation, LayerDepth) { }

		public DrawEffects2D(
			Rectangle DestinationRectangle/* = Rectangle.Empty*/,
			Color Tint/* = Color.White*/,
			//Vector2 Origin/* = Vector2.Zero*/,
			float Rotation = 0f,
			SpriteEffects SpriteEffectOrientation = SpriteEffects.None,
			float LayerDepth = 0f)
			: this(DestinationRectangle, Tint, new Vector2(float.NaN), Rotation, SpriteEffectOrientation, LayerDepth) { }

		public DrawEffects2D(
			//Rectangle DestinationRectangle/* = Rectangle.Empty*/,
			//Color Tint/* = Color.White*/,
			Vector2 Origin/* = Vector2.Zero*/,
			float Rotation = 0f,
			SpriteEffects SpriteEffectOrientation = SpriteEffects.None,
			float LayerDepth = 0f)
			: this(Rectangle.Empty, Color.White, Origin, Rotation, SpriteEffectOrientation, LayerDepth) { }

		public DrawEffects2D(
			Rectangle DestinationRectangle/* = Rectangle.Empty*/,
			//Color Tint/* = Color.White*/,
			//Vector2 Origin/* = Vector2.Zero*/,
			float Rotation = 0f,
			SpriteEffects SpriteEffectOrientation = SpriteEffects.None,
			float LayerDepth = 0f)
			: this(DestinationRectangle, Color.White, new Vector2(float.NaN), Rotation, SpriteEffectOrientation, LayerDepth) { }

		public DrawEffects2D(
			//Rectangle DestinationRectangle/* = Rectangle.Empty*/,
			Color Tint/* = Color.White*/,
			//Vector2 Origin/* = Vector2.Zero*/,
			float Rotation = 0f,
			SpriteEffects SpriteEffectOrientation = SpriteEffects.None,
			float LayerDepth = 0f)
			: this(Rectangle.Empty, Tint, new Vector2(float.NaN), Rotation, SpriteEffectOrientation, LayerDepth) { }
		#endregion
		#endregion

		#region Methods

		#endregion
	}
}