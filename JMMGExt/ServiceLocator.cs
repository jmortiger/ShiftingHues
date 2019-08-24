using System;
using System.Threading;
using System.Threading.Tasks;
using IInputService = JMMGExt.Input.IInputService;
using ITextureService = JMMGExt.ITextureService;

namespace JMMGExt
{
	public static class ServiceLocator
	{
		#region Fields and Properties
		private static IInputService InputService;
		private static ITextureService TextureService;
		private static ILogService LogService;
		#endregion

		#region Methods
		#region RegisterSerivce

		public static void RegisterService(IInputService inputService)
		{
			InputService = inputService;
			LogService?.LogMessage(new LogMessage(LogLevel.TopLevel, LogType.Log, "ServiceLocator", "InputService registered."));
		}

		public static void RegisterService(ITextureService textureService)
		{
			TextureService = textureService;
			LogService?.LogMessage(new LogMessage(LogLevel.TopLevel, LogType.Log, "ServiceLocator", "TextureService registered."));
		}

		public static void RegisterService(ILogService logService)
		{
			LogService = logService;
			LogService?.LogMessage(new LogMessage(LogLevel.TopLevel, LogType.Log, "ServiceLocator", "LogService registered."));
		}
		#endregion
		#region GetService

		public static object GetService(Type serviceType)
		{
			if (serviceType == typeof(IInputService))
				return GetInputService();
			else if (serviceType == typeof(ILogService))
				return GetLogService();
			else if (serviceType == typeof(ITextureService))
				return GetTextureService();
			else
				throw new ArgumentException("The type provided is not acceptable.", serviceType.Name);
		}

		public static IInputService GetInputService() => InputService;

		public static ILogService GetLogService() => LogService;

		public static ITextureService GetTextureService() => TextureService;
		#endregion

		public static void LogMessage(LogMessage message) => LogService?.LogMessage(message);

		public static void LogMessage(
			string system, 
			string message, 
			LogLevel logLevel = LogLevel.Verbose, 
			LogType logType = LogType.Log) => LogService?.LogMessage(new LogMessage(system, message, logLevel, logType));
		#endregion
	}
}