using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

using JMMGExt;
//using JMMGExt.Input;
using ShiftingHues.Library.Input;

namespace ShiftingHues.Library.Objects
{
	public class SimplePlayerPhysicsComponent : PhysicsObjComponent
	{
		#region Fields and Properties

		public float VMax { get; set; } = 30f;

		private Vector2 currV;
		public Vector2 CurrV { get => currV; set => currV = value; }

		//public Vector2 PrevV { get; set; }

		public float PlayerVx { get; set; } = 20f;

		public float PlayerVy { get; set; } = 30f;

		public float PlayerFy { get; set; } = 30f;

		public float GravityAccel { get; set; } = 20f;

		public bool IsGrounded { get; set; } = false;

		public float GroundHeight = 400f;
		#endregion

		#region Constructors

		public SimplePlayerPhysicsComponent(GameObject obj)
			: base(obj)
		{

		}
		#endregion

		#region Methods

		public override void CalcForces(GameTime time)
		{
			throw new NotImplementedException();
		}

		public override void Update(GameTime time)
		{
			var input = ServiceLocator.GetInputService();
			currV.X = 0;
			if (input.IsActionActive(/*Input.*/GameAction.MoveLeft))
				currV.X = -PlayerVx * (Obj.Bounds.Width / 50);
			if (input.IsActionActive(/*Input.*/GameAction.MoveRight))
				currV.X = PlayerVx * (Obj.Bounds.Width / 50);
			if (input.IsActionActive(/*Input.*/GameAction.Jump) && IsGrounded)
			{
				IsGrounded = false;
				currV.Y -= PlayerVy * (Obj.Bounds.Height / 50)/*Fy * time.ElapsedGameTime.TotalSeconds*/;
			}
			else
			{
				currV.Y += GravityAccel * (Obj.Bounds.Height / 50) * (float)time.ElapsedGameTime.TotalSeconds;
			}

			Move(time);

			// Collision
			if (Obj.Bounds.Bottom != GroundHeight)
			{
				if (Obj.Bounds.Bottom > GroundHeight)
				{
					Tranform.MoveToY(GroundHeight - Obj.Bounds.Height / 2);
					IsGrounded = true;
				}
				else if (Obj.Bounds.Bottom < GroundHeight)
					IsGrounded = false;
			}
			else
				IsGrounded = true;
			//throw new NotImplementedException();
		}

		private void Move(GameTime time)
		{
			Tranform.Move(currV * (float)time.ElapsedGameTime.TotalSeconds);
		}
		#endregion
	}
}