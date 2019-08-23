using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Microsoft.Xna.Framework;

namespace JMMGExt
{
	public class FileLogger : SimpleLoggerService
	{
		public const string DEBUG_FILE_NAME = "DebugLog.txt";
		private StreamWriter outputStream;
		public FileLogger(Game game, LogLevel logLevel, LogType logTypes, string filePath) : base(game, logLevel, logTypes)
		{
			game.Exiting += OnGameExiting;
			outputStream = File.AppendText(filePath + "\\" + DEBUG_FILE_NAME);
			outputStream.WriteLine(DateTime.Now.ToString());
			LogMessage("Logger", "FileLogger filestream created.", LogLevel.TopLevel);
			if (outputStream == null)
				LogMessage("Logger", "FileLogger filestream is null.", LogLevel.TopLevel, LogType.Warning);
		}

		public override void LogMessage(LogMessage message)
		{
			outputStream?.WriteLine(message.ComposeFullMessageString());
			base.LogMessage(message);
		}

		public override void LogMessage(string system, string message, LogLevel logLevel = LogLevel.Verbose, LogType logType = LogType.Log)
		{
			var t = new LogMessage(logLevel, logType, system, message);
			this.LogMessage(t);
			base.LogMessage(t);
		}

		private void OnGameExiting(object sender, EventArgs e)
		{
			LogMessage("Logger", "Closing logger file and disposing of resources.", LogLevel.TopLevel);
			outputStream.WriteLine("==================================================================");
			outputStream.Flush();
			outputStream.Close();
			outputStream?.Dispose();
		}
	}
}
