using Microsoft.Maui;
using Microsoft.Maui.Hosting;
using System;

namespace Proyecto_Sistema_de_horarios_U4_S4
{
    internal class Program : MauiApplication
    {
        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();

        static void Main(string[] args)
        {
            var app = new Program();
            app.Run(args);
        }
    }
}
