/*
#################################################################
#   Projeto:        GestaoDom                                   #
#   Arquivo:        Cls_0001_DBConnection.cs                    #
#   Autor:          Paulo da Cruz Coquim                        #
#   Data:           07.06.2024                                  #
#   Data alteração: 07.06.2024                                  #
#   Versão:         1.0.1                                       #
#################################################################
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System.IO;

namespace GestaoDom
{
    public static class Cls_0001_DBConnection
    {
        // Nome do arquivo de configuração
        private static readonly string configFile = "config.ini";
        // Definição para a ligação
        public static MySqlConnection GetConnection()
        {
            // Define a pesquisa no arquivo de configuração
            string[] configLines = File.ReadAllLines(configFile);
            // Define as variaveis sem valores
            string serverName = "";
            string databaseName = "";
            string username = "";
            string password = "";
            // Percorre o conteudo do ficheiro linha a linha
            foreach (string encryptedLine in configLines)
            {
                // Desencripta a informação linha a linha
                string decryptedLine = Cls_0000_Cryptography.Decrypt(encryptedLine);
                // Define a divisão em dois da linha com o separador =
                string[] keyValue = decryptedLine.Split('=');
                // Verifica se a linha tem comprimento de 2 campos
                if (keyValue.Length == 2)
                {
                    // Define o valor constante como primeira posição
                    string key = keyValue[0].Trim();
                    // Define o valor da variável como segunda posição
                    string value = keyValue[1].Trim();
                    // Procura os valores constantes e atribui valor ás variáveis
                    if (key == "Server")
                        serverName = value;
                    else if (key == "Database")
                        databaseName = value;
                    else if (key == "Username")
                        username = value;
                    else if (key == "Password")
                        password = value;
                }
            }
            // Montar a string de ligação com base nas configurações lidas
            string connectionString = $"Server={serverName};Database={databaseName};User ID={username};Password={password};";
            // Criar e retornar a conexão MySql
            MySqlConnection conn = new MySqlConnection(connectionString);
            //conn.Open();
            // Retorna o valor
            return conn;
        }
    }
}
