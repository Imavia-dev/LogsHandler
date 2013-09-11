using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            startWatcher();
        }

        public static void startWatcher() 
        {
            Console.WriteLine("Application console logsHandler en cours de démarrage…");

            FileSystemWatcher watcher = new FileSystemWatcher();

            Console.WriteLine("Nouveau FileSystemWatcher générer !");

            // Chemin du dossier a surveiller
            watcher.Path = @"C:\Users\Sebastien.FORMAGRAPH\Desktop\fsw";

            Console.WriteLine("Chemin surveillé : " + watcher.Path);
            
            // Filtres d'observation du watcher
            watcher.NotifyFilter = NotifyFilters.LastAccess | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName | NotifyFilters.Security.HasFlag(FlagsAttribute.;

            // Inclusion des sous-dossiers
            watcher.IncludeSubdirectories = true;

            // Type de fichier a rechercher
            watcher.Filter = "*.xml";

            Console.WriteLine("Type de fichiers surveillés : " + watcher.Filter);

            watcher.Created += new FileSystemEventHandler(OnStateChange);
            watcher.Deleted += new FileSystemEventHandler(OnStateChange);
            watcher.Renamed += new RenamedEventHandler(OnRenamed);

            watcher.EnableRaisingEvents = true;

            Console.WriteLine("Appuyez sur \'q\' puis entrée pour quitter l'application");
            while (Console.Read() != 'q') ;
        }

        private static void OnStateChange(object source, FileSystemEventArgs e)
        {
            Console.WriteLine("Fichier : " + e.FullPath + " " + e.ChangeType);
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            Console.WriteLine("Fichier : {0} renommé en {1}", e.OldFullPath, e.FullPath);
        }
    }
}