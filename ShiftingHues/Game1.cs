using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ShiftingHues.Input;
using ShiftingHues.UI;
// TODO: SceneManager
// TODO: UI Buttons
// TODO: Menus
namespace ShiftingHues
{
    public enum GameState
    {
        Menu,
        Playing,
        Paused,
        Loading
    }
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        #region Fields & Props
        const float WORLD_WIDTH = 16f;
        const float WORLD_HEIGHT = 9f;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont testFont1;

        public InputComponent InputComp { get; private set; }

        private InputDebug iDbg;

        private UI.UIObject testBttn;

        private string bttnTestStr = "";

        public GameState CurrentGameState { get; private set; } = GameState.Menu;
        #endregion

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            InputComp = new InputComponent(this);
            Components.Add(InputComp);

            ServiceLocator.RegisterService(InputComp);

            // TODO: Might change mouse visibility
            this.IsMouseVisible = true;
            iDbg = new InputDebug(InputComp);
        }

        #region Non-Looped
        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            testFont1 = this.Content.Load<SpriteFont>("TestFont1");
            
            Texture2D tex_start_up = this.Content.Load<Texture2D>("Raw_Files/UI/start_up");

            // Creating Buttons
            // I'm making a button, which can be activated and drawn
            Rectangle outputRect = tex_start_up.Bounds;
            outputRect.Location = new Point(200, 200);
            UI.UISpriteComponent spriteComponent = new UISpriteComponent(tex_start_up, outputRect);

            testBttn = new UI.UIObject(outputRect, spriteComponent/*, clickComponent*/);
            UI.UIObjActiveComponent clickComponent = new UIObjActiveComponent(
                (obj) =>
                {
                    for (int i = 0; i < obj?.drawableComponents?.Length; i++)
                        obj.drawableComponents[i].ColorTint = new Color(Color.White, .5f);
                },
                (obj) => bttnTestStr += "Button Pressed\n",
                (obj) =>
                {
                    for (int i = 0; i < obj?.drawableComponents?.Length; i++)
                        obj.drawableComponents[i].ColorTint = Color.White;
                },
                testBttn
                );
            testBttn.updateableComponents = new IObjUpdateComponent[] { clickComponent };
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        #endregion

        string keyDebug = "";
        //string mappingDebug = "";
        //bool mappingsBeenConverted = false;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Update Components First
            base.Update(gameTime);

            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();
            if (InputComp.IsActionActive(GameAction.ExitGame))
                Exit();

            // TODO: Add your update logic here
            keyDebug = "";
            foreach (var key in InputComp.CurrKeyboardState.GetPressedKeys())
            {
                keyDebug += key.ToString() + "; ";
            }
            var gPS = InputComp.CurrPadState;
            if (gPS.IsButtonDown(Buttons.A))
            {
                keyDebug += "A Button; ";
            }
            if (InputComp.AreActionsActive())
            {
                for (int i = 0; i < InputComponent.NUM_GAME_ACTIONS; i++)
                {
                    if (InputComp.IsActionActive((GameAction)i))
                        keyDebug += System.Enum.GetName(typeof(GameAction), i).ToUpper() + "; ";
                }
            }
           /* mappingDebug = "";
            for (int i = 0; i < InputComp.setActions.Count; i++)
            {
                mappingDebug += System.Enum.GetName(typeof(GameActions), InputComp.setActions[i]) + ": ";
                for (int j = 0; j < InputComp.mappings[InputComp.setActions[i]].Length; j++)
                {
                    mappingDebug += InputComp.mappings[InputComp.setActions[i]][j] + " ";
                }
                mappingDebug += "\n";
            }
            if (InputComp.setActions.Count < InputComponent.NUM_GAME_ACTIONS)
            {
                mappingDebug += System.Enum.GetName(typeof(GameActions), InputComp.setActions.Count) + ": ";
                if (InputComp.KBStateChanged() && InputComp.CurrKeyboardState.GetPressedKeys().Length > 0)
                {
                    InputComp.mappings.Add((GameActions)InputComp.setActions.Count, InputComp.CurrKeyboardState.GetPressedKeys());
                    InputComp.setActions.Add((GameActions)InputComp.setActions.Count);
                }
            }
            else if (!mappingsBeenConverted)
            {
                InputComp.ConvertMappingsToBindInfo();
                mappingsBeenConverted = true;
            }*/

            iDbg.Update(gameTime);

            testBttn.Update(gameTime);

            //base.Update(gameTime);
        }

        float maxRightTriggerVal = 0;
        float avgRightTriggerVal = 0;
        float totalRightTriggerVal = 0;
        float numUpdates = 0;

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            testBttn.Draw(spriteBatch/*, gameTime*/);

            spriteBatch.DrawString(testFont1, /*keyDebug*/$"{1 / gameTime.ElapsedGameTime.TotalSeconds} FPS", new Vector2(), Color.Black);
            //spriteBatch.DrawString(testFont1, /*keyDebug*/$"{/*1 / */gameTime.ElapsedGameTime.Ticks} FPS", new Vector2(0, 25), Color.Black);
            //spriteBatch.DrawString(testFont1, mappingDebug, new Vector2(0, 25), Color.Black);
            spriteBatch.DrawString(testFont1, bttnTestStr, new Vector2(0, 100), Color.Black);
            iDbg.Draw(gameTime, spriteBatch, testFont1, InputDebug.DebugPrints.CurrAction);
            totalRightTriggerVal += InputComp.CurrPadState.Triggers.Right;
            numUpdates++;
            avgRightTriggerVal = totalRightTriggerVal / numUpdates;
            maxRightTriggerVal = (InputComp.CurrPadState.Triggers.Right > maxRightTriggerVal) ? InputComp.CurrPadState.Triggers.Right : maxRightTriggerVal;
            spriteBatch.DrawString(testFont1, "Max RT: " + maxRightTriggerVal + "; Avg RT: " + avgRightTriggerVal + "; RT: " + InputComp.CurrPadState.Triggers.Right, new Vector2(0, 50), Color.Black);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
