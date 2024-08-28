/*
Frm_0002_Login.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.1
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_0002_Login.xaml
    /// </summary>
    public partial class Frm_0002_Login : Window
    {
        int pwerror = 0;
        public Frm_0002_Login()
        {
            InitializeComponent();
        }

        public event EventHandler<Cls_0002_LoginData> LoginCompleted;

        private void ActualizePwCount()
        {
            // obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // insert do novo utilizador
                    string query = "UPDATE tbl_0002_users SET user_pwcount = ? WHERE user_userID = ?";
                    // definição de query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@pwerror", pwerror);
                        cmd.Parameters.AddWithValue("@userID", Txt_User.Text);
                        // execução do comando
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // mensagem de erro da ligação
                    MessageBox.Show("Ocorreu um erro ao ligar à base de dados: " + ex.Message);
                    return;
                }
            }
        }
        private void Txt_User_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Txt_User.Text != "" && Pbx_Pw.Password != "")
                {
                    Btn_Login_Click(sender, e);
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

        private void Pbx_Pw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Txt_User.Text != "" && Pbx_Pw.Password != "")
                {
                    Btn_Login_Click(sender, e);
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
                Button btn_ShowPw = (Button)sender;
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
                Button btn_ShowPw = (Button)sender;
                btn_ShowPw.Content = image;
            }
        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            bool login = false;
            int userOrder = 0;
            string userId = "Não existe utilizador com login efetuado!";
            string userPw = "";
            string userType = "";
            string userStatus = "";
            bool userPwChg = false;
            string userLastChgPw = "";
            int userPwCount = 0;
            // verifica se os campos estão vazios
            if (Txt_User.Text != "" && Pbx_Pw.Password != "")
            {
                // verifica se o utilizador é o Master do sistema
                if (Txt_User.Text == "Master" && Pbx_Pw.Password == "moDoatseG")
                {
                    // guarda em variável o utilizador validado
                    login = true;
                    userOrder = 0;
                    userId = Txt_User.Text;
                    userType = "Administrador";
                    // MessageBox.Show("Login efetuado com sucesso!");
                    // fecha o formulário atual
                    // this.Close();
                }
                else
                {
                    // obtem a ligação
                    using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                    {
                        try
                        {
                            conn.Open();
                            // definição da consulta de validação de login
                            string query = "SELECT * FROM tbl_0002_users WHERE user_userID = ?";
                            // obtem query e ligação
                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                // Atribuição de variavel
                                cmd.Parameters.AddWithValue("@userID", Txt_User.Text);
                                // Executa o comando
                                using (MySqlDataReader reader = cmd.ExecuteReader())
                                {
                                    if (reader.Read())
                                    {
                                        userOrder = reader.GetInt32(reader.GetOrdinal("user_id"));
                                        userId = reader.GetString(reader.GetOrdinal("user_userID"));
                                        userPw = reader.GetString(reader.GetOrdinal("user_password"));
                                        userType = reader.GetString(reader.GetOrdinal("user_type"));
                                        userStatus = reader.GetString(reader.GetOrdinal("user_status"));
                                        userPwChg = reader.GetBoolean(reader.GetOrdinal("user_chgpw"));
                                        userPwCount = reader.GetInt32(reader.GetOrdinal("user_pwcount"));
                                        userLastChgPw = reader.GetString(reader.GetOrdinal("user_userlastchgpw"));
                                    }
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            // mensagem de erro da ligação
                            MessageBox.Show("Ocorreu um erro a ligar à base de dados: " + ex.Message);
                            return;
                        }
                    }
                    // Desencripta
                    string password = Pbx_Pw.Password;
                    string decryptedPassword = Cls_0000_Cryptography.Decrypt(userPw);
                    // valida a obtenção do uilizador na tabela - existe 1
                    if (userPw != "")
                    {
                        //string username 
                        string enteredUsername = Txt_User.Text;

                        // Verificar as maiúsculas
                        bool isUppercaseCorrect = (userId == enteredUsername);

                        // Verificar os espaços em branco no final
                        bool espacos = (enteredUsername.Trim() != enteredUsername);

                        if (isUppercaseCorrect && !espacos)
                        {
                            // Verifica se o utilizador está desativado
                            if (userStatus == "2")
                            {
                                MessageBox.Show("O utilizador" + Txt_User.Text + "está desativado!");
                            }
                            // Verifica se a password está bloqueada
                            if (userStatus == "1" && userPwChg == false && userPwCount > 3)
                            {
                                MessageBox.Show("O utilizador não pode iniciar sessão. A palavra-passe foi bloqueada por número de tentativas erradas! Contacte o Administrador");
                            }
                            // Verifica se a password é correta
                            if (decryptedPassword == password)
                            {
                                // Verifica se é a primeira utilização
                                if (userStatus == "1" && userPwChg == true && userPwCount <= 3 && userLastChgPw == "0")
                                {
                                    MessageBox.Show("Na primeira utilização a palavra-passe tem de ser alterada!");
                                    userId = Txt_User.Text;
                                    Frm_000304_SelfChgPw frm_000303_SelfChgPw = new Frm_000304_SelfChgPw(userOrder, userId)
                                    {
                                        Owner = this
                                    };
                                    frm_000303_SelfChgPw.ShowDialog();
                                    Pbx_Pw.Password = "";
                                }
                                // Verifica se a password foi reposta pelo administrador
                                if (userStatus == "1" && userPwChg == true && userPwCount <= 3 && userLastChgPw != "0")
                                {
                                    MessageBox.Show("A sua palavra-passe tem de ser alterada, porque foi reposta por um administrador!");
                                    userId = Txt_User.Text;
                                    Frm_000304_SelfChgPw frm_000303_SelfChgPw = new Frm_000304_SelfChgPw(userOrder, userId)
                                    {
                                        Owner = this
                                    };
                                    frm_000303_SelfChgPw.ShowDialog();
                                    Pbx_Pw.Password = "";
                                }
                                // Verifica se o utilizador está ativo, não necessita de alterar a password e não ultrapassou as tentativas de login errado
                                if (userStatus == "1" && userPwChg == false && userPwCount <= 3)
                                {
                                    // Atribui o valor de login
                                    userId = Txt_User.Text;
                                    login = true;
                                    // Elimina contagem de logins falhados
                                    pwerror = 0;
                                    ActualizePwCount();
                                    MessageBox.Show("Login efetuado com sucesso!");
                                    // Fecha o formulário atual
                                    // this.Close();
                                }
                            }
                            else
                            {
                                MessageBox.Show("Credenciais inválidas. A palavra-passe não corresponde ao utilizador!");
                                // Verificação de administrador. Não bloqueia
                                if (Txt_User.Text != "Admin")
                                {
                                    pwerror = userPwCount + 1;
                                    ActualizePwCount();
                                }
                                else
                                {
                                    pwerror = 0;
                                    ActualizePwCount();
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Credenciais inválidas. Verifique as maiúsculas e espaços em branco.");
                            return;
                        }
                    }
                    else
                    {
                        // mensagem para utilizador inexistente
                        MessageBox.Show("Credenciais inválidas. O utilizador não existe");
                        return;
                    }
                }
            }
            else
            {
                // mensagem para campos não preenchidos
                MessageBox.Show("O campo nome de utilizador ou palavra-passe, não está preenchido!");
            }

            // Cria uma instância de UserData
            Cls_0002_LoginData loginData = new Cls_0002_LoginData()
            {
                Login = login,
                LoginUserSequence = userOrder,
                LoginUserId = userId,
                LoginUserType = userType

            };

            // Dispara o evento de LoginCompleted
            LoginCompleted?.Invoke(this, loginData);

            // Fecha a janela "Login"
            this.Close();
        }

        private void Btn_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
