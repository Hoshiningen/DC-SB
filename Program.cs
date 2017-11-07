using System;

namespace DC_SB
{
    class Program
    {
        public static void Main()
        {
            try
            {
                App app = new App();
                app.InitializeComponent();
                app.Run();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Console.WriteLine("Press Enter to exit.");
                Console.ReadLine();
            }
        }
    }
}
