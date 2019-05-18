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
            InputButton b = new InputButton(InputDeviceType.Keyboard, (int)Keys.A);
            inputManager.GetInputUp((b.ButtonType)b.Button);
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {

        }
        #endregion
    }
}