/*
Frm_000301_UserAdd.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Data ultima alteração: 14.06.2024
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
    /// Interaction logic for Frm_000301_UserAdd.xaml
    /// </summary>
    public partial class Frm_000301_UserAdd : Window
    {
        private readonly string loginUserId;
        public Frm_000301_UserAdd(string loginUserId)
        {
            InitializeComponent();
            this.loginUserId = loginUserId;
            Loaded += Frm_000301_UsersAdd_Loaded;
        }

        private void Frm_000301_UsersAdd_Loaded(object sender, RoutedEventArgs e)
        {
            // Configurações da caixa de combinação tipo de utilizador
            Cbx_Type.Items.Clear();
            Cbx_Type.Items.Add("Administrador");
            Cbx_Type.Items.Add("Utilizador");
            Cbx_Type.SelectedIndex = 1;
        }

        private void Txt_UserId_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Txt_UserID.Text != "" && Txt_UserName.Text != "" && Pbx_Pw.Password != "" && Pbx_PwConf.Password != "")
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

        private void Txt_UserName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Txt_UserID.Text != "" && Txt_UserName.Text != "" && Pbx_Pw.Password != "" && Pbx_PwConf.Password != "")
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
                if (Txt_UserID.Text != "" && Txt_UserName.Text != "" && Pbx_Pw.Password != "" && Pbx_PwConf.Password != "")
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
                if (Txt_UserID.Text != "" && Txt_UserName.Text != "" && Pbx_Pw.Password != "" && Pbx_PwConf.Password != "")
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

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (Txt_UserID.Text != "" && Txt_UserName.Text != "" && Pbx_Pw.Password != "" && Pbx_PwConf.Password != "")
            {
                if (Txt_UserID.Text.Length >= 5)
                {
                    if (Pbx_Pw.Password.Length >= 8)
                    {
                        if (Pbx_Pw.Password == Pbx_PwConf.Password)
                        {
                            // encripta a palavra-passe 
                            string password = Pbx_Pw.Password;
                            string encryptedPassword = Cls_0000_Cryptography.Encrypt(password);
                            // obtem ligação
                            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                            {
                                try
                                {
                                    conn.Open();
                                    // insert do novo utilizador
                                    string query = "INSERT INTO tbl_0002_users(user_userID, user_name, user_password, user_type, user_status, user_chgpw,user_pwcount,user_usercreate, user_datecreate, user_timecreate, user_userlastchg, user_datelastchg, user_timelastchg) " +
                                        "                       VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                                    // definição de query e ligação
                                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                    {
                                        // Atribuição de variaveis com valores
                                        cmd.Parameters.AddWithValue("@UserID", Txt_UserID.Text);
                                        cmd.Parameters.AddWithValue("@Name", Txt_UserName.Text);
                                        cmd.Parameters.AddWithValue("@Pw", encryptedPassword);
                                        cmd.Parameters.AddWithValue("@Type", Cbx_Type.SelectedItem);
                                        cmd.Parameters.AddWithValue("@status", 1);
                                        cmd.Parameters.AddWithValue("@chgPw", 1);
                                        cmd.Parameters.AddWithValue("@pwCount", 0);
                                        cmd.Parameters.AddWithValue("@User", loginUserId);
                                        cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                        cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                        cmd.Parameters.AddWithValue("@UserLastChg", 0);
                                        cmd.Parameters.AddWithValue("@DataLastChg", 0);
                                        cmd.Parameters.AddWithValue("@TimeLastChg", 0);
                                        // execução do comando
                                        cmd.ExecuteNonQuery();
                                    }
                                    // Fecha o formulário                            
                                    this.Close();
                                    MessageBox.Show("Utilizador inserido com exito!");

                                }
                                catch (Exception ex)
                                {
                                    // mensagem de erro da ligação
                                    MessageBox.Show("Ocorreu um erro ao ligar à base de dados: " + ex.Message);
                                    return;
                                }
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
                        MessageBox.Show("A palavra-passe tem de ter no minimo 8 carateres");
                    }
                }
                else
                {
                    MessageBox.Show("O ID de utilizadaor tem de ter o minimo de 5 carateres!");
                }
            }
            else
            {
                MessageBox.Show("Os campos são todos de preenchimento obrigatório!");
            }
        }

        private void Pbx_Pw_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                if (Txt_UserID.Text != "" && Txt_UserName.Text != "" && Pbx_Pw.Password != "" && Pbx_PwConf.Password != "")
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
                if (Txt_UserID.Text != "" && Txt_UserName.Text != "" && Pbx_Pw.Password != "" && Pbx_PwConf.Password != "")
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

        private void Txt_UserName_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (Txt_UserName.Text.Trim() != Txt_UserName.Text);
            if (espacos)
            {
                MessageBox.Show("Verifique os espaços em branco no inicio ou fim do nome de utilizador, não são permitidos espaços em branco nestes locais!");
            }
        }

        private void Txt_UserID_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (Txt_UserName.Text.Trim() != Txt_UserName.Text || Txt_UserID.Text.Contains(" "));
            if (espacos)
            {
                MessageBox.Show("Verifique os espaços em branco! No ID de utilizador, não são permitidos espaços em branco!");
            }
            if (Txt_UserID.Text == "Master" || Txt_UserID.Text == "Admin")
            {
                MessageBox.Show("Não é possivel criar um utilizador com este nome. Nome reservado para o sistema!");
            }
            int userexists = 0;
            // obtem a ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();

                    // definição da consulta de validação de login
                    string query = "SELECT COUNT(*) FROM tbl_0002_users WHERE user_userID = ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userID", Txt_UserID.Text);
                        userexists = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    // mensagem de erro da ligação
                    MessageBox.Show("Ocorreu um erro a ligar à base de dados: " + ex.Message);
                    return;
                }
            }
            if (userexists > 0)
            {
                MessageBox.Show("Já existe o nome de utilizador ou um nome muito semelhante, o que pode colocar em risco a segurança. Por favor escolha outro ID de utilizador!");
            }
        }

        private void Cbx_Type_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
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
}
