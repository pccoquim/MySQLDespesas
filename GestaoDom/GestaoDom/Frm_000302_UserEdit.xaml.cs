/*
Frm_000302_UserEdit.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.1
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_000302_UserEdit.xaml
    /// </summary>
    public partial class Frm_000302_UserEdit : Window
    {
        private readonly int selectedUserId;
        private readonly string loginUserId;
        // int ID = 0;
        string UserID = "", Nome = "", Tipo = "", Status = "";
        public Frm_000302_UserEdit(int selectedUserId, string loginUserId)
        {
            InitializeComponent();
            this.selectedUserId = selectedUserId;
            this.loginUserId = loginUserId;
            LoadCbxType();
            LoadUser();
        }

        private void Txt_UserName_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (Txt_UserName.Text.Trim() != Txt_UserName.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim do nome de utilizador, não são permitidos espaços em branco nestes locais!");
            }
        }

        private void LoadCbxType()
        {
            // Configurações da caixa de combinação tipo de utilizador
            Cbx_Type.Items.Clear();
            Cbx_Type.Items.Add("Administrador");
            Cbx_Type.Items.Add("Utilizador");

            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // ComboBox status
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT status_cod, status_descr FROM tbl_0001_status ORDER BY status_descr";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        // Limpa a combobox antes de adicionar os itens
                        Cbx_Status.Items.Clear();
                        Cbx_Status.ItemsSource = dt.DefaultView;
                        Cbx_Status.DisplayMemberPath = "status_descr";
                        Cbx_Status.SelectedValuePath = "status_cod";
                        Cbx_Status.SelectedIndex = -1;
                        Cbx_Status.IsEditable = false;
                    }
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx!");
                }
            }
        }

        private void LoadUser()
        {
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT user_id, user_userID, user_name, user_type, user_status, user_chgpw, user_pwcount, status_descr FROM tbl_0002_users, tbl_0001_status WHERE user_status = status_cod AND user_id = ?";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@userID", selectedUserId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                UserID = reader["user_userID"].ToString();
                                Nome = reader["user_name"].ToString();
                                Tipo = reader["user_type"].ToString();
                                Status = reader["user_status"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error while loading users: " + ex.Message);
                }
                Txt_UserID.Text = UserID;
                Txt_UserName.Text = Nome;
                Cbx_Type.SelectedItem = Tipo;
                Cbx_Status.SelectedValue = Status;
            }
        }

        private void Txt_UserName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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

        private void Cbx_Type_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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

        private void Cbx_Status_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
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

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            if (Txt_UserName.Text != "")
            {
                if (Txt_UserName.Text != Nome || Convert.ToString(Cbx_Type.SelectedItem) != Tipo || Convert.ToString(Cbx_Status.SelectedValue) != Status)
                {
                    // obtem ligação
                    using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                    {
                        try
                        {
                            conn.Open();
                            // update do utilizador
                            string query = "UPDATE tbl_0002_users SET user_name = ?, user_type = ?, user_status = ?, user_userlastchg =  ?, user_datelastchg = ?, user_timelastchg = ? WHERE user_id = ?";
                            // definição de query e ligação
                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                // Atribuição de variaveis com valores
                                cmd.Parameters.AddWithValue("@UserName", Txt_UserID.Text);
                                cmd.Parameters.AddWithValue("@Type", Cbx_Type.SelectedItem);
                                cmd.Parameters.AddWithValue("@Status", Cbx_Status.SelectedValue);
                                cmd.Parameters.AddWithValue("@User", loginUserId);
                                cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                // Variácel para identificação do registo
                                cmd.Parameters.AddWithValue("@userID", selectedUserId);
                                // execução do comando
                                cmd.ExecuteNonQuery();
                            }
                            // Mensagem de execução bem sucedida
                            System.Windows.MessageBox.Show("Utilizador alterado com sucesso!");
                            // Fecha o formulário após gravar
                            this.Close();
                        }
                        catch (Exception ex)
                        {
                            // mensagem de erro da ligação
                            System.Windows.MessageBox.Show("Ocorreu um erro ao ligar à base de dados: " + ex.Message);
                            return;
                        }
                    }
                }
                else
                {
                    if (ShowConfirmation("Não foram efetuadas alterações para gravar! Pretende sair?"))
                    {
                        this.Close();
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("O nome do utilizador é de preenchimento obrigatório!");
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private bool ShowConfirmation(string message)
        {
            var confirmDialog = new Frm_0000_ConfirmDialog(message);
            confirmDialog.ShowDialog();
            return confirmDialog.Confirmed;
        }
    }
}
