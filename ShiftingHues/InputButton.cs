using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShiftingHues.Input
{
    public enum InputDeviceType
    {
        None     = 0,
        Keyboard = 1,
        GamePad  = 2,
        Mouse    = 3,
        Other    = 4
    }
    public enum MouseButtons
    {
        Left,
        Middle,
        Right,
        ScrollUp,
        ScrollDown/*,
        ScrollLeft,
        ScrollRight*//*,
        L1,
        L2,
        R1,
        R2*/
    }
    public struct InputButton
    {
        public InputDeviceType DeviceType { get; private set; }
        public int Button { get; private set; }
        public Type ButtonType
        {
            get
            {
                switch (DeviceType)
                {
                    case InputDeviceType.None:
                        return typeof(Buttons);
                        break;
                    case InputDeviceType.Keyboard:
                        return typeof(Keys);
                        break;
                    case InputDeviceType.GamePad:
                        return typeof(Buttons);
                        break;
                    case InputDeviceType.Mouse:
                        return typeof(MouseButtons);
                        break;
                    case InputDeviceType.Other:
                        return typeof(Buttons);
                        break;
                    default:
                        return typeof(Buttons);
                        break;
                }
                /*if (deviceType == InputDeviceType.Keyboard)
                    return typeof(Keys);
                else if (deviceType == InputDeviceType.GamePad)
                    return typeof(Buttons);
                else if (deviceType == InputDeviceType.Mouse)
                    return typeof(MouseButtons);
                else
                    return typeof(Buttons);*/
            }
        }
        public InputButton(InputDeviceType deviceType, int button)
        {
            this.DeviceType = deviceType;
            this.Button = button;
        }
    }
}