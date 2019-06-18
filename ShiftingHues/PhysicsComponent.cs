using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace ShiftingHues
{
	public abstract class PhysicsComponent : IUpdateableGameObjComponent
	{
		#region Fields and Properties

		public GameObject Obj { get; }

		public TranformComponent Tranform { get => Obj.tranform; }
		#endregion

		#region Constructors

		#endregion

		#region Methods

		public abstract void Update(GameTime time);
		//{
		//	throw new NotImplementedException();
		//}

		public abstract void CalcForces(GameTime time);
		#endregion
	}
}