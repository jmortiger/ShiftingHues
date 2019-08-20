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

		public Rectangle SourceRect { get => Sheet.SpriteSourceRects[SpriteIndex]; }
		#endregion

		#region Constructors
		public Sprite(SpriteSheet sheet, int spriteIndex)
		{
			this.Sheet = sheet;
			this.SpriteIndex = spriteIndex;
		}
		#endregion
	}
}