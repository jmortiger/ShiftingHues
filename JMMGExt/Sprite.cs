using Microsoft.Xna.Framework;

namespace JMMGExt.Graphics
{
	public class Sprite
	{
		#region Fields and Properties
		public SpriteSheet Sheet { get; private set; }

		/// <summary>
		/// The index of this sprite in the <see cref="Sheet"/>.
		/// </summary>
		public int SpriteIndex { get; private set; }

		public readonly DrawEffects2D SuggestedDrawEffects;

		public Rectangle SourceRect { get => Sheet.SpriteSourceRects[SpriteIndex]; }
		#endregion

		public Sprite(
			SpriteSheet sheet, 
			int spriteIndex, 
			DrawEffects2D? suggestedEffect = null)
		{
			this.Sheet = sheet;
			this.SpriteIndex = spriteIndex;
			this.SuggestedDrawEffects = suggestedEffect ?? DrawEffects2D.DefaultEffects;
		}
	}
}