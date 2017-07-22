using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace SoftuniAutozip
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Dispatcher.UnhandledException += OnDispatcherUnhandledException;
        }

        void OnDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            //string errorMessage = $"An unhandled exception occurred: {e.Exception.Message}";
            Exception mostInnerException = e.Exception;
            while (mostInnerException.InnerException!=null)
            {
                mostInnerException = mostInnerException.InnerException;
            }

            MessageBox.Show(mostInnerException.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);


            e.Handled = true;
        }
    }
}
