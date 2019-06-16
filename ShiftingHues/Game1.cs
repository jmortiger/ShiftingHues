using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ShiftingHues.Input;
using ShiftingHues.UI;

using GeonBit.UI;
using GeonBit.UI.Entities;

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
        //public UIManager UI { get; private set; }

        private InputDebug iDbg;

        //private UI.UIObject testBttn;

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
            
            // TODO: Might change mouse visibility; will conflict w/ GeonBit UI
            //this.IsMouseVisible = true;

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
			// Init the GeonBit.UI UI Manager.
			UserInterface.Initialize(Content, BuiltinThemes.editor);

			// Initialize the menu
			InitializeMenu();

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
		}// Comment this } and uncomment the next to include code comment again
            // Creating Buttons
            // I'm making a button, which can be activated and drawn
            //Rectangle outputRect = tex_start_up.Bounds;
            //outputRect.Location = new Point(200, 200);
            //UI.SpriteComponent spriteComponent = new SpriteComponent(tex_start_up, outputRect);

            //testBttn = new UI.UIObject(outputRect, spriteComponent/*, clickComponent*/);
            //UI.ActiveComponent clickComponent = new ActiveComponent(
            //    (obj) =>
            //    {
            //        for (int i = 0; i < obj?.DrawableComponents?.Length; i++)
            //            obj.DrawableComponents[i].Tint = new Color(Color.White, .5f);
            //    },
            //    (obj) => bttnTestStr += "Button Pressed\n",
            //    (obj) =>
            //    {
            //        for (int i = 0; i < obj?.DrawableComponents?.Length; i++)
            //            obj.DrawableComponents[i].Tint = Color.White;
            //    },
            //    testBttn
            //    );
            //testBttn.updateableComponents = new IObjUpdateComponent[] { clickComponent };

            //UI = new UIManager(this, new Texture2D[] { tex_start_up }, testFont1);
        //}

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }


		private void InitializeMenu()
		{
			Panel mainPanel = new Panel(
				size: new Vector2(GraphicsDevice.Viewport.Bounds.Width - 200, GraphicsDevice.Viewport.Bounds.Height), 
				skin: PanelSkin.None);
			mainPanel.Padding = Vector2.Zero;
			UserInterface.Active.AddEntity(mainPanel);

			Panel storyParPanel = new Panel(
				size: new Vector2(mainPanel.Size.X / 4, GraphicsDevice.Viewport.Bounds.Height),
				anchor: Anchor.TopLeft,
				offset: new Vector2(0, 0),
				skin: PanelSkin.None);
			storyParPanel.Padding = Vector2.Zero;

			Panel storyPanel = new Panel(
				size: new Vector2(mainPanel.Size.X / 4, GraphicsDevice.Viewport.Bounds.Height - (GraphicsDevice.Viewport.Bounds.Height * .175f)),
				anchor: Anchor.TopLeft,
				offset: new Vector2(0, (GraphicsDevice.Viewport.Bounds.Height * .175f)),
				skin: PanelSkin.None);
			storyPanel.Padding = Vector2.Zero;
			storyPanel.Visible = false;
			storyPanel.OnMouseLeave = (e) => { storyPanel.Visible = false; };
			storyParPanel.AddChild(storyPanel);

			storyParPanel.OnMouseLeave = (e) => { storyPanel.Visible = false; };
			mainPanel.AddChild(storyParPanel);

			Button storyButton = new Button(
				size: new Vector2(mainPanel.Size.X / 4, (GraphicsDevice.Viewport.Bounds.Height * .175f)),
				anchor: Anchor.TopLeft,
				offset: Vector2.Zero,
				text: "Story");
			storyButton.Padding = Vector2.Zero;
			storyButton.OnMouseEnter = (e) => { storyPanel.Visible = true; };
			storyParPanel.AddChild(storyButton);

			Button newGameButton = new Button(
				size: new Vector2(0, .15f),
				text: "New Game");
			newGameButton.Padding = Vector2.Zero;
			storyPanel.AddChild(newGameButton);

			Panel raceParPanel = new Panel(
				size: new Vector2(mainPanel.Size.X / 4, GraphicsDevice.Viewport.Bounds.Height),
				anchor: Anchor.TopLeft,
				offset: new Vector2(mainPanel.Size.X / 4, 0),
				skin: PanelSkin.None);
			raceParPanel.Padding = Vector2.Zero;

			Panel racePanel = new Panel(
				size: new Vector2(mainPanel.Size.X / 4, GraphicsDevice.Viewport.Bounds.Height - (GraphicsDevice.Viewport.Bounds.Height * .175f)),
				anchor: Anchor.TopLeft,
				offset: new Vector2(0, (GraphicsDevice.Viewport.Bounds.Height * .175f)),
				skin: PanelSkin.None);
			racePanel.Padding = Vector2.Zero;
			racePanel.Visible = false;
			racePanel.OnMouseLeave = (e) => { racePanel.Visible = false; };
			raceParPanel.AddChild(racePanel);

			raceParPanel.OnMouseLeave = (e) => { racePanel.Visible = false; };
			mainPanel.AddChild(raceParPanel);

			Button raceButton = new Button(
				size: new Vector2(mainPanel.Size.X / 4, (GraphicsDevice.Viewport.Bounds.Height * .175f)),
				anchor: Anchor.TopLeft,
				offset: Vector2.Zero,
				text: "Race");
			raceButton.Padding = Vector2.Zero;
			raceButton.OnMouseEnter = (e) => { racePanel.Visible = true; };
			raceParPanel.AddChild(raceButton);

			Button speedrunButton = new Button(
				size: new Vector2(0, .15f),
				text: "Speedrun");
			speedrunButton.Padding = Vector2.Zero;
			racePanel.AddChild(speedrunButton);

			//Panel racePanel = new Panel(
			//	size: new Vector2(mainPanel.Size.X / 4, GraphicsDevice.Viewport.Bounds.Height - (GraphicsDevice.Viewport.Bounds.Height * .175f)),
			//	anchor: Anchor.TopLeft,
			//	offset: new Vector2(mainPanel.Size.X / 4, (GraphicsDevice.Viewport.Bounds.Height * .175f)),
			//	skin: PanelSkin.Simple);
			//racePanel.Padding = Vector2.Zero;
			//racePanel.Visible = false;
			//racePanel.OnMouseLeave = (e) => { racePanel.Visible = false; };
			//mainPanel.AddChild(racePanel);

			//Button raceButton = new Button(
			//	size: new Vector2(mainPanel.Size.X / 4, (GraphicsDevice.Viewport.Bounds.Height * .175f)),
			//	anchor: Anchor.TopLeft,
			//	offset: new Vector2(mainPanel.Size.X / 4, 0),
			//	text: "Race");
			//raceButton.Padding = Vector2.Zero;
			////Action<Entity> action = (e) => { racePanel.; };
			//raceButton.OnMouseEnter = (e) => { racePanel.Visible = true; };
			//mainPanel.AddChild(raceButton);

			//Button speedrunButton = new Button(
			//	size: new Vector2(0, .15f),
			//	text: "Speedrun");
			//speedrunButton.Padding = Vector2.Zero;
			//racePanel.AddChild(speedrunButton);

			Button quitButton = new Button(
				text: "Quit Game",
				anchor: Anchor.BottomRight,
				size: new Vector2(.15f, .1f));
			quitButton.OnClick = (button) => { Exit(); };
			quitButton.Padding = Vector2.Zero;
			UserInterface.Active.AddEntity(quitButton);
		}
        #endregion

        string keyDebug = "";

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Update Components First
            base.Update(gameTime);
			
			// Check if exit button is pushed.
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

            iDbg.Update(gameTime);

			// Update the UI
			UserInterface.Active.Update(gameTime);

            //testBttn.Update(gameTime);

            //base.Update(gameTime);
        }

        float maxRightTriggerVal = 0;
        float avgRightTriggerVal = 0;
        float totalRightTriggerVal = 0;
        float numUpdates = 0;

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <remarks>
		/// Note from comments on UserInterface.Draw: if UseRenderTarget is true, this function should be called FIRST in your draw function.
		/// If UseRenderTarget is false, this function should be called LAST in your draw function.
		/// </remarks>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            spriteBatch.Begin();

            //testBttn.Draw(spriteBatch/*, gameTime*/);

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
			// Until I understand their code better, end the spritebatch before calling the UI's draw.
			UserInterface.Active.Draw(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
