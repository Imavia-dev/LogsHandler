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
            if (args.Length>0) {

                CheckParameter.checkArguments(args);        
                watcher.startWatcher();
            }
            while (Console.Read()!='q')
            {
                
            }
        
            
       }
    }
}
