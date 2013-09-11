using System;
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

    
                

        }


        private void checkArguments(string[] args)
        {
            // Verification du nombre d'arguments
            if (args.Length < 3)
            {
                Console.WriteLine("logsHandler prise en main rapide");
                Console.WriteLine("Pour activer le support de requette http utilisez l'option http:true|false");
                Console.WriteLine("Pour définir le contexte http utilisez l'option httpcontext:{Nom du contexte}");
                Console.WriteLine("Pour activer la surveillance d'un répertoire de log utilisez l'option watch:{Chemin du dossier}");
            }

            // Traitement des arguments de la ligne de commande

            // on esaie de decouper l'argument 
    
        }

    }
}
