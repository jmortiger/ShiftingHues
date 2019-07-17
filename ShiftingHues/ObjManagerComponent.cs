using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShiftingHues
{
	public class ObjManagerComponent : DrawableGameComponent
	{
		#region Fields and Properties

		public SpriteBatch spriteBatch;

		public List<GameObject> Objects { get; private set; }
		#endregion

		#region Constructors

		public ObjManagerComponent(Game game, SpriteBatch spriteBatch) : base(game)
		{
			this.spriteBatch = spriteBatch;
		}
		#endregion

		#region Methods

		public void AddObject(GameObject gameObject)
		{
			Objects.Add(gameObject);
		}

		public override void Update(GameTime gameTime)
		{
			foreach (var obj in Objects)
			{
				obj.Update(gameTime);
			}
			base.Update(gameTime);
		}

		public override void Draw(GameTime gameTime)
		{
			foreach (var obj in Objects)
			{
				obj.Draw(gameTime, spriteBatch);
			}
			base.Draw(gameTime);
		}
		#endregion
	}
}