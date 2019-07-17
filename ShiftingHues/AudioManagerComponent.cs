using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace ShiftingHues
{
	public class AudioManagerComponent : GameComponent
	{
		#region Fields and Properties

		private Dictionary<string, SoundEffect> sfxs;
		//public Dictionary<string, SoundEffect> SFX_Dictionary { get; private set; }

		public Dictionary<string, SoundEffect>.KeyCollection ResourceIDList { get => sfxs.Keys; }
		#endregion

		#region Constructors

		public AudioManagerComponent(Game game, Dictionary<string, SoundEffect> sfxs)
			: base(game)
		{
			this.sfxs = sfxs ?? new Dictionary<string, SoundEffect>();
		}
		#endregion

		#region Methods

		public void RegisterSoundEffect(string resourceID, SoundEffect sound)
		{
			if (sfxs.ContainsKey(resourceID))
				throw new ArgumentException("The provided key (" + resourceID + ") is already in use.", "resourceID");
			//if (sfxs.ContainsValue(sound))
			//	return;
			sfxs.Add(resourceID, sound);
		}

		/// <summary>
		/// TODO: Finish Doc
		/// </summary>
		/// <param name="resourceID"></param>
		/// <returns></returns>
		/// <exception cref="KeyNotFoundException"></exception>
		public SoundEffectInstance GetSoundEffectInstance(string resourceID)
		{
			try
			{
				return sfxs[resourceID].CreateInstance();
			}
			catch (KeyNotFoundException e)
			{
				throw new KeyNotFoundException("Did you give the wrong ID/not add the resource correctly?", e);
			}
		}
		#endregion
	}
}