/*
Frm_000304_SelfChgPw.xaml.cs
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
    /// Interaction logic for Frm_000304_SelfChgPw.xaml
    /// </summary>
    public partial class Frm_000304_SelfChgPw : Window
    {
        private readonly string loginUserId;
        private readonly int loginUserSequence;

        public Frm_000304_SelfChgPw(int loginUserSequence, string loginUserId)
        {
            InitializeComponent();
            this.loginUserId = loginUserId;
            this.loginUserSequence = loginUserSequence;
        }

        private void Btn_ShowOldPw_Click(object sender, RoutedEventArgs e)
        {
            if (Pbx_OldPw.IsVisible)
            {
                Pbx_OldPw.Visibility = Visibility.Hidden;
                Txt_OldPw.Visibility = Visibility.Visible;
                Txt_OldPw.Text = Pbx_OldPw.Password;
                // Define a imagem do botão
                string imagePath = "/Imagens/Yes.png";
                Image image = new Image
                {
                    Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute))
                };
                // Define a imagem como conteudo do botão
                Button btn_ShowOldPw = (Button)sender;
                btn_ShowOldPw.Content = image;
            }
            else
            {
                Pbx_OldPw.Visibility = Visibility.Visible;
                Txt_OldPw.Visibility = Visibility.Hidden;
                Pbx_OldPw.Password = Txt_OldPw.Text;
                // Define a imagem do botão
                string imagePath = "/Imagens/No.png";
                Image image = new Image
                {
                    Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute))
                };
                // Define a imagem como conteudo do botão
                Button btn_ShowOldPw = (Button)sender;
                btn_ShowOldPw.Content = image;
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

        private void Btn_ShowPwConf_Click(object sender, RoutedEventArgs e)
        {
            if (Pbx_PwConf.IsVisible)
            {
                Pbx_PwConf.Visibility = Visibility.Hidden;
                Txt_PwConf.Visibility = Visibility.Visible;
                Txt_PwConf.Text = Pbx_PwConf.Password;
                // Define a imagem do botão
                string imagePath = "/Imagens/Yes.png";
                Image image = new Image
                {
                    Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute))
                };
                // Define a imagem como conteudo do botão
                Button btn_ShowPwConf = (Button)sender;
                btn_ShowPwConf.Content = image;
            }
            else
            {
                Pbx_PwConf.Visibility = Visibility.Visible;
                Txt_PwConf.Visibility = Visibility.Hidden;
                Pbx_PwConf.Password = Txt_PwConf.Text;
                // Define a imagem do botão
                string imagePath = "/Imagens/No.png";
                Image image = new Image
                {
                    Source = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute))
                };
                // Define a imagem como conteudo do botão
                Button btn_ShowPwConf = (Button)sender;
                btn_ShowPwConf.Content = image;
            }
        }

        private void Pbx_OldPw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Pbx_OldPw.Password != "" && Pbx_Pw.Password != "" && Pbx_PwConf.Password != "")
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

        private void Txt_OldPw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Pbx_OldPw.Password != "" && Pbx_Pw.Password != "" && Pbx_PwConf.Password != "")
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

        private void Pbx_Pw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Pbx_OldPw.Password != "" && Pbx_Pw.Password != "" && Pbx_PwConf.Password != "")
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

        private void Txt_Pw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Pbx_OldPw.Password != "" && Pbx_Pw.Password != "" && Pbx_PwConf.Password != "")
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

        private void Pbx_PwConf_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Pbx_OldPw.Password != "" && Pbx_Pw.Password != "" && Pbx_PwConf.Password != "")
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

        private void Txt_PwConf_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Pbx_OldPw.Password != "" && Pbx_Pw.Password != "" && Pbx_PwConf.Password != "")
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

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            string userpw = "";
            if (Pbx_OldPw.Password != "")
            {
                // obtem a ligação
                using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                {
                    try
                    {
                        conn.Open();
                        // definição da consulta de validação de login
                        string query = "SELECT * FROM tbl_0002_users WHERE user_id = ?";
                        // obtem query e ligação
                        using (MySqlCommand command = new MySqlCommand(query, conn))
                        {
                            // Atribuição de variavel
                            command.Parameters.AddWithValue("@userID", loginUserSequence);
                            // Executa o comando
                            using (MySqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    userpw = reader.GetString(reader.GetOrdinal("user_password"));
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
                string password = Pbx_OldPw.Password;
                string decryptedPassword = Cls_0000_Cryptography.Decrypt(userpw);
                if (decryptedPassword == password)
                {
                    if (Pbx_Pw.Password.Length >= 8)
                    {
                        if (Pbx_Pw.Password == Pbx_PwConf.Password)
                        {
                            if (decryptedPassword != Pbx_Pw.Password)
                            {
                                string encryptedPassword = Cls_0000_Cryptography.Encrypt(Pbx_Pw.Password);
                                // obtem ligação
                                using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                                {
                                    try
                                    {
                                        conn.Open();
                                        // insert do novo utilizador
                                        string query = "UPDATE tbl_0002_users SET user_password = ?, user_chgpw = '0', user_userlastchgpw =  ?, user_datelastchgpw = ?, user_timelastchgpw = ? WHERE user_ID = ?";
                                        // definição de query e ligação
                                        using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                        {
                                            // Atribuição de variaveis com valores
                                            cmd.Parameters.AddWithValue("@Pw", encryptedPassword);
                                            cmd.Parameters.AddWithValue("@User", loginUserId);
                                            cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                            cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                            // Variavel para identificação do registo
                                            cmd.Parameters.AddWithValue("@userID", loginUserSequence);
                                            // execução do comando
                                            cmd.ExecuteNonQuery();
                                        }
                                        MessageBox.Show("A palavra-passe alterada com sucesso!");

                                    }
                                    catch (Exception ex)
                                    {
                                        // mensagem de erro da ligação
                                        MessageBox.Show("Ocorreu um erro ao ligar à base de dados: " + ex.Message);
                                        return;
                                    }
                                    // Fecha o formulário atual
                                    this.Close();
                                }
                            }
                            else
                            {
                                MessageBox.Show("A nova palavra-passe não pode ser igual à atual!");
                            }
                        }
                        else
                        {
                            // mensagem de verificação da palavra-passe diferente
                            MessageBox.Show("A palavra-passe e a palavra-passe de verificação não condizem!");
                        }
                    }
                    else
                    {
                        MessageBox.Show("A nova palavra-passe tem de ter comprimento igual ou superior a oito carateres!");
                    }
                }
                else
                {
                    MessageBox.Show("A palavra-passe atual está incorreta!");
                }
            }
            else
            {
                MessageBox.Show("O campo palavra-passe atual, não está preenchido. Este campo é de preenchimento obrigatório!");
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
