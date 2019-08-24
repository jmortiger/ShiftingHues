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
		Minimal = 1,
		Median = 2,
		Verbose = 3,
		/// <summary>
		/// Only top level game components are logging.
		/// </summary>
		TopLevel
	}

	[Flags]
	public enum LogType
	{
		None			= 0b00000,
		Log				= 0b00001,
		Assert			= 0b00010,
		SoftError		= 0b00100,
		Warning			= 0b01000,
		CriticalError	= 0b10000,

		All				= 0b11111,
		AllErrors		= 0b10100
	}

	public interface ILogService
	{
		LogLevel CurrentLogLevel { get; }

		LogType CurrentLogTypes { get; }

		void LogMessage(LogMessage message);

		void LogMessage(string system, string message, LogLevel logLevel = LogLevel.Verbose, LogType logType = LogType.Log);

		//bool Assert(object expected, object toCompare);
	}

	public class SimpleLoggerService : GameComponent, ILogService
	{
		private readonly LogLevel currentLogLevel;
		public LogLevel CurrentLogLevel => currentLogLevel;

		private readonly LogType currentLogTypes;
		public LogType CurrentLogTypes => currentLogTypes;

		public SimpleLoggerService(Game game, LogLevel logLevel, LogType logTypes) : base(game)
		{
			currentLogLevel = logLevel;
			currentLogTypes = logTypes;
			TestLogger();
		}

		/// <summary>
		/// Checks and logs the proper creation and registration of the <see cref="SimpleLoggerService"/>.
		/// </summary>
		private void TestLogger()
		{
			this.LogMessage(new LogMessage(LogLevel.TopLevel, LogType.Log, "Logger", $"Logger of type {typeof(SimpleLoggerService).Name} constructed."));
			ServiceLocator.RegisterService(this);
			if (ServiceLocator.GetLogService() == this)
				this.LogMessage(new LogMessage(LogLevel.TopLevel, LogType.Log, "Logger", $"Logger of type {typeof(SimpleLoggerService).Name} accessible from ServiceLocator."));
			else
				this.LogMessage(new LogMessage(LogLevel.TopLevel, LogType.Warning, "Logger", $"Logger of type {typeof(SimpleLoggerService).Name} not accessible from ServiceLocator."));
		}


		public virtual void LogMessage(LogMessage message)
		{
			if (currentLogLevel >= message.Level && CurrentLogTypes.HasFlag(message.Type))
				Console.WriteLine(message.ComposeFullMessageString());
			//throw new NotImplementedException();
		}

		public virtual void LogMessage(
			string system, 
			string message, 
			LogLevel logLevel = LogLevel.Verbose, 
			LogType logType = LogType.Log) => LogMessage(new LogMessage(logLevel, logType, system, message));
	}

	//public static class DebugLog
	//{
	//	private static string log = "";

	//	public static void LogMessage(string message) => log += "\n" + message;
	//}
}
