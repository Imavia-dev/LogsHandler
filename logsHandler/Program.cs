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
         

                bool error = CheckParameter.checkArguments(args);
                if (!error)
                {
                    watcher.startWatcher();
                    while (Console.Read() != 'q')
                    {

                    }
                }
          }
    }
}
