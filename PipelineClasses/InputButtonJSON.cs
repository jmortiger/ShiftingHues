using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace PipelineClasses
{
	[DataContract(Name = "InputDevice")]
	public enum InputDeviceTypeJSON
	{
		[EnumMember]
		None = 0,
		[EnumMember]
		Keyboard = 1,
		[EnumMember]
		GamePad = 2,
		[EnumMember]
		Mouse = 3,
		[EnumMember]
		Other = 4
	}

	[JsonObject]
	[Serializable]
	public class InputButtonJSON
	{
		[JsonProperty("DeviceType")]
		public /*InputDeviceTypeJSON*/int DeviceType { get; private set; }

		[JsonProperty("Button")]
		public object Button { get; private set; }

		[JsonConstructor]
		public InputButtonJSON(/*InputDeviceTypeJSON*/int DeviceType, object Button)
		{
			this.DeviceType = /*(InputDeviceTypeJSON)*/DeviceType;
			this.Button = Button;
		}
	}

	// TODO: finish input serialization
	public class BindInfoJSON
	{

	}
}
