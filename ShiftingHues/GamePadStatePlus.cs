
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShiftingHues.Input.GamePadExt
{
	public struct GamePadStatePlus
    {
        public readonly GamePadState State;

        public float LStickX { get; private set; }
        public float LStickY { get; private set; }
        public float RStickX { get; private set; }
        public float RStickY { get; private set; }
        public float LTrigger { get; private set; }
        public float RTrigger { get; private set; }

        public GamePadButtons Buttons { get => State.Buttons; }
        public GamePadDPad DPad { get => State.DPad; }
        public bool IsConnected { get => State.IsConnected; }
        public int PacketNumber { get => State.PacketNumber; }
        public GamePadThumbSticks ThumbSticks { get => State.ThumbSticks; }
        public GamePadTriggers Triggers { get => State.Triggers; }

        public GamePadStatePlus(GamePadState state)
        {
            this.State = state;
            this.LStickX = state.ThumbSticks.Left.X;
            this.LStickY = state.ThumbSticks.Left.Y;
            this.RStickX = state.ThumbSticks.Right.X;
            this.RStickY = state.ThumbSticks.Right.Y;
            this.LTrigger = state.Triggers.Left;
            this.RTrigger = state.Triggers.Right;
        }

        public GamePadStatePlus(GamePadState state, LinearDeadzoneInfo triggerDZs, StickDeadzoneInfo stickDZs)
        {
            this.State = state;
            Vector2 lS = stickDZs.GetStickVal(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
            Vector2 rS = stickDZs.GetStickVal(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
            this.LStickX = lS.X;
            this.LStickY = lS.Y;
            this.RStickX = rS.X;
            this.RStickY = rS.Y;
            this.LTrigger = triggerDZs.GetValAbs(state.Triggers.Left);
            this.RTrigger = triggerDZs.GetValAbs(state.Triggers.Right);
        }

        public GamePadStatePlus(GamePadState state, LinearDeadzoneInfo lTriggerDZ, LinearDeadzoneInfo rTriggerDZ, StickDeadzoneInfo stickDZs)
        {
            this.State = state;
            Vector2 lS = stickDZs.GetStickVal(state.ThumbSticks.Left.X, state.ThumbSticks.Left.Y);
            Vector2 rS = stickDZs.GetStickVal(state.ThumbSticks.Right.X, state.ThumbSticks.Right.Y);
            this.LStickX = lS.X;
            this.LStickY = lS.Y;
            this.RStickX = rS.X;
            this.RStickY = rS.Y;
            this.LTrigger = lTriggerDZ.GetValAbs(state.Triggers.Left);
            this.RTrigger = rTriggerDZ.GetValAbs(state.Triggers.Right);
        }
        /// <summary>
        /// Determines whether specified input device buttons are pressed in this GamePadState.
        /// </summary>
        /// <param name="button">Buttons to query. Specify a single button, or combine multiple buttons using a bitwise OR operation.</param>
        /// <returns><c>true</c>, if button was pressed, <c>false</c> otherwise.</returns>
        public bool IsButtonDown(Buttons button) => State.IsButtonDown(button);

        public bool IsButtonUp(Buttons button) => State.IsButtonUp(button);
    }
}