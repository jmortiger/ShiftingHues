using Microsoft.Xna.Framework;
using JMMGExt;

namespace JMMGExt.Graphics
{
	public enum AnimationState
	{
		Active = 0,
		Inactive = 1,
		ToRemove = 2
	}
	/// <summary>
	/// TODO: Finish Docs
	/// TODO: Change to state pattern
	/// </summary>
	public class Animation
	{
		#region Properties
		public Sprite[] Frames { get; private set; }
		public DrawEffects2D[] SuggestedDrawEffects { get; private set; }
		public int NumFrames { get => Frames.Length; }
		public bool UseEachFramesDrawEffects { get; set; } = false;
		public DrawEffects2D DrawEffects { get; set; }
		public float SuggestedFPS { get; }
		#endregion

		#region Constructors
		private Animation(bool UseEachFramesDrawEffects, DrawEffects2D DrawEffects, float fps)
		{
			this.UseEachFramesDrawEffects = UseEachFramesDrawEffects;
			this.DrawEffects = DrawEffects;
			this.SuggestedFPS = fps;
		}

		public Animation(Sprite[] frames, DrawEffects2D? drawEffects = null, float fps = 60)
			: this(false, drawEffects ?? new DrawEffects2D(), fps)
		{
			this.Frames = frames;
			this.SuggestedDrawEffects = new DrawEffects2D[frames.Length];
			for (int i = 0; i < frames.Length; i++)
			{
				this.SuggestedDrawEffects[i] = new DrawEffects2D(0);
			}
		}

		public Animation(Sprite[] frames, DrawEffects2D[] spriteDrawEffects, float fps = 60)
			: this(true, new DrawEffects2D(), fps)
		{
			this.Frames = frames;
			this.SuggestedDrawEffects = spriteDrawEffects;
		}

		public Animation(SpriteInstance[] frames, float fps = 60)
			: this(true, new DrawEffects2D(), fps)
		{
			this.Frames = new Sprite[frames.Length];
			this.SuggestedDrawEffects = new DrawEffects2D[frames.Length];
			for (int i = 0; i < frames.Length; i++)
			{
				this.Frames[i] = frames[i].Sprite;
				this.SuggestedDrawEffects[i] = frames[i].DrawEffects;
			}
		}
		#endregion

		//public SpriteInstance[] CopySpriteInstances()
		//{
		//	var copy = new SpriteInstance[AnimationFrames.Length];
		//	for (int i = 0; i < AnimationFrames.Length; i++)
		//		copy[i] = new SpriteInstance(AnimationFrames[i]);
		//	return copy;
		//}
	}
}