using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShiftingHues
{
	public enum EntityFields
	{
		None,
		Anchor,
		Background,
		//Children,
		Draggable,
		//EntityDefaultSize,
		FillColor,
		//InternalDestRect,
		IsFocused,
		IsMouseDown,
		IsMouseOver,
		MaxSize,
		MinSize,
		Offset,
		Opacity,
		OutlineColor,
		OutlineOpacity,
		OutlineWidth,
		Padding,
		//Parent,
		RawStyleSheet,
		Scale,
		ShadowColor,
		ShadowOffset,
		ShadowScale,
		Size,
		SpaceAfter,
		SpaceBefore,
		State,
		Visible
	}
	public enum SerializedCommands
	{
		/// <summary>
		/// Denotes an if statement.
		/// </summary>
		IF,
		/// <summary>
		/// Denotes an else statement.
		/// </summary>
		ELSE,
		/// <summary>
		/// Denotes an assignment.
		/// </summary>
		ASSIGN,
		/// <summary>
		/// Denotes the end of a line.
		/// </summary>
		STOP
	}
	/// <summary>
	/// 
	/// </summary>
	/// <!--IF this.-->
	class EntityCallbackSerialization
	{
		#region Fields and Properties

		#endregion

		#region Constructors

		#endregion

		#region Methods

		#endregion
	}
}