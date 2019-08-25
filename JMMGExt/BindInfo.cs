using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace JMMGExt.Input
{
	/// <summary>
	/// Stores information about the inputs bound to a specific <see cref="GameAction"/>.
	/// </summary>
	public class BindInfo
	{
		#region Fields and Properties
		/// <summary>
		/// Reference to the <see cref="IInputService"/>.
		/// </summary>
		private IInputService inputManager;
		private IInputService InputManager
		{
			get
			{
				if (inputManager == null)
					inputManager = ServiceLocator.GetInputService();
				return inputManager;
			}
			set => inputManager = value;
		}

		/// <summary>
		/// The action triggered by this binding.
		/// </summary>
		public GameAction BoundAction { get; private set; }

		#region Binds
		private List<InputButton> InputBttnToggles;

		/// <summary>
		/// The list of inputs that trigger the action and the ways that the pressing of the inputs fires the action.
		/// </summary>
		public Dictionary<InputButton, InputTrigger> InputBttnBindings { get; private set; }

		private List<Keys> KeyToggles;

		/// <summary>
		/// The list of keys that trigger the action and the ways that the pressing of the keys fires the action.
		/// </summary>
		public Dictionary<Keys, InputTrigger> KeyBindings { get; private set; }

		private List<Buttons> ButtonToggles;

		/// <summary>
		/// The list of buttons that trigger the action and the ways that the pressing of the buttons fires the action.
		/// </summary>
		public Dictionary<Buttons, InputTrigger> ButtonBindings { get; private set; }
		#endregion
		#endregion

		#region Constructors

		public BindInfo(IInputService inputManager,
						GameAction boundAction,
						Dictionary<InputButton, InputTrigger> inputBttnBindings = null,
						Dictionary<Keys, InputTrigger> keyBindings = null,
						Dictionary<Buttons, InputTrigger> buttonBindings = null)
		{
			this.InputManager = inputManager;
			this.BoundAction = boundAction;
			this.KeyBindings = keyBindings;
			this.ButtonBindings = buttonBindings;
			this.InputBttnBindings = inputBttnBindings;

			CheckForToggles();
		}

		public BindInfo(IInputService inputManager,
						GameAction boundAction,
						Dictionary<Keys, InputTrigger> keyBindings = null,
						Dictionary<Buttons, InputTrigger> buttonBindings = null)
		{
			this.InputManager = inputManager;
			this.BoundAction = boundAction;
			this.KeyBindings = keyBindings;
			this.ButtonBindings = buttonBindings;
			this.InputBttnBindings = new Dictionary<InputButton, InputTrigger>();

			CheckForToggles();
		}

		public BindInfo(GameAction boundAction,
						Dictionary<InputButton, InputTrigger> inputBttnBindings = null,
						Dictionary<Keys, InputTrigger> keyBindings = null,
						Dictionary<Buttons, InputTrigger> buttonBindings = null)
		{
			this.BoundAction = boundAction;
			this.KeyBindings = keyBindings;
			this.ButtonBindings = buttonBindings;
			this.InputBttnBindings = inputBttnBindings;

			CheckForToggles();
		}

		public BindInfo(GameAction boundAction,
						Dictionary<Keys, InputTrigger> keyBindings = null,
						Dictionary<Buttons, InputTrigger> buttonBindings = null)
		{
			this.BoundAction = boundAction;
			this.KeyBindings = keyBindings;
			this.ButtonBindings = buttonBindings;
			this.InputBttnBindings = new Dictionary<InputButton, InputTrigger>();

			CheckForToggles();
		}
		#endregion

		#region Methods
		private void CheckForToggles()
		{
			var iKs = InputBttnBindings?.Keys;
			if (iKs != null)
			{
				foreach (InputButton iBttn in iKs)
				{// TODO: Implement Deadzone
					switch (InputBttnBindings[iBttn])
					{
						case InputTrigger.Toggle:
							if (InputBttnToggles == null)
								InputBttnToggles = new List<InputButton>();
							if (!InputBttnToggles.Contains(iBttn))
								InputBttnToggles.Add(iBttn);
							break;
						case InputTrigger.OnPress:
						case InputTrigger.OnRelease:
						case InputTrigger.PushAndHold:
						default:
							break;
					}
				}
				InputBttnToggles.TrimExcess();
			}
			var bKs = KeyBindings?.Keys;
			if (bKs != null)
			{
				foreach (Keys key in bKs)
				{// TODO: Implement Deadzone
					switch (KeyBindings[key])
					{
						case InputTrigger.Toggle:
							if (KeyToggles == null)
								KeyToggles = new List<Keys>();
							if (!KeyToggles.Contains(key))
								KeyToggles.Add(key);
							break;
						case InputTrigger.OnPress:
						case InputTrigger.OnRelease:
						case InputTrigger.PushAndHold:
						default:
							break;
					}
				}
				KeyToggles.TrimExcess();
			}
			var bBs = ButtonBindings?.Keys;
			if (bBs != null)
			{
				foreach (Buttons button in bBs)
				{// TODO: Implement Deadzone
					switch (ButtonBindings[button])
					{
						case InputTrigger.Toggle:
							if (ButtonToggles == null)
								ButtonToggles = new List<Buttons>();
							if (!ButtonToggles.Contains(button))
								ButtonToggles.Add(button);
							break;
						case InputTrigger.OnPress:
						case InputTrigger.OnRelease:
						case InputTrigger.PushAndHold:
						default:
							break;
					}
				}
				ButtonToggles.TrimExcess();
			}
		}

		/// <summary>
		/// Used for inputs that have an <see cref="InputTrigger"/> of <see cref="InputTrigger.Toggle"/>.
		/// </summary>
		public bool IsToggledOn { get; private set; } = false;

		#region IsActionTriggered
		/// <summary>
		/// Checks the bindings to see if the action is triggered.
		/// </summary>
		/// <returns><c>true</c> if any of the bindings are active, <c>false</c> otherwise.</returns>
		/// <remarks>This overload is not likely to check every value. It will immediately return 
		/// when the first active trigger is found, unless none of the triggers are active. The only 
		/// cases in which all triggers are processed is when there are no active triggers and when 
		/// the last trigger is the only active one.</remarks>
		public bool IsActionTriggered()
		{
			// Prevents multiple toggles at once
			bool hasToggled = false;

			foreach (var iBttn in InputBttnToggles)
			{
				// If the button was pressed this frame & hasn't been toggled, switch the toggle.
				if (InputManager.GetInputDown(iBttn) && !hasToggled)
				{
					IsToggledOn = !IsToggledOn;
					hasToggled = true;
				}
				if (IsToggledOn)
					return true;
			}

			foreach (var button in ButtonToggles)
			{
				// If the button was pressed this frame & hasn't been toggled, switch the toggle.
				if (InputManager.GetInputDown(button) && !hasToggled)
				{
					IsToggledOn = !IsToggledOn;
					hasToggled = true;
				}
				if (IsToggledOn)
					return true;
			}

			foreach (var key in KeyToggles)
			{
				// If the button was pressed this frame & hasn't been toggled, switch the toggle.
				if (InputManager.GetInputDown(key) && !hasToggled)
				{
					IsToggledOn = !IsToggledOn;
					hasToggled = true;
				}
				if (IsToggledOn)
					return true;
			}

			// InputButtons
			var iKs = InputBttnBindings?.Keys;
			if (iKs != null)
			{
				foreach (InputButton iBttn in iKs)
				{// TODO: Implement Deadzone
					switch (InputBttnBindings[iBttn])
					{
						case InputTrigger.Toggle:
							//// If the button was pressed this frame & hasn't been toggled, switch the toggle.
							//if (InputManager.GetInputDown(iBttn) && !hasToggled)
							//{
							//	IsToggledOn = !IsToggledOn;
							//	hasToggled = true;
							//}
							//if (IsToggledOn)
							//	return true;
							break;
						case InputTrigger.OnPress:
							if (InputManager.GetInputDown(iBttn))
								return true;

							break;
						case InputTrigger.OnRelease:
							if (InputManager.GetInputUp(iBttn))
								return true;

							break;
						case InputTrigger.PushAndHold:
						default:
							if (InputManager.GetInput(iBttn))
								return true;

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
						case InputTrigger.Toggle:
							//// If the key was pressed this frame & hasn't been toggled, switch the toggle.
							//if (InputManager.GetInputDown(key) && !hasToggled)
							//{
							//	IsToggledOn = !IsToggledOn;
							//	hasToggled = true;
							//}
							//if (IsToggledOn)
							//	return true;
							break;
						case InputTrigger.OnPress:
							if (InputManager.GetInputDown(key))
								return true;

							break;
						case InputTrigger.OnRelease:
							if (InputManager.GetInputUp(key))
								return true;

							break;
						case InputTrigger.PushAndHold:
						default:
							if (InputManager.GetInput(key))
								return true;

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
						case InputTrigger.Toggle:
							//// If the button was pressed this frame & hasn't been toggled, switch the toggle.
							//if (InputManager.GetInputDown(button) && !hasToggled)
							//{
							//	IsToggledOn = !IsToggledOn;
							//	hasToggled = true;
							//}
							//if (IsToggledOn)
							//	return true;
							break;
						case InputTrigger.OnPress:
							if (InputManager.GetInputDown(button))
								return true;

							break;
						case InputTrigger.OnRelease:
							if (InputManager.GetInputUp(button))
								return true;

							break;
						case InputTrigger.PushAndHold:
						default:
							if (InputManager.GetInput(button))
								return true;

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
		/// <param name="trigger">The <see cref="InputButton"/> that triggered the action.</param>
		/// <returns><c>true</c> if any of the bindings are active, <c>false</c> otherwise.</returns>
		/// <remarks>This overload is not likely to check every value. It will immediately return 
		/// when the first active trigger is found, unless none of the triggers are active. The only 
		/// cases in which all triggers are processed is when there are no active triggers and when 
		/// the last trigger is the only active one. The out param <paramref name="trigger"/> is the 
		/// first found trigger; there may be more. If you require a complete list of all bindings 
		/// that are triggering this <see cref="GameAction"/>, or require that all bindings are 
		/// processed, use the <see cref="IsActionTriggered(out InputButton[])"/> overload.</remarks>
		public bool IsActionTriggered(out InputButton trigger)
		{
			// Prevents multiple toggles at once
			bool hasToggled = false;

			foreach (var iBttn in InputBttnToggles)
			{
				// If the button was pressed this frame & hasn't been toggled, switch the toggle.
				if (InputManager.GetInputDown(iBttn) && !hasToggled)
				{
					IsToggledOn = !IsToggledOn;
					hasToggled = true;
				}
				if (IsToggledOn)
				{
					trigger = iBttn;
					return true;
				}
			}

			foreach (var key in KeyToggles)
			{
				// If the button was pressed this frame & hasn't been toggled, switch the toggle.
				if (InputManager.GetInputDown(key) && !hasToggled)
				{
					IsToggledOn = !IsToggledOn;
					hasToggled = true;
				}
				if (IsToggledOn)
				{
					trigger = new InputButton(key);
					return true;
				}
			}

			foreach (var button in ButtonToggles)
			{
				// If the button was pressed this frame & hasn't been toggled, switch the toggle.
				if (InputManager.GetInputDown(button) && !hasToggled)
				{
					IsToggledOn = !IsToggledOn;
					hasToggled = true;
				}
				if (IsToggledOn)
				{
					trigger = new InputButton(button);
					return true;
				}
			}

			// InputButtons
			var iKs = InputBttnBindings?.Keys;
			if (iKs != null)
			{
				foreach (InputButton iBttn in iKs)
				{// TODO: Implement Deadzone
					switch (InputBttnBindings[iBttn])
					{
						case InputTrigger.Toggle:
							//// If the key was pressed this frame & hasn't been toggled, switch the toggle.
							//if (InputManager.GetInputDown(iBttn) && !hasToggled)
							//{
							//	IsToggledOn = !IsToggledOn;
							//	hasToggled = true;
							//}
							//if (IsToggledOn)
							//{
							//	trigger = iBttn;
							//	return true;
							//}
							break;
						case InputTrigger.OnPress:
							if (InputManager.GetInputDown(iBttn))
							{
								trigger = iBttn;
								return true;
							}
							break;
						case InputTrigger.OnRelease:
							if (InputManager.GetInputUp(iBttn))
							{
								trigger = iBttn;
								return true;
							}
							break;
						case InputTrigger.PushAndHold:
						default:
							if (InputManager.GetInput(iBttn))
							{
								trigger = iBttn;
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
						case InputTrigger.Toggle:
							//// If the key was pressed this frame & hasn't been toggled, switch the toggle.
							//if (InputManager.GetInputDown(key) && !hasToggled)
							//{
							//	IsToggledOn = !IsToggledOn;
							//	hasToggled = true;
							//}
							//if (IsToggledOn)
							//{
							//	trigger = new InputButton(key);
							//	return true;
							//}
							break;
						case InputTrigger.OnPress:
							if (InputManager.GetInputDown(key))
							{
								trigger = new InputButton(key);
								return true;
							}
							break;
						case InputTrigger.OnRelease:
							if (InputManager.GetInputUp(key))
							{
								trigger = new InputButton(key);
								return true;
							}
							break;
						case InputTrigger.PushAndHold:
						default:
							if (InputManager.GetInput(key))
							{
								trigger = new InputButton(key);
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
						case InputTrigger.Toggle:
							//// If the button was pressed this frame & hasn't been toggled, switch the toggle.
							//if (InputManager.GetInputDown(button) && !hasToggled)
							//{
							//	IsToggledOn = !IsToggledOn;
							//	hasToggled = true;
							//}
							//if (IsToggledOn)
							//{
							//	trigger = new InputButton(button);
							//	return true;
							//}
							break;
						case InputTrigger.OnPress:
							if (InputManager.GetInputDown(button))
							{
								trigger = new InputButton(button);
								return true;
							}
							break;
						case InputTrigger.OnRelease:
							if (InputManager.GetInputUp(button))
							{
								trigger = new InputButton(button);
								return true;
							}
							break;
						case InputTrigger.PushAndHold:
						default:
							if (InputManager.GetInput(button))
							{
								trigger = new InputButton(button);
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

		/// <summary>
		/// Checks the bindings to see if the action is triggered.
		/// </summary>
		/// <param name="triggers">The <see cref="InputButton"/> that triggered the action.</param>
		/// <returns><c>true</c> if any of the bindings are active, <c>false</c> otherwise.</returns>
		/// <remarks>This will not exit until all binds have been checked to ensure that all triggers
		/// have been found. Do not use this overload unless you need all bindings evaluated and/or 
		/// need all of the <see cref="InputButton"/>s that triggered this <see cref="GameAction"/>.</remarks>
		public bool IsActionTriggered(out InputButton[] triggers)
		{
			List<InputButton> triggersList = new List<InputButton>();
			bool returnVal = false;

			// Prevents multiple inputs from toggling the action off and on
			bool hasToggled = false;

			foreach (var iBttn in InputBttnToggles)
			{
				// If the button was pressed this frame & hasn't been toggled, switch the toggle.
				if (InputManager.GetInputDown(iBttn) && !hasToggled)
				{
					IsToggledOn = !IsToggledOn;
					hasToggled = true;
				}
				if (IsToggledOn)
				{
					// Additional check to prevent all iBttns that toggle from registering as the one that toggled it.
					if (InputManager.GetInputDown(iBttn))
						triggersList.Add(iBttn);
					returnVal = true;
				}
			}

			foreach (var key in KeyToggles)
			{
				// If the button was pressed this frame & hasn't been toggled, switch the toggle.
				if (InputManager.GetInputDown(key) && !hasToggled)
				{
					IsToggledOn = !IsToggledOn;
					hasToggled = true;
				}
				if (IsToggledOn)
				{
					// Additional check to prevent all keys that toggle from registering as the one that toggled it.
					if (InputManager.GetInputDown(key))
						triggersList.Add(new InputButton(key));
					returnVal = true;
				}
			}

			foreach (var button in ButtonToggles)
			{
				// If the button was pressed this frame & hasn't been toggled, switch the toggle.
				if (InputManager.GetInputDown(button) && !hasToggled)
				{
					IsToggledOn = !IsToggledOn;
					hasToggled = true;
				}
				if (IsToggledOn)
				{
					// Additional check to prevent all buttons that toggle from registering as the one that toggled it.
					if (InputManager.GetInputDown(button))
						triggersList.Add(new InputButton(button));
					returnVal = true;
				}
			}

			// InputButtons
			var iKs = InputBttnBindings?.Keys;
			if (iKs != null)
			{
				foreach (InputButton iBttn in iKs)
				{
					// TODO: Implement Deadzone
					switch (InputBttnBindings[iBttn])
					{
						case InputTrigger.Toggle:
							//// If the key was pressed this frame & hasn't been toggled, switch the toggle.
							//if (InputManager.GetInputDown(iBttn) && !hasToggled)
							//{
							//	IsToggledOn = !IsToggledOn;
							//	hasToggled = true;
							//}
							//if (IsToggledOn)
							//{
							//	// Additional check to prevent all iBttns that toggle from registering as the one that toggled it.
							//	if (InputManager.GetInputDown(iBttn))
							//		triggersList.Add(iBttn);
							//	returnVal = true;
							//}
							break;
						case InputTrigger.OnPress:
							if (InputManager.GetInputDown(iBttn))
							{
								triggersList.Add(iBttn);
								returnVal = true;
							}
							break;
						case InputTrigger.OnRelease:
							if (InputManager.GetInputUp(iBttn))
							{
								triggersList.Add(iBttn);
								returnVal = true;
							}
							break;
						case InputTrigger.PushAndHold:
						default:
							if (InputManager.GetInput(iBttn))
							{
								triggersList.Add(iBttn);
								returnVal = true;
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
						case InputTrigger.Toggle:
							//// If the key was pressed this frame & hasn't been toggled, switch the toggle.
							//if (InputManager.GetInputDown(key) && !hasToggled)
							//{
							//	IsToggledOn = !IsToggledOn;
							//	hasToggled = true;
							//}
							//if (IsToggledOn)
							//{
							//	// Additional check to prevent all keys that toggle from registering as the one that toggled it.
							//	if (InputManager.GetInputDown(key))
							//		triggersList.Add(new InputButton(key));
							//	returnVal = true;
							//}
							break;
						case InputTrigger.OnPress:
							if (InputManager.GetInputDown(key))
							{
								triggersList.Add(new InputButton(key));
								returnVal = true;
							}
							break;
						case InputTrigger.OnRelease:
							if (InputManager.GetInputUp(key))
							{
								triggersList.Add(new InputButton(key));
								returnVal = true;
							}
							break;
						case InputTrigger.PushAndHold:
						default:
							if (InputManager.GetInput(key))
							{
								triggersList.Add(new InputButton(key));
								returnVal = true;
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
						case InputTrigger.Toggle:
							//// If the button was pressed this frame & hasn't been toggled, switch the toggle.
							//if (InputManager.GetInputDown(button) && !hasToggled)
							//{
							//	IsToggledOn = !IsToggledOn;
							//	hasToggled = true;
							//}
							//if (IsToggledOn)
							//{
							//	// Additional check to prevent all buttons that toggle from registering as the one that toggled it.
							//	if (InputManager.GetInputDown(button))
							//		triggersList.Add(new InputButton(button));
							//	returnVal = true;
							//}
							break;
						case InputTrigger.OnPress:
							if (InputManager.GetInputDown(button))
							{
								triggersList.Add(new InputButton(button));
								returnVal = true;
							}
							break;
						case InputTrigger.OnRelease:
							if (InputManager.GetInputUp(button))
							{
								triggersList.Add(new InputButton(button));
								returnVal = true;
							}
							break;
						case InputTrigger.PushAndHold:
						default:
							if (InputManager.GetInput(button))
							{
								triggersList.Add(new InputButton(button));
								returnVal = true;
							}
							break;
					}
				}
			}

			triggers = triggersList.ToArray();

			// Finally, if none of the registered binds were activated, then return false.
			return returnVal;
		}
		#endregion
		#region OverwriteBindings

		public void OverwriteBindings(Dictionary<Buttons, InputTrigger> newBindings)
		{
			ButtonBindings = newBindings;
			CheckForToggles();
		}

		public void OverwriteBindings(Dictionary<InputButton, InputTrigger> newBindings)
		{
			InputBttnBindings = newBindings;
			CheckForToggles();
		}
		#endregion

		public void AddBinding(InputButton button, InputTrigger inputType)
		{
			InputBttnBindings?.Add(button, inputType);
			if (inputType == InputTrigger.Toggle)
				InputBttnToggles.Add(button);
		}

		public void ClearBindings()
		{
			InputBttnBindings.Clear();
			InputBttnToggles.Clear();
			KeyBindings.Clear();
			KeyToggles.Clear();
			ButtonBindings.Clear();
			ButtonToggles.Clear();
		}

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