using System;
using System.Threading.Tasks;
using Dmm.Core;
using Gtk;
using UI = Gtk.Builder.ObjectAttribute;

namespace Dmm.Linux
{
    class MainWindow : Window
    {
        [UI] private readonly Button _loginButton = null;
        [UI] private readonly Button _doSomethingButton = null;

        public MainWindow() : this(new Builder("MainWindow.glade"))
        {
        }

        private MainWindow(Builder builder) : base(builder.GetRawOwnedObject("MainWindow"))
        {
            builder.Autoconnect(this);

            DeleteEvent += Window_DeleteEvent;
            _loginButton.Clicked += LoginButtonClicked;
            _doSomethingButton.Clicked += DoSomethingButtonClicked;
        }

        private void Window_DeleteEvent(object sender, DeleteEventArgs a)
        {
            Application.Quit();
        }

        private void LoginButtonClicked(object sender, EventArgs a)
        {
            Task.Run(async () => await DmmApi.LoginAsync("", ""));
        }

        private void DoSomethingButtonClicked(object sender, EventArgs e)
        {
            Task.Run(async () => await DmmApi.LaunchGame("", ""));
        }
    }
}