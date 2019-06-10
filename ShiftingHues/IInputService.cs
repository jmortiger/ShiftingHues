using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShiftingHues.Input
{
    public delegate void MouseEvent(MouseEventArgs e);
    public delegate void MenuEvent(MenuEventArgs e);

    public class MenuEventArgs
    {
        public readonly GameAction menuAction;
        //InputButton trigger;

        public MenuEventArgs(GameAction menuAction)
        {
            this.menuAction = menuAction;
        }
    }

    public interface IInputService
    {
        #region Events

        event MouseEvent OnRelease;

        event MouseEvent OnClick;

        event MouseEvent OnMouseMove;

        event MenuEvent MenuLeft;

        event MenuEvent MenuRight;

        event MenuEvent MenuUp;

        event MenuEvent MenuDown;

        event MenuEvent MenuAccept;

        event MenuEvent MenuCancel;
        #endregion

        #region Input States
        KeyboardState CurrKeyboardState { get; }
        KeyboardState PrevKeyboardState { get; }

        GamePadState CurrPadState { get; }
        GamePadState PrevPadState { get; }

        MouseStatePlus CurrMouseState { get; }
        MouseStatePlus PrevMouseState { get; }

        List<GameAction> CurrActions { get; }
        List<GameAction> PrevActions { get; }

        List<InputButton> CurrButtons { get; }
        List<InputButton> PrevButtons { get; }
        #endregion
        #region GetInput
        bool GetInput(InputButton bttn);
        bool GetInputUp(InputButton bttn);
        bool GetInputDown(InputButton bttn);
        bool GetInput(Keys key);
        bool GetInputUp(Keys key);
        bool GetInputDown(Keys key);
        bool GetInput(Buttons button);
        bool GetInputUp(Buttons button);
        bool GetInputDown(Buttons button);
        bool GetInput(MouseButtons button);
        bool GetInputUp(MouseButtons button);
        bool GetInputDown(MouseButtons button);
        #endregion
        bool GetMouseBounds(Rectangle bounds);
        bool GetMouseBoundsEnter(Rectangle bounds);
        bool GetMouseBoundsExit(Rectangle bounds);
        bool IsActionActive(GameAction action);
        bool WasActionActive(GameAction action);
        void RegisterEvent(Action actionToFire, Func<bool> fireCondition);
    }
}