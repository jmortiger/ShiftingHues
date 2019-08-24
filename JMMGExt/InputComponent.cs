using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace JMMGExt.Input
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
		ExitGame,
		ToggleConsole,
		ToggleDebugMenu
	}
	public class InputComponent : GameComponent, IInputService
	{
		#region Fields and Properties
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

		private Vector2 nextMousePosit = new Vector2(float.NaN, float.NaN);
		public Vector2 NextMousePosit { get => nextMousePosit; private set => nextMousePosit = value; }

		private List<Tuple<Func<bool>, Action>> ObjsToFire = new List<Tuple<Func<bool>, Action>>(3);

		#region MouseEvents

		public event MouseEvent OnMouseMove;

		public event MouseEvent OnRelease;

		public event MouseEvent OnClick;
		#endregion

		#region MenuEvents

		public event MenuEvent MenuLeft;

		public event MenuEvent MenuRight;

		public event MenuEvent MenuUp;

		public event MenuEvent MenuDown;

		public event MenuEvent MenuAccept;

		public event MenuEvent MenuCancel;

		public event MenuEvent AnyMenuInput;
		#endregion
		#endregion

		public InputComponent(Game game, List<BindInfo> binds = null)
			: base(game)
		{
			this.Binds = binds ?? new List<BindInfo>(NumGameActions);
			this.CurrActions = new List<GameAction>(NumGameActions);
			this.PrevActions = new List<GameAction>(NumGameActions);

			// Handle input first
			UpdateOrder = 1;

			ConstructDefaultConfig();
		}

		#region Methods
		public override void Update(GameTime gameTime)
		{
			ServiceLocator.LogMessage("Input", "Update started", LogLevel.Verbose, LogType.Log);
			// Store old states & actions
			var tPrev = PrevActions; // Reuse PrevActions to prevent unneed mem alloc
			PrevKeyboardState = CurrKeyboardState;
			PrevPadState = CurrPadState;
			PrevMouseState = CurrMouseState;
			PrevActions = CurrActions;
			// Get new states & actions
			CurrKeyboardState = Keyboard.GetState();
			CurrPadState = GamePad.GetState(PlayerIndex.One);
			if (!float.IsNaN(NextMousePosit.X) || !float.IsNaN(NextMousePosit.Y))
			{
				Mouse.SetPosition((int)NextMousePosit.X, (int)NextMousePosit.Y);
				nextMousePosit.X = float.NaN;
				nextMousePosit.Y = float.NaN;
			}
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

			if (AreActionsActive())
			{
				if (IsActionActive(GameAction.MenuUp))
					MenuUp?.Invoke(new MenuEventArgs(GameAction.MenuUp));
				if (IsActionActive(GameAction.MenuDown))
					MenuDown?.Invoke(new MenuEventArgs(GameAction.MenuDown));
				if (IsActionActive(GameAction.MenuLeft))
					MenuLeft?.Invoke(new MenuEventArgs(GameAction.MenuLeft));
				if (IsActionActive(GameAction.MenuRight))
					MenuRight?.Invoke(new MenuEventArgs(GameAction.MenuRight));
				if (IsActionActive(GameAction.MenuAccept))
					MenuAccept?.Invoke(new MenuEventArgs(GameAction.MenuAccept));
				if (IsActionActive(GameAction.MenuCancel))
					MenuCancel?.Invoke(new MenuEventArgs(GameAction.MenuCancel));
			}

			base.Update(gameTime);
			ServiceLocator.LogMessage("Input", "Update finished", LogLevel.Verbose, LogType.Log);
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

		/// <summary>
		/// Get if the mouse is in the specified <see cref="Rectangle"/>.
		/// </summary>
		/// <param name="bounds">The <see cref="Rectangle"/> to check.</param>
		/// <returns><c>true</c> if the mouse is inside, <c>false</c> otherwise.</returns>
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

		public void SetNextMousePosition(Vector2 newPos)
		{
			NextMousePosit = newPos;
			ServiceLocator.LogMessage("Input", $"Mouse position to be set to {{{newPos.ToString()}}} next frame.");
		}

		//public void SetNextMousePosition(Point newPos) => SetNextMousePosition(newPos.ToVector2());

		//public void SetNextMousePosition(FPoint newPos) => SetNextMousePosition(newPos.ToVector2());

		//public void UpdateMousePosit(Vector2 newPos)
		//{
		//	Mouse.SetPosition((int)newPos.X, (int)newPos.Y);
		//	MouseState temp = new MouseState(
		//		(int)newPos.X,
		//		(int)newPos.Y,
		//		CurrMouseState.State.ScrollWheelValue,
		//		CurrMouseState.State.LeftButton,
		//		CurrMouseState.State.MiddleButton,
		//		CurrMouseState.State.RightButton,
		//		CurrMouseState.State.XButton1,
		//		CurrMouseState.State.XButton2,
		//		CurrMouseState.State.HorizontalScrollWheelValue);
		//	CurrMouseState = new MouseStatePlus(temp, PrevMouseState.State);
		//}

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

			// Console
			bB = new Dictionary<InputButton, InputTypes>(1)
			{
				{new InputButton(Keys.OemTilde), InputTypes.OnPress }
			};
			Binds.Add(new BindInfo(this, GameAction.ToggleConsole, bB));

			//// Debug
			//bB = new Dictionary<InputButton, InputTypes>(1)
			//{
			//	{new InputButton(Keys.OemTilde), InputTypes.Toggle }
			//};
			//Binds.Add(new BindInfo(this, GameAction.ToggleConsole, bB));
		}
		#endregion

		#region Static Methods
		public static bool IsCharacter(Keys key)
		{
			switch (key)
			{
				case Keys.None:
					return false;
				case Keys.Back:
					return false; // May change
				case Keys.Tab:
					return true; // May change
				case Keys.Enter:
					return true; // May change
				case Keys.CapsLock:
					return false;
				case Keys.Escape:
					return false;
				case Keys.Space:
					return true; // May change
				case Keys.PageUp:
				case Keys.PageDown:
					return false;
				case Keys.End:
				case Keys.Home:
					return false;
				case Keys.Left:
				case Keys.Up:
				case Keys.Right:
				case Keys.Down:
					return false;
				case Keys.Select:
					return false;
				case Keys.Print:
					return false;
				case Keys.Execute:
					return false;
				case Keys.PrintScreen:
					return false;
				case Keys.Insert:
					return false;
				case Keys.Delete:
					return false; // May change
				case Keys.Help:
					return false;
				case Keys.D0:
				case Keys.D1:
				case Keys.D2:
				case Keys.D3:
				case Keys.D4:
				case Keys.D5:
				case Keys.D6:
				case Keys.D7:
				case Keys.D8:
				case Keys.D9:
					return false;
				case Keys.A:
				case Keys.B:
				case Keys.C:
				case Keys.D:
				case Keys.E:
				case Keys.F:
				case Keys.G:
				case Keys.H:
				case Keys.I:
				case Keys.J:
				case Keys.K:
				case Keys.L:
				case Keys.M:
				case Keys.N:
				case Keys.O:
				case Keys.P:
				case Keys.Q:
				case Keys.R:
				case Keys.S:
				case Keys.T:
				case Keys.U:
				case Keys.V:
				case Keys.W:
				case Keys.X:
				case Keys.Y:
				case Keys.Z:
					return true;
				case Keys.LeftWindows:
				case Keys.RightWindows:
					return false;
				case Keys.Apps:
					return false;
				case Keys.Sleep:
					return false;
				case Keys.NumPad0:
				case Keys.NumPad1:
				case Keys.NumPad2:
				case Keys.NumPad3:
				case Keys.NumPad4:
				case Keys.NumPad5:
				case Keys.NumPad6:
				case Keys.NumPad7:
				case Keys.NumPad8:
				case Keys.NumPad9:
					return true; // May change
				case Keys.Multiply:
				case Keys.Add:
					return true;
				case Keys.Separator:
					return false; // May change
				case Keys.Subtract:
					return true;
				case Keys.Decimal:
					return true;
				case Keys.Divide:
					return true; // May change
				case Keys.F1:
				case Keys.F2:
				case Keys.F3:
				case Keys.F4:
				case Keys.F5:
				case Keys.F6:
				case Keys.F7:
				case Keys.F8:
				case Keys.F9:
				case Keys.F10:
				case Keys.F11:
				case Keys.F12:
				case Keys.F13:
				case Keys.F14:
				case Keys.F15:
				case Keys.F16:
				case Keys.F17:
				case Keys.F18:
				case Keys.F19:
				case Keys.F20:
				case Keys.F21:
				case Keys.F22:
				case Keys.F23:
				case Keys.F24:
					return false;
				case Keys.NumLock:
					return false;
				case Keys.Scroll:
					return false;
				case Keys.LeftShift:
				case Keys.RightShift:
					return false; // May change
				case Keys.LeftControl:
				case Keys.RightControl:
				case Keys.LeftAlt:
				case Keys.RightAlt: // May change
					return false;
				//case Keys.BrowserBack:
				//	break;
				//case Keys.BrowserForward:
				//	break;
				//case Keys.BrowserRefresh:
				//	break;
				//case Keys.BrowserStop:
				//	break;
				//case Keys.BrowserSearch:
				//	break;
				//case Keys.BrowserFavorites:
				//	break;
				//case Keys.BrowserHome:
				//	break;
				//case Keys.VolumeMute:
				//	break;
				//case Keys.VolumeDown:
				//	break;
				//case Keys.VolumeUp:
				//	break;
				//case Keys.MediaNextTrack:
				//	break;
				//case Keys.MediaPreviousTrack:
				//	break;
				//case Keys.MediaStop:
				//	break;
				//case Keys.MediaPlayPause:
				//	break;
				//case Keys.LaunchMail:
				//	break;
				//case Keys.SelectMedia:
				//	break;
				//case Keys.LaunchApplication1:
				//	break;
				//case Keys.LaunchApplication2:
				//	break;
				case Keys.OemSemicolon:
				case Keys.OemPlus:
				case Keys.OemComma:
				case Keys.OemMinus:
				case Keys.OemPeriod:
				case Keys.OemQuestion:
				case Keys.OemTilde:
				case Keys.OemOpenBrackets:
				case Keys.OemPipe:
				case Keys.OemCloseBrackets:
				case Keys.OemQuotes:
					return true; // May change
				case Keys.Oem8:
					return false; // IDFK what this is
				case Keys.OemBackslash:
					return true; // May change
				case Keys.ProcessKey:
				case Keys.Attn:
				case Keys.Crsel:
				case Keys.Exsel:
				case Keys.EraseEof:
				case Keys.Play:
				case Keys.Zoom:
				case Keys.Pa1:
				case Keys.OemClear:
				case Keys.ChatPadGreen:
				case Keys.ChatPadOrange:
				case Keys.Pause:
				case Keys.ImeConvert:
				case Keys.ImeNoConvert:
				case Keys.Kana:
				case Keys.Kanji:
				case Keys.OemAuto:
				case Keys.OemCopy:
				case Keys.OemEnlW:
					return false;
				default:
					return false;
			}
			//return false;
		}

		public static string GetCurrentCharacterAsString(KeyboardState? state = null, bool acceptEnter = false, bool acceptBackspace = false)
		{
			KeyboardState keyboardState;
			try
			{
				keyboardState = state ?? ServiceLocator.GetInputService().CurrKeyboardState;
			}
			catch (NullReferenceException e) // TODO: Rework once error handling w/ soft noncritical errors can pass
			{
				Console.WriteLine(e);
				return String.Empty;
				//throw;
			}
			//KeyboardState keyboardState = state ?? ServiceLocator.GetInputService().CurrKeyboardState;
			string returnString = "";
			var keys = keyboardState.GetPressedKeys();
			for (int i = 0; i < keys.Length; i++)
			{
				switch (keys[i])
				{
					//case Keys.None:
					//	break;
					case Keys.Back:
						returnString = ((acceptBackspace) ? "\b" : "") + returnString;
						break;
					case Keys.Tab:
						returnString += "\t";
						break;
					case Keys.Enter:
						returnString += (acceptEnter) ? "\n" : "";
						break;
					//case Keys.CapsLock:
					//	break;
					//case Keys.Escape:
					//	break;
					case Keys.Space:
						returnString += " ";
						break;
					//case Keys.PageUp:
					//	break;
					//case Keys.PageDown:
					//	break;
					//case Keys.End:
					//	break;
					//case Keys.Home:
					//	break;
					//case Keys.Left:
					//	break;
					//case Keys.Up:
					//	break;
					//case Keys.Right:
					//	break;
					//case Keys.Down:
					//	break;
					//case Keys.Select:
					//	break;
					//case Keys.Print:
					//	break;
					//case Keys.Execute:
					//	break;
					//case Keys.PrintScreen:
					//	break;
					//case Keys.Insert:
					//	break;
					//case Keys.Delete:
					//	break;
					//case Keys.Help:
					//	break;
					//case Keys.D0:
					//	break;
					//case Keys.D1:
					//	break;
					//case Keys.D2:
					//	break;
					//case Keys.D3:
					//	break;
					//case Keys.D4:
					//	break;
					//case Keys.D5:
					//	break;
					//case Keys.D6:
					//	break;
					//case Keys.D7:
					//	break;
					//case Keys.D8:
					//	break;
					//case Keys.D9:
					//	break;
					case Keys.A:
						returnString += (ShouldCapitalize(keyboardState)) ? "A" : "a";
						break;
					case Keys.B:
						returnString += (ShouldCapitalize(keyboardState)) ? "B" : "b";
						break;
					case Keys.C:
						returnString += (ShouldCapitalize(keyboardState)) ? "C" : "c";
						break;
					case Keys.D:
						returnString += (ShouldCapitalize(keyboardState)) ? "D" : "d";
						break;
					case Keys.E:
						returnString += (ShouldCapitalize(keyboardState)) ? "E" : "e";
						break;
					case Keys.F:
						returnString += (ShouldCapitalize(keyboardState)) ? "F" : "f";
						break;
					case Keys.G:
						returnString += (ShouldCapitalize(keyboardState)) ? "G" : "g";
						break;
					case Keys.H:
						returnString += (ShouldCapitalize(keyboardState)) ? "H" : "h";
						break;
					case Keys.I:
						returnString += (ShouldCapitalize(keyboardState)) ? "I" : "i";
						break;
					case Keys.J:
						returnString += (ShouldCapitalize(keyboardState)) ? "J" : "j";
						break;
					case Keys.K:
						returnString += (ShouldCapitalize(keyboardState)) ? "K" : "k";
						break;
					case Keys.L:
						returnString += (ShouldCapitalize(keyboardState)) ? "L" : "l";
						break;
					case Keys.M:
						returnString += (ShouldCapitalize(keyboardState)) ? "M" : "m";
						break;
					case Keys.N:
						returnString += (ShouldCapitalize(keyboardState)) ? "N" : "n";
						break;
					case Keys.O:
						returnString += (ShouldCapitalize(keyboardState)) ? "O" : "o";
						break;
					case Keys.P:
						returnString += (ShouldCapitalize(keyboardState)) ? "P" : "p";
						break;
					case Keys.Q:
						returnString += (ShouldCapitalize(keyboardState)) ? "Q" : "q";
						break;
					case Keys.R:
						returnString += (ShouldCapitalize(keyboardState)) ? "R" : "r";
						break;
					case Keys.S:
						returnString += (ShouldCapitalize(keyboardState)) ? "S" : "s";
						break;
					case Keys.T:
						returnString += (ShouldCapitalize(keyboardState)) ? "T" : "t";
						break;
					case Keys.U:
						returnString += (ShouldCapitalize(keyboardState)) ? "U" : "u";
						break;
					case Keys.V:
						returnString += (ShouldCapitalize(keyboardState)) ? "V" : "v";
						break;
					case Keys.W:
						returnString += (ShouldCapitalize(keyboardState)) ? "W" : "w";
						break;
					case Keys.X:
						returnString += (ShouldCapitalize(keyboardState)) ? "X" : "x";
						break;
					case Keys.Y:
						returnString += (ShouldCapitalize(keyboardState)) ? "Y" : "y";
						break;
					case Keys.Z:
						returnString += (ShouldCapitalize(keyboardState)) ? "Z" : "z";
						break;
					//case Keys.LeftWindows:
					//	break;
					//case Keys.RightWindows:
					//	break;
					//case Keys.Apps:
					//	break;
					//case Keys.Sleep:
					//	break;
					case Keys.NumPad0:
						returnString += (keyboardState.NumLock) ? "0" : "";
						break;
					case Keys.NumPad1:
						returnString += (keyboardState.NumLock) ? "1" : "";
						break;
					case Keys.NumPad2:
						returnString += (keyboardState.NumLock) ? "2" : "";
						break;
					case Keys.NumPad3:
						returnString += (keyboardState.NumLock) ? "3" : "";
						break;
					case Keys.NumPad4:
						returnString += (keyboardState.NumLock) ? "4" : "";
						break;
					case Keys.NumPad5:
						returnString += (keyboardState.NumLock) ? "5" : "";
						break;
					case Keys.NumPad6:
						returnString += (keyboardState.NumLock) ? "6" : "";
						break;
					case Keys.NumPad7:
						returnString += (keyboardState.NumLock) ? "7" : "";
						break;
					case Keys.NumPad8:
						returnString += (keyboardState.NumLock) ? "8" : "";
						break;
					case Keys.NumPad9:
						returnString += (keyboardState.NumLock) ? "9" : "";
						break;
					case Keys.Multiply:
						returnString += "*";
						break;
					case Keys.Add:
						returnString += "+";
						break;
					//case Keys.Separator:
					//	break;
					case Keys.Subtract:
						returnString += "-";
						break;
					case Keys.Decimal:
						returnString += ".";
						break;
					case Keys.Divide:
						returnString += "/";
						break;
					//case Keys.F1:
					//	break;
					//case Keys.F2:
					//	break;
					//case Keys.F3:
					//	break;
					//case Keys.F4:
					//	break;
					//case Keys.F5:
					//	break;
					//case Keys.F6:
					//	break;
					//case Keys.F7:
					//	break;
					//case Keys.F8:
					//	break;
					//case Keys.F9:
					//	break;
					//case Keys.F10:
					//	break;
					//case Keys.F11:
					//	break;
					//case Keys.F12:
					//	break;
					//case Keys.F13:
					//	break;
					//case Keys.F14:
					//	break;
					//case Keys.F15:
					//	break;
					//case Keys.F16:
					//	break;
					//case Keys.F17:
					//	break;
					//case Keys.F18:
					//	break;
					//case Keys.F19:
					//	break;
					//case Keys.F20:
					//	break;
					//case Keys.F21:
					//	break;
					//case Keys.F22:
					//	break;
					//case Keys.F23:
					//	break;
					//case Keys.F24:
					//	break;
					//case Keys.NumLock:
					//	break;
					//case Keys.Scroll:
					//	break;
					//case Keys.LeftShift:
					//	break;
					//case Keys.RightShift:
					//	break;
					//case Keys.LeftControl:
					//	break;
					//case Keys.RightControl:
					//	break;
					//case Keys.LeftAlt:
					//	break;
					//case Keys.RightAlt:
					//	break;
					//case Keys.BrowserBack:
					//	break;
					//case Keys.BrowserForward:
					//	break;
					//case Keys.BrowserRefresh:
					//	break;
					//case Keys.BrowserStop:
					//	break;
					//case Keys.BrowserSearch:
					//	break;
					//case Keys.BrowserFavorites:
					//	break;
					//case Keys.BrowserHome:
					//	break;
					//case Keys.VolumeMute:
					//	break;
					//case Keys.VolumeDown:
					//	break;
					//case Keys.VolumeUp:
					//	break;
					//case Keys.MediaNextTrack:
					//	break;
					//case Keys.MediaPreviousTrack:
					//	break;
					//case Keys.MediaStop:
					//	break;
					//case Keys.MediaPlayPause:
					//	break;
					//case Keys.LaunchMail:
					//	break;
					//case Keys.SelectMedia:
					//	break;
					//case Keys.LaunchApplication1:
					//	break;
					//case Keys.LaunchApplication2:
					//	break;
					case Keys.OemSemicolon:
						returnString += ";";
						break;
					case Keys.OemPlus:
						returnString += "+";
						break;
					case Keys.OemComma:
						returnString += ",";
						break;
					case Keys.OemMinus:
						returnString += "-";
						break;
					case Keys.OemPeriod:
						returnString += ".";
						break;
					case Keys.OemQuestion:
						returnString += "?";
						break;
					case Keys.OemTilde:
						returnString += "~";
						break;
					case Keys.OemOpenBrackets:
						returnString += "(";
						break;
					case Keys.OemPipe:
						returnString += "|";
						break;
					case Keys.OemCloseBrackets:
						returnString += ")";
						break;
					case Keys.OemQuotes:
						returnString += "\"";
						break;
					//case Keys.Oem8:
					//	break;
					case Keys.OemBackslash:
						returnString += "\\";
						break;
					//case Keys.ProcessKey:
					//	break;
					//case Keys.Attn:
					//	break;
					//case Keys.Crsel:
					//	break;
					//case Keys.Exsel:
					//	break;
					//case Keys.EraseEof:
					//	break;
					//case Keys.Play:
					//	break;
					//case Keys.Zoom:
					//	break;
					//case Keys.Pa1:
					//	break;
					//case Keys.OemClear:
					//	break;
					//case Keys.ChatPadGreen:
					//	break;
					//case Keys.ChatPadOrange:
					//	break;
					//case Keys.Pause:
					//	break;
					//case Keys.ImeConvert:
					//	break;
					//case Keys.ImeNoConvert:
					//	break;
					//case Keys.Kana:
					//	break;
					//case Keys.Kanji:
					//	break;
					//case Keys.OemAuto:
					//	break;
					//case Keys.OemCopy:
					//	break;
					//case Keys.OemEnlW:
					//	break;
					default:
						break;
				}
			}
			return returnString;
		}

		public static bool ShouldCapitalize(KeyboardState state) => (state.CapsLock ^/*!=*/ (state.IsKeyDown(Keys.LeftShift) || state.IsKeyDown(Keys.RightShift)));
		#endregion
	}
}