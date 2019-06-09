using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using ShiftingHues.Input;

namespace ShiftingHues.UI
{
    public class UIManager : GameComponent
    {
        #region Fields and Properties

        public UIObject RootUIObject { get; private set; }

        public ContainerComponent RootContainer { get; private set; }

        public Texture2D[] textures;

        public SpriteFont font;
        #endregion

        #region Constructors

        public UIManager(Game game, Texture2D[] textures, SpriteFont font) : base(game)
        {
            this.textures = textures;
            this.font = font;
        }
        #endregion

        #region Methods

        public void ConstructMenu()
        {
            RootUIObject = new UIObject(Game.GraphicsDevice.Viewport.Bounds);
            UI.ContainerComponent rootContainer = new ContainerComponent(RootUIObject);
            RootUIObject.AddDrawableComponent(rootContainer);

            int subMenuWidth = (RootUIObject.Bounds.Width - 200) / 4;
            int subMenuHeight = RootUIObject.Bounds.Height;

            int edgeToSubMenuTitleWidth = 50 + (subMenuWidth / 2);

            int menuTextHeight = RootUIObject.Bounds.Height - 300;

            UI.UIObject storySubMenu = new UIObject(new Rectangle(50, 0, subMenuWidth, subMenuHeight));
            UI.TextComponent storyText = new TextComponent("Story", font, new Vector2(edgeToSubMenuTitleWidth, menuTextHeight), Color.Black, true);
            UI.ContainerComponent storyContainer = new ContainerComponent(storySubMenu);
            storyContainer.IsVisible = false;
            Action<UI.UIObject> selectAction1 = (obj) =>
            {

            };

        }

        public override void Update(GameTime gameTime)
        {
            //ServiceLocator.GetInputService().OnMouseMove += OnMouseMove;
            // Start check chain for UIObjs


            base.Update(gameTime);
        }
        #endregion
    }
}