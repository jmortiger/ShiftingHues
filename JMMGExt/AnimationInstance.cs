using System;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace JMMGExt.Graphics
{
	/// <summary>
	/// A specfic instance of an <see cref="Animation"/>.
	/// </summary>
	public class AnimationInstance
	{
		#region State Classes
		private interface IAnimationState
		{
			//AnimationInstance Instance { get; }

			Func<GameTime, IAnimationState> Update { get; }

			Action<SpriteBatch> Draw { get; }

			//IAnimationState Update(GameTime time);

			//void Draw(SpriteBatch batch);
		}

		private class InactiveState : IAnimationState
		{
			//private AnimationInstance animation;

			public Func<GameTime, IAnimationState> Update { get; }

			public Action<SpriteBatch> Draw { get; }

			internal InactiveState(AnimationInstance animation)
			{
				//this.animation = animation;
				Update = (GameTime time) =>
				{
					if (animation.IsPlaying)
						return new ActiveState(animation)?.Update(time);
					return this;
				};
				Draw = null;
			}

			//public IAnimationState Update(GameTime time)
			//{
			//	if (animation.IsPlaying)
			//		return new ActiveState(animation);
			//	return this;
			//}

			//public void Draw(SpriteBatch batch)
			//{
			//	//throw new NotImplementedException();
			//}
		}

		private class PausedState : IAnimationState
		{
			//private AnimationInstance animation;

			public Func<GameTime, IAnimationState> Update { get; }

			public Action<SpriteBatch> Draw { get; }

			internal PausedState(AnimationInstance animation)
			{
				//this.animation = animation;
				Update = (GameTime time) =>
				{
					if (animation.IsPlaying)
						return new ActiveState(animation)?.Update(time);
					return this;
				};
				Draw = (SpriteBatch batch) =>
				{
					if (animation.Posit == null)
					{
						if (animation.Animation.UseEachFramesDrawEffects)
							animation.AnimationFrames[animation.CurrentFrame].DrawSprite(batch);
						else
							animation.AnimationFrames[animation.CurrentFrame].DrawSprite(batch, animation.Animation.DrawEffects);
					}
					else
					{
						if (animation.Animation.UseEachFramesDrawEffects)
							animation.AnimationFrames[animation.CurrentFrame].DrawSprite(batch, null, animation.Posit.Value);
						else
							animation.AnimationFrames[animation.CurrentFrame].DrawSprite(batch, animation.Animation.DrawEffects, animation.Posit.Value);
					}
				};
			}

			//public IAnimationState Update(GameTime time)
			//{
			//	if (animation.IsPlaying)
			//		return new ActiveState(animation);
			//	return this;
			//}

			//public void Draw(SpriteBatch batch)
			//{
			//	if (animation.Animation.UseEachFramesDrawEffects)
			//		animation.AnimationFrames[animation.CurrentFrame].DrawSprite(batch);
			//	else
			//		animation.AnimationFrames[animation.CurrentFrame].DrawSprite(batch, animation.Animation.DrawEffects);
			//	//throw new NotImplementedException();
			//}
		}

		private class StoppedState : IAnimationState
		{
			//private AnimationInstance animation;

			public Func<GameTime, IAnimationState> Update { get; }

			public Action<SpriteBatch> Draw { get; }

			internal StoppedState(AnimationInstance animation)
			{
				//this.animation = animation;
				Update = (GameTime time) =>
				{
					if (animation.IsPlaying)
						return new ActiveState(animation)?.Update(time);
					return this;
				};
				Draw = null;
			}

			//public IAnimationState Update(GameTime time)
			//{
			//	if (animation.IsPlaying)
			//		return new ActiveState(animation);
			//	return this;
			//}

			//public void Draw(SpriteBatch batch) { }
		}

		private class ActiveState : IAnimationState
		{
			//private AnimationInstance animation;

			public Func<GameTime, IAnimationState> Update { get; }

			public Action<SpriteBatch> Draw { get; }

			internal ActiveState(AnimationInstance animation)
			{
				//this.animation = animation;
				Update = (GameTime gameTime) =>
				{
					IAnimationState toReturn = this;
					if (animation.IsPlaying)
					{
						// Add to the time counter. - J Mor
						animation.timeSinceLastFrameChange += (float)gameTime.ElapsedGameTime.TotalSeconds;

						// If enough time has gone by to actually change frames...
						while (animation.timeSinceLastFrameChange >= animation.SecondsPerFrame)
						{
							// ...then update the frame, and...
							animation.CurrentFrame++;
							// ...if the current frame is the last, ...
							if (animation.CurrentFrame >= animation.Animation.NumFrames)
							{
								if (animation.IsLooped) // ...and the animation is set to wrap...
								{
									// ...then update the frame # & the times looped.
									animation.CurrentFrame = 0;
									animation.TimesLooped++;
								}
								else // if not, the animation is done, and should be removed soon.
								{
									toReturn = new CompletedState(animation);
									animation.CurrentFrame--; // To avoid IndexOutOfBounds error in draw func.
								}
							}
							// Remove one "frame" worth of time
							animation.timeSinceLastFrameChange -= animation.SecondsPerFrame;

							// If the animation isn't allowed to skip animation frames, then break out of the loop.
							if (!animation.CanSkipFrames)
								break;
						}
					}
					else
						toReturn = new PausedState(animation);

					return toReturn;
				};
				Draw = (SpriteBatch batch) =>
				{
					if (animation.Posit == null)
					{
						if (animation.Animation.UseEachFramesDrawEffects)
							animation.AnimationFrames[animation.CurrentFrame].DrawSprite(batch);
						else
							animation.AnimationFrames[animation.CurrentFrame].DrawSprite(batch, animation.Animation.DrawEffects);
					}
					else
					{
						if (animation.Animation.UseEachFramesDrawEffects)
							animation.AnimationFrames[animation.CurrentFrame].DrawSprite(batch, null, animation.Posit.Value);
						else
							animation.AnimationFrames[animation.CurrentFrame].DrawSprite(batch, animation.Animation.DrawEffects, animation.Posit.Value);
					}
					//throw new NotImplementedException();
				};
			}

			//public IAnimationState Update(GameTime gameTime)
			//{
			//	IAnimationState toReturn = this;
			//	if (animation.IsPlaying)
			//	{
			//		// Add to the time counter. - J Mor
			//		animation.timeSinceLastFrameChange += (float)gameTime.ElapsedGameTime.TotalSeconds;

			//		// If enough time has gone by to actually flip frames... - J Mor
			//		if (animation.timeSinceLastFrameChange >= animation.SecondsPerFrame)
			//		{
			//			// ...then update the frame, and... - J Mor
			//			animation.CurrentFrame++;
			//			// ...if the current frame is the last, ... - J Mor
			//			if (animation.CurrentFrame >= animation.Animation.NumFrames)
			//			{
			//				if (animation.IsLooped) // ...and the animation is set to wrap... - J Mor
			//				{
			//					// ...then update the frame # & the times looped. - J Mor
			//					animation.CurrentFrame = 0;
			//					animation.TimesLooped++;
			//				}
			//				else // if not, the animation is done, and should be removed soon. - J Mor
			//				{
			//					toReturn = new CompletedState(animation);
			//					animation.CurrentFrame--; // To avoid IndexOutOfBounds error. - J Mor
			//				}
			//			}
			//			//SpriteSheet.UpdateImgSourceRectangle();
			//			// Remove one "frame" worth of time
			//			animation.timeSinceLastFrameChange -= animation.SecondsPerFrame;
			//		}
			//	}
			//	else
			//		toReturn = new PausedState(animation);

			//	return toReturn;
			//}

			//public void Draw(SpriteBatch batch)
			//{
			//	if (animation.Posit == null)
			//	{
			//		if (animation.Animation.UseEachFramesDrawEffects)
			//			animation.AnimationFrames[animation.CurrentFrame].DrawSprite(batch);
			//		else
			//			animation.AnimationFrames[animation.CurrentFrame].DrawSprite(batch, animation.Animation.DrawEffects);
			//	}
			//	else
			//	{
			//		if (animation.Animation.UseEachFramesDrawEffects)
			//			animation.AnimationFrames[animation.CurrentFrame].DrawSprite(batch, null, animation.Posit.Value);
			//		else
			//			animation.AnimationFrames[animation.CurrentFrame].DrawSprite(batch, animation.Animation.DrawEffects, animation.Posit.Value);
			//	}
			//	//throw new NotImplementedException();
			//}
		}

		private class CompletedState : IAnimationState
		{
			//private AnimationInstance animation;

			public Func<GameTime, IAnimationState> Update { get; }

			public Action<SpriteBatch> Draw { get; }

			internal CompletedState(AnimationInstance animation)
			{
				//this.animation = animation;
				Update = (GameTime time) => { return this; };
				Draw = null;
			}

			//public IAnimationState Update(GameTime time)
			//{
			//	return this;
			//	//throw new NotImplementedException();
			//}

			//public void Draw(SpriteBatch batch)
			//{
			//	//throw new NotImplementedException();
			//}
		}
		#endregion

		public event EventHandler Completed;
		public event EventHandler Paused;
		public event EventHandler Played;
		public event EventHandler Stopped;
		public event EventHandler AfterReset;

		#region Fields and Properties
		// TODO: Consider a IsReversed thing

		public Animation Animation { get; }

		private SpriteInstance[] AnimationFrames { get; set; }

		public float SecondsPerFrame { get; set; }

		public float FPS { get => 1f / SecondsPerFrame; set => SecondsPerFrame = 1f / value; }
		public int TimesLooped { get; private set; }

		/// <summary>
		/// Counts time since last frame.
		/// </summary>
		private float timeSinceLastFrameChange;

		public int CurrentFrame { get; private set; } = 0;

		public bool IsPlaying { get; private set; } = false;

		public Point? Posit = null;

		#region Advanced Control

		/// <summary>
		/// Controls whether to loop the animation or not.
		/// </summary>
		public bool IsLooped { get; set; } = false;

		/// <summary>
		/// Controls whether the anim can skip frames of animation to 
		/// "catch up" to the frame it should be at if its running slow.
		/// </summary>
		public bool CanSkipFrames { get; set; } = false;

		// TODO: Figure out crossfade
		///// <summary>
		///// Controls whether to crossfade animation frames.
		///// </summary>
		//public bool CrossfadeFrames { get; set; } = false;
		#endregion

		private IAnimationState CurrState { get; set; }/* = new InactiveState(this);*/
		#endregion

		#region Constructors

		public AnimationInstance(Animation animation, float fps, bool isLooped = false)
		{
			this.Animation = animation;
			this.FPS = fps;
			this.IsLooped = isLooped;

			InitializeAnimation();
		}

		public AnimationInstance(Animation animation, bool isLooped = false)
		{
			this.Animation = animation;
			this.FPS = animation.SuggestedFPS;
			this.IsLooped = isLooped;

			InitializeAnimation();
		}
		#endregion

		#region Methods

		private void InitializeAnimation()
		{
			// Initialize Frames
			AnimationFrames = new SpriteInstance[Animation.NumFrames];
			for (int i = 0; i < Animation.NumFrames; i++)
			{
				AnimationFrames[i] = new SpriteInstance(Animation.Frames[i], Animation.SuggestedDrawEffects[i]);
			}

			// Initialize state
			CurrState = new InactiveState(this);
		}

		public void Play()
		{
			IsPlaying = true;
			CurrState = new ActiveState(this);
			Played?.Invoke(this, new EventArgs());
		}

		public void Pause()
		{
			IsPlaying = false;
			CurrState = new PausedState(this);
			Paused?.Invoke(this, new EventArgs());
		}

		public void Stop()
		{
			IsPlaying = false;
			CurrState = new StoppedState(this);
			Stopped?.Invoke(this, new EventArgs());
		}

		public void Reset()
		{
			IsPlaying = false;
			TimesLooped = 0;
			timeSinceLastFrameChange = 0;
			CurrentFrame = 0;
			CurrState = new InactiveState(this);
			AfterReset?.Invoke(this, new EventArgs());
		}

		public void Update(GameTime time) => CurrState = CurrState.Update?.Invoke(time);//CurrState = CurrState.Update(time);

		public void Draw(SpriteBatch batch) => CurrState.Draw?.Invoke(batch);//CurrState.Draw(batch);
		#endregion
	}
}