using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JMMGExt;
using JMMGExt.Graphics;

namespace JMMGExt.Objects.Components
{
	// TODO: Finish particle system
	public class ParticleSystem2D : IGameObjComponent, IDrawable2D, IUpdateable
	{
		#region Events
		public event EventHandler<EventArgs> EnabledChanged;
		public event EventHandler<EventArgs> UpdateOrderChanged;
		public event EventHandler<EventArgs> DrawOrderChanged;
		public event EventHandler<EventArgs> VisibleChanged;
		#endregion

		public GameObject Obj { get; }

		public TranformComponent Tranform { get => Obj.Tranform; }

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

		private ParticleSystem2D(GameObject obj)
		{
			obj.AddComponent(this);
			obj.AddUpdateableComponent(this);
			obj.AddDrawableComponent(this);
		}

		public ParticleSystem2D(
			GameObject obj,
			int maxParticles,
			Func<bool> createCondition,
			Func<bool> destroyCondition)
			: this(obj)
		{

		}

		public void Update(GameTime gameTime)
		{
			throw new NotImplementedException();
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			throw new NotImplementedException();
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
		{
			throw new NotImplementedException();
		}
	}

	public class Particle2D : IUpdateable, IDrawable2D
	{
		#region Events
		public event EventHandler<EventArgs> EnabledChanged;
		public event EventHandler<EventArgs> UpdateOrderChanged;
		public event EventHandler<EventArgs> DrawOrderChanged;
		public event EventHandler<EventArgs> VisibleChanged;
		#endregion

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

		private SpriteInstance Sprite;

		public Vector2 Velocity = Vector2.Zero;

		public Vector2 Force = Vector2.Zero;

		public Vector2 RelPosit = Vector2.Zero;

		public float remainingLifetime = 0f;

		public Color Tint { get => Sprite.DrawEffects.Tint; set => Sprite.DrawEffects.Tint = value; }

		public bool InUse { get; private set; } = false;

		public readonly Action<Particle2D, GameTime> update;

		public readonly Action<Particle2D, GameTime, SpriteBatch> draw;

		public readonly ParticleSystem2D particleSystem;

		#region Predefined Update and Draws
		public readonly static Action<Particle2D, GameTime> constForceUpdate = (e, gameTime) =>
		{
			e.remainingLifetime -= (float)(gameTime.ElapsedGameTime.TotalMilliseconds / 1000d);
			e.Velocity += e.Force * (float)gameTime.ElapsedGameTime.TotalSeconds;
			//e.Force = Vector2.Zero;
			e.RelPosit += e.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
		};

		public readonly static Action<Particle2D, GameTime> nonconstForceUpdate = (e, gameTime) =>
		{
			e.Velocity += e.Force * (float)gameTime.ElapsedGameTime.TotalSeconds;
			e.Force = Vector2.Zero;
			e.RelPosit += e.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
		};

		public readonly static Action<Particle2D, GameTime, SpriteBatch> standardDraw = (e, gameTime, spriteBatch) =>
		{
			Vector2 Posit = e.particleSystem.Tranform.Posit + e.RelPosit;
			e.Sprite.Position = Posit;
		};
		#endregion

		public Particle2D(
			ParticleSystem2D particleSystem,
			SpriteInstance sprite,
			float remainingLifetime,
			Action<Particle2D, GameTime> update,
			Action<Particle2D, GameTime, SpriteBatch> draw)
		{
			this.particleSystem = particleSystem;
			this.Sprite = sprite;
			this.remainingLifetime = remainingLifetime;
			this.update = update;
			this.draw = draw;
		}

		public void Start(
			float remainingLifetime,
			SpriteInstance sprite,
			Vector2 Force,
			Vector2 Velocity,
			Vector2 RelPosit)
		{
			this.Sprite = sprite ?? Sprite;
			this.remainingLifetime = remainingLifetime;
			this.Force = Force;
			this.Velocity = Velocity;
			this.RelPosit = RelPosit;

			this.InUse = true;
		}

		public void Update(GameTime gameTime)
		{
			if (InUse)
				update?.Invoke(this, gameTime);
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			if (InUse)
				draw?.Invoke(this, gameTime, spriteBatch);
		}

		public void Draw(SpriteBatch spriteBatch, GameTime gameTime) => this.Draw(gameTime, spriteBatch);
	}
}
