using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using JMMGExt;
using JMMGExt.Input;

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

        private readonly UIObject uiObject;

        private UIObject toLeft;
        public UIObject ToLeft
        {
            get => toLeft;
            set
            {
                toLeft = value;
                ServiceLocator.GetInputService().MenuLeft += OnMenuLeft;
            }
        }
        
        private UIObject toRight;
        public UIObject ToRight
        {
            get => toRight;
            set
            {
                toRight = value;
                ServiceLocator.GetInputService().MenuRight += OnMenuRight;
            }
        }
        
        private UIObject toUp;
        public UIObject ToUp
        {
            get => toUp;
            set
            {
                toUp = value;
                ServiceLocator.GetInputService().MenuUp += OnMenuUp;
            }
        }

        private UIObject toDown;
        public UIObject ToDown
        {
            get => toDown;
            set
            {
                toDown = value;
                ServiceLocator.GetInputService().MenuDown += OnMenuDown;
            }
        }

        private UIObject toAccept;
        public UIObject ToAccept
        {
            get => toAccept;
            set
            {
                toAccept = value;
                ServiceLocator.GetInputService().MenuAccept += OnMenuAccept;
            }
        }

        public bool IsSelected { get; private set; } = false;
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
                      return ServiceLocator.GetInputService().GetMouseBoundsEnter(obj.Bounds) && !ServiceLocator.GetInputService().GetInputUp(/*Input.*/MouseButtons.Left);
                  },
                  () =>
                  {
                      return ServiceLocator.GetInputService().GetMouseBounds(obj.Bounds) && ServiceLocator.GetInputService().GetInputUp(/*Input.*/MouseButtons.Left);
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

            /*Input.*/IInputService input = ServiceLocator.GetInputService();
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
        #endregion

        #region Methods

        private void Input_OnRelease(/*Input.*/MouseEventArgs e)
        {
            if (activationCondit()) activationAction?.Invoke(uiObject);
        }

        private void Input_OnMouseMove(/*Input.*/MouseEventArgs e)
        {
            if (selectCondit())
            {
                selectAction?.Invoke(uiObject);
                IsSelected = true;
            }
            if (deselectCondit())
            {
                deselectAction?.Invoke(uiObject);
                IsSelected = false;
            }
            //throw new NotImplementedException();
        }

        private void OnMenuLeft(/*Input.*/MenuEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnMenuRight(/*Input.*/MenuEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnMenuUp(/*Input.*/MenuEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnMenuDown(/*Input.*/MenuEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnMenuAccept(/*Input.*/MenuEventArgs e)
        {

            throw new NotImplementedException();
        }

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