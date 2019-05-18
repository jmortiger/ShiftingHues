using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShiftingHues.Input
{
    // My current thinking is to allow for maximal player remaping by having most player actions bound to an "Action". Each action wil be associated with whatever input triggers it. So remapping controls can be done with anything; even finer inputs like left and right joystick movements can be simplified to varing levels of intentsity and what values trigger them, which would allow for button-only inputs to achive similar effects with modifier keys. Additionally, if an input is bound to something more accurately suited to it (i.e camera movement to mouse movement), I can have more advanced functionality baked in for that specific case.
    public enum GameActions
    {
        MoveLeft,
        MoveRight,
        Sprint,
        Jump,
        Pause,
        MenuLeft,
        MenuRight,
        MenuUp,
        MenuDown,
        MenuAccept,
        MenuCancel,
        ExitGame
    }
    public class InputComponent : GameComponent
    {
        #region Fields and Properties
        public const int NUM_GAME_ACTIONS = 12;
        public static int NumGameActions { get => Enum.GetNames(typeof(GameActions)).Length; }

        // TODO: Test just for initial bindings setup
        public List<GameActions> setActions = new List<GameActions>(NUM_GAME_ACTIONS);

        /// <summary>
        /// DEPRECATED
        /// </summary>
        public Dictionary<GameActions, Keys[]> mappings;

        public List<BindInfo> Binds { get; private set; }

        #region Input States
        public KeyboardState CurrKeyboardState { get; private set; }
        public KeyboardState PrevKeyboardState { get; private set; }

        public GamePadState CurrPadState { get; private set; }
        public GamePadState PrevPadState { get; private set; }

        public List<GameActions> CurrActions { get; private set; }
        public List<GameActions> PrevActions { get; private set; }
        #endregion
        #endregion

        #region Constructors
        public InputComponent(Game game,
                              List<BindInfo> binds = null,
                              Dictionary<GameActions, Keys[]> mappings = null)
            : base(game)
        {
            this.Binds = (binds != null) ? binds : new List<BindInfo>(NUM_GAME_ACTIONS);
            this.mappings = (mappings != null) ? mappings : new Dictionary<GameActions, Keys[]>(NUM_GAME_ACTIONS);
            this.CurrActions = new List<GameActions>(NUM_GAME_ACTIONS);
            this.PrevActions = new List<GameActions>(NUM_GAME_ACTIONS);

            // Handle input first
            UpdateOrder = 1;
            
            ConstructDefaultConfig();
        }
        #endregion

        #region Methods
        public override void Update(GameTime gameTime)
        {
            // Store old states & actions
            var tPrev = PrevActions; // Reuse PrevActions to prevent unneed mem alloc
            PrevKeyboardState = CurrKeyboardState;
            PrevPadState = CurrPadState;
            PrevActions = CurrActions;
            // Get new states & actions
            CurrKeyboardState = Keyboard.GetState();
            CurrPadState = GamePad.GetState(PlayerIndex.One);
            CurrActions = tPrev; // Reuse PrevActions to prevent unneed mem alloc
            CurrActions.Clear();
            for (int i = 0; i < Binds.Count; i++)
                if (!CurrActions.Contains(Binds[i].BoundAction) && Binds[i].IsActionTriggered())
                    CurrActions.Add(Binds[i].BoundAction);

            base.Update(gameTime);
        }

        public void ConvertMappingsToBindInfo()
        {
            foreach (GameActions gA in mappings.Keys)
            {
                Dictionary<Keys, InputTypes> kB = new Dictionary<Keys, InputTypes>(1);
                kB.Add(mappings[gA][0], InputTypes.PushAndHold);
                Binds.Add(new BindInfo(this, gA, kB));
            }
            //throw new NotImplementedException();
        }

        /// <summary>
        /// DEPRECATED; TODO: Finish doc
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool GetAction(GameActions action)
        {
            for (int i = 0; i < mappings.Count; i++)
            {
                if (CurrKeyboardState[(mappings[action])[i]] == KeyState.Down)
                    return true;
            }
            return false;
        }

        #region GetInput Methods
        #region InputButton
        /// <summary>
        /// Get the current state of the specified button.
        /// </summary>
        /// <param name="bttn">The button to check.</param>
        /// <returns><c>true</c> if the button is currently down, <c>false</c> otherwise.</returns>
        public bool GetInput(InputButton bttn)
        {
            switch (bttn.DeviceType)
            {
                case InputDeviceType.None:
                    return false;
                    break;
                case InputDeviceType.Keyboard:
                    return GetInput((Keys)bttn.Button);
                    break;
                case InputDeviceType.GamePad:
                    return GetInput((Buttons)bttn.Button);
                    break;
                case InputDeviceType.Mouse:
                    throw new NotImplementedException();
                    break;
                case InputDeviceType.Other:
                    throw new NotImplementedException();
                    break;
                default:
                    return false;
                    break;
            }
        }

        /// <summary>
        /// Get if the specified key was released this frame. TODO: Change doc
        /// </summary>
        /// <param name="bttn">The key to check.</param>
        /// <returns><c>true</c> if the key is currently up and was down last frame, <c>false</c> otherwise.</returns>
        public bool GetInputUp(InputButton bttn)
        {
            switch (bttn.DeviceType)
            {
                case InputDeviceType.None:
                    return false;
                    break;
                case InputDeviceType.Keyboard:
                    return GetInputUp((Keys)bttn.Button);
                    break;
                case InputDeviceType.GamePad:
                    return GetInputUp((Buttons)bttn.Button);
                    break;
                case InputDeviceType.Mouse:
                    throw new NotImplementedException();
                    break;
                case InputDeviceType.Other:
                    throw new NotImplementedException();
                    break;
                default:
                    return false;
                    break;
            }
        }

        /// <summary>
        /// Get if the specified key was pressed this frame. TODO: Change doc
        /// </summary>
        /// <param name="bttn">The key to check.</param>
        /// <returns><c>true</c> if the key is currently down and was up last frame, <c>false</c> otherwise.</returns>
        public bool GetInputDown(InputButton bttn)
        {
            switch (bttn.DeviceType)
            {
                case InputDeviceType.None:
                    return false;
                    break;
                case InputDeviceType.Keyboard:
                    return GetInputDown((Keys)bttn.Button);
                    break;
                case InputDeviceType.GamePad:
                    return GetInputDown((Buttons)bttn.Button);
                    break;
                case InputDeviceType.Mouse:
                    throw new NotImplementedException();
                    break;
                case InputDeviceType.Other:
                    throw new NotImplementedException();
                    break;
                default:
                    return false;
                    break;
            }
        }
        #endregion

        #region Keyboard Keys
        /// <summary>
        /// Get the current state of the specified key.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns><c>true</c> if the key is currently down, <c>false</c> otherwise.</returns>
        public bool GetInput(Keys key) => CurrKeyboardState.IsKeyDown(key);

        /// <summary>
        /// Get if the specified key was released this frame.
        /// </summary>
        /// <param name="bttn">The key to check.</param>
        /// <returns><c>true</c> if the key is currently up and was down last frame, <c>false</c> otherwise.</returns>
        public bool GetInputUp(Keys key) => PrevKeyboardState.IsKeyDown(key) && CurrKeyboardState.IsKeyUp(key);

        /// <summary>
        /// Get if the specified key was pressed this frame.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns><c>true</c> if the key is currently down and was up last frame, <c>false</c> otherwise.</returns>
        public bool GetInputDown(Keys key) => CurrKeyboardState.IsKeyDown(key) && PrevKeyboardState.IsKeyUp(key);
        #endregion

        #region GamePad Buttons
        /// <summary>
        /// Get the current state of the specified <see cref="GamePad"/> button.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <returns><c>true</c> if the button is currently down, <c>false</c> otherwise.</returns>
        public bool GetInput(Buttons button) => CurrPadState.IsButtonDown(button);

        /// <summary>
        /// TODO: Finish docs
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool GetInputUp(Buttons button) => PrevPadState.IsButtonDown(button) && CurrPadState.IsButtonUp(button);

        /// <summary>
        /// TODO: Finish docs
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool GetInputDown(Buttons button) => CurrPadState.IsButtonDown(button) && PrevPadState.IsButtonUp(button);
        #endregion
        #endregion

        #region Deprecated
        /// <summary>
        /// Get the current state of the specified key.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns><c>true</c> if the key is currently down, <c>false</c> otherwise.</returns>
        public bool GetKey(Keys key) => CurrKeyboardState.IsKeyDown(key);

        /// <summary>
        /// Get if the specified key was released this frame.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns><c>true</c> if the key is currently up and was down last frame, <c>false</c> otherwise.</returns>
        public bool GetKeyUp(Keys key) => PrevKeyboardState.IsKeyDown(key) && CurrKeyboardState.IsKeyUp(key);

        /// <summary>
        /// Get if the specified key was pressed this frame.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns><c>true</c> if the key is currently down and was up last frame, <c>false</c> otherwise.</returns>
        public bool GetKeyDown(Keys key) => CurrKeyboardState.IsKeyDown(key) && PrevKeyboardState.IsKeyUp(key);

        /// <summary>
        /// Get the current state of the specified <see cref="GamePad"/> button.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <returns><c>true</c> if the button is currently down, <c>false</c> otherwise.</returns>
        public bool GetPadBttn(Buttons button) => CurrPadState.IsButtonDown(button);

        /// <summary>
        /// TODO: Finish doc
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool GetPadBttnUp(Buttons button) => PrevPadState.IsButtonDown(button) && CurrPadState.IsButtonUp(button);

        /// <summary>
        /// TODO: Finish doc
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool GetPadBttnDown(Buttons button) => CurrPadState.IsButtonDown(button) && PrevPadState.IsButtonUp(button); 
        #endregion

        public bool KBStateChanged() => CurrKeyboardState != PrevKeyboardState;

        public bool GPStateChanged() => CurrPadState != PrevPadState;

        #region Action Checks
        /// <summary>
        /// TODO: Finish doc
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool IsActionActive(GameActions action) => CurrActions.Contains(action);

        /// <summary>
        /// TODO: Finish doc
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool WasActionActive(GameActions action) => PrevActions.Contains(action);

        /// <summary>
        /// TODO: Finish doc
        /// </summary>
        /// <returns></returns>
        public bool AreActionsActive() => CurrActions.Count != 0;

        /// <summary>
        /// TODO: Finish doc
        /// </summary>
        /// <returns></returns>
        public bool WereActionsActive() => PrevActions.Count != 0;
        #endregion

        public void ConstructDefaultConfig()
        {
            // ExitGame: Escape Key (Held)
            Dictionary<Keys, InputTypes> kB = new Dictionary<Keys, InputTypes>(1);
            kB.Add(Keys.Escape, InputTypes.PushAndHold);
            Binds.Add(new BindInfo(this, GameActions.ExitGame, kB));

            Dictionary<InputButton, InputTypes> bB = new Dictionary<InputButton, InputTypes>(2);
            bB.Add(new InputButton(InputDeviceType.GamePad, (int)Buttons.Back), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.Keyboard, (int)Keys.Escape), InputTypes.PushAndHold);
            Binds.Add(new BindInfo(this, GameActions.ExitGame, bB));

            // MoveLeft/MenuLeft: A Key, Left Key, NumPad4
            //kB = new Dictionary<Keys, InputTypes>(3);
            //kB.Add(Keys.A, InputTypes.PushAndHold);
            //kB.Add(Keys.Left, InputTypes.PushAndHold);
            //kB.Add(Keys.NumPad4, InputTypes.PushAndHold);
            //Binds.Add(new BindInfo(this, GameActions.MenuLeft, kB));
            //Binds.Add(new BindInfo(this, GameActions.MoveLeft, kB));

            bB = new Dictionary<InputButton, InputTypes>(5);
            bB.Add(new InputButton(InputDeviceType.GamePad, (int)Buttons.DPadLeft), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.GamePad, (int)Buttons.LeftThumbstickLeft), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.Keyboard, (int)Keys.A), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.Keyboard, (int)Keys.Left), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.Keyboard, (int)Keys.NumPad4), InputTypes.PushAndHold);
            Binds.Add(new BindInfo(this, GameActions.MenuLeft, bB));
            Binds.Add(new BindInfo(this, GameActions.MoveLeft, bB));

            //kB = new Dictionary<Keys, InputTypes>(3);
            //kB.Add(Keys.D, InputTypes.PushAndHold);
            //kB.Add(Keys.Right, InputTypes.PushAndHold);
            //kB.Add(Keys.NumPad6, InputTypes.PushAndHold);
            //Binds.Add(new BindInfo(this, GameActions.MenuRight, kB));
            //Binds.Add(new BindInfo(this, GameActions.MoveRight, kB));

            bB = new Dictionary<InputButton, InputTypes>(5);
            bB.Add(new InputButton(InputDeviceType.GamePad, (int)Buttons.DPadRight), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.GamePad, (int)Buttons.LeftThumbstickRight), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.Keyboard, (int)Keys.D), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.Keyboard, (int)Keys.Right), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.Keyboard, (int)Keys.NumPad6), InputTypes.PushAndHold);
            Binds.Add(new BindInfo(this, GameActions.MenuRight, bB));
            Binds.Add(new BindInfo(this, GameActions.MoveRight, bB));

            //kB = new Dictionary<Keys, InputTypes>(3);
            //kB.Add(Keys.W, InputTypes.PushAndHold);
            //kB.Add(Keys.Up, InputTypes.PushAndHold);
            //kB.Add(Keys.NumPad8, InputTypes.PushAndHold);
            //Binds.Add(new BindInfo(this, GameActions.MenuUp, kB));
            //Binds.Add(new BindInfo(this, GameActions.Jump, kB));

            bB = new Dictionary<InputButton, InputTypes>(5);
            bB.Add(new InputButton(InputDeviceType.GamePad, (int)Buttons.DPadUp), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.GamePad, (int)Buttons.LeftThumbstickUp), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.Keyboard, (int)Keys.W), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.Keyboard, (int)Keys.Up), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.Keyboard, (int)Keys.NumPad8), InputTypes.PushAndHold);
            Binds.Add(new BindInfo(this, GameActions.MenuUp, bB));
            Binds.Add(new BindInfo(this, GameActions.Jump, bB));

            //kB = new Dictionary<Keys, InputTypes>(1);
            //kB.Add(Keys.Space, InputTypes.PushAndHold);
            //Binds.Add(new BindInfo(this, GameActions.Jump, kB));

            bB = new Dictionary<InputButton, InputTypes>(2);
            bB.Add(new InputButton(InputDeviceType.GamePad, (int)Buttons.A), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.Keyboard, (int)Keys.Space), InputTypes.PushAndHold);
            Binds.Add(new BindInfo(this, GameActions.Jump, bB));

            //kB = new Dictionary<Keys, InputTypes>(3);
            //kB.Add(Keys.S, InputTypes.PushAndHold);
            //kB.Add(Keys.Down, InputTypes.PushAndHold);
            //kB.Add(Keys.NumPad2, InputTypes.PushAndHold);
            //Binds.Add(new BindInfo(this, GameActions.MenuDown, kB));

            bB = new Dictionary<InputButton, InputTypes>(5);
            bB.Add(new InputButton(InputDeviceType.GamePad, (int)Buttons.DPadDown), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.GamePad, (int)Buttons.LeftThumbstickDown), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.Keyboard, (int)Keys.S), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.Keyboard, (int)Keys.Down), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.Keyboard, (int)Keys.NumPad2), InputTypes.PushAndHold);
            Binds.Add(new BindInfo(this, GameActions.MenuDown, bB));

            //kB = new Dictionary<Keys, InputTypes>(2);
            //kB.Add(Keys.LeftShift, InputTypes.PushAndHold);
            //kB.Add(Keys.RightShift, InputTypes.PushAndHold);
            //Binds.Add(new BindInfo(this, GameActions.Sprint, kB));

            bB = new Dictionary<InputButton, InputTypes>(4);
            //bB.Add(new InputButton(InputDeviceType.GamePad, (int)Buttons.RightTrigger), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.GamePad, (int)Buttons.X), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.Keyboard, (int)Keys.LeftShift), InputTypes.PushAndHold);
            bB.Add(new InputButton(InputDeviceType.Keyboard, (int)Keys.RightShift), InputTypes.PushAndHold);
            Binds.Add(new BindInfo(this, GameActions.Sprint, bB));

            //kB = new Dictionary<Keys, InputTypes>(1);
            //kB.Add(mappings[gA][0], InputTypes.PushAndHold);
            //Binds.Add(new BindInfo(this, gA, kB));
        }
        #endregion
    }
}