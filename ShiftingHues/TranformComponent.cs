using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using JMMGExt;

namespace ShiftingHues.Library.Objects
{
	public class TranformComponent : IGameObjComponent, IUpdateable
	{
		public event EventHandler<EventArgs> EnabledChanged;
		public event EventHandler<EventArgs> UpdateOrderChanged;
		
		#region Fields and Properties
		public GameObject Obj { get; }
		public TranformComponent Tranform { get => this/*Obj.tranform*/; } // Either should work
		public Vector2 Forward { get; private set; } = Vector2.UnitX;
		public Vector2 Up { get; private set; } = Vector2.UnitY;
		//public float Rotation { get => (float)Math.Acos(Vector2.Dot(Forward, Vector2.UnitX)); }
		public FPoint Posit { get; private set; }
		private FRectangle bounds;
		public FRectangle Bounds { get => bounds; }

		#region IUpdateable Overloads
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

		public TranformComponent(GameObject obj, FRectangle bounds, Vector2 fwd)
			:this(obj, bounds, bounds.Location, fwd) { }

		public TranformComponent(GameObject obj, FRectangle bounds, float rotation)
			: this(obj, bounds, bounds.Location, rotation) { }

		public TranformComponent(GameObject obj, FRectangle bounds, FPoint position, Vector2 fwd)
		{
			Obj = obj;
			Posit = position;
			//Rotate((float)Math.Acos(Vector2.Dot(Vector2.UnitX, fwd)));
			this.bounds = bounds;
		}

		public TranformComponent(GameObject obj, FRectangle bounds, FPoint position, float rotation)
		{
			Obj = obj;
			Posit = position;
			//Rotate(rotation);
			this.bounds = bounds;
		}
		#endregion

		#region Methods

		public void Update(GameTime time)
		{
			bounds.Offset(Posit - bounds.Location);
			//throw new NotImplementedException();
		}

		public void Move(Vector2 offset)
		{
			Posit += offset;
		}

		public void MoveTo(Vector2 position)
		{
			Posit = new FPoint(position);
		}

		public void MoveTo(FPoint position)
		{
			Posit = position;
		}

		public void MoveToX(float xCoord)
		{
			Posit = new FPoint(xCoord, Posit.Y);
		}

		public void MoveToY(float yCoord)
		{
			Posit = new FPoint(Posit.X, yCoord);
		}

		public void SetLocation(Vector2 posit)
		{
			Posit = new FPoint(posit);
			bounds.Location = new FPoint(posit);
		}

		//public void Rotate(float degrees)
		//{
		//	throw new NotImplementedException();
		//}
		#endregion
	}
}