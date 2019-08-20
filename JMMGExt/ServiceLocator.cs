using System.Threading;
using System.Threading.Tasks;
using IInputService = JMMGExt.Input.IInputService;

namespace JMMGExt
{
	public static class ServiceLocator
	{
		#region Fields and Properties
		private static IInputService InputService;
		private static ILogService LogService;
		#endregion

		#region Methods
		#region RegisterSerivce

		public static void RegisterService(IInputService inputService) => InputService = inputService;

		public static void RegisterService(ILogService logService) => LogService = logService;
		#endregion

		public static IInputService GetInputService() => InputService;

		public static ILogService GetLogService() => LogService;
		#endregion
	}
}