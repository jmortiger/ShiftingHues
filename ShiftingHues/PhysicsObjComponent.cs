using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using JMMGExt;

namespace ShiftingHues.Library.Objects
{
	public abstract class PhysicsObjComponent : IGameObjComponent, IUpdateable
	{
		public event EventHandler<EventArgs> EnabledChanged;
		public event EventHandler<EventArgs> UpdateOrderChanged;

		#region Fields and Properties

		public GameObject Obj { get; }

		public TranformComponent Tranform { get => Obj.Tranform; }

		#region IUpdateable Overrides
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
		#endregion
		#endregion

		#region Constructors

		public PhysicsObjComponent(GameObject obj)
		{
			Obj = obj;
			obj.AddComponent(this);
			obj.AddUpdateableComponent(this);
		}
		#endregion

		#region Methods

		public abstract void Update(GameTime time);
		//{
		//	throw new NotImplementedException();
		//}

		public abstract void CalcForces(GameTime time);
		#endregion
	}

	public abstract class StaticPhysicsComponent : IGameObjComponent
	{
		#region Fields and Properties

		public GameObject Obj { get; }

		public TranformComponent Tranform { get => Obj.Tranform; }

		public FRectangle Hitbox { get => Obj.Bounds; }
		#endregion

		#region Constructors

		public StaticPhysicsComponent(GameObject obj)
		{
			Obj = obj;
			obj.AddComponent(this);
		}
		#endregion
	}
}