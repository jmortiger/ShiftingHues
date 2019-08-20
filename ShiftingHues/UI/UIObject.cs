using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ShiftingHues.Library.Input;

namespace ShiftingHues.UI
{
	public class UIObjEventArgs
	{
		public readonly object sender;
		public readonly Type type;
	}
	public class UIObject
	{
        public event /*Input.*/MouseEvent MouseEntered;

		#region Fields and Properties

		public Rectangle Bounds { get; private set; }

        private IObjDrawComponent[] drawableComponents;
        public IObjDrawComponent[] DrawableComponents
        {
            get { return drawableComponents; }
            //set { drawableComponents = value; }
        }


        public IObjUpdateComponent[] updateableComponents;

        private bool wasEnteredChecked = false;
        #endregion

        #region Constructors

        //public UIObject(Rectangle bounds)
		//{
			//this.Bounds = bounds;
		//}

        //public UIObject(Rectangle bounds, IObjDrawComponent[] drawableComponents = null, IObjUpdateComponent[] updateableComponents = null)
        //{
        //    this.Bounds = bounds;
        //    this.drawableComponents = drawableComponents;
        //    this.updateableComponents = updateableComponents;
        //}

        public UIObject(Rectangle bounds, IObjDrawComponent drawableComponent = null, IObjUpdateComponent updateableComponent = null)
        {
            this.Bounds = bounds;
            this.drawableComponents = new IObjDrawComponent[]{ drawableComponent };
            this.updateableComponents = new IObjUpdateComponent[] { updateableComponent };
        }
		#endregion

		#region Methods

		public void RegisterToMouseEnter(UI.ContainerComponent container) => container.MouseEntered += OnMouseEnterParentContainer;

		private void OnMouseEnterParentContainer(/*Input.*/MouseEventArgs e)
        {
            if (Bounds.Contains(e.CurrState.Position) && !Bounds.Contains(e.PrevState.Position))
                MouseEntered?.Invoke(e);
            wasEnteredChecked = true;
        }

        public void SetVisibility(bool isVisible)
        {
            for (int i = 0; i < drawableComponents.Length; i++)
            {
                drawableComponents[i].IsVisible = isVisible;
            }
        }


        public void AddDrawableComponent(IObjDrawComponent comp)
        {
            IObjDrawComponent[] temp = new IObjDrawComponent[drawableComponents.Length + 1];
            for (int i = 0; i < DrawableComponents.Length; i++)
            {
                if (DrawableComponents[i] == comp)
                    return; // Temp *should* be deallocated
                temp[i] = DrawableComponents[i];
            }
            temp[temp.Length - 1] = comp;
        }

        /// <summary>
        /// TODO: Finish Doc
        /// </summary>
        /// <param name="time"></param>
        /// <remarks></remarks>
        public virtual void Update(GameTime time)
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
        Color Tint { get; set; }

        bool IsVisible { get; set; }

        void Draw(SpriteBatch batch, GameTime time = null);
	}
}