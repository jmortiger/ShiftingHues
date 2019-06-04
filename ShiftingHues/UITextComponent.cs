using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShiftingHues.UI
{
    class UITextComponent : IObjDrawComponent
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
		
		/// <summary>
		/// The font to use.
		/// </summary>
        public SpriteFont font;

		/// <summary>
		/// The color to tint the text.
		/// </summary>
        public Color ColorTint { get; set; }
        #endregion

        #region Constructors

        public UITextComponent(
			string text, 
			SpriteFont font, 
			Vector2 posit = new Vector2(), 
			Color color = new Color())
        {
            this.text = text;
            this.font = font;
            this.posit = posit;
            this.ColorTint = color;
        }

        //public UITextComponent(string text, SpriteFont font)
        //{
        //    this.text = text;
        //    this.font = font;
        //    this.posit = Vector2.Zero;
        //    this.color = Color.WhiteSmoke;
        //}
        #endregion

        public void Draw(SpriteBatch batch, GameTime time = null)
        {
            batch.DrawString(font, text, posit, ColorTint);
            //throw new NotImplementedException();
        }
    }
}