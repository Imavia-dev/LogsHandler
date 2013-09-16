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
        private string _emetteur;
        private bool sendRequest=false; 

        public StackHandler(string[] files)
        {
            fileToSend = files; 
        }

        public bool handleRequestProcess()
        {
            Console.WriteLine("nombre de fichier à traites {0}", fileToSend.Length);

            if (fileToSend.Length > 0 && _index<fileToSend.Length)
            {
                string fileInProgress = fileToSend[_index];

                Console.WriteLine("Traitement du fichier {0}", fileToSend[_index]);
                XmlDocument doc = new XmlDocument();
                doc.Load(fileInProgress);
                // Recuperation du sender 
                try
                {
                    XmlNodeList players = doc.GetElementsByTagName("Player");
                    XmlNode player = players[0];
                    _emetteur = player.Attributes["emetteur"].Value;
                    Console.WriteLine("emetteur du fichier {0}", _emetteur);
                    sendRequest = true;
                }
                catch (Exception)
                {
                    Console.WriteLine("Erreur Impossible de determiner le sender");
                }
               

                if (sendRequest)
                {
                    Console.WriteLine("Initialisation  de la requète ", fileToSend[_index]);
                    //Console.WriteLine("Fichier : " + e.FullPath + " " + e.ChangeType);
                    httpPostRequest request = new httpPostRequest();
                    string xmldata = doc.DocumentElement.OuterXml;
                    Console.WriteLine("contenu du fichier xml chargé {0}", xmldata);
                    string checksum = request.calculateMd5Checksum(xmldata);


                    request.url = CheckParameter.url;
                    // On remplit le tableau de parametre 
                    //Nom du 1er paramètre
                    request.parametersName.Add("checksum");
                    //Nom du 2ieme paramètre
                    request.parametersName.Add("xmlData");
                    // Nom du 3ième Paramètre
                    request.parametersName.Add("logType");
                    request.parametersName.Add("emetteur");

                    // Remplissage du tableau des valeurs de la requête 
                    // Première Valeur (checksum)
                    request.parametersValue.Add(checksum.ToLower());
                    // Deuxiemme valeur (chaine de données xml)
                    request.parametersValue.Add(xmldata);
                    request.parametersValue.Add("Gameplay");
                    request.parametersValue.Add(_emetteur);
                    //envoi de la requete 
                    request.sendPostHttpRequest();
                    Console.WriteLine("donnee envoyées...");
                    // Recuperation de la valeur du statusCode de la reponse
                    Console.WriteLine("Status Code {0}", request.statusCode);
                    if (request.statusCode == "OK")
                    {

                        try
                        {
                            File.Delete(fileInProgress);
                        }
                        catch (IOException ex)
                        {
                            Console.WriteLine(ex.Message);

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

                    Console.WriteLine(request.stringResponse);
                    request.Dispose();
                }
                while (_index < fileToSend.Length)
                {
                    _index++;
                    handleRequestProcess();
                }


            } else
            {
               return true ;  

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