using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShiftingHues;

namespace ShiftingHues.UI
{
    public class ContainerComponent : UI.IObjDrawComponent
    {
        public Input.MouseEvent MouseEntered;

        #region Fields and Properties

        public UIObject[] Children { get; private set; }

        private bool isVisible;
        public bool IsVisible
        {
            get => isVisible;
            set
            {
                // If it's setting it to the way it is, do nothing
                if (value == this.IsVisible)
                    return;
                this.isVisible = value;
                //if (isVisible)
                //{
                for (int i = 0; i < Children.Length; i++)
                {
                    Children[i].SetVisibility(value);
                }
                //}
            }
        }

        public bool IsActive { get; private set; }

        public Color Tint { get; set; }

        private UIObject myObj;

        public Rectangle Bounds { get => myObj.Bounds; /*private set;*/ }
        #endregion

        #region Constructors
        public ContainerComponent(UIObject obj)
        {
            this.myObj = obj;
            obj.MouseEntered += OnObjEntered;
        }
        #endregion

        #region Methods

        private void OnObjEntered(Input.MouseEventArgs e) => MouseEntered?.Invoke(e);

        public void SetVisibility(bool isVisible)
        {
            // If it's setting it to the way it is, do nothing
            if (isVisible == this.IsVisible)
                return;
            this.IsVisible = isVisible;
            //if (isVisible)
            //{
                for (int i = 0; i < Children.Length; i++)
                {
                    Children[i].SetVisibility(isVisible);
                }
            //}

            //throw new NotImplementedException();
        }

        // TODO: Incomplete
        public void SetActive(bool isActive)
        {
            // If it's setting it to the way it is, do nothing
            if (isActive == this.IsActive)
                return;
            this.IsActive = isActive;
            //if (isVisible)
            //{
            //for (int i = 0; i < Children.Length; i++)
            //{
            //    Children[i].SetVisibility(isActive);
            //}
            //}

            //throw new NotImplementedException();
        }

        public void SetChildVisibility(int index, bool isVisible)
        {

            throw new NotImplementedException();
        }

        public void SetChildActive(int index, bool isActive)
        {

            throw new NotImplementedException();
        }

        public void AddObj(UIObject obj)
        {
            UIObject[] temp = new UIObject[Children.Length + 1];
            for (int i = 0; i < Children.Length; i++)
            {
                if (Children[i] == obj)
                    return; // Temp *should* be deallocated
                temp[i] = Children[i];
            }
            obj.RegisterToMouseEnter(this);
            temp[temp.Length - 1] = obj;
            //throw new NotImplementedException();
        }

        public void Draw(SpriteBatch batch, GameTime time = null)
        {
            if (!IsVisible)
                return;
            foreach (var obj in Children)
            {
                obj.Draw(batch, time);
            }
            //throw new NotImplementedException();
        }
        #endregion
    }
}