using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using JMMGExt.Input;

namespace ShiftingHues.Library.Input
{
	/// <summary>
	/// Stores information about the inputs bound to a specific <see cref="GameAction"/>.
	/// </summary>
	public class BindInfo
	{
		#region Fields and Properties
		/// <summary>
		/// Reference to the <see cref="InputComponent"/>.
		/// </summary>
		private IInputService inputManager;

		/// <summary>
		/// The action triggered by this binding.
		/// </summary>
		public GameAction BoundAction { get; private set; }

		#region Binds
		/// <summary>
		/// The list of inputs that trigger the action and the ways that the pressing of the inputs fires the action.
		/// </summary>
		public Dictionary<InputButton, InputTypes> InputBttnBindings { get; private set; }

		/// <summary>
		/// The list of keys that trigger the action and the ways that the pressing of the keys fires the action.
		/// </summary>
		public Dictionary<Keys, InputTypes> KeyBindings { get; }

		/// <summary>
		/// The list of buttons that trigger the action and the ways that the pressing of the buttons fires the action.
		/// </summary>
		public Dictionary<Buttons, InputTypes> ButtonBindings { get; private set; }
		#endregion
		#endregion

		#region Constructors

		public BindInfo(IInputService inputManager,
						GameAction boundAction,
						Dictionary<InputButton, InputTypes> inputBttnBindings = null,
						Dictionary<Keys, InputTypes> keyBindings = null,
						Dictionary<Buttons, InputTypes> buttonBindings = null)
		{
			this.inputManager = inputManager;
			this.BoundAction = boundAction;
			this.KeyBindings = keyBindings;
			this.ButtonBindings = buttonBindings;
			this.InputBttnBindings = inputBttnBindings;
		}

		public BindInfo(IInputService inputManager,
						GameAction boundAction,
						Dictionary<Keys, InputTypes> keyBindings = null,
						Dictionary<Buttons, InputTypes> buttonBindings = null)
		{
			this.inputManager = inputManager;
			this.BoundAction = boundAction;
			this.KeyBindings = keyBindings;
			this.ButtonBindings = buttonBindings;
			this.InputBttnBindings = new Dictionary<InputButton, InputTypes>();
		}
		#endregion

		#region Methods
		/// <summary>
		/// Used for inputs that have an <see cref="InputTypes"/> of <see cref="InputTypes.Toggle"/>.
		/// </summary>
		public bool IsToggledOn { get; private set; } = false;
		/// <summary>
		/// Checks the bindings to see if the action is triggered.
		/// </summary>
		/// <returns><c>true</c> if any of the bindings are active, <c>false</c> otherwise.</returns>
		public bool IsActionTriggered()
		{
			// Prevents multiple toggles at once
			bool hasToggled = false;

			// InputButtons
			var iKs = InputBttnBindings?.Keys;
			if (iKs != null)
			{
				foreach (InputButton iBttn in iKs)
				{
					// TODO: Implement Deadzone
					switch (InputBttnBindings[iBttn])
					{
						case InputTypes.Toggle:
							// If the key was pressed this frame & hasn't been toggled, switch the toggle.
							if (inputManager.GetInputDown(iBttn) && !hasToggled)
							{
								IsToggledOn = !IsToggledOn;
								hasToggled = true;
							}
							if (IsToggledOn)
							{
								return true;
							}

							break;
						case InputTypes.OnPress:
							if (inputManager.GetInputDown(iBttn))
							{
								return true;
							}

							break;
						case InputTypes.OnRelease:
							if (inputManager.GetInputUp(iBttn))
							{
								return true;
							}

							break;
						case InputTypes.PushAndHold:
						default:
							if (inputManager.GetInput(iBttn))
							{
								return true;
							}

							break;
					}
				}
			}

			// Keys
			var bKs = KeyBindings?.Keys;
			if (bKs != null)
			{
				foreach (Keys key in bKs)
				{
					switch (KeyBindings[key])
					{
						case InputTypes.Toggle:
							// If the key was pressed this frame & hasn't been toggled, switch the toggle.
							if (inputManager.GetInputDown(key) && !hasToggled)
							{
								IsToggledOn = !IsToggledOn;
								hasToggled = true;
							}
							if (IsToggledOn)
							{
								return true;
							}

							break;
						case InputTypes.OnPress:
							if (inputManager.GetInputDown(key))
							{
								return true;
							}

							break;
						case InputTypes.OnRelease:
							if (inputManager.GetInputUp(key))
							{
								return true;
							}

							break;
						case InputTypes.PushAndHold:
						default:
							if (inputManager.GetInput(key))
							{
								return true;
							}

							break;
					}
				}
			}

			// GamePad Buttons
			var bBs = ButtonBindings?.Keys;
			if (bBs != null)
			{
				foreach (Buttons button in bBs)
				{
					switch (ButtonBindings[button])
					{
						case InputTypes.Toggle:
							// If the button was pressed this frame & hasn't been toggled, switch the toggle.
							if (inputManager.GetInputDown(button) && !hasToggled)
							{
								IsToggledOn = !IsToggledOn;
								hasToggled = true;
							}
							if (IsToggledOn)
							{
								return true;
							}

							break;
						case InputTypes.OnPress:
							if (inputManager.GetInputDown(button))
							{
								return true;
							}

							break;
						case InputTypes.OnRelease:
							if (inputManager.GetInputUp(button))
							{
								return true;
							}

							break;
						case InputTypes.PushAndHold:
						default:
							if (inputManager.GetInput(button))
							{
								return true;
							}

							break;
					}
				}
			}

			// Finally, if none of the registered binds were activated, then return false.
			return false;
		}

