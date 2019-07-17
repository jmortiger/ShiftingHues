using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using tainicom.Aether.Physics2D;
using tainicom.Aether.Physics2D.Dynamics;

namespace ShiftingHues
{
	public abstract class IntegratedPhysicsComponent : IGameObjComponent, IUpdateable
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

		public IntegratedPhysicsComponent(GameObject obj)
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

		//public abstract void CalcForces(GameTime time);
		#endregion
	}

	public class IntegratedPlayerPhysicsComponent : IntegratedPhysicsComponent
	{
		#region Fields and Properties
		public bool IsGrounded { get; private set; }

		public Body PlayerBody { get; private set; }
		#endregion

		#region Constructors

		public IntegratedPlayerPhysicsComponent(GameObject obj, Body playerBody)
			: base(obj)
		{
			this.PlayerBody = playerBody;
			playerBody.OnCollision += PlayerBody_OnCollision;
			playerBody.OnSeparation += PlayerBody_OnSeparation;
		}

		private void PlayerBody_OnSeparation(Fixture sender, Fixture other, tainicom.Aether.Physics2D.Dynamics.Contacts.Contact contact)
		{
			if (other.Body.BodyType == BodyType.Static)
			{
				IsGrounded = false;
			}
		}

		private bool PlayerBody_OnCollision(Fixture sender, Fixture other, tainicom.Aether.Physics2D.Dynamics.Contacts.Contact contact)
		{
			if (other.Body.BodyType == BodyType.Static)
			{
				IsGrounded = true;
			}
			return true;
			//throw new NotImplementedException();
		}
		#endregion

		#region Methods

		public override void Update(GameTime time)
		{
			var input = ServiceLocator.GetInputService();

			if (input.IsActionActive(Input.GameAction.MoveLeft))
				PlayerBody.ApplyLinearImpulse(new Vector2(-100, 0));
			if (input.IsActionActive(Input.GameAction.MoveRight))
				PlayerBody.ApplyLinearImpulse(new Vector2(100, 0));
			if (input.IsActionActive(Input.GameAction.Jump) && IsGrounded)
			{
				IsGrounded = false;
				PlayerBody.ApplyLinearImpulse(new Vector2(0, -200));
			}
			else
			{
				
			}
			//throw new NotImplementedException();
		}
		#endregion
	}
}