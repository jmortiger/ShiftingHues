using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using GeonBit.UI;
using GeonBit.UI.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

using JMMGExt;
using JMMGExt.Input;

/// <summary>
/// <see cref="ShiftingHues.UI"/> contains all UI things.
/// </summary>
namespace ShiftingHues.UI
{
	public class UIEntityExtraData
	{
		#region Fields and Properties
		public readonly Entity attachedEntity;

		#region Neighboring Entities
		/// <summary>
		/// The <see cref="Entity"/> to the left of <see cref="attachedEntity"/> in the menu.
		/// </summary>
		public Entity ToLeft;
		/// <summary>
		/// The <see cref="Entity"/> to the right of <see cref="attachedEntity"/> in the menu.
		/// </summary>
		public Entity ToRight;
		/// <summary>
		/// The <see cref="Entity"/> above <see cref="attachedEntity"/> in the menu.
		/// </summary>
		public Entity Above;
		/// <summary>
		/// The <see cref="Entity"/> below <see cref="attachedEntity"/> in the menu.
		/// </summary>
		public Entity Below;
		#endregion

		#region EventCallbacks
		/// <summary>
		/// An <see cref="EventCallback"/> fired when this is selected and the menu up is fired.
		/// </summary>
		/// <remarks>This is fired before attempting to switch to <see cref="Above"/> (provided <see cref="AutoSwitch"/> is <c>true</c>).</remarks>
		public EventCallback OnMenuUp;
		/// <summary>
		/// An <see cref="EventCallback"/> fired when this is selected and the menu down is fired.
		/// </summary>
		/// <remarks>This is fired before attempting to switch to <see cref="Below"/> (provided <see cref="AutoSwitch"/> is <c>true</c>).</remarks>
		public EventCallback OnMenuDown;
		/// <summary>
		/// An <see cref="EventCallback"/> fired when this is selected and the menu left is fired.
		/// </summary>
		/// <remarks>This is fired before attempting to switch to <see cref="ToLeft"/> (provided <see cref="AutoSwitch"/> is <c>true</c>).</remarks>
		public EventCallback OnMenuLeft;
		/// <summary>
		/// An <see cref="EventCallback"/> fired when this is selected and the menu right is fired.
		/// </summary>
		/// <remarks>This is fired before attempting to switch to <see cref="ToRight"/> (provided <see cref="AutoSwitch"/> is <c>true</c>).</remarks>
		public EventCallback OnMenuRight;
		/// <summary>
		/// An <see cref="EventCallback"/> fired when this is selected and the menu accept is fired.
		/// </summary>
		/// <remarks>This is fired before attempting to perform the accept action.</remarks>
		public EventCallback OnMenuAccept;
		#endregion

		/// <summary>
		/// Controls whether to switch the selected <see cref="Entity"/> automatically.
		/// </summary>
		public bool AutoSwitch = true;

		public bool IsSelected { get => attachedEntity.IsMouseOver; }
		#endregion

		#region Constructors

		public UIEntityExtraData(
			Entity attachedEntity, 
			Entity ToLeft	= null, 
			Entity ToRight	= null, 
			Entity Above	= null, 
			Entity Below	= null)
		{
			this.attachedEntity = attachedEntity;
			attachedEntity.AttachedData = this;
			this.ToLeft = ToLeft;
			this.ToRight = ToRight;
			this.Above = Above;
			this.Below = Below;
			InitializeEventListeners();
		}
		#endregion

		#region Methods

		private void InitializeEventListeners()
		{
			var input = ServiceLocator.GetInputService();
			//input.MenuLeft += Input_MenuLeft;
			//input.MenuRight += Input_MenuRight;
			//input.MenuUp += Input_MenuUp;
			//input.MenuDown += Input_MenuDown;
			input.AnyMenuInput += OnAnyMenuInput;
		}

		private void OnAnyMenuInput(/*Input.*/MenuEventArgs e)
		{
			if (!IsSelected)
				return;
			switch (e.menuAction)
			{
				case /*Input.*/GameAction.MenuLeft:
					FireMenuNav(OnMenuLeft, ToLeft);
					break;
				case /*Input.*/GameAction.MenuRight:
					FireMenuNav(OnMenuRight, ToRight);
					break;
				case /*Input.*/GameAction.MenuUp:
					FireMenuNav(OnMenuUp, Above);
					break;
				case /*Input.*/GameAction.MenuDown:
					FireMenuNav(OnMenuDown, Below);
					break;
				case /*Input.*/GameAction.MenuAccept:
					// TODO: Handle accept
					break;
				case /*Input.*/GameAction.MenuCancel:
					// TODO: Handle cancel
					break;
				default:
					break;
			}
		}

