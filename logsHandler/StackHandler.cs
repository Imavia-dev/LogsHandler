using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Net;

namespace logsHandler
{
     class StackHandler
    {
        StringBuilder errorBuffer = new StringBuilder();
        public string[] fileToSend;
        private int _index = 0; 

        public StackHandler(string[] files)
        {
            fileToSend = files; 
        }

        public bool handleRequestProcess()
        {
            Console.WriteLine("nombre de fichier à traites {0}", fileToSend.Length);

            if (fileToSend.Length > 0)
            {
                string fileInProgress = fileToSend[_index];

                Console.WriteLine("Traitement du fichier {0}", fileToSend[_index]);
                XmlDocument doc = new XmlDocument();
                doc.Load(fileInProgress);
                Console.WriteLine("Initialisation  de la requète ", fileToSend[_index]);
                //Console.WriteLine("Fichier : " + e.FullPath + " " + e.ChangeType);
                httpPostRequest request = new httpPostRequest();
                string xmldata = doc.DocumentElement.OuterXml;
                Console.WriteLine("contenu du fichier xml chargé {0}", xmldata);
                string checksum = request.calculateMd5Checksum(xmldata);


                request.url = "http://srvweb/claroline/web/app_dev.php/imavia_tracking/logshome";
                // On remplit le tableau de parametre 
                //Nom du 1er paramètre
                request.parametersName.Add("checksum");
                //Nom du 2ieme paramètre
                request.parametersName.Add("xmlData");
                // Nom du 3ième Paramètre
                request.parametersName.Add("logType");

                // Remplissage du tableau des valeurs de la requête 
                // Première Valeur (checksum)
                request.parametersValue.Add(checksum.ToLower());
                // Deuxiemme valeur (chaine de données xml)
                request.parametersValue.Add(xmldata);
                request.parametersValue.Add("Gameplay");
                //envoi de la requete 
                request.sendPostHttpRequest();
                Console.WriteLine("donnee envoyées...");
                // Recuperation de la valeur du statusCode de la reponse

                if (request.statusCode == "OK")
                {

                    try
                    {
                        File.Delete(fileInProgress);
                    }
                    catch (IOException ex)
                    {
                        Console.WriteLine(ex.Message);
                        throw;
                    }
                    //Supression du fichier 
                }
                else
                {
                    string decodedStringresponse = ParseXmlResponse(request.stringResponse);
                    errorBuffer.AppendLine(decodedStringresponse);
                    //Chemin de l'execution de l'application
                    Console.WriteLine("chemin de l'application", System.Reflection.Assembly.GetExecutingAssembly().Location);
                    string path = System.Reflection.Assembly.GetExecutingAssembly().Location;
                    string logFolderPath = path + "\\logs\\";
                    Console.WriteLine("dossier des logs {0}", logFolderPath);
                    string logFileName = "logs.txt";
                    Console.WriteLine("fichiers des logs {0}", logFileName);

                    if (!Directory.Exists(logFolderPath))
                    {
                        Directory.CreateDirectory(logFolderPath);
                    }

                    StreamWriter LogsManager = File.CreateText(logFolderPath + logFileName);
                    LogsManager.Write(errorBuffer.ToString());
                    LogsManager.Close();

                }

                Console.WriteLine(ParseXmlResponse(request.stringResponse));
                request.Dispose();
                while (_index < fileToSend.Length)
                {
                    _index++;
                    handleRequestProcess();
                }
            }
            return true; 

        }
         
        string ParseXmlResponse(string response)
        {

            StringBuilder stringToXml = new StringBuilder();
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(response);
       

            XmlNodeList messagesList = doc.GetElementsByTagName("message");
            XmlNodeList contentList = doc.GetElementsByTagName("content");

       
           if (messagesList.Count > 0)
           {
                XmlNode message = messagesList.Item(0);
                stringToXml.Append(message.InnerText);
           }

           if (contentList.Count > 0)
           {
               XmlNode content = contentList.Item(0);
               stringToXml.Append(content.InnerText);
           }
           

            return stringToXml.ToString();
        }


    }
}