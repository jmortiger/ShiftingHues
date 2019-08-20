using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShiftingHues.UI
{
    public class TextComponent : IObjDrawComponent
    {
        #region Fields and Properties
		/// <summary>
		/// The text's on screen position.
		/// </summary>
        public Vector2 posit;
		
		/// <summary>
		/// The text to draw.
		/// </summary>
        public string text;

        public bool isCentered;
		
		/// <summary>
		/// The font to use.
		/// </summary>
        public SpriteFont font;

        public bool IsVisible { get; /*private*/ set; }

		/// <summary>
		/// The color to tint the text.
		/// </summary>
        public Color Tint { get; set; }

        public Rectangle Bounds { get; private set; }
        #endregion

        #region Constructors

        public TextComponent(
            string text,
            SpriteFont font,
            Vector2 posit = new Vector2(),
            Color color = new Color(),
            bool isCentered = false)
        {
            this.Bounds = Rectangle.Empty;
            this.text = text;
            this.font = font;
            this.posit = posit;
            this.Tint = color;
            this.isCentered = isCentered;
        }

        public TextComponent(
            Rectangle bounds,
            string text,
            SpriteFont font,
            Vector2 posit = new Vector2(),
            Color color = new Color(),
            bool isCentered = false)
        {
            this.Bounds = bounds;
            this.text = text;
            this.font = font;
            this.posit = posit;
            this.Tint = color;
            this.isCentered = isCentered;
        }

        //public UITextComponent(string text, SpriteFont font)
        //{
        //    this.text = text;
        //    this.font = font;
        //    this.posit = Vector2.Zero;
        //    this.color = Color.WhiteSmoke;
        //}
        #endregion
        /// <summary>
        /// TODO: Finish Docs
        /// TODO: Finish handling text bound to a box
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="time"></param>
        public void Draw(SpriteBatch batch, GameTime time = null)
        {
            Vector2 posit = this.posit;
            if (isCentered)
            {
                posit = new Vector2(this.posit.X - font.MeasureString(text).X / 2, this.posit.Y);
            }
            batch.DrawString(font, text, posit, Tint);
            //throw new NotImplementedException();
        }
    }
}