using System.Windows;

namespace DC_SB.Utils
{
    public static class ErrorHandler
    {
        private static Windows.ErrorWindow window;
        public static Window Owner { get; set; }

        public static void Raise(string message)
        {
            if (window == null || !window.IsActive)
            {
                window = new Windows.ErrorWindow();
                window.Owner = Owner;
            }
            window.ShowError(message);
        }

        public static void Raise(string message, params string[] parameters)
        {
            Raise(string.Format(message, parameters));
        }

        public static void RaiseDialog(string message)
        {
            if (window == null || !window.IsActive)
            {
                window = new Windows.ErrorWindow();
                window.Owner = Owner;
            }
            window.ShowErrorDialog(message);
        }

        public static void RaiseDialog(string message, params string[] parameters)
        {
            Raise(string.Format(message, parameters));
        }
    }
}
