using System;
using System.Windows.Forms;
using company_management.View;
using company_management.DAO;
using company_management.DTO;
using System.Data;

namespace company_management
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormLogin());
        }

    }
}
