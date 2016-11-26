using DeckAPI;
using StorageAPI;
using UserInterfaceAPI;

namespace Orchestration
{
    public static class ServiceOrchestration
    {
        public static void InstallServices()
        {
            DisposableDeckFactory.Instance = new DisposableDeckFactoryImpl();
            UserInterface.Instance = new InteractiveCommandLine();
            Storage.Instance = new FileBasedStorage();
            RandomAPI.Random.Instance = new RandomImpl();
        }

        public static void InstallServicesForTests()
        {
            DisposableDeckFactory.Instance = new DisposableDeckFactoryImpl();
            UserInterface.Instance = new InteractiveCommandLine();
            Storage.Instance = new MemoryBasedStorage();
            RandomAPI.Random.Instance = new RandomImpl();
        }

        public static void UninstallServices()
        {
            DisposableDeckFactory.Instance = null;
            UserInterface.Instance = null;
            Storage.Instance = null;
            RandomAPI.Random.Instance = null;
        }
   }
}