		/// <summary>
		/// Checks the bindings to see if the action is triggered.
		/// </summary>
		/// <returns><c>true</c> if any of the bindings are active, <c>false</c> otherwise.</returns>
		public bool IsActionTriggered(out InputButton trigger)
		{
			// Prevents multiple toggles at once
			bool hasToggled = false;

			// InputButtons
			var iKs = InputBttnBindings?.Keys;
			if (iKs != null)
			{
				foreach (InputButton iBttn in iKs)
				{
					// TODO: Implement Deadzone
					switch (InputBttnBindings[iBttn])
					{
						case InputTypes.Toggle:
							// If the key was pressed this frame & hasn't been toggled, switch the toggle.
							if (inputManager.GetInputDown(iBttn) && !hasToggled)
							{
								IsToggledOn = !IsToggledOn;
								hasToggled = true;
							}
							if (IsToggledOn)
							{
								trigger = iBttn;
								return true;
							}
							break;
						case InputTypes.OnPress:
							if (inputManager.GetInputDown(iBttn))
							{
								trigger = iBttn;
								return true;
							}
							break;
						case InputTypes.OnRelease:
							if (inputManager.GetInputUp(iBttn))
							{
								trigger = iBttn;
								return true;
							}
							break;
						case InputTypes.PushAndHold:
						default:
							if (inputManager.GetInput(iBttn))
							{
								trigger = iBttn;
								return true;
							}
							break;
					}
				}
			}

			// Finally, if none of the registered binds were activated, then return false.
			trigger = new InputButton();
			return false;
		}

		public void OverwriteBindings(Dictionary<Buttons, InputTypes> newBindings) => ButtonBindings = newBindings;

		public void AddBinding(InputButton button, InputTypes inputType) => InputBttnBindings.Add(button, inputType);

		public void ResetBindings() => InputBttnBindings.Clear();

		public string _ToString()
		{
			string bigStr = "";
			if (InputBttnBindings != null)
			{
				bigStr += $"{Enum.GetName(typeof(GameAction), BoundAction)}: ";
				foreach (InputButton iB in InputBttnBindings.Keys)
				{
					bigStr += $"{iB._ToString()}, ";
				}
			}
			return bigStr;
		}
		#endregion
	}
}