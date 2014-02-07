using System;
using System.Configuration;

namespace SessionEr
{
    class Program
    {       
        static void Main(string[] args)
        {
            var usuario    = ConfigurationManager.AppSettings["usuario"];
            var intervalo  = Convert.ToInt32(ConfigurationManager.AppSettings["intervalo"]);
            var repeticoes = Convert.ToInt32(ConfigurationManager.AppSettings["repeticoes"]);
            var detalhado  = Convert.ToBoolean(ConfigurationManager.AppSettings["detalhado"]);

            new Monitorador().Monitorar(usuario, intervalo, repeticoes, detalhado);
        }        
    }
}
