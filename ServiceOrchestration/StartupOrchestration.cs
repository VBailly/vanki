
using UserInterfaceAPI;

namespace Orchestration
{
    public class StartupOrchestration
    {
        public static int Main(string[] args)
        {
            return StartProgram(args);
        }

        public static int StartProgram(string[] args)
        {
            ServiceOrchestration.InstallServices();
            UserInterface.Instance.Launch(args);

            return 0;
        }
    }
}
