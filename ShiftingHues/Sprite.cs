using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShiftingHues.Graphics
{
	public class Sprite
	{
		#region Fields and Properties
		public SpriteSheet Sheet { get; private set; }

		/// <summary>
		/// The index of this sprite in the <see cref="Sheet"/>.
		/// </summary>
		public int SpriteIndex { get; private set; }

		public Rectangle SourceRect { get => Sheet.SpriteSourceRects[SpriteIndex]; }
		#endregion

		#region Constructors
		public Sprite(SpriteSheet sheet, int spriteIndex)
		{
			this.Sheet = sheet;
			this.SpriteIndex = spriteIndex;
		}
		#endregion

		#region Methods

		#endregion
	}

	public class SpriteInstance
	{
		#region Fields & Properties

		public Sprite Sprite { get; }

		private DrawEffects2D drawEffects;
		public DrawEffects2D DrawEffects { get => drawEffects; private set => drawEffects = value; }

		public Rectangle SourceRect { get => Sprite.SourceRect; }

		//private Rectangle destinationRectangle;
		public Rectangle DestinationRectangle { get => drawEffects.DestinationRectangle; set => drawEffects.DestinationRectangle = value; }

		public Point Position
		{
			get => DestinationRectangle.Location;
			set => drawEffects.DestinationRectangle.Location = value;
		}
		#endregion

		//public bool UsePosition { get; set; } = false;

		public SpriteInstance(Sprite sprite, DrawEffects2D drawEffects, Rectangle destinationRectangle)
		{
			this.Sprite = sprite;
			this.DrawEffects = drawEffects;
			this.DestinationRectangle = destinationRectangle;
		}

		public SpriteInstance(Sprite sprite, DrawEffects2D drawEffects, Rectangle? destinationRectangle = null)
		{
			this.Sprite = sprite;
			this.DrawEffects = drawEffects;
			this.DestinationRectangle = destinationRectangle ?? drawEffects.DestinationRectangle/*sprite.SourceRect*/;
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="orig"><see cref="SpriteInstance"/> to copy.</param>
		/// <remarks>I want the <see cref="Sprite"/> to be a reference, and since <see cref="DrawEffects2D"/> and <see cref="Rectangle"/> are structs, it will copy the values.</remarks>
		public SpriteInstance(SpriteInstance orig)
			: this(orig.Sprite, orig.drawEffects, orig.DestinationRectangle) { }
		#region Methods

		public void DrawSprite(SpriteBatch batch, DrawEffects2D? drawEffects, bool usePosit = false)
		{
			if (!usePosit)
				batch.Draw(
					Sprite.Sheet.Texture,
					DestinationRectangle,
					Sprite.SourceRect,
					drawEffects?.Tint						?? this.DrawEffects.Tint,
					drawEffects?.Rotation					?? this.DrawEffects.Rotation,
					drawEffects?.Origin						?? this.DrawEffects.Origin,
					drawEffects?.SpriteEffectOrientation	?? this.DrawEffects.SpriteEffectOrientation,
					drawEffects?.LayerDepth					?? this.DrawEffects.LayerDepth);
			else
				batch.Draw(
					Sprite.Sheet.Texture,
					new Vector2(drawEffects?.DestinationRectangle.X ?? Position.X, drawEffects?.DestinationRectangle.Y ?? Position.Y),
					Sprite.SourceRect,
					drawEffects?.Tint						?? this.DrawEffects.Tint,
					drawEffects?.Rotation					?? this.DrawEffects.Rotation,
					drawEffects?.Origin						?? this.DrawEffects.Origin,
					Vector2.One,
					drawEffects?.SpriteEffectOrientation	?? this.DrawEffects.SpriteEffectOrientation,
					drawEffects?.LayerDepth					?? this.DrawEffects.LayerDepth);
		}

		public void DrawSprite(SpriteBatch batch, bool usePosit = false)
		{
			if (!usePosit)
				batch.Draw(
					Sprite.Sheet.Texture,
					DestinationRectangle,
					Sprite.SourceRect,
					DrawEffects.Tint,
					DrawEffects.Rotation,
					DrawEffects.Origin,
					DrawEffects.SpriteEffectOrientation,
					DrawEffects.LayerDepth);
			else
				batch.Draw(
					Sprite.Sheet.Texture,
					new Vector2(Position.X, Position.Y),
					Sprite.SourceRect,
					DrawEffects.Tint,
					DrawEffects.Rotation,
					DrawEffects.Origin,
					Vector2.One,
					DrawEffects.SpriteEffectOrientation,
					DrawEffects.LayerDepth);
		}

		public void DrawSpriteBasic(SpriteBatch batch)
		{
			batch.Draw(
				Sprite.Sheet.Texture,
				DestinationRectangle,
				Sprite.SourceRect,
				DrawEffects.Tint);
		}

		public void DrawSpriteBasic(SpriteBatch batch, Rectangle destinationRectangle)
		{
			batch.Draw(
				Sprite.Sheet.Texture,
				destinationRectangle,
				Sprite.SourceRect,
				DrawEffects.Tint);
		}

		public void DrawSpriteBasic(SpriteBatch batch, Rectangle destinationRectangle, Color tint)
		{
			batch.Draw(
				Sprite.Sheet.Texture,
				destinationRectangle,
				Sprite.SourceRect,
				tint);
		}

		public void DrawSpriteBasic(SpriteBatch batch, Vector2 position)
		{
			batch.Draw(
				Sprite.Sheet.Texture,
				position,
				Sprite.SourceRect,
				DrawEffects.Tint);
		}

		public void DrawSpriteBasic(SpriteBatch batch, Vector2 position, Color tint)
		{
			batch.Draw(
				Sprite.Sheet.Texture,
				position,
				Sprite.SourceRect,
				tint);
		}
		#endregion
	}
}