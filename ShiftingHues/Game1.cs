using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using ShiftingHues.Input;
//using ShiftingHues.UI;
using ShiftingHues.Graphics;

using GeonBit.UI;
using GeonBit.UI.Entities;

using tainicom.Aether.Physics2D.Collision;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Diagnostics;

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

		public Point ScreenCenter { get => GraphicsDevice.Viewport.Bounds.Center; }
		public Vector2 ScreenCenterV2 { get => GraphicsDevice.Viewport.Bounds.Center.ToVector2(); }

		private InputDebug iDbg;

        //private UI.UIObject testBttn;

        private string bttnTestStr = "";

        public GameState CurrentGameState { get; private set; } = GameState.Menu;

		private GameObject playerTest;

		private SimplePlayerPhysicsComponent simplePlayer;

		private World _world;
		private Body _playerBody;
		private Body _groundBody;
		private Vector2 playerDimensions = new Vector2(256, 256);
		//private FRectangle groundHitbox;
		// Simple camera controls
		private Vector3 _cameraPosition = new Vector3(0, 1.70f, 0); // camera is 1.7 meters above the ground
		float cameraViewWidth = 12.5f; // camera is 12.5 meters wide.
		private BasicEffect spriteBatchEffect;
		private Texture2D tex_start_up;

		DebugView debugView;
		#endregion

		public Game1()
        {
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

			// Initialize the menu
			InitializeMenu();

			// Create world
			_world = new World();

			_world.Gravity = new Vector2(0, 500);

			// Create player fixture
			_playerBody = _world.CreateRectangle(256, 256, 1 / 65536, ScreenCenterV2, 0, BodyType.Dynamic);

			// Give it some bounce and friction
			_playerBody.SetRestitution(0.3f);
			_playerBody.SetFriction(0.75f);

			_playerBody.Tag = new string[] { "player" };
			_playerBody.FixtureList[0].Tag = new string[] { "playerFixture" };
			_playerBody.FixedRotation = true;

			// Create ground fixture
			FRectangle temp = new FRectangle(0, (4 * graphics.PreferredBackBufferHeight) / 5, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight / 5);
			_groundBody = _world.CreateRectangle(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight / 5, 1, temp.Center.ToVector2()/*new Vector2(0, (4 * graphics.PreferredBackBufferHeight) / 5)*/, 0, BodyType.Static);
			_groundBody.IgnoreGravity = true;

			// Give it some bounce and friction
			_groundBody.SetRestitution(0.3f);
			_groundBody.SetFriction(0.5f);

			_groundBody.Tag = new string[] { "ground" };
			_groundBody.FixtureList[0].Tag = new string[] { "groundFixture" };

			debugView = new DebugView(_world);
			debugView.DefaultShapeColor = Color.White;
			debugView.SleepingShapeColor = Color.LightGray;
			debugView.Enabled = true;
			debugView.AppendFlags(DebugViewFlags.AABB);

			_playerBody.OnCollision += _playerBody_OnCollision;
			_playerBody.OnSeparation += _playerBody_OnSeparation;
			base.Initialize();
        }

		private void _playerBody_OnSeparation(Fixture sender, Fixture other, tainicom.Aether.Physics2D.Dynamics.Contacts.Contact contact)
		{
			//throw new NotImplementedException();
		}

		private bool _playerBody_OnCollision(Fixture sender, Fixture other, tainicom.Aether.Physics2D.Dynamics.Contacts.Contact contact)
		{
			//if (sender == other || contact.FixtureA == contact.FixtureB)
			//{
			//	Console.WriteLine("wtf");
			//}
			//throw new NotImplementedException();
			return true;
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			// Create a new SpriteBatch, which can be used to draw textures.
			spriteBatch = new SpriteBatch(GraphicsDevice);

			// We use a BasicEffect to pass our view/projection in _spriteBatch (physics engine)
			spriteBatchEffect = new BasicEffect(graphics.GraphicsDevice);
			spriteBatchEffect.TextureEnabled = true;

			testFont1 = this.Content.Load<SpriteFont>("TestFont1");

			tex_start_up = this.Content.Load<Texture2D>("Raw_Files/UI/start_up");

			//Texture2D tex_player_idle = this.Content.Load<Texture2D>("Raw_Files/PC-Idle");
			Texture2D tex_player_idle = this.Content.Load<Texture2D>("Raw_Files/PC-Idle NoBorder2");
			SpriteSheetJSON idleSheetInfo = this.Content.Load<SpriteSheetJSON>("Raw_Files/Spritesheet - PC-Idle NoBorder2");

			playerTest = new GameObject(new FRectangle(ScreenCenterV2, 256), ScreenCenterV2, Vector2.UnitX);
			var spriteSheet = new SpriteSheet(tex_player_idle, idleSheetInfo.GetSourceRectangles()/*2, 4, 256, 256, 8*/);
			var playerIdleAnim = new Animation(spriteSheet.Sprites, new DrawEffects2D(new Vector2(.5f, .5f), new Vector2(256/2)), 12);
			var idleAnimInst = new AnimationInstance(playerIdleAnim, true);
			var graphics2D = new Graphics2DComponent(playerTest, null, new AnimationInstance[1] { idleAnimInst });
			//simplePlayer = new SimplePlayerPhysicsComponent(playerTest);
			var playerPhysics = new IntegratedPlayerPhysicsComponent(playerTest, _playerBody);
			graphics2D.drawAnim = true;
			idleAnimInst.Play();

			debugView.LoadContent(graphics.GraphicsDevice, Content);
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
                        keyDebug += Enum.GetName(typeof(GameAction), i).ToUpper() + "; ";
                }
            }

            iDbg.Update(gameTime);

			// Update the UI
			UserInterface.Active.Update(gameTime);

			playerTest.Update(gameTime);

			// We update the world
			_world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);

			playerTest.Tranform.SetLocation(_playerBody.Position);

			debugView.UpdatePerformanceGraph(_world.UpdateTime);

			//playerTest.Tranform.Update(gameTime);

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
			
            spriteBatch.Begin();
			
            spriteBatch.DrawString(testFont1, /*keyDebug*/$"{1 / gameTime.ElapsedGameTime.TotalSeconds} FPS", new Vector2(), Color.Black);
            //spriteBatch.DrawString(testFont1, /*keyDebug*/$"{/*1 / */gameTime.ElapsedGameTime.Ticks} FPS", new Vector2(0, 25), Color.Black);
            //spriteBatch.DrawString(testFont1, mappingDebug, new Vector2(0, 25), Color.Black);
            spriteBatch.DrawString(testFont1, bttnTestStr, new Vector2(0, 100), Color.Black);
            iDbg.Draw(gameTime, spriteBatch, testFont1, InputDebug.DebugPrints.CurrAction);
			//totalRightTriggerVal += InputComp.CurrPadState.Triggers.Right;
			//numUpdates++;
			//avgRightTriggerVal = totalRightTriggerVal / numUpdates;
			//maxRightTriggerVal = (InputComp.CurrPadState.Triggers.Right > maxRightTriggerVal) ? InputComp.CurrPadState.Triggers.Right : maxRightTriggerVal;
			//spriteBatch.DrawString(testFont1, "Max RT: " + maxRightTriggerVal + "; Avg RT: " + avgRightTriggerVal + "; RT: " + InputComp.CurrPadState.Triggers.Right, new Vector2(0, 50), Color.Black);

			spriteBatch.End();
			// Until I understand their code better, end the spritebatch before calling the UI's draw.
			UserInterface.Active.Draw(spriteBatch);
			spriteBatch.Begin();
			spriteBatch.DrawString(testFont1, "Bottom: " + playerTest.Bounds.Bottom/*UserInterface.Active.MouseInputProvider.MousePosition*/, new Vector2(0, 50), Color.Black);
			//spriteBatch.DrawString(testFont1, "Velocity: " + simplePlayer.CurrV.X/*UserInterface.Active.MouseInputProvider.MousePosition*/, new Vector2(0, 75), Color.Black);
			playerTest.Draw(spriteBatch, gameTime);
			//tainicom.Aether.Physics2D.Collision.AABB aabb;
			//_playerBody.FixtureList[0].GetAABB(out aabb, 0);
			//Rectangle fuck = new Rectangle((int)(_playerBody.Position.X - aabb.Extents.X), (int)(_playerBody.Position.Y - aabb.Extents.Y), (int)aabb.Width, (int)aabb.Height);
			//spriteBatch.DrawString(testFont1, fuck.ToString(), new Vector2(0, 75), Color.Black);
			//spriteBatch.Draw(tex_start_up, fuck, Color.White);
			spriteBatch.DrawString(testFont1, GetBoundingFRectangle(_playerBody).Rectangle.ToString(), new Vector2(0, 75), Color.Black);
			//spriteBatch.Draw(tex_start_up, GetBoundingFRectangle(_playerBody).Rectangle, Color.White);
			spriteBatch.Draw(tex_start_up, GetBoundingFRectangle(_groundBody).Rectangle, Color.White);
			//spriteBatch.Draw(tex_start_up, new Rectangle((int)(_groundBody.Position.X - _groundBody.FixtureList[0].Proxies[0].AABB.Extents.X), (int)(_groundBody.Position.Y - _groundBody.FixtureList[0].Proxies[0].AABB.Extents.Y), (int)(_groundBody.FixtureList[0].Proxies[0].AABB.Width), (int)(_groundBody.FixtureList[0].Proxies[0].AABB.Height)), Color.White);
			//spriteBatch.Draw(tex_start_up, new Rectangle(0, (4 * graphics.PreferredBackBufferHeight) / 5, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight / 5), Color.White);
			spriteBatchEffect.View = Matrix.CreateLookAt(_cameraPosition, _cameraPosition + Vector3.Forward, Vector3.Up);
			spriteBatchEffect.Projection = Matrix.CreateOrthographic(cameraViewWidth, cameraViewWidth / GraphicsDevice.Viewport.AspectRatio, 0f, -1f);
			//debugView.RenderDebugData(spriteBatchEffect.Projection, spriteBatchEffect.View);
			debugView.BeginCustomDraw(spriteBatchEffect.Projection, spriteBatchEffect.View);
			foreach (var fix in _playerBody.FixtureList)
			{
				foreach (var prox in fix.Proxies)
				{
					AABB bb = prox.AABB;
					debugView.DrawAABB(ref bb, Color.Black);
				}
			}
			debugView.EndCustomDraw();
			//debugView.DrawAABB(ref aabb, Color.Black);
			//spriteBatchEffect.View = Matrix.CreateLookAt(_cameraPosition, _cameraPosition + Vector3.Forward, Vector3.Up);
			//spriteBatchEffect.Projection = Matrix.CreateOrthographic(cameraViewWidth, cameraViewWidth / GraphicsDevice.Viewport.AspectRatio, 0f, -1f);
			//debugView.RenderDebugData(spriteBatchEffect.Projection, spriteBatchEffect.View);
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
