
using System.Collections;
using System.Collections.Generic;
using System ; 
using System.Net;
using System.Text; 
using System.IO ;
using System.Security.Cryptography;

/* Classe pour envoyer des requetes http post via webrequest 
 * @Author JV
 * @Date 22/07/2013
 */

public class httpPostRequest : IDisposable {

	//Constructeur 
	public httpPostRequest ()
	{
		this.parametersName = new List<string>() ; 
		this.parametersValue =new List<string>() ; 
	}

	#region Déclaration des variables
	//Variable pour l'url 
	private string _url ;
	//Nom des différents paramètres de la requete
	private  List<string> _parametersName ; 
	// Valeur des différents paramètres de la requete 
	private List<string> _parametersValue ;
    private string _stringResponse;

    private string _statusCode;


    #endregion
    
    #region gettersetter
    public string url {
		get {
			return this._url;
		}
		set {
			_url = value;
		}
	}

	public List<string> parametersName {
		get {
			return this._parametersName;
		}
		set {
			_parametersName = value;
		}
	}

	public List<string> parametersValue {
		get {
			return this._parametersValue;
		}
		set {
			_parametersValue = value;
		}
	}
    
    public string stringResponse
    {
        get { return _stringResponse; }
        set { _stringResponse = value; }
    }

    public string statusCode
    {
        get { return _statusCode; }
        set { _statusCode = value; }
    }
    
    

	#endregion
	
	#region Methodes 

	/**
	 * Fonction qui permet d'envoyer une requète post via HttpWebrequest
	 * Attention à la concordance Nom et Valeur des paramètres
	 * @Author JV 
	 * @CreationDate 22/07/2013
	 */
	public void sendPostHttpRequest()
	{
		
		// Construteur de Chaine pour les données en POST 
		StringBuilder Postdata = new StringBuilder() ; 
		// Classe d'encodage des caractères
		UTF8Encoding encoder = new UTF8Encoding();
							
		// On parcours la liste pour ajouter un couple nom du parametre et valeur
		for (int i=0;i<this.parametersName.Count;i++)
		{
			
			// On verifie que les couples nom et valeur ne sont pas vides 
			if (this.parametersName[i].ToString().Length>0 && this.parametersValue[i].ToString().Length>0) {
				// On ajoute le nom et la valeur au données à envoyées
				Postdata.Append(this.parametersName[i]  + "=" + this.parametersValue[i].Replace("+","%2B"));	
				
			 }
			
			// Atention au esperluette qui est requis entre chaque paramètre
			// Il ne faut pas en mettre sur le dernier parametre
			if (i<this.parametersName.Count && this.parametersName.Count>1){
				Postdata.Append("&");
			}
		}
		
		// Transformation de la chaine à encoder en byte (Attention à l'encodage)
		
		byte[] dataToPost = encoder.GetBytes(Postdata.ToString());
		
		if (dataToPost.Length > 0) {
			
				// On cree une Web request 
				HttpWebRequest request = WebRequest.Create(this.url) as HttpWebRequest;
				// On définit la methode d'envoi à post 
             	request.Method = "POST";
                request.KeepAlive = false; 
				// On indique que les données qui transitent sont de type formulaire
				request.ContentType = "application/x-www-form-urlencoded";
				// On indique la taille totale des données à envoyées
				request.ContentLength = dataToPost.Length;
                request.ServicePoint.ConnectionLeaseTimeout = 5000;
                request.ServicePoint.MaxIdleTime = 5000;
				
				
	     	
                try
                {
                   // On recupère le flux d'ecriture de la requet que l'on viens de creer
                   using (Stream ecriveur = request.GetRequestStream())
                   {
                       ecriveur.Write(dataToPost, 0, dataToPost.Length);
                       ecriveur.Flush();
                       ecriveur.Close();
                   }
                   using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                   {
                      
                       using (Stream responseStream = response.GetResponseStream())
                       {
                           statusCode = response.StatusCode.ToString();
                           using (StreamReader reader = new StreamReader(responseStream))
                           {
                               stringResponse = reader.ReadToEnd();
                               reader.Close();
                           }
                           responseStream.Flush();
                            responseStream.Close();
                       }
                      response.Close();


                   }
             
                }
                catch (WebException ex)
                {

                    Console.WriteLine(ex.Message +  " " + ex.InnerException );
                
                }
         }
    }
	
    /**
   * Fonction calculateMd5Checksum
   * Permet de calculer le checksum d'une chaine en MD5 
   * @Param string chaine representant la chaine pour le calcul du hashcode
   * @Author JV
   */

    public string calculateMd5Checksum(string chaine)
    {
        //Encoder Utf8        


        UTF8Encoding utf8 = new UTF8Encoding();
        string checksum = "";
        // Cryptage en MD5   
        MD5 hashkey = MD5.Create();
        // On convertit la chaine en Byte
        byte[] data = utf8.GetBytes(chaine);
        // On calcule la clé hash 
        byte[] checksumhash = hashkey.ComputeHash(data);
        // on retourne la valeur de la clé
        checksum = BitConverter.ToString(checksumhash).Replace("-", string.Empty);
        Console.WriteLine("Checksum du fichier xml " + checksum);
        return checksum;

    }





#endregion
	
	
	#region Interface
	public void Dispose(){
		GC.SuppressFinalize (this);
	}
	#endregion
}
