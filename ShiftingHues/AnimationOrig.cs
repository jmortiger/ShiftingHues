using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShiftingHues.Obsolete.Graphics
{
	public enum AnimationState
	{
		Active = 0,
		Inactive = 1,
		ToRemove = 2
	}

	/// <summary>
	/// TODO: Finish Docs
	/// </summary>
	public class AnimationOrig
	{
		#region Fields

		/// <summary>
		/// Counts time since last frame. - J Mor
		/// </summary>
		private float timeSinceLastFrameChange;
		#endregion

		#region Properties
		/// <summary>
		/// The area on the texture that will be drawn.
		/// </summary>
		public Rectangle CurrImgSourceRectangle { get => SpriteSheet.CurrImgSourceRectangle; }

		/// <summary>
		/// The area of the screen this will be drawn to.
		/// </summary>
		public Rectangle CurrentScreenBox { get; set; }
		public float SecondsPerFrame { get; set; }
		public float FPS { get => 1.0f / SecondsPerFrame; set => SecondsPerFrame = 1.0f / value; }
		public bool IsLooped { get; set; }
		public int TimesLooped { get; set; }
		//public OriginPosition OriginLocation { get => originLocation; }

		public SpriteSheetOrig SpriteSheet { get; }
		public AnimationState State { get; set; }
		public int SpritesInSheet { get => SpriteSheet.SpritesInSheet; }
		#endregion

		#region Constructor
		//private Animation(OriginPosition originLocation, SpriteEffects spriteEffect, Vector2 originOfRotation)
		//{
		//	this.originLocation = originLocation;
		//	this.spriteEffect = spriteEffect;
		//	this.originOfRotation = originOfRotation;
		//}
		//public Animation(Texture2D spriteSheet, Rectangle currentFrameSpriteBox, int spritesInSheet, Rectangle currentScreenBox)
		//{
		//	this.currentFrameSpriteBox = currentFrameSpriteBox;
		//	this.spritesInSheet = spritesInSheet;
		//	this.currentScreenBox = currentScreenBox;
		//}
		public AnimationOrig(
			Texture2D spriteSheet,
			int rows, int columns,
			int spriteWidth, int spriteHeight,
			int spritesInSheet,
			Point locationOnScreen,
			int projectedWidth, int projectedHeight)
		{
			//this.CurrImgSourceRectangle = new Rectangle(0, 0, spriteWidth, spriteHeight);
			//this.SpritesInSheet = spritesInSheet;
			this.CurrentScreenBox = new Rectangle(locationOnScreen.X, locationOnScreen.Y, projectedWidth, projectedHeight);

			this.SpriteSheet = new SpriteSheetOrig(spriteSheet, rows, columns, spriteWidth, spriteHeight, spritesInSheet);
			this.State = AnimationState.Inactive;
			Initialize();
		}
		/// <summary>
		/// TODO: Finish Docs
		/// </summary>
		/// <param name="spriteSheet"></param>
		/// <param name="rows"></param>
		/// <param name="columns"></param>
		/// <param name="spriteWidth"></param>
		/// <param name="spriteHeight"></param>
		/// <param name="spritesInSheet"></param>
		/// <param name="locationOnScreen"></param>
		/// <param name="projectedWidth"></param>
		/// <param name="projectedHeight"></param>
		/// <param name="animFPS">The frames per second of this animation.</param>
		public AnimationOrig(
			Texture2D spriteSheet,
			int rows, int columns,
			int spriteWidth, int spriteHeight,
			int spritesInSheet,
			Point locationOnScreen,
			int projectedWidth, int projectedHeight,
			float animFPS)
			: this(
				  new SpriteSheetOrig(spriteSheet, rows, columns, spriteWidth, spriteHeight, spritesInSheet),
				  locationOnScreen,
				  projectedWidth, projectedHeight,
				  animFPS)
		{
			////this.CurrImgSourceRectangle = new Rectangle(0, 0, spriteWidth, spriteHeight);
			////this.SpritesInSheet = spritesInSheet;
			//this.CurrentScreenBox = new Rectangle(locationOnScreen.X, locationOnScreen.Y, projectedWidth, projectedHeight);

			//this.SpriteSheet = new SpriteSheet(spriteSheet, rows, columns, spriteWidth, spriteHeight, spritesInSheet);
			//this.State = AnimationState.Inactive;
			//Initialize(animFPS);
		}

		public AnimationOrig(
			SpriteSheetOrig sheet,
			Point locationOnScreen,
			int projectedWidth,
			int projectedHeight,
			float animFPS)
		{
			this.SpriteSheet = sheet;

			//this.CurrImgSourceRectangle = sheet.CurrImgSourceRectangle;
			//this.SpritesInSheet = sheet.SpritesInSheet;
			this.CurrentScreenBox = new Rectangle(locationOnScreen.X, locationOnScreen.Y, projectedWidth, projectedHeight);

			this.State = AnimationState.Inactive;
			Initialize(animFPS);
		}
		#endregion

		#region Methods

		private void Initialize()
		{
			SpriteSheet.CurrentFrame = 0;
			SecondsPerFrame = 1.0f / (float)60;
			timeSinceLastFrameChange = 0f;
		}
		private void Initialize(float fps)
		{
			SpriteSheet.CurrentFrame = 0;
			SecondsPerFrame = 1.0f / (float)fps;
			timeSinceLastFrameChange = 0f;
		}

		public void Update(GameTime gameTime)
		{
			if (State == AnimationState.Inactive)
				State = AnimationState.Active;
			if (State == AnimationState.ToRemove)
				return; // If the animation is set to be removed, then end the update. - J Mor

			// Add to the time counter. - J Mor
			timeSinceLastFrameChange += (float)gameTime.ElapsedGameTime.TotalSeconds;

			// If enough time has gone by to actually flip frames... - J Mor
			if (timeSinceLastFrameChange >= SecondsPerFrame)
			{
				// ...then update the frame, and... - J Mor
				SpriteSheet.CurrentFrame++;
				// ...if the current frame is the last, ... - J Mor
				if (SpriteSheet.CurrentFrame >= SpriteSheet.SpritesInSheet)
				{
					if (IsLooped) // ...and the animation is set to wrap... - J Mor
					{
						// ...then update the frame # & the times looped. - J Mor
						SpriteSheet.CurrentFrame = 0;
						TimesLooped++;
					}
					else // if not, the animation is done, and should be removed soon. - J Mor
					{
						State = AnimationState.ToRemove;
						SpriteSheet.CurrentFrame--; // To avoid IndexOutOfBounds error. - J Mor
					}
				}
				SpriteSheet.UpdateImgSourceRectangle();
				// Remove one "frame" worth of time
				timeSinceLastFrameChange -= SecondsPerFrame;
			}
		}

		public void Draw(SpriteBatch sb)
		{
			if (State == AnimationState.ToRemove)
				return; // If the animation is set to be removed, then leave. - J Mor
			if (SpriteSheet.Scale == Vector2.One)
				sb.Draw(SpriteSheet.Sheet, CurrentScreenBox, SpriteSheet.CurrImgSourceRectangle, SpriteSheet.ColorMask, SpriteSheet.Rotation, SpriteSheet.OriginOfRotation, SpriteSheet.SpriteEffect, SpriteSheet.LayerDepth);
			else
				sb.Draw(SpriteSheet.Sheet, new Vector2(CurrentScreenBox.X, CurrentScreenBox.Y), SpriteSheet.CurrImgSourceRectangle, SpriteSheet.ColorMask, SpriteSheet.Rotation, SpriteSheet.OriginOfRotation, SpriteSheet.Scale, SpriteSheet.SpriteEffect, SpriteSheet.LayerDepth);
			// sb.Draw(spriteSheet, new Vector2(currentScreenBox.X, currentScreenBox.Y), currentScreenBox, currentFrameSpriteBox, originOfRotation, Rotation, Scale, Color.White, SpriteEffects.None, 0);
		}
		#endregion
	}
}