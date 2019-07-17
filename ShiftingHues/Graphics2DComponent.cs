using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShiftingHues.Graphics;

namespace ShiftingHues
{
	public class Graphics2DComponent : IGameObjComponent, IDrawable2D, IUpdateable
	{
		#region Events
		public event EventHandler<EventArgs> EnabledChanged;
		public event EventHandler<EventArgs> UpdateOrderChanged;
		public event EventHandler<EventArgs> DrawOrderChanged;
		public event EventHandler<EventArgs> VisibleChanged;
		#endregion

		#region Fields and Properties
		#region Overloads

		public GameObject Obj { get; }

		public TranformComponent Tranform { get => Obj.Tranform; }

		#region IUpdateable & IDrawable2D Overrides
		private bool enabled;
		public bool Enabled
		{
			get => enabled;
			set
			{
				if (enabled != value)
				{
					enabled = value;
					EnabledChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		private int updateOrder;
		public int UpdateOrder
		{
			get => updateOrder;
			set
			{
				if (updateOrder != value)
				{
					updateOrder = value;
					UpdateOrderChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		private bool visible;
		public bool Visible
		{
			get => visible;
			set
			{
				if (visible != value)
				{
					visible = value;
					VisibleChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}

		private int drawOrder;
		public int DrawOrder
		{
			get => drawOrder;
			set
			{
				if (drawOrder != value)
				{
					drawOrder = value;
					DrawOrderChanged?.Invoke(this, EventArgs.Empty);
				}
			}
		}
		#endregion
		#endregion

		public bool drawSprite = false;
		public readonly SpriteComponent spriteComponent;
		public bool drawAnim = false;
		public readonly AnimationComponent animationComponent;
		#endregion

		#region Constructors
		private Graphics2DComponent(GameObject obj)
		{
			this.Obj = obj;
			obj.AddComponent(this);
			obj.AddDrawableComponent(this);
			obj.AddUpdateableComponent(this);
		}

		public Graphics2DComponent(GameObject obj, SpriteComponent spriteComponent, AnimationComponent animationComponent)
			: this(obj)
		{
			this.spriteComponent = spriteComponent;
			this.animationComponent = animationComponent;
		}

		public Graphics2DComponent(GameObject obj, SpriteInstance[] sprites, AnimationInstance[] animations)
			: this(obj)
		{
			this.spriteComponent = sprites != null ? new SpriteComponent(sprites) : null;
			this.animationComponent = animations != null ? new AnimationComponent(animations) : null;
		}

		public Graphics2DComponent(GameObject obj, SpriteInstance sprite, AnimationInstance animation)
			: this(obj)
		{
			this.spriteComponent = sprite != null ? new SpriteComponent(sprite) : null;
			this.animationComponent = animation != null ? new AnimationComponent(animation) : null;
		}
		#endregion

		#region Methods

		public void Update(GameTime time)
		{
			if (drawAnim)
			{
				// TODO: Debug lines for animation speed modding. Remove later.
				if (ServiceLocator.GetInputService().GetInputDown(Microsoft.Xna.Framework.Input.Keys.Add))
					animationComponent.CurrAnimation.FPS++;
				if (ServiceLocator.GetInputService().GetInputDown(Microsoft.Xna.Framework.Input.Keys.Subtract))
					animationComponent.CurrAnimation.FPS--;
				// End Debugs

				animationComponent.CurrAnimation.Update(time);
				animationComponent.CurrAnimation.Posit = (Point)Tranform.Posit;
			}
			if (drawSprite)
			{
				spriteComponent.CurrSprite.PositionAsPoint = (Point)Tranform.Posit;
			}
			//throw new NotImplementedException();
		}

		public void Draw(SpriteBatch batch, GameTime time)
		{
			if (drawSprite)
				spriteComponent.CurrSprite.DrawSprite(batch, null, Tranform.Posit.ToPoint());
			if (drawAnim)
				animationComponent.CurrAnimation.Draw(batch);
			//throw new NotImplementedException();
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch) => Draw(spriteBatch, gameTime);
		#endregion
	}

	public class SpriteComponent
	{
		public SpriteInstance[] Sprites { get; private set; }
		public int currSpriteIndex;
		public SpriteInstance CurrSprite { get => Sprites[currSpriteIndex]; }

		#region Constructors
		public SpriteComponent(SpriteInstance[] sprites, int currSpriteIndex = 0)
		{
			Sprites = sprites;
			this.currSpriteIndex = currSpriteIndex;
		}
		public SpriteComponent(SpriteInstance sprite, int currSpriteIndex = 0)
		{
			Sprites = new SpriteInstance[1] { sprite };
			this.currSpriteIndex = currSpriteIndex;
		}
		#endregion
	}

	public class AnimationComponent
	{
		public AnimationInstance[] Animations { get; private set; }
		public int currAnimationIndex;
		public AnimationInstance CurrAnimation { get => Animations[currAnimationIndex]; }

		#region Constructors
		public AnimationComponent(AnimationInstance[] animations, int currAnimationIndex = 0)
		{
			Animations = animations;
			this.currAnimationIndex = currAnimationIndex;
		}
		public AnimationComponent(AnimationInstance animation, int currAnimationIndex = 0)
		{
			Animations = new AnimationInstance[1] { animation };
			this.currAnimationIndex = currAnimationIndex;
		}
		#endregion
	}
}