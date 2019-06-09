﻿using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShiftingHues.UI
{
	/// <summary>
	/// A component that can be activated, like a button.
	/// </summary>
	public class ActiveComponent : UI.IObjUpdateComponent
    {
        // TODO: Consider removing; this isn't a class, nor a component, it's just a setup for Inputmanager now.
        #region Fields and Properties

        //public Rectangle bounds;

        public Action<UI.UIObject> selectAction;

        public Action<UI.UIObject> deselectAction;

        public Action<UI.UIObject> activationAction;

        private Func<bool> selectCondit;
        
        private Func<bool> deselectCondit;
        
        private Func<bool> activationCondit;

        private UIObject uiObject;

        public UIObject toLeft;

        public UIObject toRight;

        public UIObject toUp;

        public UIObject toDown;

        public bool WasHovered { get; private set; } = false;
        #endregion

        #region Constructors
        public ActiveComponent(
            Action<UI.UIObject> selectAction, 
            Action<UI.UIObject> activationAction, 
            Action<UI.UIObject> deselectAction, 
            UI.UIObject obj)
            : this(selectAction, 
                  activationAction, 
                  deselectAction,
                  () =>
                  {
                      return ServiceLocator.GetInputService().GetMouseBoundsEnter(obj.Bounds) && !ServiceLocator.GetInputService().GetInputUp(Input.MouseButtons.Left);
                  },
                  () =>
                  {
                      return ServiceLocator.GetInputService().GetMouseBounds(obj.Bounds) && ServiceLocator.GetInputService().GetInputUp(Input.MouseButtons.Left);
                  },
                  () =>
                  {
                      return !ServiceLocator.GetInputService().GetMouseBounds(obj.Bounds) && obj.Bounds.Contains(ServiceLocator.GetInputService().PrevMouseState.Position);
                  },
                  obj
                  ) { }

        public ActiveComponent(
            Action<UI.UIObject> selectAction, 
            Action<UI.UIObject> activationAction, 
            Action<UI.UIObject> deselectAction, 
            Func<bool> selectCondit,
            Func<bool> activationCondit,
            Func<bool> deselectCondit,
            UI.UIObject obj)
        {
            this.selectAction = selectAction;
            this.activationAction = activationAction;
            this.deselectAction = deselectAction;

            Input.IInputService input = ServiceLocator.GetInputService();
            //input.RegisterEvent(
            //    () => activationAction(obj),
            //    activationCondit
            //    );
            //input.RegisterEvent(
            //    () => deselectAction(obj),
            //    selectCondit
            //    );
            //input.RegisterEvent(
            //    () => selectAction(obj),
            //    deselectCondit
            //    );

            this.selectCondit = selectCondit;
            this.activationCondit = activationCondit;
            this.deselectCondit = deselectCondit;
            this.uiObject = obj;
            input.OnMouseMove += Input_OnMouseMove;
            input.OnRelease += Input_OnRelease;
        }

        private void Input_OnRelease(Input.MouseEventArgs e)
        {
            if (activationCondit()) activationAction?.Invoke(uiObject);
        }

        private void Input_OnMouseMove(Input.MouseEventArgs e)
        {
            if (selectCondit()) selectAction?.Invoke(uiObject);
            if (deselectCondit()) deselectAction?.Invoke(uiObject);
            //throw new NotImplementedException();
        }
        #endregion

        #region Methods
        /// <summary>
        /// TODO: Finish Docs
        /// TODO: Change implementation to not directly require input checking
        /// </summary>
        /// <param name="time"></param>
        /// <param name="obj"></param>
        public void Update(GameTime time, UI.UIObject obj)
        {
            //var mS = Mouse.GetState();
            //if (obj == null) return;
            //if (obj.Bounds.Contains(mS.Position))
            //{
            //    if (mS.LeftButton == ButtonState.Pressed)
            //        activationAction?.Invoke(obj);
            //    else
            //    {
            //        hoverAction?.Invoke(obj);
            //        WasHovered = true;
            //    }
            //}
            //else if (WasHovered)
            //{
            //    stopHoverAction?.Invoke(obj);
            //    WasHovered = false;
            //}
            //throw new NotImplementedException();
        }
        #endregion
    }
}