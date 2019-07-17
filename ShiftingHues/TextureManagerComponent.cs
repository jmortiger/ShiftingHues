using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ShiftingHues.Graphics;

namespace ShiftingHues
{
	public class TextureManagerComponent : GameComponent
	{
		#region Fields and Properties
		private Dictionary<string, Sprite> sprites;
		private Dictionary<string, Animation> animations;
		private SpriteSheet[] spriteSheets;
		#endregion

		#region Constructors
		
		//public TextureManagerComponent(Game game, int numSpriteSheets)
		//	: this(game)
		//{

		//}

		public TextureManagerComponent(
			Game game, 
			Dictionary<string, Sprite> sprites, 
			Dictionary<string, Animation> animations, 
			SpriteSheet[] spriteSheets)
			: base(game)
		{
			this.sprites = sprites ?? new Dictionary<string, Sprite>();
			this.animations = animations ?? new Dictionary<string, Animation>();
			this.spriteSheets = spriteSheets;
		}
		#endregion

		#region Methods

		public void RegisterSprite(string resourceID, SpriteSheet spriteSheet, int index)
		{
			if (sprites.ContainsKey(resourceID))
			{
				throw new ArgumentException("The provided key (" + resourceID + ") is in use already.", "resourceID");
			}
			else
				sprites.Add(resourceID, spriteSheet[index]);
		}

		public void RegisterSpritesFromSheet(string[] resourceIDs, SpriteSheet spriteSheet)
		{
			if (resourceIDs.Length != spriteSheet.Sprites.Length)
				throw new ArgumentException("The list of keys (resourceIDs) must be the same length as the number of sprites in the sheet.", "resourceID");
			for (int i = 0; i < resourceIDs.Length || i < spriteSheet.Sprites.Length; i++)
				sprites.Add(resourceIDs[i], spriteSheet.Sprites[i]);
		}

		private Sprite GetSprite(string resourceID)
		{
			try
			{
				return sprites[resourceID];
			}
			catch (KeyNotFoundException e)
			{
				throw new KeyNotFoundException("The provided key (" + resourceID + ") was not found in the dictionary.", e);
			}
			//if (sprites.ContainsKey(resourceID))
			//	return sprites[resourceID];
			//else
			//	throw new KeyNotFoundException("The provided key (" + resourceID + ") was not found in the dictionary.");
		}

		public void RegisterAnimation(string resourceID, string[] spriteIds, DrawEffects2D[] suggestedDrawEffects, float fps)
		{
			if (spriteIds.Length != suggestedDrawEffects.Length)
				throw new ArgumentException("The list of keys (spriteIDs) must be the same length as the suggestedDrawEffects.", "spriteIDs");
			if (animations.ContainsKey(resourceID))
				throw new ArgumentException("The provided key (" + resourceID + ") is in use already.", "resourceID");
			var animFrames = new Sprite[spriteIds.Length];
			for (int i = 0; i < spriteIds.Length; i++)
				animFrames[i] = GetSprite(spriteIds[i]);
			animations.Add(resourceID, new Animation(animFrames, suggestedDrawEffects, fps));
		}

		public void RegisterAnimation(string resourceID, string[] spriteIds, DrawEffects2D suggestedDrawEffect, float fps)
		{
			if (animations.ContainsKey(resourceID))
				throw new ArgumentException("The provided key (" + resourceID + ") is in use already.", "resourceID");
			var animFrames = new Sprite[spriteIds.Length];
			for (int i = 0; i < spriteIds.Length; i++)
				animFrames[i] = GetSprite(spriteIds[i]);
			animations.Add(resourceID, new Animation(animFrames, suggestedDrawEffect, fps));
		}

		public SpriteInstance GetSpriteInstance(string resourceID, DrawEffects2D? drawEffects = null, Rectangle? destinationRectangle = null)
		{
			try
			{
				return new SpriteInstance(sprites[resourceID], drawEffects, destinationRectangle);
			}
			catch (KeyNotFoundException e)
			{
				throw new KeyNotFoundException("The provided key (" + resourceID + ") was not found in the dictionary.", e);
			}
		}

		public SpriteInstance GetSpriteInstance(string resourceID, FPoint position, DrawEffects2D? drawEffects = null)
		{
			try
			{
				return new SpriteInstance(sprites[resourceID], drawEffects, position);
			}
			catch (KeyNotFoundException e)
			{
				throw new KeyNotFoundException("The provided key (" + resourceID + ") was not found in the dictionary.", e);
			}
		}
		#endregion
	}
}