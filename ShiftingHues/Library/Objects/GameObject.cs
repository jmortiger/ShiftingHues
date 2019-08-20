using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShiftingHues.Library.Objects
{
	public interface IGameObjComponent
	{
		GameObject Obj { get; }
		TranformComponent Tranform { get; }
	}

	public class GameObject : IUpdateable, IDrawable2D
	{
		#region Events
		public event EventHandler<EventArgs> EnabledChanged;
		public event EventHandler<EventArgs> UpdateOrderChanged;
		public event EventHandler<EventArgs> DrawOrderChanged;
		public event EventHandler<EventArgs> VisibleChanged;
		#endregion

		#region Fields and Properties

		private TranformComponent tranform;
		public TranformComponent Tranform { get => tranform; private set => tranform = value; }

		public FRectangle Bounds { get => tranform.Bounds; }

		#region Overrides
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

		#region Components

		public IGameObjComponent[] components;

		public IUpdateable[] updateableComponents;

		public IDrawable2D[] drawableComponents;

		public bool HasUpdateableComponents { get => updateableComponents != null && updateableComponents.Length != 0; }

		public bool HasDrawableComponents { get => drawableComponents != null && drawableComponents.Length != 0; }
		#endregion
		#endregion

		#region Constructors

		public GameObject(FRectangle bounds, FPoint position, Vector2 fwd)
		{
			tranform = new TranformComponent(this, bounds, position, fwd);
			AddComponent(tranform);
			AddUpdateableComponent(tranform);
		}

		public GameObject(FRectangle bounds, Vector2 position, Vector2 fwd)
			:this(bounds, new FPoint(position), fwd) { }

		public GameObject(FRectangle bounds, Vector2 fwd)
			: this(bounds, bounds.Location, fwd) { }
		#endregion

		#region Methods
		#region Non-Static
		#region Add Components

		/// <summary>
		/// Adds the given <see cref="IGameObjComponent"/> to the component list.
		/// </summary>
		/// <param name="gameObjComponent">The <see cref="IGameObjComponent"/> to add.</param>
		public void AddComponent(IGameObjComponent gameObjComponent)
		{
			if (components == null)
			{
				components = new IGameObjComponent[1] { gameObjComponent };
				return;
			}
			var temp = components;
			components = new IGameObjComponent[temp.Length + 1];
			for (int i = 0; i < temp.Length; i++)
			{
				components[i] = temp[i];
			}
			components[temp.Length] = gameObjComponent;
		}

		/// <summary>
		/// Adds the given <see cref="IUpdateable"/> component to the list of updateable components.
		/// <see cref="AddComponent(IGameObjComponent)"/> must be called separately.
		/// </summary>
		/// <param name="gameObjComponent">The <see cref="IUpdateable"/> component to add.</param>
		public void AddUpdateableComponent(IUpdateable gameObjComponent)
		{
			if (updateableComponents == null)
			{
				updateableComponents = new IUpdateable[1] { gameObjComponent };
				return;
			}
			var temp = this.updateableComponents;
			updateableComponents = new IUpdateable[temp.Length + 1];
			for (int i = 0; i < temp.Length; i++)
			{
				updateableComponents[i] = temp[i];
			}
			updateableComponents[temp.Length] = gameObjComponent;
			//AddComponent(gameObjComponent);
		}


		/// <summary>
		/// Adds the given <see cref="IDrawable2D"/> component to the list of drawable components.
		/// <see cref="AddComponent(IGameObjComponent)"/> must be called separately.
		/// </summary>
		/// <param name="gameObjComponent">The <see cref="IDrawable2D"/> component to add.</param>
		public void AddDrawableComponent(IDrawable2D gameObjComponent)
		{
			if (drawableComponents == null)
			{
				drawableComponents = new IDrawable2D[1] { gameObjComponent };
				return;
			}
			var temp = this.drawableComponents;
			drawableComponents = new IDrawable2D[temp.Length + 1];
			for (int i = 0; i < temp.Length; i++)
			{
				drawableComponents[i] = temp[i];
			}
			drawableComponents[temp.Length] = gameObjComponent;
			//AddComponent(gameObjComponent);
		}
		#endregion

		public void Update(GameTime time)
		{
			foreach (var comp in updateableComponents)
				if (comp != Tranform)
					comp.Update(time);
			Tranform.Update(time);
		}

		//public void LateUpdate(GameTime time)
		//{

		//}

		public void Draw(SpriteBatch batch, GameTime time)
		{
			foreach (var comp in drawableComponents)
				comp.Draw(batch, time);
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch) => Draw(spriteBatch, gameTime);
		#endregion
		//#region Static
		//public static GameObject CreateGameObject(
		//	FRectangle bounds, Vector2 fwd,
		//	SpriteComponent spriteComponent, AnimationComponent animationComponent
		//	)
		//{
		//	var obj = new GameObject(bounds, fwd);
		//	var twod = new Graphics2DComponent(obj, spriteComponent, animationComponent);
		//	return obj;
		//}
		//#endregion
		#endregion
	}
}