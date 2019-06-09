using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShiftingHues.UI
{
    public class SpriteComponent : IObjDrawComponent
    {
        public Texture2D Sprite   { get; /*private*/ set; }
        public Rectangle DrawRect { get; /*private*/ set; }
        public bool IsVisible { get; /*private */set; }
        public Color Tint { get; set; }

        #region Ctors
        public SpriteComponent(Texture2D sprite, Rectangle drawRect, Color color)
        {
            this.Sprite = sprite;
            this.DrawRect = drawRect;
            this.Tint = color;
        }

        public SpriteComponent(Texture2D sprite, Rectangle drawRect)
            : this(sprite, drawRect, Color.White) { }
        #endregion

        public void Draw(SpriteBatch batch, GameTime time = null)
        {
            batch.Draw(Sprite, DrawRect, Tint);
        }
    }
}