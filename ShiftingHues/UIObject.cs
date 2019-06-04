using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShiftingHues.UI
{
	public class UIObjEventArgs
	{
		public readonly object sender;
		public readonly Type type;
	}
	public class UIObject
	{
		#region Fields and Properties

		public Rectangle Bounds { get; private set; }

        public IObjDrawComponent[] drawableComponents;

        public IObjUpdateComponent[] updateableComponents;
        #endregion

        #region Constructors

        //public UIObject(Rectangle bounds)
		//{
			//this.Bounds = bounds;
		//}

        public UIObject(Rectangle bounds, IObjDrawComponent[] drawableComponents = null, IObjUpdateComponent[] updateableComponents = null)
        {
            this.Bounds = bounds;
            this.drawableComponents = drawableComponents;
            this.updateableComponents = updateableComponents;
        }

        public UIObject(Rectangle bounds, IObjDrawComponent drawableComponent = null, IObjUpdateComponent updateableComponent = null)
        {
            this.Bounds = bounds;
            this.drawableComponents = new IObjDrawComponent[]{ drawableComponent };
            this.updateableComponents = new IObjUpdateComponent[] { updateableComponent };
        }
        #endregion

        #region Methods
        /// <summary>
        /// TODO: Finish Doc
        /// </summary>
        /// <param name="time"></param>
        /// <remarks></remarks>
        public void Update(GameTime time)
        {
            for (int i = 0; i < updateableComponents?.Length; i++)
            {
                updateableComponents?[i]?.Update(time, this);
            }
        }

        /// <summary>
        /// TODO: Finish Doc
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="time"></param>
        /// <remarks>Everything from button prompts to buttons to text needs to be drawn, but are all drawn different ways.</remarks>
        public void Draw(SpriteBatch batch, GameTime time = null)
        {
            for (int i = 0; i < drawableComponents?.Length; i++)
            {
                drawableComponents?[i]?.Draw(batch);
            }
        }
		#endregion
	}

	public interface IObjUpdateComponent
	{
		void Update(GameTime time, UIObject obj/* = null*/);
	}

	public interface IObjDrawComponent
	{
        Color ColorTint { get; set; }

        void Draw(SpriteBatch batch, GameTime time = null);
	}
}