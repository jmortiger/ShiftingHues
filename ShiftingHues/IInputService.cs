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
		Vector2 NextMousePosit { get; }
		#region GetInput
		/// <summary>
		/// Get the current state of the specified button.
		/// </summary>
		/// <param name="bttn">The button to check.</param>
		/// <returns><c>true</c> if the button is currently down, <c>false</c> otherwise.</returns>
		bool GetInput(InputButton bttn);

		/// <summary>
		/// Get if the specified input was released this frame.
		/// </summary>
		/// <param name="bttn">The input to check.</param>
		/// <returns><c>true</c> if the input is currently up and was down last frame, <c>false</c> otherwise.</returns>
		bool GetInputUp(InputButton bttn);

		/// <summary>
		/// Get if the specified input was pressed this frame.
		/// </summary>
		/// <param name="bttn">The input to check.</param>
		/// <returns><c>true</c> if the input is currently down and was up last frame, <c>false</c> otherwise.</returns>
		bool GetInputDown(InputButton bttn);

		/// <summary>
		/// Get the current state of the specified key.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <returns><c>true</c> if the key is currently down, <c>false</c> otherwise.</returns>
		bool GetInput(Keys key);

		/// <summary>
		/// Get if the specified key was released this frame.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <returns><c>true</c> if the key is currently up and was down last frame, <c>false</c> otherwise.</returns>
		bool GetInputUp(Keys key);

		/// <summary>
		/// Get if the specified key was pressed this frame.
		/// </summary>
		/// <param name="key">The key to check.</param>
		/// <returns><c>true</c> if the key is currently down and was up last frame, <c>false</c> otherwise.</returns>
		bool GetInputDown(Keys key);

		/// <summary>
		/// Get the current state of the specified <see cref="GamePad"/> button.
		/// </summary>
		/// <param name="button">The button to check.</param>
		/// <returns><c>true</c> if the button is currently down, <c>false</c> otherwise.</returns>
		bool GetInput(Buttons button);

		/// <summary>
		/// Get if the specified button was released this frame.
		/// </summary>
		/// <param name="button">The button to check.</param>
		/// <returns><c>true</c> if <paramref name="button"/> is currently up and was down last frame, <c>false</c> otherwise.</returns>
		bool GetInputUp(Buttons button);

		/// <summary>
		/// Get if the specified key was pressed this frame.
		/// </summary>
		/// <param name="button">The button to check.</param>
		/// <returns><c>true</c> if <paramref name="button"/> is currently down and was up last frame, <c>false</c> otherwise.</returns>
		bool GetInputDown(Buttons button);

		/// <summary>
		/// Get the current state of the specified <see cref="Mouse"/> button.
		/// </summary>
		/// <param name="button">The button to check.</param>
		/// <returns><c>true</c> if the button is currently down, <c>false</c> otherwise.</returns>
		bool GetInput(MouseButtons button);

		/// <summary>
		/// Get if the specified button was released this frame.
		/// </summary>
		/// <param name="button">The button to check.</param>
		/// <returns><c>true</c> if the <paramref name="button"/> is currently up and was down last frame, <c>false</c> otherwise.</returns>
		bool GetInputUp(MouseButtons button);

		/// <summary>
		/// Get if the specified key was pressed this frame.
		/// </summary>
		/// <param name="button">The button to check.</param>
		/// <returns><c>true</c> if <paramref name="button"/> is currently down and was up last frame, <c>false</c> otherwise.</returns>
		bool GetInputDown(MouseButtons button);
        #endregion
        bool GetMouseBounds(Rectangle bounds);
        bool GetMouseBoundsEnter(Rectangle bounds);
        bool GetMouseBoundsExit(Rectangle bounds);
        bool IsActionActive(GameAction action);
        bool WasActionActive(GameAction action);
        void RegisterEvent(Action actionToFire, Func<bool> fireCondition);
		void SetNextMousePosition(Vector2 newPos);
    }
}