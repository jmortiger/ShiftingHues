using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ShiftingHues.Graphics
{
	/// <summary>
	/// A specfic instance of an <see cref="Animation"/>.
	/// </summary>
	public class AnimationInstance
	{
		#region State Classes
		interface IAnimationState
		{
			//AnimationInstance Instance { get; }

			IAnimationState Update(GameTime time);

			void Draw(SpriteBatch batch);
		}

		class InactiveState : IAnimationState
		{
			AnimationInstance animation;

			internal InactiveState(AnimationInstance animation)
			{
				this.animation = animation;
			}

			public IAnimationState Update(GameTime time)
			{
				if (animation.IsPlaying)
					return new ActiveState(animation);
				return this;
			}

			public void Draw(SpriteBatch batch)
			{
				//throw new NotImplementedException();
			}
		}

		class PausedState : IAnimationState
		{
			AnimationInstance animation;

			internal PausedState(AnimationInstance animation)
			{
				this.animation = animation;
			}

			public IAnimationState Update(GameTime time)
			{
				if (animation.IsPlaying)
					return new ActiveState(animation);
				return this;
			}

			public void Draw(SpriteBatch batch)
			{
				if (animation.Animation.UseEachFramesDrawEffects)
					animation.AnimationFrames[animation.CurrentFrame].DrawSprite(batch);
				else
					animation.AnimationFrames[animation.CurrentFrame].DrawSprite(batch, animation.Animation.DrawEffects);
				//throw new NotImplementedException();
			}
		}

		class ActiveState : IAnimationState
		{
			AnimationInstance animation;

			internal ActiveState(AnimationInstance animation)
			{
				this.animation = animation;
			}

			public IAnimationState Update(GameTime gameTime)
			{
				IAnimationState toReturn = this;
				if (animation.IsPlaying)
				{
					// Add to the time counter. - J Mor
					animation.timeSinceLastFrameChange += (float)gameTime.ElapsedGameTime.TotalSeconds;

					// If enough time has gone by to actually flip frames... - J Mor
					if (animation.timeSinceLastFrameChange >= animation.SecondsPerFrame)
					{
						// ...then update the frame, and... - J Mor
						animation.CurrentFrame++;
						// ...if the current frame is the last, ... - J Mor
						if (animation.CurrentFrame >= animation.Animation.NumFrames)
						{
							if (animation.IsLooped) // ...and the animation is set to wrap... - J Mor
							{
								// ...then update the frame # & the times looped. - J Mor
								animation.CurrentFrame = 0;
								animation.TimesLooped++;
							}
							else // if not, the animation is done, and should be removed soon. - J Mor
							{
								toReturn = new CompletedState(animation);
								animation.CurrentFrame--; // To avoid IndexOutOfBounds error. - J Mor
							}
						}
						//SpriteSheet.UpdateImgSourceRectangle();
						// Remove one "frame" worth of time
						animation.timeSinceLastFrameChange -= animation.SecondsPerFrame;
					}
				}
				else
					toReturn = new PausedState(animation);

				return toReturn;
			}

			public void Draw(SpriteBatch batch)
			{
				if (animation.Animation.UseEachFramesDrawEffects)
					animation.AnimationFrames[animation.CurrentFrame].DrawSprite(batch);
				else
					animation.AnimationFrames[animation.CurrentFrame].DrawSprite(batch, animation.Animation.DrawEffects);
				//throw new NotImplementedException();
			}
		}

		class CompletedState : IAnimationState
		{
			AnimationInstance animation;

			internal CompletedState(AnimationInstance animation)
			{
				this.animation = animation;
			}

			public IAnimationState Update(GameTime time)
			{
				return this;
				//throw new NotImplementedException();
			}

			public void Draw(SpriteBatch batch)
			{
				//throw new NotImplementedException();
			}
		}
		#endregion

		#region Fields and Properties
		// TODO: Consider a IsReversed thing

		public Animation Animation { get; }

		private SpriteInstance[] AnimationFrames { get; set; }

		public float SecondsPerFrame { get; set; }

		public float FPS { get => 1f / SecondsPerFrame; set => SecondsPerFrame = 1f / value; }

		public bool IsLooped { get; set; } = false;
		public int TimesLooped { get; private set; }

		/// <summary>
		/// Counts time since last frame.
		/// </summary>
		private float timeSinceLastFrameChange;

		public int CurrentFrame { get; set; } = 0;

		public bool IsPlaying { get; private set; } = false;

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

		public void Update(GameTime time) => CurrState = CurrState.Update(time);

		public void Draw(SpriteBatch batch) => CurrState.Draw(batch);
		#endregion
	}
}