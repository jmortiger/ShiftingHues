using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//using ShiftingHues.Input;
//using ShiftingHues.UI;
//using ShiftingHues.Graphics;
//using ShiftingHues.Objects;

using GeonBit.UI;
using GeonBit.UI.Entities;

using tainicom.Aether.Physics2D.Collision;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Diagnostics;

using PipelineClasses;

using JMMGExt;
using JMMGExt.Graphics;
using JMMGExt.Input;
using JMMGExt.Objects;
using JMMGExt.Objects.Components;

// TODO: SceneManager
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

		public Point ViewportCenter { get => GraphicsDevice.Viewport.Bounds.Center; }
		public Vector2 ViewportCenterV2 { get => ViewportCenter.ToVector2(); }

		public Point ViewportDimensions { get => GraphicsDevice.Viewport.Bounds.Size; }
		public Vector2 ViewportDimensionsV2 { get => ViewportDimensions.ToVector2(); }

		private InputDebug iDbg;

        //private UI.UIObject testBttn;

        private string bttnTestStr = "";

        public GameState CurrentGameState { get; private set; } = GameState.Menu;

		private GameObject playerTest;

		//private SimplePlayerPhysicsComponent simplePlayer;

		private World _world;
		private Body _playerBody;
		private Body _groundBody;
		//private Vector2 playerDimensions = new Vector2(256, 256);
		//private FRectangle groundHitbox;
		// Simple camera controls
		//private Vector3 _cameraPosition = new Vector3(0, 1.70f, 0); // camera is 1.7 meters above the ground
		//float cameraViewWidth = 12.5f; // camera is 12.5 meters wide.
		//private BasicEffect spriteBatchEffect;
		private Texture2D tex_start_up;

		private TextInput debugConsole;
		#endregion

		public Game1()
        {
			// Before ANYTHING ELSE, get the logger up and running.



            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            
            InputComp = new InputComponent(this);
            Components.Add(InputComp);

            ServiceLocator.RegisterService(InputComp);
            
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

			//// Init the debugConsole
			//debugConsole = new TextInput(
			//	multiline: false,
			//	size: new Vector2(1f / 3f, 0f),
			//	anchor: Anchor.TopLeft);

			InputComp.OnMouseMove += InputComp_OnMouseMove;

			// Initialize the menu
			ContructMenu();

			// Create world
			_world = new World();

			_world.Gravity = new Vector2(0, 500);

			// Create player fixture
			_playerBody = _world.CreateRectangle(256, 256, 1 / 65536, ViewportCenterV2, 0, BodyType.Dynamic);

			//// Give it some bounce and friction
			//_playerBody.SetRestitution(0.3f);
			//_playerBody.SetFriction(0.75f);

			_playerBody.Tag = new string[] { "player" };
			_playerBody.FixtureList[0].Tag = new string[] { "playerFixture" };
			_playerBody.FixedRotation = true;

			// Create ground fixture
			FRectangle temp = new FRectangle(0, (4 * graphics.PreferredBackBufferHeight) / 5, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight / 5);
			_groundBody = _world.CreateRectangle(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight / 5, 1, temp.Center.ToVector2()/*new Vector2(0, (4 * graphics.PreferredBackBufferHeight) / 5)*/, 0, BodyType.Static);
			_groundBody.IgnoreGravity = true;

			//// Give it some bounce and friction
			//_groundBody.SetRestitution(0.3f);
			//_groundBody.SetFriction(0.5f);

			_groundBody.Tag = new string[] { "ground" };
			_groundBody.FixtureList[0].Tag = new string[] { "groundFixture" };

			//_playerBody.OnCollision += _playerBody_OnCollision;
			//_playerBody.OnSeparation += _playerBody_OnSeparation;

			
			base.Initialize();
        }

		private void InputComp_OnMouseMove(MouseEventArgs e)
		{
			if (!UserInterface.Active.ShowCursor) UserInterface.Active.ShowCursor = true;
		}

		//private void _playerBody_OnSeparation(Fixture sender, Fixture other, tainicom.Aether.Physics2D.Dynamics.Contacts.Contact contact)
		//{
		//	//throw new NotImplementedException();
		//}

		//private bool _playerBody_OnCollision(Fixture sender, Fixture other, tainicom.Aether.Physics2D.Dynamics.Contacts.Contact contact)
		//{
		//	//if (sender == other || contact.FixtureA == contact.FixtureB)
		//	//{
		//	//	Console.WriteLine("wtf");
		//	//}
		//	//throw new NotImplementedException();
		//	return true;
		//}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			testFont1 = this.Content.Load<SpriteFont>("TestFont1");

			tex_start_up = this.Content.Load<Texture2D>("Raw_Files/UI/start_up");

			//Texture2D tex_player_idle = this.Content.Load<Texture2D>("Raw_Files/PC-Idle");
			Texture2D tex_player_idle = this.Content.Load<Texture2D>("Raw_Files/PC-Idle NoBorder2");
			SpriteSheetJSON idleSheetInfo = this.Content.Load<SpriteSheetJSON>("Raw_Files/Spritesheet - PC-Idle NoBorder2");

			playerTest = new GameObject(new FRectangle(ViewportCenterV2, 256), ViewportCenterV2, Vector2.UnitX);
			var spriteSheet = new SpriteSheet(tex_player_idle, idleSheetInfo.GetSourceRectangles()/*2, 4, 256, 256, 8*/);
			var playerIdleAnim = new Animation(spriteSheet.Sprites, new DrawEffects2D(new Vector2(.5f, .5f), new Vector2(256/2)), 12);
			var idleAnimInst = new AnimationInstance(playerIdleAnim, true);
			var graphics2D = new Graphics2DComponent(playerTest, null, new AnimationInstance[1] { idleAnimInst });
			//simplePlayer = new SimplePlayerPhysicsComponent(playerTest);
			var playerPhysics = new IntegratedPlayerPhysicsComponent(playerTest, _playerBody);
			graphics2D.drawAnim = true;
			idleAnimInst.Play();
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

		private void ConstructMenu(string menuID)
		{
			switch (menuID)
			{
				case "ME":
					{
						Panel mainPanel = new Panel(
						   size: new Vector2(GraphicsDevice.Viewport.Bounds.Width - 200, GraphicsDevice.Viewport.Bounds.Height),
						   skin: PanelSkin.None)
						{
							Padding = Vector2.Zero,
							Identifier = "MainPanel"
						};
						UserInterface.Active.AddEntity(mainPanel);

						Panel storyParentPanel = new Panel(
							size: new Vector2(mainPanel.Size.X / 4, GraphicsDevice.Viewport.Bounds.Height),
							anchor: Anchor.TopLeft,
							offset: new Vector2(0, 0),
							skin: PanelSkin.None)
						{
							Identifier = "storyParentPanel",
							Padding = Vector2.Zero
						};

						Panel storyPanel = new Panel(
							size: new Vector2(mainPanel.Size.X / 4, GraphicsDevice.Viewport.Bounds.Height - (GraphicsDevice.Viewport.Bounds.Height * .175f)),
							anchor: Anchor.TopLeft,
							offset: new Vector2(0, (GraphicsDevice.Viewport.Bounds.Height * .175f)),
							skin: PanelSkin.None)
						{
							Padding = Vector2.Zero,
							Visible = false
						};
						storyPanel.OnMouseLeave = (e) => { storyPanel.Visible = false; };
						storyPanel.Identifier = "storyPanel";
						storyParentPanel.AddChild(storyPanel);

						storyParentPanel.OnMouseLeave = (e) => { storyPanel.Visible = false; };
						mainPanel.AddChild(storyParentPanel);

						Button storyButton = new Button(
							size: new Vector2(mainPanel.Size.X / 4, (GraphicsDevice.Viewport.Bounds.Height * .175f)),
							anchor: Anchor.TopLeft,
							offset: Vector2.Zero,
							text: "Story")
						{
							Padding = Vector2.Zero,
							OnMouseEnter = (e) => { storyPanel.Visible = true; }
						};
						storyParentPanel.AddChild(storyButton);

						Button newGameButton = new Button(
							size: new Vector2(0, .15f),
							text: "New Game")
						{
							Padding = Vector2.Zero
						};
						storyPanel.AddChild(newGameButton);

						Panel raceParPanel = new Panel(
							size: new Vector2(mainPanel.Size.X / 4, GraphicsDevice.Viewport.Bounds.Height),
							anchor: Anchor.TopLeft,
							offset: new Vector2(mainPanel.Size.X / 4, 0),
							skin: PanelSkin.None)
						{
							Padding = Vector2.Zero
						};

						Panel racePanel = new Panel(
							size: new Vector2(mainPanel.Size.X / 4, GraphicsDevice.Viewport.Bounds.Height - (GraphicsDevice.Viewport.Bounds.Height * .175f)),
							anchor: Anchor.TopLeft,
							offset: new Vector2(0, (GraphicsDevice.Viewport.Bounds.Height * .175f)),
							skin: PanelSkin.None)
						{
							Padding = Vector2.Zero,
							Visible = false
						};
						racePanel.OnMouseLeave = (e) => { racePanel.Visible = false; };
						raceParPanel.AddChild(racePanel);

						raceParPanel.OnMouseLeave = (e) => { racePanel.Visible = false; };
						mainPanel.AddChild(raceParPanel);

						Button raceButton = new Button(
							size: new Vector2(mainPanel.Size.X / 4, (GraphicsDevice.Viewport.Bounds.Height * .175f)),
							anchor: Anchor.TopLeft,
							offset: Vector2.Zero,
							text: "Race")
						{
							Padding = Vector2.Zero,
							OnMouseEnter = (e) => { racePanel.Visible = true; }
						};
						raceParPanel.AddChild(raceButton);

						Button speedrunButton = new Button(
							size: new Vector2(0, .15f),
							text: "Speedrun")
						{
							Padding = Vector2.Zero
						};
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

						/*raceButton.AttachedData = */
						new UI.UIEntityExtraData(
attachedEntity: raceButton,
ToLeft: storyButton,
Below: speedrunButton);

						/*storyButton.AttachedData = */
						new UI.UIEntityExtraData(
attachedEntity: storyButton,
ToRight: raceButton,
Below: newGameButton);

						/*newGameButton.AttachedData = */
						new UI.UIEntityExtraData(
attachedEntity: newGameButton,
Above: storyButton,
ToRight: raceButton);

						/*speedrunButton.AttachedData = */
						new UI.UIEntityExtraData(
attachedEntity: speedrunButton,
Above: raceButton,
ToLeft: storyButton);

						Button quitButton = new Button(
							text: "Quit Game",
							anchor: Anchor.BottomRight,
							size: new Vector2(.15f, .1f))
						{
							OnClick = (button) => { Exit(); },
							Padding = Vector2.Zero
						};
						UserInterface.Active.AddEntity(quitButton);
						break;
					}

				case "ME 2":
					{
						Panel rootPanel = new Panel(
							size: ViewportDimensionsV2,
							skin: PanelSkin.Default)
						{
							Identifier = "RootPanel",
							Padding = Vector2.Zero
						};
						UserInterface.Active.AddEntity(rootPanel);

						// PANELS
						Vector2 subPanelSize = new Vector2(rootPanel.Size.X / 6, rootPanel.Size.Y);

						Panel storyPanel = new Panel(
							size: subPanelSize,
							anchor: Anchor.TopLeft,
							offset: new Vector2(subPanelSize.X, 0),
							skin: PanelSkin.Default)
						{
							Identifier = "StoryPanel",
							Padding = Vector2.Zero
						};
						rootPanel.AddChild(storyPanel);

						Panel racePanel = new Panel(
							size: subPanelSize,
							anchor: Anchor.TopLeft,
							offset: new Vector2(subPanelSize.X * 2, 0),
							skin: PanelSkin.Default)
						{
							Identifier = "RacePanel",
							Padding = Vector2.Zero
						};
						rootPanel.AddChild(racePanel);

						// TOP LEVEL BUTTONS
						Vector2 buttonSize = new Vector2(subPanelSize.X, 100);

						Button storyButton = new Button(
							size: buttonSize,
							text: "Story",
							anchor: storyPanel.Anchor,
							offset: storyPanel.Offset)
						{
							Identifier = "StoryButton"
						};
						storyButton.OnMouseEnter = (e) =>
						{
							storyPanel.Visible = true;
							storyPanel.OnMouseLeave = (arg) =>
							{
								storyPanel.Visible = false;
								storyPanel.OnMouseLeave = null;
							};
					//ServiceLocator.GetInputService().OnMouseMove += (arg) =>
					//{
					//	if (!storyPanel.IsMouseOver)
					//		storyPanel.Visible = false;
					//};
				};
						storyButton.OnMouseLeave = (e) =>
						{
							if (!storyPanel.CalcDestRect().Contains(/*ServiceLocator.GetInputService().CurrMouseState.Position*/UserInterface.Active.MouseInputProvider.MousePosition))
								storyPanel.OnMouseLeave(storyPanel);
						};
						rootPanel.AddChild(storyButton);

						Button raceButton = new Button(
							 size: buttonSize,
							 text: "Race",
							 anchor: racePanel.Anchor,
							 offset: racePanel.Offset)
						{
							Identifier = "RaceButton"
						};
						raceButton.OnMouseEnter = (e) =>
						{
							racePanel.Visible = true;
							racePanel.OnMouseLeave = (arg) =>
							{
								racePanel.Visible = false;
								racePanel.OnMouseLeave = null;
							};
					//ServiceLocator.GetInputService().OnMouseMove += (arg) =>
					//{
					//	if (!racePanel.IsMouseOver)
					//		racePanel.Visible = false;
					//};
				};
						raceButton.OnMouseLeave = (e) =>
						{
							if (!racePanel.CalcDestRect().Contains(/*ServiceLocator.GetInputService().CurrMouseState.Position*/UserInterface.Active.MouseInputProvider.MousePosition))
								racePanel.OnMouseLeave(racePanel);
						};
						rootPanel.AddChild(raceButton);

						// NESTLED BUTTONS
						Button newGameButton = new Button(
							text: "New Game",
							skin: ButtonSkin.Alternative,
							anchor: Anchor.TopLeft,
							offset: new Vector2(0, buttonSize.Y),
							size: buttonSize)
						{
							Identifier = "NewGameButton"
						};
						storyPanel.AddChild(newGameButton);
						break;
					}

				case "Hotline Miami":
					{
						// X: Options take center 3rd of screen, title takes center 3/5s
						// Y: Options take up lower 2/3s
						var rootPanel = new Panel(
							size: ViewportDimensionsV2,
							skin: PanelSkin.None,
							anchor: Anchor.TopLeft)
						{
							Identifier = "Root Panel",
							Padding = Vector2.Zero
						};
						UserInterface.Active.AddEntity(rootPanel);

						// Title
						var title = new RichParagraph(
							text: @"{{GOLD}}HOTLINE MIAMI",
							anchor: Anchor.TopCenter,
							size: new Vector2(3f / 5f, 1f / 3f)/*,
					offset: new Vector2(1f / 5f, 0)*/)
						{
							Identifier = "Title"/*,
					MaxSize = new Vector2(3f / 5f, 1f / 3f)*/
						};
						rootPanel.AddChild(title);

						// OPTIONS
						// Options Panel
						var optionsPanel = new Panel(
							size: new Vector2(1f / 3f, 2f / 3f),
							skin: PanelSkin.None,
							anchor: Anchor.BottomCenter,
							offset: Vector2.Zero)
						{
							Identifier = "Options Panel"/*,
					Padding = Vector2.Zero*/
						};
						rootPanel.AddChild(optionsPanel);

						// Options
						var newGameHeader = new Header(
							text: "New Game",
							anchor: Anchor.AutoCenter)
						{
							Identifier = "New Game Option"
							/*Padding = Vector2.Zero*//*,
							SpaceAfter = Vector2.Zero,
							SpaceBefore = Vector2.Zero*/
						};
						optionsPanel.AddChild(newGameHeader);

						var chaptersHeader = new Header(
							text: "Chapters",
							anchor: Anchor.AutoCenter)
						{
							Identifier = "Chapters Option"
							/*Padding = Vector2.Zero*//*,
							SpaceAfter = Vector2.Zero,
							SpaceBefore = Vector2.Zero*/
						};
						optionsPanel.AddChild(chaptersHeader);

						var optionsHeader = new Header(
							text: "Options",
							anchor: Anchor.AutoCenter)
						{
							Identifier = "Options Options"
							/*Padding = Vector2.Zero*//*,
							SpaceAfter = Vector2.Zero,
							SpaceBefore = Vector2.Zero*/
						};
						optionsPanel.AddChild(optionsHeader);
						break;
					}
			}
		}

		private void ContructMenu()
		{
			this.ConstructMenu("Hotline Miami");
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
                for (int i = 0; i < InputComponent.NumGameActions; i++)
                {
                    if (InputComp.IsActionActive((GameAction)i))
                        keyDebug += Enum.GetName(typeof(GameAction), i).ToUpper() + "; ";
                }
            }

			// MIDI
			//if (InputComp.GetInputDown(Keys.Home))
			//	MidiPlayground.Test1();

            iDbg.Update(gameTime);

			// Update the UI
			UserInterface.Active.Update(gameTime);

			playerTest.Update(gameTime);

			// We update the world
			_world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

			playerTest.Tranform.SetLocation(_playerBody.Position);

			//playerTest.Tranform.Update(gameTime);

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
			numUpdates++;
			//if (numUpdates % 5 != 0)
			//	return;
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// Until I understand their code better, end the spritebatch before calling the UI's draw.
			UserInterface.Active.Draw(spriteBatch);

			spriteBatch.Begin();
			
            spriteBatch.DrawString(testFont1, /*keyDebug*/$"{1 / gameTime.ElapsedGameTime.TotalSeconds} FPS", new Vector2(), Color.Black);
            //spriteBatch.DrawString(testFont1, /*keyDebug*/$"{/*1 / */gameTime.ElapsedGameTime.Ticks} FPS", new Vector2(0, 25), Color.Black);
            //spriteBatch.DrawString(testFont1, mappingDebug, new Vector2(0, 25), Color.Black);
            spriteBatch.DrawString(testFont1, bttnTestStr, new Vector2(0, 100), Color.Black);
            iDbg.Draw(gameTime, spriteBatch, testFont1, InputDebug.DebugPrints.CurrAction);
			//totalRightTriggerVal += InputComp.CurrPadState.Triggers.Right;
			//avgRightTriggerVal = totalRightTriggerVal / numUpdates;
			//maxRightTriggerVal = (InputComp.CurrPadState.Triggers.Right > maxRightTriggerVal) ? InputComp.CurrPadState.Triggers.Right : maxRightTriggerVal;
			//spriteBatch.DrawString(testFont1, "Max RT: " + maxRightTriggerVal + "; Avg RT: " + avgRightTriggerVal + "; RT: " + InputComp.CurrPadState.Triggers.Right, new Vector2(0, 50), Color.Black);

			spriteBatch.End();
			spriteBatch.Begin();
			spriteBatch.DrawString(testFont1, "Bottom: " + playerTest.Bounds.Bottom/*UserInterface.Active.MouseInputProvider.MousePosition*/, new Vector2(0, 50), Color.Black);
			//spriteBatch.DrawString(testFont1, "Velocity: " + simplePlayer.CurrV.X/*UserInterface.Active.MouseInputProvider.MousePosition*/, new Vector2(0, 75), Color.Black);
			playerTest.Draw(spriteBatch, gameTime);
			spriteBatch.DrawString(testFont1, GetBoundingFRectangle(_playerBody).Rectangle.ToString(), new Vector2(0, 75), Color.Black);
			spriteBatch.DrawString(testFont1, _playerBody.LinearVelocity.ToString(), new Vector2(0, 100), Color.Black);
			spriteBatch.End();
            base.Draw(gameTime);
        }

		public static FRectangle GetBoundingFRectangle(Body body)
		{
			List<FRectangle> rects = new List<FRectangle>();
			foreach (var fixture in body.FixtureList)
			{
				foreach (var proxy in fixture.Proxies)
				{
					rects.Add(new FRectangle(body.Position.X - proxy.AABB.Extents.X, body.Position.Y - proxy.AABB.Extents.Y, proxy.AABB.Width, proxy.AABB.Height));
				}
			}
			FRectangle final = rects[0];
			for (int i = 1; i < rects.Count; i++)
			{
				final = FRectangle.Intersect(final, rects[i]);
			}
			return final;
		}

		public static Rectangle GetBoundingRectangle(Body body) => GetBoundingFRectangle(body).Rectangle;
		//{
		//	List<FRectangle> rects = new List<FRectangle>();
		//	foreach (var fixture in body.FixtureList)
		//	{
		//		foreach (var proxy in fixture.Proxies)
		//		{
		//			rects.Add(new FRectangle(body.Position.X - proxy.AABB.Extents.X, body.Position.Y - proxy.AABB.Extents.Y, proxy.AABB.Width, proxy.AABB.Height));
		//		}
		//	}
		//	FRectangle final = rects[0];
		//	for (int i = 1; i < rects.Count; i++)
		//	{
		//		final = FRectangle.Intersect(final, rects[i]);
		//	}
		//	return final.Rectangle;
		//}
	}
}
