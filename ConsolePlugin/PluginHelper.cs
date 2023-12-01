using Plugin;

namespace ConsolePlugin
{
    internal static class PluginHelper
    {
        internal static string DoMagicOne(Command com)
        {
            var str = string.Empty;

            //shuffle though all
            foreach (var data in com.Input)
            {
                var obj = DataRegister.Store[data];
                str += obj.ToString();
            }


            var result = "Here we go: " + str;
            return result;
        }

        internal static string DoMagicTwo(Command com)
        {
            var str = string.Empty;

            //shuffle though all
            foreach (var data in com.Input)
            {
                var obj = DataRegister.Store[data];
                str += obj.ToString();
            }


            var result = "Here we go: " + str;
            return result;
        }
    }
}