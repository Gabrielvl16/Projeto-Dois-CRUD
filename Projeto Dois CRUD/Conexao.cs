using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace SeuProjeto
{
    public class Conexao
    {
        private static string connString = "server=localhost;user=root;database=bebidas;password=;";
        public static MySqlConnection ObterConexao()
        {
            MySqlConnection conexao = new MySqlConnection(connString);
            conexao.Open();
            return conexao;
        }
    }
}