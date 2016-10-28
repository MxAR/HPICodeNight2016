using System.Threading.Tasks;

namespace HCGServer.Services
{
    public class Dummy
    {
        public static Task dummy_task()
        {
            return Task.FromResult(0);
        }

        public static void dummy_void()
        {
            return;
        }
    }
}
