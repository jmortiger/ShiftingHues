using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

namespace ShiftingHues
{
	// Current logic:
	// 1. The player can't rotate
	// 2. Friction is applied opposite the Forward vector
	public class PlayerPhysicsComponent : PhysicsComponent
	{
		#region Fields and Properties

		public float playerForceX = 10f;
		public float playerForceY = 20f;

		public float Mass { get; private set; } = 1f;
		public Vector2 GravityAccel { get; private set; } = new Vector2(0, -10f);
		public Vector2 GravityForce { get => Mass * GravityAccel; }

		private Vector2 currF;
		public Vector2 CurrF { get => currF; private set => currF = value; }
		private Vector2 prevF;
		public Vector2 PrevF { get => prevF; private set => prevF = value; }
		private Vector2 currA;
		public Vector2 CurrA { get => currA; private set => currA = value; }
		private Vector2 currV;
		public Vector2 CurrV { get => currV; private set => currV = value; }
		private Vector2 prevV;
		public Vector2 PrevV { get => prevV; private set => prevV = value; }
		private Vector2 currDeltaPosit;

		//public float MaxVelocity { get; private set; }
		public float Coeff { get; private set; } = .5f;

		public bool IsGrounded { get; private set; } = true;
		#endregion

		#region Constructors

		#endregion

		#region Methods

		public override void Update(GameTime time)
		{
			CheckGrounded();
			var input = ServiceLocator.GetInputService();
			if (input.IsActionActive(Input.GameAction.MoveLeft))
			{
				currF.X -= playerForceX;
			}
			if (input.IsActionActive(Input.GameAction.MoveRight))
			{
				currF.X += playerForceX;
			}
			if (IsGrounded && input.IsActionActive(Input.GameAction.Jump))
			{
				currF.Y -= playerForceY;
			}
			CalcForces(time);
			CalcMove(time);
			Tranform.Move(currDeltaPosit);
			//throw new NotImplementedException();
		}

		//public Vector2 GetFric(Vector2 desiredVel)
		//{
		//	if ()
		//}

		private void CheckGrounded()
		{
			// TODO: Finish
			if (Obj.tranform.Posit.Y >= 50)
				IsGrounded = true;
			else
				IsGrounded = false;
		}

		public override void CalcForces(GameTime time)
		{
			// Gravity
			currF += GravityForce;

			// Friction
			Vector2 frictionalForce = Vector2.Zero;
			Vector2 normalForce = Vector2.Zero;
			if (IsGrounded)
			{
				normalForce.Y = CurrF.Y * -1;
				currF += normalForce;
				frictionalForce += (Obj.tranform.Forward * -1) * (normalForce.Length() * Coeff);
			}
			Vector2 projF = (currV / (float)time.ElapsedGameTime.TotalSeconds) * Mass;
			projF.Y = 0;
			frictionalForce = Vector2.Clamp(frictionalForce, Vector2.Zero, projF);
			currF += frictionalForce;
			//throw new NotImplementedException();
		}

		protected void CalcMove(GameTime time)
		{
			currA = currF / Mass;
			prevV = currV;
			currV = currA * (float)time.ElapsedGameTime.TotalSeconds;
			currDeltaPosit = currV * (float)time.ElapsedGameTime.TotalSeconds;
		}

		//public float GetForceOfFriction(float fX, float fY)
		//{
		//	if (fY < 0)
		//	{
		//		float fric = fY * Coeff;
		//		if (fX < 0)
		//		{
		//			fric *= -1;
		//		}
		//		return fric;
		//	}
		//	else
		//		return 0;
		//}
		#endregion
	}
}