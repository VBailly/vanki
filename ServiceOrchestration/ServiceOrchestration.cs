using DeckAPI;
using UserInterfaceAPI;

namespace Orchestration
{
    public static class ServiceOrchestration
    {
        public static void InstallServices()
        {
            DisposableDeckFactory.Instance = new DisposableDeckFactoryImpl();
            UserInterface.Instance = new InteractiveCommandLine();
        }

        public static void UninstallServices()
        {
            DisposableDeckFactory.Instance = null;
            UserInterface.Instance = null;
        }
   }
}

