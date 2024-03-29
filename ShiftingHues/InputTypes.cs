﻿namespace ShiftingHues.Library.Input
{
	/// <summary>
	/// The different ways an input (i.e. a button press) can trigger an action.
	/// </summary>
	public enum InputTypes
	{
		/// <summary>
		/// A continued press of a button (e.g firing an automatic gun, moving forward, ...).
		/// </summary>
		PushAndHold,
		/// <summary>
		/// A switch from active to inactive (this is an accesible alternative to <see cref="PushAndHold"/>).
		/// </summary>
		Toggle,
		/// <summary>
		/// Fires only when button is first pressed, not held (e.g firing a pistol, melee attack, ...)
		/// </summary>
		OnPress,
		/// <summary>
		/// Fires only when button is released (e.g firing a bow (Minecraft), ...)
		/// </summary>
		OnRelease
	}
}