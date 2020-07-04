using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
namespace CompiladorCono
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            string[] args = Environment.GetCommandLineArgs();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (args.Length > 1)
            {
                // Console.WriteLine("Cono");
                Lexico lexico = new Lexico(args[1]);
                Console.WriteLine("ARCHIVO:\n"+lexico.Abrir()+"\n\nLEXICO:\n");
                String str;
                if( lexico.Abrir() == "Error al abrir archivo")
                {
                    str = "Error al abrir archivo";
                }
                else
                {
                    str = lexico.Analizador_Lexico();
                }
                lexico.guardar_archivo(str,args[1]);
                Console.WriteLine(str);
            }
            else
            {
                Application.Run(new CompiladorCono());
            }
            
        }
    }
}
