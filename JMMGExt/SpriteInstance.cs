using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JMMGExt;

namespace JMMGExt.Graphics
{
	public class SpriteInstance
	{
		#region Fields & Properties

		public Sprite Sprite { get; }

		public DrawEffects2D DrawEffects;

		public Rectangle SourceRect { get => Sprite.SourceRect; }

		public Rectangle DestinationRectangle
		{
			get => DrawEffects.GetDestinationRectangle(PositionAsPoint, SourceRect.Size);
			set
			{
				DrawEffects.SetDestinationRectangleSize(SourceRect, value.Size);
				PositionAsPoint = value.Location;
			}
		}

		public FPoint Position { get; set; }

		public Point PositionAsPoint { get => Position.ToPoint(); set => Position = (FPoint)value; }
		#endregion

		#region Constructors

		private SpriteInstance(Sprite sprite, DrawEffects2D? drawEffects = null)
		{
			this.Sprite = sprite;
			this.DrawEffects = drawEffects ?? DrawEffects2D.DefaultEffects;
		}

		public SpriteInstance(
			Sprite sprite,
			DrawEffects2D? drawEffects = null,
			Rectangle? destinationRectangle = null)
			: this(sprite, drawEffects)
		{
			this.DestinationRectangle = destinationRectangle ?? sprite.SourceRect;
		}

		public SpriteInstance(
			Sprite sprite,
			DrawEffects2D? drawEffects,
			FPoint position)
			: this(sprite, drawEffects)
		{
			this.Position = position;
		}

		/// <summary>
		/// Copy constructor
		/// </summary>
		/// <param name="orig"><see cref="SpriteInstance"/> to copy.</param>
		/// <remarks>I want the <see cref="Sprite"/> to be a reference, and since <see cref="DrawEffects2D"/> and <see cref="Rectangle"/> are structs, it will copy the values.</remarks>
		public SpriteInstance(SpriteInstance orig)
			: this(orig.Sprite, orig.DrawEffects, orig.DestinationRectangle) { }
		#endregion

		#region Methods
		#region DrawSprite

		public void DrawSprite(SpriteBatch batch, DrawEffects2D? drawEffects, Rectangle destinationRectangle)
		{
			batch.Draw(
				Sprite.Sheet.Texture,
				destinationRectangle,
				Sprite.SourceRect,
				drawEffects?.Tint ?? this.DrawEffects.Tint,
				drawEffects?.Rotation ?? this.DrawEffects.Rotation,
				drawEffects?.Origin ?? this.DrawEffects.Origin,
				drawEffects?.SpriteEffectOrientation ?? this.DrawEffects.SpriteEffectOrientation,
				drawEffects?.LayerDepth ?? this.DrawEffects.LayerDepth);
		}

		public void DrawSprite(SpriteBatch batch, DrawEffects2D? drawEffects, Point destinationPoint)
		{
			batch.Draw(
				Sprite.Sheet.Texture,
				new Vector2(destinationPoint.X, destinationPoint.Y),
				Sprite.SourceRect,
				drawEffects?.Tint ?? this.DrawEffects.Tint,
				drawEffects?.Rotation ?? this.DrawEffects.Rotation,
				drawEffects?.Origin ?? this.DrawEffects.Origin,
				drawEffects?.Scale ?? Vector2.One,
				drawEffects?.SpriteEffectOrientation ?? this.DrawEffects.SpriteEffectOrientation,
				drawEffects?.LayerDepth ?? this.DrawEffects.LayerDepth);
		}

		public void DrawSprite(SpriteBatch batch, DrawEffects2D? drawEffects, bool usePosit = true)
		{
			if (!usePosit)
				batch.Draw(
					Sprite.Sheet.Texture,
					this.DestinationRectangle,
					Sprite.SourceRect,
					drawEffects?.Tint ?? this.DrawEffects.Tint,
					drawEffects?.Rotation ?? this.DrawEffects.Rotation,
					drawEffects?.Origin ?? this.DrawEffects.Origin,
					drawEffects?.SpriteEffectOrientation ?? this.DrawEffects.SpriteEffectOrientation,
					drawEffects?.LayerDepth ?? this.DrawEffects.LayerDepth);
			else
				batch.Draw(
					Sprite.Sheet.Texture,
					Position.ToVector2(),
					Sprite.SourceRect,
					drawEffects?.Tint ?? this.DrawEffects.Tint,
					drawEffects?.Rotation ?? this.DrawEffects.Rotation,
					drawEffects?.Origin ?? this.DrawEffects.Origin,
					drawEffects?.Scale ?? Vector2.One,
					drawEffects?.SpriteEffectOrientation ?? this.DrawEffects.SpriteEffectOrientation,
					drawEffects?.LayerDepth ?? this.DrawEffects.LayerDepth);
		}

		public void DrawSprite(SpriteBatch batch, bool usePosit = true)
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
					Position.ToVector2(),
					Sprite.SourceRect,
					DrawEffects.Tint,
					DrawEffects.Rotation,
					DrawEffects.Origin,
					DrawEffects.Scale,
					DrawEffects.SpriteEffectOrientation,
					DrawEffects.LayerDepth);
		}
		#endregion

		#region DrawSpriteBasic
		public void DrawSpriteBasic(SpriteBatch batch)
		{
			batch.Draw(
				Sprite.Sheet.Texture,
				DestinationRectangle,
				Sprite.SourceRect,
				DrawEffects.Tint);
		}

		public void DrawSpriteBasic(SpriteBatch batch, Rectangle destinationRectangle, Color? tint = null)
		{
			batch.Draw(
				Sprite.Sheet.Texture,
				destinationRectangle,
				Sprite.SourceRect,
				tint ?? DrawEffects.Tint);
		}

		public void DrawSpriteBasic(SpriteBatch batch, Vector2 position, Color? tint = null)
		{
			batch.Draw(
				Sprite.Sheet.Texture,
				position,
				Sprite.SourceRect,
				tint ?? DrawEffects.Tint);
		}
		#endregion
		#endregion
	}
}