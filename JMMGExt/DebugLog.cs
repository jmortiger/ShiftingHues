using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using JMMGExt.Input;

namespace JMMGExt
{
	public enum LogLevel
	{
		/// <summary>
		/// No logging at all.
		/// </summary>
		None = 0,
		/// <summary>
		/// Only top level game components are logging.
		/// </summary>
		TopLevel = 1,
		Minimal = 2,
		Verbose = 3
	}

	[Flags]
	public enum LogType
	{
		None = 0b00000,
		Log = 0b00001,
		Assert = 0b00010,
		Error = 0b00100,
		Warning = 0b01000,
		CriticalError = 0b10000
	}

	public interface ILogService
	{
		LogLevel CurrentLogLevel { get; }

		LogType CurrentLogTypes { get; }

		void LogMessage(LogMessage message);

		//bool Assert(object expected, object toCompare);
	}

	public class SimpleLoggerService : ILogService
	{
		private LogLevel currentLogLevel;
		public LogLevel CurrentLogLevel => currentLogLevel;

		private LogType currentLogTypes;
		public LogType CurrentLogTypes => currentLogTypes;

		public SimpleLoggerService(LogLevel logLevel, LogType logTypes)
		{
			currentLogLevel = logLevel;
			currentLogTypes = logTypes;
			ServiceLocator.RegisterService(this);
			TestLogger();
		}

		/// <summary>
		/// Checks and logs the proper creation and registration of the <see cref="SimpleLoggerService"/>.
		/// TODO: Finish
		/// </summary>
		private void TestLogger()
		{
			this.LogMessage(new LogMessage(LogLevel.TopLevel, LogType.Log, "Logger", $"Logger of type {typeof(SimpleLoggerService).Name}constructed."));

		}


		public void LogMessage(LogMessage message)
		{
			if (currentLogLevel >= message.Level && CurrentLogTypes.HasFlag(message.Type))
				Console.WriteLine(message.ComposeFullMessageString());
			//throw new NotImplementedException();
		}
	}

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

	public static class DebugLog
	{
		private static string log = "";

		public static void LogMessage(string message) => log += "\n" + message;
	}
}
