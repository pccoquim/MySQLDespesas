/*
Frm_000101_ConfigConnection.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.1
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;


namespace GestaoDom
{
    /// <summary>
    /// Lógica interna para Frm_000101_ConfigConnection.xaml
    /// </summary>
    public partial class Frm_000101_ConfigConnection : Window
    {

        // Data/Hora
        private readonly string data = Cls_0002_ActualDateTime.Date, hora = Cls_0002_ActualDateTime.Time;
        // Base de daddos e utilizador
        private readonly string dataBaseName = "orcdom";
        private readonly string newUsername = "UserGestao12345678";
        private readonly string newPassword = "87654321oatseGresU";
        private bool dbExists = true;

        public Frm_000101_ConfigConnection()
        {
            InitializeComponent();
            Loaded += Frm_000101_ConfigConnection_Loaded;
        }

        private void Frm_000101_ConfigConnection_Loaded(object sender, RoutedEventArgs e)
        {
            LoadDataFromFile();
        }


        private void LoadDataFromFile()
        {
            // Define a pesquisa no arquivo de configuração
            string[] configLines = File.ReadAllLines("config.ini");
            // Define as variaveis sem valores
            string serverName = "";
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
                    string databaseName;

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
            Txt_Server.Text = serverName;
            Txt_User.Text = username;
            Pbx_Pw.Password = password;
        }

        private void Ligacao()
        {
            string server = Txt_Server.Text;
            string username = Txt_User.Text;
            string password = Pbx_Pw.Password;

            string connectionString = $"Server={server};Database={dataBaseName};User ID={username};Password={password};";
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string checkDatabaseQuery = $"SELECT COUNT(*) FROM information_schema.schemata WHERE schema_name = '{dataBaseName}'";
                    using (MySqlCommand cmd = new MySqlCommand(checkDatabaseQuery, conn))
                    {
                        int databaseCount = Convert.ToInt32(cmd.ExecuteScalar());

                        if (databaseCount > 0)
                        {
                            dbExists = true;
                        }
                        else
                        {
                            dbExists = false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro na ligação: " + ex.Message);
                }
            }
        }

        private void Txt_Server_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Txt_Server.Text != "" && Txt_User.Text != "" && Pbx_Pw.Password != "")
                {
                    Btn_Save_Click(sender, e);
                }
                else
                {
                    // Impedir que a tecla Enter seja inserida no campo atual
                    e.Handled = true;

                    // Mover o foco para o próximo campo de entrada
                    TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                    if (Keyboard.FocusedElement is UIElement element)
                        element.MoveFocus(request);
                }
            }
        }

        private void Txt_User_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Txt_Server.Text != "" && Txt_User.Text != "" && Pbx_Pw.Password != "")
                {
                    Btn_Save_Click(sender, e);
                }
                else
                {
                    // Impedir que a tecla Enter seja inserida no campo atual
                    e.Handled = true;

                    // Mover o foco para o próximo campo de entrada
                    TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                    if (Keyboard.FocusedElement is UIElement element)
                        element.MoveFocus(request);
                }
            }
        }

        private void Txt_Pw_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Txt_Server.Text != "" && Txt_User.Text != "" && Pbx_Pw.Password != "")
                {
                    Btn_Save_Click(sender, e);
                }
                else
                {
                    // Impedir que a tecla Enter seja inserida no campo atual
                    e.Handled = true;

                    // Mover o foco para o próximo campo de entrada
                    TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                    if (Keyboard.FocusedElement is UIElement element)
                        element.MoveFocus(request);
                }
            }
        }

        private void Pbx_Pw_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Txt_Server.Text != "" && Txt_User.Text != "" && Pbx_Pw.Password != "")
                {
                    Btn_Save_Click(sender, e);
                }
                else
                {
                    // Impedir que a tecla Enter seja inserida no campo atual
                    e.Handled = true;

                    // Mover o foco para o próximo campo de entrada
                    TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                    if (Keyboard.FocusedElement is UIElement element)
                        element.MoveFocus(request);
                }
            }
        }

        private void Btn_ShowPw_Click(object sender, RoutedEventArgs e)
        {
            if (Pbx_Pw.IsVisible)
            {
                Pbx_Pw.Visibility = Visibility.Hidden;
                Txt_Pw.Visibility = Visibility.Visible;
                Txt_Pw.Text = Pbx_Pw.Password;
                // Define a imagem do botão
                string imagePath = "/Imagens/Yes.png";
                Image image = new Image
                {
                    Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute))
                };
                // Define a imagem como conteudo do botão
                System.Windows.Controls.Button btn_ShowPw = (System.Windows.Controls.Button)sender;
                btn_ShowPw.Content = image;
            }
            else
            {
                Pbx_Pw.Visibility = Visibility.Visible;
                Txt_Pw.Visibility = Visibility.Hidden;
                Pbx_Pw.Password = Txt_Pw.Text;
                // Define a imagem do botão
                string imagePath = "/Imagens/No.png";
                Image image = new Image
                {
                    Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute))
                };
                // Define a imagem como conteudo do botão
                System.Windows.Controls.Button btn_ShowPw = (System.Windows.Controls.Button)sender;
                btn_ShowPw.Content = image;
            }
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            Ligacao();
            string caminhoArquivo = "config.ini";
            string novoNome = "config" + data + hora + ".ini";
            if (dbExists)
            {
                try
                {
                    // Verifica se o arquivo existe
                    if (File.Exists(caminhoArquivo))
                    {
                        // Extrai o diretório e o nome do arquivo
                        string diretorio = System.IO.Path.GetDirectoryName(caminhoArquivo);
                        string nomeArquivo = System.IO.Path.GetFileName(caminhoArquivo);

                        // Constrói o novo caminho com o novo nome do arquivo
                        string novoCaminho = System.IO.Path.Combine(diretorio, novoNome);

                        // Move o arquivo para o novo caminho
                        File.Move(caminhoArquivo, novoCaminho);

                        string server = Txt_Server.Text;
                        string username = Txt_User.Text;
                        string password = Txt_Pw.Text;
                        // Ficheiro de configuração
                        string configFile = "config.ini";
                        // Encriptar informações
                        string servidor = "Server=" + server;
                        string serv = Cls_0000_Cryptography.Encrypt(servidor);
                        string baseDados = "Database=" + dataBaseName;
                        string db = Cls_0000_Cryptography.Encrypt(baseDados);
                        string utilizador = "Username=" + newUsername;
                        string util = Cls_0000_Cryptography.Encrypt(utilizador);
                        string palavraPasse = "Password=" + newPassword;
                        string pw = Cls_0000_Cryptography.Encrypt(palavraPasse);

                        // Gravar as configurações no arquivo de configuração    
                        string configData = $"{serv}\n{db}\n{util}\n{pw}";
                        File.WriteAllText(configFile, configData);
                        //DialogResult = DialogResult.Ok;

                        System.Windows.MessageBox.Show("Alteração da configuração concluida com exito!");
                        System.Windows.Application.Current.Shutdown();
                    }
                    else
                    {
                        string server = Txt_Server.Text;
                        // Ficheiro de configuração
                        string configFile = "config.ini";
                        // Encriptar informações
                        string servidor = "Server=" + server;
                        string serv = Cls_0000_Cryptography.Encrypt(servidor);
                        string baseDados = "Database=" + dataBaseName;
                        string db = Cls_0000_Cryptography.Encrypt(baseDados);
                        string utilizador = "Username=" + newUsername;
                        string util = Cls_0000_Cryptography.Encrypt(utilizador);
                        string palavraPasse = "Password=" + newPassword;
                        string pw = Cls_0000_Cryptography.Encrypt(palavraPasse);

                        // Gravar as configurações no arquivo de configuração    
                        string configData = $"{serv}\n{db}\n{util}\n{pw}";
                        File.WriteAllText(configFile, configData);

                        System.Windows.MessageBox.Show("Configuração da localização da base de dados, concluida com exito!");
                        System.Windows.Application.Current.Shutdown();
                    }
                }
                catch (IOException)
                {
                    System.Windows.MessageBox.Show("Erro arquivo (X1001)!");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("A base de dados não existe (X1002)!");
            }
        }

        private void Btn_Exit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
