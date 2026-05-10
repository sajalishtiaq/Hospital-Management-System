// Program.cs
// Hospital Management System
// Group: Saliha Noor (24i-3066), Sajal Ishtiaq (24i-3041)

using System;
using System.Windows.Forms;

namespace HospitalManagement
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            // Required for ReportViewer spatial types
            SqlServerTypes.Utilities.LoadNativeAssemblies(AppDomain.CurrentDomain.BaseDirectory);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginForm());
        }
    }
}