using System;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ShiftingHues.Library.Input
{
    [DataContract(Name = "InputDevice")]
    public enum InputDeviceType
    {
        [EnumMember]
        None     = 0,
        [EnumMember]
        Keyboard = 1,
        [EnumMember]
        GamePad  = 2,
        [EnumMember]
        Mouse    = 3,
        [EnumMember]
        Other    = 4
    }

    [DataContract(Name = "InputBttn")]
    public struct InputButton
    {
        #region Properties
        [DataMember]
        public InputDeviceType DeviceType { get; private set; }
        [DataMember]
        public object Button { get; private set; }
        public float Deadzone { get; private set; }
        public Type ButtonType
        {
            get
            {
                switch (DeviceType)
                {
                    case InputDeviceType.None:
                        return typeof(Buttons);
                    case InputDeviceType.Keyboard:
                        return typeof(Keys);
                    case InputDeviceType.GamePad:
                        return typeof(Buttons);
                    case InputDeviceType.Mouse:
                        return typeof(MouseButtons);
                    case InputDeviceType.Other:
                        return typeof(Buttons);
                    default:
                        return typeof(Buttons);
                }
            }
        }
        #endregion

        #region Constructors
        #region With Deadzone

        private InputButton(InputDeviceType deviceType, object button, float deadzone)
        {
            this.DeviceType = deviceType;
            this.Button = button;
            this.Deadzone = deadzone;
        }

        //public InputButton(Keys key, float deadzone)
        //    : this(InputDeviceType.Keyboard, key, deadzone) { }
        public InputButton(Buttons bttn, float deadzone)
            : this(InputDeviceType.GamePad, bttn, deadzone) { }
        public InputButton(MouseButtons bttn, float deadzone)
            : this(InputDeviceType.Mouse, bttn, deadzone) { }
        #endregion
        #region Without Deadzone

        private InputButton(InputDeviceType deviceType, object button)
            : this(deviceType, button, 0f)
        {
            this.DeviceType = deviceType;
            this.Button = button;
        }

        public InputButton(Keys key)
            : this(InputDeviceType.Keyboard, key) { }
        public InputButton(Buttons bttn)
            : this(InputDeviceType.GamePad, bttn, ((bttn & (Buttons.LeftTrigger | Buttons.RightTrigger)) == (Buttons.LeftTrigger | Buttons.RightTrigger)) ? .25f : 0f) { }
        public InputButton(MouseButtons bttn)
            : this(InputDeviceType.Mouse, bttn) { }
        #endregion
        #endregion

        #region Is Same Button

        public bool IsSameButton(Keys key) => (DeviceType == InputDeviceType.Keyboard) ? key == (Keys)Button : false;

        public bool IsSameButton(Buttons bttn) => (DeviceType == InputDeviceType.GamePad) ? bttn == (Buttons)Button : false;

        public bool IsSameButton(MouseButtons bttn) => (DeviceType == InputDeviceType.Mouse) ? bttn == (MouseButtons)Button : false;
        #endregion

        public string _ToString()
        {
            string buttonName = "";
            switch (DeviceType)
            {
                case InputDeviceType.None:
                    break;
                case InputDeviceType.Keyboard:
                    buttonName = Enum.GetName(typeof(Keys), (Keys)Button);
                    break;
                case InputDeviceType.GamePad:
                    buttonName = Enum.GetName(typeof(Buttons), (Buttons)Button);
                    break;
                case InputDeviceType.Mouse:
                    buttonName = Enum.GetName(typeof(MouseButtons), (MouseButtons)Button);
                    break;
                case InputDeviceType.Other:
                    break;
                default:
                    break;
            }
            return $"{Enum.GetName(typeof(InputDeviceType), DeviceType)} {buttonName}";
        }
    }
}