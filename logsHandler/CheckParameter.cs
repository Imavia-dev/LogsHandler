using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; 

namespace logsHandler
{
    static class CheckParameter
    {
        public static string watchDirectory;
        public static string url;
        public static string extensionFilter;

        public static bool checkArguments(string[] args)
        {
            // Verification du nombre d'arguments
            bool error = false ;

            if (args.Length < 3)
            {
                Console.WriteLine("logsHandler prise en main rapide");
                Console.WriteLine("Pour activer la surveillance d'un répertoire de log utilisez l'option path:{Chemin du dossier}");
                Console.WriteLine("Filtre des extensions filter:[*.{extension du fichier}]");
                Console.WriteLine("url de destination des fichier url:{route}");
                error = true;
            }
            else
            {

                //Console.WriteLine("nombre d'arguments corrects") ; 
                // Traitement des arguments de la ligne de commande

                // on esaie de decouper l'argument 

                for (int i = 0; i < args.Length; i++)
                {
                    // On recherche dans l'argument le caractère ":" 
                    string argument = args[i];
                    //Console.Write(argument);

                    int argumentIndex = argument.IndexOf(":");
                    if (argumentIndex == -1)
                    {
                        Console.Write("Les options doivent s'écrirent sous la forme de \r\n [nom de l'otion]:[Valeur du paramètre] ");

                        break;
                    }
                    else
                    {
                        string optionName = argument.Substring(0, (argumentIndex));
                        string optionValue = argument.Substring(argumentIndex + 1);

                        Console.WriteLine("option : {0} Valeur : {1}", optionName, optionValue);


                        switch (optionName)
                        {
                         
                            case "path" :

                                if (!Directory.Exists(optionValue))
                                {
                                    error=true; 
                                     Console.WriteLine("le chemin {0} n'existe pas ", optionValue);
                                } else 
                                {
                                    watchDirectory=optionValue;
                                }
                              break ; 

                            case "url" :
                                if (optionValue.Length>1)
                                {
                                    url = optionValue; 
                                }
                               else
                                {
                                    error = true;
                                    Console.WriteLine("l'option {0} n'est pas une route valide ", optionValue);
                                }
                           break;

                           case "filter":

                           if (!optionValue.Contains("*."))
                           {
                               error = true;
                               Console.WriteLine("Le filtre de fichier {0} n'est pas valide ", optionValue);
                           }
                           else
                           {
                               extensionFilter = optionValue;
                           }
                           break; 

                           default :
                           error = true;
                           Console.WriteLine("l'option {0} n'est pas valide ", optionName);
                           break; 

                         
                        }
                    }
                }
            }

            return error; 

        }
    }
}
