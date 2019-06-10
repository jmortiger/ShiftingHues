using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;

using GeonBit.UI;

namespace ShiftingHues
{
	/// <summary>
	/// A wrapper around <see cref="GeonBit.UI.UserInterface"/>, allowing for conformity to components.
	/// TODO: Finsh
	/// </summary>
	public class GeonBitUIComponent : DrawableGameComponent
	{
		#region Fields and Properties

		#endregion

		#region Constructors

		public GeonBitUIComponent(Game game, string theme) : base(game)
		{
			throw new NotImplementedException();
		}

		public GeonBitUIComponent(Game game, GeonBit.UI.BuiltinThemes builtinThemes) : base(game)
		{
			throw new NotImplementedException();
		}

		public GeonBitUIComponent(Game game) 
			: this(game, BuiltinThemes.editor)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region Methods

		public override void Initialize()
		{
			base.Initialize();
		}
		#endregion
	}
}