using System;
using Gtk;
using Serilog;

namespace Dmm.Linux;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Verbose()
            .WriteTo.Console()
            .CreateLogger();
        
        Log.Information("Starting application");

        try
        {
            Application.Init();

            Application app = new("org.Dmm.Linux", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            MainWindow win = new();
            app.AddWindow(win);

            win.Show();
            Application.Run();
        }
        catch (Exception e)
        {
            Log.Error(e, "An exception was thrown while running the application");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}