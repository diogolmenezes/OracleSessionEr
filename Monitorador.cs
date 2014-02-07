using Framework.Log;
using Oracle.DataAccess.Client;
using System.Configuration;
using System.Threading;

namespace SessionEr
{
    /// <summary>
    /// Monitora o numero de sessões do ORACLE
    /// </summary>
    public class Monitorador
    {
        public void Monitorar(string usuario, int intervalo, int vezes, bool detalhar = false)
        {
            LogHelper.Write(string.Format("Iniciando teste para o usuário {0} de {1} em {1} segundos por {2} vezes", usuario, intervalo, vezes));

            using (var conexao = new OracleConnection(ConfigurationManager.ConnectionStrings["Oracle"].ConnectionString))
            {
                for (int i = 1; i <= vezes; i++)
                {
                    try
                    {
                        conexao.Open();

                        if (detalhar)
                            Detalhado(conexao, usuario);
                        else
                            Simples(conexao, usuario);
                        
                    }
                    finally
                    {
                        conexao.Close();
                        Thread.Sleep(intervalo * 1000);
                    }
                }
            }   
        }

        private void Simples(OracleConnection conexao, string usuario)
        {
            var SQL = string.Format("select count(1) as total from v$session where username = '{0}'", usuario);
            using (var reader = new OracleCommand(SQL, conexao).ExecuteReader())
            {
                while (reader.Read())
                {
                    LogHelper.Write(reader["total"].ToString());
                }
            }
        }

        private void Detalhado(OracleConnection conexao, string usuario)
        {
            Simples(conexao, usuario);

            var SQL = string.Format("select count(1) as total, osuser, machine, program, state  from v$session where username = '{0}' group by osuser, machine, program, state", usuario);
            using (var reader = new OracleCommand(SQL, conexao).ExecuteReader())
            {
                while (reader.Read())
                {
                    LogHelper.Write(string.Format("TOTAL[{0}] PROGRAM[{1}] STATE[{2}], OSUSER[{3}], MACHINE[{4}]", reader["total"], reader["program"], reader["state"], reader["osuser"], reader["machine"]).ToString());
                }
            }
        }
    }
}
