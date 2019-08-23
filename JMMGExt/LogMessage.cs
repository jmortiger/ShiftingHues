namespace JMMGExt
{
	public class LogMessage
	{
		/// <summary>
		/// The text of the message;
		/// </summary>
		public readonly string Message;
		/// <summary>
		/// The level of the message, which determines if the message is logged.
		/// </summary>
		public readonly LogLevel Level;
		/// <summary>
		/// A short hand way of identifying what game system this message is tied to;
		/// </summary>
		public readonly string System;
		/// <summary>
		/// The type of message this is.
		/// </summary>
		public readonly LogType Type;

		public LogMessage(LogLevel logLevel, LogType logType, string system, string message)
		{
			Level = logLevel;
			Type = logType;
			System = system;
			Message = message;
		}

		public string ComposeShortMessageString() => $"[{System}]: {Message}";

		public string ComposeFullMessageString() => $"{{{Type.ToString()}}} - Level {(int)Level}, [{System}]: {Message}";
	}

	//public static class DebugLog
	//{
	//	private static string log = "";

	//	public static void LogMessage(string message) => log += "\n" + message;
	//}
}
