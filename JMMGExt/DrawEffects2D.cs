using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JMMGExt.Graphics
{
	/// <summary>
	/// A container for advanced information about how to draw a <see cref="Texture2D"/>. 
	/// A structure containing the extra information the <see cref="SpriteBatch.Draw(Texture2D, Rectangle, Rectangle?, Color, float, Vector2, SpriteEffects, float)"/> method can accept.
	/// </summary>
	public struct DrawEffects2D
	{
		#region Fields and Properties

		public static readonly DrawEffects2D DefaultEffects = new DrawEffects2D(Vector2.One, Color.White, Vector2.Zero, 0f, SpriteEffects.None, 0f);

		public Vector2 Scale;

		public Color Tint;

		public float Rotation;

		public Vector2 Origin;

		public SpriteEffects SpriteEffectOrientation;

		/// <summary>
		/// The layer to draw to. Must be between <c>0.0f</c> & <c>1.0f</c>. <c>0.0f</c> is the bottom layer.
		/// </summary>
		/// <remarks>https://www.dreamincode.net/forums/topic/395207-draw-textures-in-certain-orderlayer-depth/</remarks>
		public float LayerDepth;

		public bool WasInitialized { get; private set; }
		#endregion

		#region Constructors

		public DrawEffects2D(
			Vector2 Scale,
			Color Tint,
			Vector2 Origin,
			float Rotation = 0f,
			SpriteEffects SpriteEffectOrientation = SpriteEffects.None,
			float LayerDepth = 0f)
		{
			this.Scale = Scale;
			this.Tint = Tint;
			this.Rotation = Rotation;
			this.Origin = Origin;
			this.SpriteEffectOrientation = SpriteEffectOrientation;
			this.LayerDepth = LayerDepth;

			this.WasInitialized = true;
		}

		#region Overloads

		public DrawEffects2D(
			//Vector2 Scale,
			Color Tint,
			Vector2 Origin,
			float Rotation = 0f,
			SpriteEffects SpriteEffectOrientation = SpriteEffects.None,
			float LayerDepth = 0f)
			: this(Vector2.One, Tint, Origin, Rotation, SpriteEffectOrientation, LayerDepth) { }

		public DrawEffects2D(
			Vector2 Scale,
			//Color Tint,
			Vector2 Origin,
			float Rotation = 0f,
			SpriteEffects SpriteEffectOrientation = SpriteEffects.None,
			float LayerDepth = 0f)
			: this(Scale, Color.White, Origin, Rotation, SpriteEffectOrientation, LayerDepth) { }

		public DrawEffects2D(
			Vector2 Scale,
			Color Tint,
			//Vector2 Origin,
			float Rotation = 0f,
			SpriteEffects SpriteEffectOrientation = SpriteEffects.None,
			float LayerDepth = 0f)
			: this(Scale, Tint, Vector2.Zero, Rotation, SpriteEffectOrientation, LayerDepth) { }

		//public DrawEffects2D(
		//	//Vector2 Scale,
		//	//Color Tint,
		//	Vector2 Origin,
		//	float Rotation = 0f,
		//	SpriteEffects SpriteEffectOrientation = SpriteEffects.None,
		//	float LayerDepth = 0f)
		//	: this(Vector2.One, Color.White, Origin, Rotation, SpriteEffectOrientation, LayerDepth) { }

		public DrawEffects2D(
			Vector2 Scale,
			//Color Tint,
			//Vector2 Origin,
			float Rotation = 0f,
			SpriteEffects SpriteEffectOrientation = SpriteEffects.None,
			float LayerDepth = 0f)
			: this(Scale, Color.White, Vector2.Zero, Rotation, SpriteEffectOrientation, LayerDepth) { }

		public DrawEffects2D(
			//Vector2 Scale,
			Color Tint,
			//Vector2 Origin,
			float Rotation = 0f,
			SpriteEffects SpriteEffectOrientation = SpriteEffects.None,
			float LayerDepth = 0f)
			: this(Vector2.One, Tint, Vector2.Zero, Rotation, SpriteEffectOrientation, LayerDepth) { }

		///// <summary>
		///// Sets parameters to their preferred defaults (instead of setting Scale to {0, 0}, etc.).
		///// </summary>
		///// <param name="setDefaults">Does nothing, is just here to differentiate from the default constructor.</param>
		//public DrawEffects2D(int setDefaults)
		//	: this(Vector2.One, Color.White, Vector2.Zero, 0f, SpriteEffects.None, 0f) { }
		#endregion
		#endregion

		#region Methods

		public void SetDestinationRectangleSize(FRectangle sourceRectangle, Vector2 desiredSize)
		{
			Scale.X = desiredSize.X / sourceRectangle.Width;
			Scale.Y = desiredSize.Y / sourceRectangle.Height;
		}

		public void SetDestinationRectangleSize(Rectangle sourceRectangle, Point desiredSize)
		{
			Scale.X = desiredSize.X / sourceRectangle.Width;
			Scale.Y = desiredSize.Y / sourceRectangle.Height;
		}

		public FRectangle GetDestinationRectangle(FPoint location, Vector2 sourceSize) => new FRectangle(location, new Vector2(sourceSize.X * Scale.X, sourceSize.Y * Scale.Y));
		public FRectangle GetDestinationRectangle(FPoint location, FPoint sourceSize) => new FRectangle(location, new Vector2(sourceSize.X * Scale.X, sourceSize.Y * Scale.Y));
		public Rectangle GetDestinationRectangle(Point location, Point sourceSize) => new Rectangle(location, new Point((int)(sourceSize.X * Scale.X), (int)(sourceSize.Y * Scale.Y)));
		#endregion
	}
}