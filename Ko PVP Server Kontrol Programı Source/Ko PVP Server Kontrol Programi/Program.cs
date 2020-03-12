using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ko_PVP_Server_Kontrol_Programi
{
    static class Program
    {
        
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (System.Diagnostics.Process.GetProcessesByName("Ko PVP Server Kontrol Programi").Length > 1) 
            { 
                DialogResult hata = 
                    MessageBox.Show("Erişim Engellendi! Program ikinci kez açılamaz.","Hata",MessageBoxButtons.OK,MessageBoxIcon.Error); 
                Application.Exit(); 
            } 
            else 
            {
                Application.EnableVisualStyles(); 
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frm_DosyaYol()); 
            }
        }
    }
}
