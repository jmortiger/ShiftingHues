using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using GeonBit.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ShiftingHues;

namespace ShiftingHues.Library.Input
{
	class GeonUIInputExtension
	{
		#region Fields and Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion
	}
	public class UIKeyboardMouseEmulator : IMouseInput
	{
		private Input.IInputService inputComponent;
		private Input.IInputService Input
		{
			get
			{
				if (inputComponent == null)
					inputComponent = ServiceLocator.GetInputService();
				return inputComponent;
			}
		}

		private MouseStatePlus MousePlus => Input.CurrMouseState;

		public Vector2 MousePosition => MousePlus.PositionAsVector2;

		public Vector2 MousePositionDiff => new Vector2(MousePlus.PositDelta.X, MousePlus.PositDelta.Y);

		public Point MousePositionAsPoint => MousePlus.Position;
		public Point MousePositionDiffAsPoint => MousePlus.PositDelta;

		public int MouseWheel => MousePlus.ScrollWheelValue;

		public int MouseWheelChange => MousePlus.ScrollWheelDelta;

		/// <summary>
		/// Return if any of mouse buttons was clicked (eg released after being pressed down) this frame.
		/// </summary>
		/// <returns>True if any mouse button was clicked.</returns>
		public bool AnyMouseButtonClicked()
		{
			return AnyMouseButtonClicked(false);
		}

		public bool AnyMouseButtonClicked(bool countScroll)
		{
			return !MousePlus.AnyButtonPressed(countScroll) && Input.PrevMouseState.AnyButtonPressed(countScroll);
		}

		public bool AnyMouseButtonDown()
		{
			return AnyMouseButtonDown(false);
		}

		public bool AnyMouseButtonDown(bool countScroll)
		{
			return MousePlus.AnyButtonPressed(countScroll);
		}

		public bool AnyMouseButtonPressed()
		{
			return AnyMouseButtonPressed(false);
		}

		public bool AnyMouseButtonPressed(bool countScroll)
		{
			return MousePlus.AnyButtonPressed(countScroll) && !Input.PrevMouseState.AnyButtonPressed(countScroll);
		}

		public bool AnyMouseButtonReleased()
		{
			return AnyMouseButtonReleased(false);
		}

		public bool AnyMouseButtonReleased(bool countScroll)
		{
			return !MousePlus.AnyButtonPressed(countScroll) && Input.PrevMouseState.AnyButtonPressed(countScroll);
		}

		public bool MouseButtonClick(MouseButton button = MouseButton.Left)
		{
			switch (button)
			{
				case MouseButton.Left:
					return Input.GetInputUp(MouseButtons.Left);
				case MouseButton.Right:
					return Input.GetInputUp(MouseButtons.Right);
				case MouseButton.Middle:
					return Input.GetInputUp(MouseButtons.Middle);
				default:
					return false;
			}
		}

		public bool MouseButtonDown(MouseButton button = MouseButton.Left)
		{
			switch (button)
			{
				case MouseButton.Left:
					return Input.GetInput(MouseButtons.Left);
				case MouseButton.Right:
					return Input.GetInput(MouseButtons.Right);
				case MouseButton.Middle:
					return Input.GetInput(MouseButtons.Middle);
				default:
					return false;
			}
		}

		public bool MouseButtonPressed(MouseButton button = MouseButton.Left)
		{
			switch (button)
			{
				case MouseButton.Left:
					return Input.GetInputDown(MouseButtons.Left);
				case MouseButton.Right:
					return Input.GetInputDown(MouseButtons.Right);
				case MouseButton.Middle:
					return Input.GetInputDown(MouseButtons.Middle);
				default:
					return false;
			}
		}

		public bool MouseButtonReleased(MouseButton button = MouseButton.Left)
		{
			return MouseButtonClick(button);
		}

		public Vector2 TransformMousePosition(Matrix? transform)
		{
			throw new NotImplementedException();
		}

		public void Update(GameTime gameTime)
		{
			throw new NotImplementedException();
		}

		public void UpdateMousePosition(Vector2 pos)
		{
			Input.SetNextMousePosition(pos);
			//throw new NotImplementedException();
		}
	}
}