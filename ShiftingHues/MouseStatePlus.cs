using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShiftingHues.Library.Input
{

    public enum MouseButtons
    {
        Left,
        Middle,
        Right,
        ScrollUp,
        ScrollDown,
        ScrollLeft,
        ScrollRight,
        XButton1,
        XButton2,
        ScrollVert,
        ScrollHoriz
    }

    public enum MouseValues
    {
        X,
        Y,
        ScrollY,
        ScrollX/*,
        Position*/
    }

    public struct MouseStatePlus
    {
        public readonly MouseState State;
        public readonly int ScrollWheelDelta;
        public readonly int HorizontalScrollWheelDelta;
        public readonly Point PositDelta;

        /// <summary>
        /// Gets if the scroll wheel was rolled up.
        /// </summary>
        public ButtonState ScrollUp { get => (ScrollWheelDelta > 0) ? ButtonState.Pressed : ButtonState.Released; }
        /// <summary>
        /// Gets if the scroll wheel was rolled down.
        /// </summary>
        public ButtonState ScrollDown { get => (ScrollWheelDelta < 0) ? ButtonState.Pressed : ButtonState.Released; }
		/// <summary>
		/// Gets cursor position as a <see cref="Vector2"/>.
		/// </summary>
		/// <remarks>Creates a new <see cref="Vector2"/>.</remarks>
		public Vector2 PositionAsVector2 { get => new Vector2(X, Y); }

		#region MouseState Properties
		/// <summary>
		/// Returns the cumulative horizontal scroll wheel value since the game start.
		/// </summary>
		public int HorizontalScrollWheelValue { get => State.HorizontalScrollWheelValue; }
        /// <summary>
        /// Gets state of the left mouse button.
        /// </summary>
        public ButtonState LeftButton { get => State.LeftButton; }
        /// <summary>
        /// Gets state of the middle mouse button.
        /// </summary>
        public ButtonState MiddleButton { get => State.MiddleButton; }
        /// <summary>
        /// Gets cursor position.
        /// </summary>
        public Point Position { get => State.Position; }
		/// <summary>
		/// Gets horizontal position of the cursor in relation to the window.
		/// </summary>
		public int X { get => State.X; }
        /// <summary>
        /// Gets vertical position of the cursor in relation to the window.
        /// </summary>
        public int Y { get => State.Y; }
        /// <summary>
        /// Gets state of the right mouse button.
        /// </summary>
        public ButtonState RightButton { get => State.RightButton; }
        /// <summary>
        /// Returns cumulative scroll wheel value since the game start.
        /// </summary>
        public int ScrollWheelValue { get => State.ScrollWheelValue; }
        /// <summary>
        /// Gets state of the XButton1.
        /// </summary>
        public ButtonState XButton1 { get => State.XButton1; }
        /// <summary>
        /// Gets state of the XButton2.
        /// </summary>
        public ButtonState XButton2 { get => State.XButton2; }
        #endregion

        public MouseStatePlus(MouseState myState, MouseState myPrevState)
        {
            this.State = myState;
            this.ScrollWheelDelta = myPrevState.ScrollWheelValue - myState.ScrollWheelValue;
            this.HorizontalScrollWheelDelta = myPrevState.HorizontalScrollWheelValue - myState.HorizontalScrollWheelValue;
            this.PositDelta = myPrevState.Position - myState.Position;
        }

        public bool GetButton(MouseButtons button)
        {
            // TODO: Check How Horizontal scrolling is calculated (is left -/+)
            switch (button)
            {
                case MouseButtons.Left:
                    return LeftButton == ButtonState.Pressed;
                case MouseButtons.Middle:
                    return MiddleButton == ButtonState.Pressed;
                case MouseButtons.Right:
                    return RightButton == ButtonState.Pressed;
                case MouseButtons.ScrollUp:
                    return ScrollWheelDelta > 0;
                case MouseButtons.ScrollDown:
                    return ScrollWheelDelta < 0;
                case MouseButtons.ScrollLeft:
                    return HorizontalScrollWheelDelta < 0;
                case MouseButtons.ScrollRight:
                    return HorizontalScrollWheelDelta > 0;
                case MouseButtons.XButton1:
                    return XButton1 == ButtonState.Pressed;
                case MouseButtons.XButton2:
                    return XButton1 == ButtonState.Pressed;
                case MouseButtons.ScrollVert:
                    return ScrollWheelDelta != 0;
                case MouseButtons.ScrollHoriz:
                    return HorizontalScrollWheelDelta != 0;
                default:
                    return false;
            }
        }

		public bool AnyButtonPressed(bool countScroll = true)
		{
			return
				LeftButton == ButtonState.Pressed ||
				MiddleButton == ButtonState.Pressed ||
				RightButton == ButtonState.Pressed ||
				(countScroll && (
					ScrollWheelDelta > 0 ||
					ScrollWheelDelta < 0 ||
					HorizontalScrollWheelDelta < 0 ||
					HorizontalScrollWheelDelta > 0)) ||
				XButton1 == ButtonState.Pressed ||
				XButton2 == ButtonState.Pressed;
		}

        public MouseButtons[] GetPressedButtons()
        {
            List<MouseButtons> pressed = new List<MouseButtons>(2);
            for (int i = 0; i < Enum.GetNames(typeof(MouseButtons)).Length; i++)
            {
                if (GetButton((MouseButtons)i))
                    pressed.Add((MouseButtons)i);
            }
            return pressed.ToArray();
        }

        #region Equality & Hash Code

        public static bool operator ==(MouseStatePlus left, MouseStatePlus right)
        {
            return left.State == right.State &&
                   left.ScrollWheelDelta == right.ScrollWheelDelta &&
                   left.HorizontalScrollWheelDelta == right.HorizontalScrollWheelDelta &&
                   left.PositDelta == right.PositDelta;
        }

        public static bool operator !=(MouseStatePlus left, MouseStatePlus right) => !(left == right);

        /// <summary>
        /// Compares whether current instance is equal to specified object.
        /// </summary>
        /// <param name="obj">The <see cref="MouseStatePlus"/> to compare.</param>
        /// <returns><c>true</c> if equal, <c>false</c> otherwise.</returns>
        public override bool Equals(object obj)
        {
            return (obj is MouseStatePlus) ?
                (this == (MouseStatePlus)obj) :
                false;
        }

        /// <summary>
        /// Gets the hash code for <see cref="MouseStatePlus"/> instance.
        /// </summary>
        /// <returns>Hash code of the object.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = State.GetHashCode();
                hashCode = (hashCode * 397) ^ ScrollWheelDelta;
                hashCode = (hashCode * 397) ^ HorizontalScrollWheelDelta;
                hashCode = (hashCode * 397) ^ PositDelta.X;
                hashCode = (hashCode * 397) ^ PositDelta.Y;
                return hashCode;
            }
        }
        #endregion
    }

    public class MouseEventArgs
    {
        public readonly MouseStatePlus CurrState;
        public readonly MouseStatePlus PrevState;

        public MouseEventArgs(MouseStatePlus CurrState, MouseStatePlus PrevState)
        {
            this.CurrState = CurrState;
            this.PrevState = PrevState;
        }
    }
}