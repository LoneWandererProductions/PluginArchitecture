using Plugin;

namespace ConsolePlugin
{
    internal static class PluginHelper
    {
        internal static void DoMagic()
        {
            var com = new CommandResult();

            com.Count++;

            com.Result = "Here we go";

            ConsolePlugin.Result[0] = com;
        }
    }
}