/*
Frm_000303_UserDelete.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.1
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Windows;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_000303_UserDelete.xaml
    /// </summary>
    public partial class Frm_000303_UserDelete : Window
    {
        private readonly int selectedUserId;
        string UserID = "", Nome = "", Tipo = "", Status = "";

        public Frm_000303_UserDelete(int selectedUserId)
        {
            InitializeComponent();
            this.selectedUserId = selectedUserId;
            LoadUser();
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
                                Status = reader["status_descr"].ToString();
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
                Txt_Type.Text = Tipo;
                Txt_Status.Text = Status;
            }
        }

        private bool ShowConfirmation(string message)
        {
            var confirmDialog = new Frm_0000_ConfirmDialog(message);
            confirmDialog.ShowDialog();
            return confirmDialog.Confirmed;
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            int used = 0;
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // Definição de procura de registos na tabela clientes
                    string query = "SELECT COUNT(*) FROM tbl_0002_users WHERE user_userlastchgpw <> '0' AND user_id = ?";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userID", selectedUserId);
                        used = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    if (used > 0)
                    {
                        MessageBox.Show("Não é possível eliminar o utilizador! Já foram efetuadas ações com este utilizador!");
                        return;
                    }
                    else
                    {
                        if (ShowConfirmation("Tem a certeza, que deseja eliminar o utilizador?"))
                        {
                            // Definição de procura de registos na tabela clientes
                            string queryDelete = "DELETE FROM tbl_0002_users WHERE user_id = ?";
                            // Define a consulta e a ligação
                            using (MySqlCommand cmdDelete = new MySqlCommand(queryDelete, conn))
                            {
                                cmdDelete.Parameters.AddWithValue("@userID", selectedUserId);
                                cmdDelete.ExecuteNonQuery();
                            }
                        }
                        MessageBox.Show("Utilizador eliminado com sucesso!");
                        this.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao ligar à base de dados: " + ex.Message);
                }
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
