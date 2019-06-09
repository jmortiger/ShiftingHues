using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShiftingHues.Input
{
    // My current thinking is to allow for maximal player remaping by having most player actions bound to an "Action". Each action wil be associated with whatever input triggers it. So remapping controls can be done with anything; even finer inputs like left and right joystick movements can be simplified to varing levels of intentsity and what values trigger them, which would allow for button-only inputs to achive similar effects with modifier keys. Additionally, if an input is bound to something more accurately suited to it (i.e camera movement to mouse movement), I can have more advanced functionality baked in for that specific case.
    public enum GameAction
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
    public class InputComponent : GameComponent, IInputService
    {
        #region Fields and Properties
        public const int NUM_GAME_ACTIONS = 12;
        public static int NumGameActions { get => Enum.GetNames(typeof(GameAction)).Length; }

        public List<BindInfo> Binds { get; private set; }

        #region Input States
        public KeyboardState CurrKeyboardState { get; private set; } = new KeyboardState();
        public KeyboardState PrevKeyboardState { get; private set; } = new KeyboardState();

        public GamePadState CurrPadState { get; private set; } = GamePadState.Default;
        public GamePadState PrevPadState { get; private set; } = GamePadState.Default;

        public MouseStatePlus CurrMouseState { get; private set; } = new MouseStatePlus();
        public MouseStatePlus PrevMouseState { get; private set; } = new MouseStatePlus();

        public List<GameAction> CurrActions { get; private set; }
        public List<GameAction> PrevActions { get; private set; }

        public List<InputButton> CurrButtons { get; private set; } = new List<InputButton>();
        public List<InputButton> PrevButtons { get; private set; } = new List<InputButton>();
        #endregion

        private List<Tuple<Func<bool>, Action>> ObjsToFire = new List<Tuple<Func<bool>, Action>>(3);

        //public delegate void MouseClick(MouseEventArgs e);

        #region MouseEvents

        public event MouseEvent OnMouseMove;

        public event MouseEvent OnRelease;

        public event MouseEvent OnClick;
        #endregion

        #region MenuEvents

        public event MenuEvent OnLeft;

        public event MenuEvent OnRight;

        public event MenuEvent OnUp;

        public event MenuEvent OnDown;
        #endregion

        //public event EventHandler<MouseEventArgs> OnClick;
        #endregion

        #region Constructors
        public InputComponent(Game game,
                              List<BindInfo> binds = null)
            : base(game)
        {
            this.Binds = binds ?? new List<BindInfo>(NUM_GAME_ACTIONS);
            this.CurrActions = new List<GameAction>(NUM_GAME_ACTIONS);
            this.PrevActions = new List<GameAction>(NUM_GAME_ACTIONS);

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
            //SecPMouseState = PrevMouseState;
            PrevMouseState = CurrMouseState;
            PrevActions = CurrActions;
            // Get new states & actions
            CurrKeyboardState = Keyboard.GetState();
            CurrPadState = GamePad.GetState(PlayerIndex.One);
            CurrMouseState = new MouseStatePlus(Mouse.GetState(), PrevMouseState.State);
            CurrActions = tPrev; // Reuse PrevActions to prevent unneed mem alloc
            CurrActions.Clear();
            for (int i = 0; i < Binds.Count; i++)
                if (!CurrActions.Contains(Binds[i].BoundAction) && Binds[i].IsActionTriggered())
                    CurrActions.Add(Binds[i].BoundAction);

            var prevButtons = PrevButtons;
            PrevButtons = CurrButtons;
            CurrButtons = prevButtons;
            CurrButtons.Clear();
            var currPressedKeys = CurrKeyboardState.GetPressedKeys();
            for (int i = 0; i < currPressedKeys.Length; i++)
            {
                CurrButtons.Add(new InputButton(currPressedKeys[i]));
            }

            var currPressedGP = GetPressedButtons();
            for (int i = 0; i < currPressedGP.Length; i++)
            {
                CurrButtons.Add(new InputButton(currPressedGP[i]));
            }

            var currPressedMB = CurrMouseState.GetPressedButtons();
            for (int i = 0; i < currPressedMB.Length; i++)
            {
                CurrButtons.Add(new InputButton(currPressedMB[i]));
            }

            // Basic event system
            for (int i = 0; i < ObjsToFire.Count; i++)
            {
                if (ObjsToFire[i].Item1())
                    ObjsToFire[i].Item2();
            }

            if (GetInputDown(MouseButtons.Left))
                OnClick?.Invoke(new MouseEventArgs(CurrMouseState, PrevMouseState));
            if (GetInputUp(MouseButtons.Left))
                OnRelease?.Invoke(new MouseEventArgs(CurrMouseState, PrevMouseState));
            if (CurrMouseState.PositDelta != Point.Zero)
                OnMouseMove?.Invoke(new MouseEventArgs(CurrMouseState, PrevMouseState));

            base.Update(gameTime);
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
                case InputDeviceType.Keyboard:
                    return GetInput((Keys)bttn.Button);
                case InputDeviceType.GamePad:
                    return GetInput((Buttons)bttn.Button);
                case InputDeviceType.Mouse:
                    return GetInput((MouseButtons)bttn.Button);
                case InputDeviceType.Other:
                    throw new NotImplementedException();
                default:
                    return false;
            }
        }

        /// <summary>
        /// Get if the specified input was released this frame.
        /// </summary>
        /// <param name="bttn">The input to check.</param>
        /// <returns><c>true</c> if the input is currently up and was down last frame, <c>false</c> otherwise.</returns>
        public bool GetInputUp(InputButton bttn)
        {
            switch (bttn.DeviceType)
            {
                case InputDeviceType.None:
                    return false;
                case InputDeviceType.Keyboard:
                    return GetInputUp((Keys)bttn.Button);
                case InputDeviceType.GamePad:
                    return GetInputUp((Buttons)bttn.Button);
                case InputDeviceType.Mouse:
                    return GetInputUp((MouseButtons)bttn.Button);
                case InputDeviceType.Other:
                    throw new NotImplementedException();
                default:
                    return false;
            }
        }

        /// <summary>
        /// Get if the specified input was pressed this frame.
        /// </summary>
        /// <param name="bttn">The input to check.</param>
        /// <returns><c>true</c> if the input is currently down and was up last frame, <c>false</c> otherwise.</returns>
        public bool GetInputDown(InputButton bttn)
        {
            switch (bttn.DeviceType)
            {
                case InputDeviceType.None:
                    return false;
                case InputDeviceType.Keyboard:
                    return GetInputDown((Keys)bttn.Button);
                case InputDeviceType.GamePad:
                    return GetInputDown((Buttons)bttn.Button);
                case InputDeviceType.Mouse:
                    return GetInputDown((MouseButtons)bttn.Button);
                case InputDeviceType.Other:
                    throw new NotImplementedException();
                default:
                    return false;
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
        /// <param name="key">The key to check.</param>
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
        /// Get if the specified button was released this frame.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <returns><c>true</c> if <paramref name="button"/> is currently up and was down last frame, <c>false</c> otherwise.</returns>
        public bool GetInputUp(Buttons button) => PrevPadState.IsButtonDown(button) && CurrPadState.IsButtonUp(button);

        /// <summary>
        /// Get if the specified key was pressed this frame.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <returns><c>true</c> if <paramref name="button"/> is currently down and was up last frame, <c>false</c> otherwise.</returns>
        public bool GetInputDown(Buttons button) => CurrPadState.IsButtonDown(button) && PrevPadState.IsButtonUp(button);
        #endregion

        #region Mouse Buttons
        /// <summary>
        /// Get the current state of the specified <see cref="Mouse"/> button.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <returns><c>true</c> if the button is currently down, <c>false</c> otherwise.</returns>
        public bool GetInput(MouseButtons button) => CurrMouseState.GetButton(button);

        /// <summary>
        /// Get if the specified button was released this frame.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <returns><c>true</c> if the <paramref name="button"/> is currently up and was down last frame, <c>false</c> otherwise.</returns>
        public bool GetInputUp(MouseButtons button) => PrevMouseState.GetButton(button) && !(CurrMouseState.GetButton(button));

        /// <summary>
        /// Get if the specified key was pressed this frame.
        /// </summary>
        /// <param name="button">The button to check.</param>
        /// <returns><c>true</c> if <paramref name="button"/> is currently down and was up last frame, <c>false</c> otherwise.</returns>
        public bool GetInputDown(MouseButtons button) => CurrMouseState.GetButton(button) && !(PrevMouseState.GetButton(button));
        #endregion
        #endregion

        public bool GetMouseBounds(Rectangle bounds) => bounds.Contains(CurrMouseState.Position);
        public bool GetMouseBoundsEnter(Rectangle bounds) => bounds.Contains(CurrMouseState.Position) && !bounds.Contains(PrevMouseState.Position);
        public bool GetMouseBoundsExit(Rectangle bounds) => !bounds.Contains(CurrMouseState.Position) && bounds.Contains(PrevMouseState.Position);

        #region StateChanged

        public bool KBStateChanged() => CurrKeyboardState != PrevKeyboardState;

        public bool GPStateChanged() => CurrPadState != PrevPadState;

        public bool MouseStateChanged() => CurrMouseState != PrevMouseState;
        #endregion

        #region Action Checks
        /// <summary>
        /// TODO: Finish doc
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool IsActionActive(GameAction action) => CurrActions.Contains(action);

        /// <summary>
        /// TODO: Finish doc
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public bool WasActionActive(GameAction action) => PrevActions.Contains(action);

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

        public Buttons[] GetPressedButtons()
        {
            List<Buttons> pressed = new List<Buttons>(2);
            var vals = Enum.GetValues(typeof(Buttons));
            foreach (var val in vals)
                if (GetInput((Buttons)val))
                    pressed.Add((Buttons)val);
            return pressed.ToArray();
        }

        public void AddBind(BindInfo bindInfo)
        {

            throw new NotImplementedException();
        }

        public void AddBinds(BindInfo[] bindInfo)
        {

            throw new NotImplementedException();
        }

        //public void RegisterEvent(Func<bool> condit, )

        public void RegisterEvent(Action actionToFire, Func<bool> fireCondition)
        {
            ObjsToFire.Add(new Tuple<Func<bool>, Action>(fireCondition, actionToFire));
        }

        public void ConstructDefaultConfig()
        {
            // ExitGame: Escape Key (Held)
            Dictionary<InputButton, InputTypes> bB = new Dictionary<InputButton, InputTypes>(2)
            {
                { new InputButton(Buttons.Back), InputTypes.PushAndHold },
                { new InputButton(Keys.Escape), InputTypes.PushAndHold }
            };
            Binds.Add(new BindInfo(this, GameAction.ExitGame, bB));

            // MoveLeft/MenuLeft: A Key, Left Key, NumPad4
            bB = new Dictionary<InputButton, InputTypes>(5)
            {
                { new InputButton(Buttons.DPadLeft), InputTypes.PushAndHold },
                { new InputButton(Buttons.LeftThumbstickLeft), InputTypes.PushAndHold },
                { new InputButton(Keys.A), InputTypes.PushAndHold },
                { new InputButton(Keys.Left), InputTypes.PushAndHold },
                { new InputButton(Keys.NumPad4), InputTypes.PushAndHold }
            };
            Binds.Add(new BindInfo(this, GameAction.MenuLeft, bB));
            Binds.Add(new BindInfo(this, GameAction.MoveLeft, bB));

            bB = new Dictionary<InputButton, InputTypes>(5)
            {
                { new InputButton(Buttons.DPadRight), InputTypes.PushAndHold },
                { new InputButton(Buttons.LeftThumbstickRight), InputTypes.PushAndHold },
                { new InputButton(Keys.D), InputTypes.PushAndHold },
                { new InputButton(Keys.Right), InputTypes.PushAndHold },
                { new InputButton(Keys.NumPad6), InputTypes.PushAndHold }
            };
            Binds.Add(new BindInfo(this, GameAction.MenuRight, bB));
            Binds.Add(new BindInfo(this, GameAction.MoveRight, bB));

            bB = new Dictionary<InputButton, InputTypes>(5)
            {
                { new InputButton(Buttons.DPadUp), InputTypes.PushAndHold },
                { new InputButton(Buttons.LeftThumbstickUp), InputTypes.PushAndHold },
                { new InputButton(Keys.W), InputTypes.PushAndHold },
                { new InputButton(Keys.Up), InputTypes.PushAndHold },
                { new InputButton(Keys.NumPad8), InputTypes.PushAndHold }
            };
            Binds.Add(new BindInfo(this, GameAction.MenuUp, bB));
            Binds.Add(new BindInfo(this, GameAction.Jump, bB));

            bB = new Dictionary<InputButton, InputTypes>(3)
            {
                { new InputButton(Buttons.A), InputTypes.PushAndHold },
                { new InputButton(Keys.Space), InputTypes.PushAndHold },
                { new InputButton(MouseButtons.ScrollUp), InputTypes.PushAndHold }
            };
            Binds.Add(new BindInfo(this, GameAction.Jump, bB));

            bB = new Dictionary<InputButton, InputTypes>(5)
            {
                { new InputButton(Buttons.DPadDown), InputTypes.PushAndHold },
                { new InputButton(Buttons.LeftThumbstickDown), InputTypes.PushAndHold },
                { new InputButton(Keys.S), InputTypes.PushAndHold },
                { new InputButton(Keys.Down), InputTypes.PushAndHold },
                { new InputButton(Keys.NumPad2), InputTypes.PushAndHold }
            };
            Binds.Add(new BindInfo(this, GameAction.MenuDown, bB));

            bB = new Dictionary<InputButton, InputTypes>(3)
            {
                //{ new InputButton(Buttons.RightTrigger), InputTypes.PushAndHold },
                { new InputButton(Buttons.X), InputTypes.PushAndHold },
                { new InputButton(Keys.LeftShift), InputTypes.PushAndHold },
                { new InputButton(Keys.RightShift), InputTypes.PushAndHold }
            };
            Binds.Add(new BindInfo(this, GameAction.Sprint, bB));
        }
        #endregion
    }
}