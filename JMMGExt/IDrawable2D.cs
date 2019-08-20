using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JMMGExt
{
	public interface IDrawable2D
	{
		event EventHandler<EventArgs> DrawOrderChanged;
		event EventHandler<EventArgs> VisibleChanged;
		int DrawOrder { get; }
		bool Visible { get; }
		void Draw(GameTime gameTime, SpriteBatch spriteBatch);
		void Draw(SpriteBatch spriteBatch, GameTime gameTime);
	}
}