using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShiftingHues.Graphics
{
	/// <summary>
	/// A specfic instance of an <see cref="Animation"/>.
	/// </summary>
	public class AnimationInstance
	{
		#region Fields and Properties

		public Animation Animation { get; }

		public float SecondsPerFrame { get; set; }

		public float FPS { get => 1f / SecondsPerFrame; set => SecondsPerFrame = 1f / value; }

		public bool IsLooped { get; set; } = false;
		public int TimesLooped { get; private set; }

		// TODO: Consider a IsReversed thing
		#endregion

		#region Constructors
		
		public AnimationInstance(Animation animation, float fps, bool isLooped = false)
		{
			this.Animation = animation;
			this.FPS = fps;
			this.IsLooped = isLooped;
		}
		#endregion

		#region Methods

		#endregion
	}
}