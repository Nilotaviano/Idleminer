using System.Linq;
using System.Windows.Forms;

namespace Idleminer
{
    class Program
    {
        private const string NO_PATH_SPECIFIED_MESSAGE = "Specify the path: ";

        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var parameters = new Parameters(args.First());
            var application = new NotificationIcon(parameters);

            Application.Run(application);
        }
    }
}
