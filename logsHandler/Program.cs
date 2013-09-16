using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.IO;

namespace logsHandler
{
    class Program
    {
        static Timer t = new Timer();
     

        static void Main(string[] args)
        {
            t.Interval = 15000;
            

                bool error = CheckParameter.checkArguments(args);
                if (!error)
                {
                    t.Enabled = true;
                    t.Elapsed += new ElapsedEventHandler(getFileList);

                    //watcher.startWatcher();
                    while (Console.Read() != 'q')
                    {
                        
                    }
                }
          }

        public static void getFileList(Object source, ElapsedEventArgs e) {
            Console.WriteLine("Démarrage de l'envoi des fichiers");
            bool executed = false ;
            t.Enabled = false;
            Console.WriteLine("desactivation du timer");
    
            Console.WriteLine("recupération de la liste des xml dans le dossier");
            string[] fileToSend = Directory.GetFiles(CheckParameter.watchDirectory, CheckParameter.extensionFilter);
            Console.WriteLine("liste des fichiers à traités : ");
            foreach (string f in fileToSend)
            {
                Console.WriteLine(f);
            }


            // Envoi de la liste à une fonction qui doit gerer la pile de requete
            Console.WriteLine("Envoi des fichiers en cours .... ");

            StackHandler sh = new StackHandler(fileToSend);

            
            executed = sh.handleRequestProcess();

            Console.WriteLine("Envoi terminée avec succès {0}" , executed);
            
            if (executed)
            {
                Console.WriteLine("Activation du timer");
                t.Interval = 60000;
                t.Enabled = true; 

            }

        }


    }
}
