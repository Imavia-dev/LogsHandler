using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace logsHandler
{
    class Program
    {
        public static bool enableHttp;
        public static string httpcontext;
        public static string watchDirectory;

        static void Main(string[] args)
        {


            if (args.Length < 3)
            {
                Console.WriteLine("logsHandler prise en main rapide");
                Console.WriteLine("Pour activer le support de requette http utilisez l'option http:true|false");
                Console.WriteLine("Pour définir le contexte http utilisez l'option httpcontext:{Nom du contexte}");
                Console.WriteLine("Pour activer la surveillance d'un répertoire de log utilisez l'option watch:{Chemin du dossier}");
            }
            else
            {
                //Console.WriteLine("Debut de traitements des argument");
                checkArguments(args);
            }

            while (Console.Read()!='q')
            {
                
            }
            
       }


        private static void checkArguments(string[] args)
        {
            // Verification du nombre d'arguments
         
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

                        Console.WriteLine ("option : {0} Valeur : {1}",optionName,optionValue);


                        switch (optionName)
                        {
                            case "http" :
                                if (optionValue=="true")
                                {
                                    Console.Write("Valeur true");
                                    enableHttp = true;
                                }
                                else if (optionValue == "false")
                                {
                                    Console.Write("Valeur false");
                                    enableHttp = false;
                                }
                                else {
                                    Console.Write("La valeur {0} est invalide", optionValue);
                                    break;
                                }
                                break;


                            default:
                                break;
                        }
                    }


                }

            
    
        }

    }
}
