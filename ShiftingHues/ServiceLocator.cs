using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ShiftingHues
{
    public static class ServiceLocator
    {
        #region Fields and Properties
        private static Input.IInputService InputService;
        #endregion
        
        #region Methods

        public static void RegisterService(Input.IInputService inputService) => InputService = inputService;

        public static Input.IInputService GetInputService() => InputService;
        #endregion
    }
}