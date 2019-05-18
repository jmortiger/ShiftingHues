using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShiftingHues.Input
{
    public enum InputTypes
    {
        /// <summary>
        /// A continued press of a button (e.g firing an automatic gun, moving forward, ...).
        /// </summary>
        PushAndHold,
        /// <summary>
        /// A switch from active to inactive (this is an accesible alternative to <see cref="PushAndHold"/>).
        /// </summary>
        Toggle,
        /// <summary>
        /// Fires only when button is first pressed, not held (e.g firing a pistol, melee attack, ...)
        /// </summary>
        OnPress,
        /// <summary>
        /// Fires only when button is released (e.g firing a bow (Minecraft), ...)
        /// </summary>
        OnRelease
    }
    public class BindInfo
    {
        #region Fields and Properties
        /// <summary>
        /// Reference to the <see cref="InputComponent"/>.
        /// </summary>
        private InputComponent inputManager;

        /// <summary>
        /// The action triggered by this binding.
        /// </summary>
        public GameActions BoundAction { get; private set; }

        #region Binds
        private Dictionary<InputButton, InputTypes> inputBttnBindings;
        /// <summary>
        /// The list of inputs that trigger the action and the ways that the pressing of the inputs fires the action.
        /// </summary>
        public Dictionary<InputButton, InputTypes> InputBttnBindings
        {
            get { return inputBttnBindings; }
            //set { keyBindings = value; }
        }

        private Dictionary<Keys, InputTypes> keyBindings;
        /// <summary>
        /// The list of keys that trigger the action and the ways that the pressing of the keys fires the action.
        /// </summary>
        public Dictionary<Keys, InputTypes> KeyBindings
        {
            get { return keyBindings; }
            //set { keyBindings = value; }
        }

        private Dictionary<Buttons, InputTypes> buttonBindings;
        /// <summary>
        /// The list of buttons that trigger the action and the ways that the pressing of the buttons fires the action.
        /// </summary>
        public Dictionary<Buttons, InputTypes> ButtonBindings
        {
            get { return buttonBindings; }
            //set { buttonBindings = value; }
        }
        #endregion
        #endregion

        #region Constructors

        public BindInfo(InputComponent inputManager, 
                        GameActions boundAction,
                        Dictionary<InputButton, InputTypes> inputBttnBindings = null, 
                        Dictionary<Keys, InputTypes> keyBindings = null,
                        Dictionary<Buttons, InputTypes> buttonBindings = null)
        {
            this.inputManager = inputManager;
            this.BoundAction = boundAction;
            this.keyBindings = keyBindings;
            this.buttonBindings = buttonBindings;
            this.inputBttnBindings = inputBttnBindings;
        }

        public BindInfo(InputComponent inputManager,
                        GameActions boundAction,
                        Dictionary<Keys, InputTypes> keyBindings = null,
                        Dictionary<Buttons, InputTypes> buttonBindings = null)
        {
            this.inputManager = inputManager;
            this.BoundAction = boundAction;
            this.keyBindings = keyBindings;
            this.buttonBindings = buttonBindings;
            this.inputBttnBindings = new Dictionary<InputButton, InputTypes>();
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
                                return true;
                            break;
                        case InputTypes.OnPress:
                            if (inputManager.GetInputDown(iBttn))
                                return true;
                            break;
                        case InputTypes.OnRelease:
                            if (inputManager.GetInputUp(iBttn))
                                return true;
                            break;
                        case InputTypes.PushAndHold:
                        default:
                            if (inputManager.GetInput(iBttn))
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
                        case InputTypes.Toggle:
                            // If the key was pressed this frame & hasn't been toggled, switch the toggle.
                            if (inputManager.GetInputDown(key) && !hasToggled)
                            {
                                IsToggledOn = !IsToggledOn;
                                hasToggled = true;
                            }
                            if (IsToggledOn)
                                return true;
                            break;
                        case InputTypes.OnPress:
                            if (inputManager.GetInputDown(key))
                                return true;
                            break;
                        case InputTypes.OnRelease:
                            if (inputManager.GetInputUp(key))
                                return true;
                            break;
                        case InputTypes.PushAndHold:
                        default:
                            if (inputManager.GetInput(key))
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
                        case InputTypes.Toggle:
                            // If the button was pressed this frame & hasn't been toggled, switch the toggle.
                            if (inputManager.GetInputDown(button) && !hasToggled)
                            {
                                IsToggledOn = !IsToggledOn;
                                hasToggled = true;
                            }
                            if (IsToggledOn)
                                return true;
                            break;
                        case InputTypes.OnPress:
                            if (inputManager.GetInputDown(button))
                                return true;
                            break;
                        case InputTypes.OnRelease:
                            if (inputManager.GetInputUp(button))
                                return true;
                            break;
                        case InputTypes.PushAndHold:
                        default:
                            if (inputManager.GetInput(button))
                                return true;
                            break;
                    }
                }
            }

            // Finally, if none of the registered binds were activated, then return false.
            return false;
        }

        public void OverwriteBindings(Dictionary<Buttons, InputTypes> newBindings)
        {
            buttonBindings = newBindings;
        }
        #endregion
    }
}