using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ShiftingHues.Input;

namespace ShiftingHues
{
    class InputDebug
    {
        #region Fields and Properties
        private InputComponent inputManager;
        private string menuString = 
            "Press the # in () to rebind that action.\n" +
            "\tMoveLeft & MenuLeft (1)";
        private string inputList;
        private string pressedInputs;
        private string currActions;
        [Flags]
        public enum DebugPrints
        {
            None         = 0B000000,
            Bindings     = 0B000001,
            PressedBttns = 0B000010,
            CurrAction   = 0B000100
        }
        #endregion

        #region Constructors
        public InputDebug(InputComponent input)
        {
            this.inputManager = input;
        }
        #endregion

        #region Methods
        public void Update(GameTime gameTime)
        {
            //InputButton b = new InputButton(InputDeviceType.Keyboard, (int)Keys.A);
            //inputManager.GetInputUp((b.ButtonType)b.Button);
            inputList = "";
            foreach (var bind in inputManager.Binds)
            {
                inputList += $"{bind._ToString()}\n";
            }
            pressedInputs = "";
            foreach (var pressedBttn in inputManager.CurrButtons)
            {
                pressedInputs += $"{pressedBttn._ToString()}; ";
            }
            currActions = "";
            foreach (var cA in inputManager.CurrActions)
            {
                currActions += $"{cA.ToString()}; ";
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch, SpriteFont font, DebugPrints debugPrints)
        {
            switch (debugPrints)
            {
                case DebugPrints.None:
                    break;
                case DebugPrints.Bindings:
                    spriteBatch.DrawString(font, inputList, new Vector2(0, 25), Color.Black);
                    break;
                case DebugPrints.PressedBttns:
                    spriteBatch.DrawString(font, pressedInputs, new Vector2(0, 25), Color.Black);
                    break;
                case DebugPrints.CurrAction:
                    spriteBatch.DrawString(font, currActions, new Vector2(0, 25), Color.Black);
                    break;
                default:
                    break;
            }
        }
        #endregion
    }
}