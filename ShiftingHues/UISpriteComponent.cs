using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShiftingHues.UI
{
    class UISpriteComponent : IObjDrawComponent
    {
        public Texture2D Sprite   { get; /*private*/ set; }
        public Rectangle DrawRect { get; /*private*/ set; }
        public Color ColorTint { get; set; }

        #region Ctors
        public UISpriteComponent(Texture2D sprite, Rectangle drawRect, Color color)
        {
            this.Sprite = sprite;
            this.DrawRect = drawRect;
            this.ColorTint = color;
        }

        public UISpriteComponent(Texture2D sprite, Rectangle drawRect)
            : this(sprite, drawRect, Color.White) { }
        #endregion

        public void Draw(SpriteBatch batch, GameTime time = null)
        {
            batch.Draw(Sprite, DrawRect, ColorTint);
        }
    }
}