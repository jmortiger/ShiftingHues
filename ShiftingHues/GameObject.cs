using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShiftingHues
{
	public interface IGameObjComponent
	{
		GameObject Obj { get; }
		TranformComponent Tranform { get; }
	}

	public interface IUpdateableGameObjComponent : IGameObjComponent
	{

		void Update(GameTime time);
	}

	public interface IDrawableGameObjComponent : IGameObjComponent
	{

		void Draw(SpriteBatch batch, GameTime time);
	}

	public class GameObject
	{
		#region Fields and Properties

		public TranformComponent tranform;

		public IGameObjComponent[] components;

		public IUpdateableGameObjComponent[] updateableComponents;

		public IDrawableGameObjComponent[] drawableComponents;

		public Rectangle Bounds { get; private set; }
		#endregion

		#region Constructors

		public GameObject()
		{

		}
		#endregion

		#region Methods

		public void Update(GameTime time)
		{
			foreach (var comp in updateableComponents)
				if (comp != tranform)
					comp.Update(time);
			tranform.Update(time);
		}

		public void Draw(SpriteBatch batch, GameTime time)
		{
			foreach (var comp in drawableComponents)
				comp.Draw(batch, time);
		}
		#endregion
	}
}