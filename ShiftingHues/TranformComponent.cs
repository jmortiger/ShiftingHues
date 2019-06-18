using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ShiftingHues
{
	public class TranformComponent : IUpdateableGameObjComponent
	{
		#region Fields and Properties
		public GameObject Obj { get; }
		public TranformComponent Tranform { get => this/*Obj.tranform*/; } // Either should work
		public Vector2 Forward { get; private set; } = Vector2.UnitX;
		public Vector2 Up { get; private set; } = Vector2.UnitY;
		public float Rotation { get => (float)Math.Acos(Vector2.Dot(Forward, Vector2.UnitX)); }
		public FPoint Posit { get; private set; }
		#endregion

		#region Constructors

		public TranformComponent(GameObject obj, FPoint position, Vector2 fwd)
		{
			Obj = obj;
			Posit = position;
			Rotate((float)Math.Acos(Vector2.Dot(Vector2.UnitX, fwd)));
		}

		public TranformComponent(GameObject obj, FPoint position, float rotation)
		{
			Obj = obj;
			Posit = position;
			Rotate(rotation);
		}
		#endregion

		#region Methods

		public void Update(GameTime time)
		{
			throw new NotImplementedException();
		}

		public void Move(Vector2 offset)
		{
			Posit += offset;
		}

		public void Rotate(float degrees)
		{
			throw new NotImplementedException();
		}
		#endregion
	}
}