		private void FireMenuNav(EventCallback callback, Entity nextEntity)
		{
			callback?.Invoke(attachedEntity);
			if (AutoSwitch && nextEntity?.AttachedData != null && nextEntity.AttachedData.GetType() == typeof(UIEntityExtraData))
				((UIEntityExtraData)ToLeft.AttachedData).SelectSelf();
		}
		//private void Input_AnyMenuInput(/*Input.*/MenuEventArgs e)
		//{
		//	if (IsSelected)
		//	{
		//		switch (e.menuAction)
		//		{
		//			case /*Input.*/GameAction.MenuLeft:
		//				OnMenuLeft?.Invoke(attachedEntity);
		//				if (ToLeft != null && AutoSwitch && ToLeft.AttachedData.GetType() == typeof(UIEntityExtraData))
		//					((UIEntityExtraData)ToLeft.AttachedData).SelectSelf();
		//				return;
		//			case /*Input.*/GameAction.MenuRight:
		//				OnMenuRight?.Invoke(attachedEntity);
		//				if (ToRight != null && AutoSwitch && ToRight.AttachedData.GetType() == typeof(UIEntityExtraData))
		//					((UIEntityExtraData)ToRight.AttachedData).SelectSelf();
		//				return;
		//			case /*Input.*/GameAction.MenuUp:
		//				OnMenuUp?.Invoke(attachedEntity);
		//				if (Above != null && AutoSwitch && Above.AttachedData.GetType() == typeof(UIEntityExtraData))
		//					((UIEntityExtraData)Above.AttachedData).SelectSelf();
		//				return;
		//			case /*Input.*/GameAction.MenuDown:
		//				OnMenuDown?.Invoke(attachedEntity);
		//				if (Below != null && AutoSwitch && Below.AttachedData.GetType() == typeof(UIEntityExtraData))
		//					((UIEntityExtraData)Below.AttachedData).SelectSelf();
		//				return;
		//			case /*Input.*/GameAction.MenuAccept:
		//				// TODO: Handle accept
		//				break;
		//			case /*Input.*/GameAction.MenuCancel:
		//				// TODO: Handle cancel
		//				break;
		//			default:
		//				return;
		//		}
		//	}
		//}

		//private void Input_MenuDown(/*Input.*/MenuEventArgs e)
		//{
		//	if (IsSelected)
		//	{
		//		OnMenuDown?.Invoke(attachedEntity);
		//		if (Below != null && AutoSwitch && Below.AttachedData.GetType() == typeof(UIEntityExtraData))
		//			((UIEntityExtraData)Below.AttachedData).SelectSelf();
		//	}
		//}

		//private void Input_MenuUp(/*Input.*/MenuEventArgs e)
		//{
		//	if (IsSelected)
		//	{
		//		OnMenuUp?.Invoke(attachedEntity);
		//		if (Above != null && AutoSwitch && Above.AttachedData.GetType() == typeof(UIEntityExtraData))
		//			((UIEntityExtraData)Above.AttachedData).SelectSelf();
		//	}
		//}

		//private void Input_MenuRight(/*Input.*/MenuEventArgs e)
		//{
		//	if (IsSelected)
		//	{
		//		OnMenuRight?.Invoke(attachedEntity);
		//		if (ToRight != null && AutoSwitch && ToRight.AttachedData.GetType() == typeof(UIEntityExtraData))
		//			((UIEntityExtraData)ToRight.AttachedData).SelectSelf();
		//	}
		//}

		//private void Input_MenuLeft(/*Input.*/MenuEventArgs e)
		//{
		//	if (IsSelected)
		//	{
		//		OnMenuLeft?.Invoke(attachedEntity);
		//		if (ToLeft != null && AutoSwitch && ToLeft.AttachedData.GetType() == typeof(UIEntityExtraData))
		//			((UIEntityExtraData)ToLeft.AttachedData).SelectSelf();
		//	}
		//}

		/// <summary>
		/// Selects this <see cref="Entity"/> by placeing the mouse cursor over itself and hiding the mouse cursor.
		/// </summary>
		public void SelectSelf()
		{
			ServiceLocator.GetInputService().SetNextMousePosition(attachedEntity.GetActualDestRect().Center.ToVector2());
			UserInterface.Active.ShowCursor = false;
			//IsSelected = true;
		}
		#endregion
	}
}