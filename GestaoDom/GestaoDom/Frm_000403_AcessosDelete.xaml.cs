/*
Frm_000403_AcessosDelete.xaml.cs
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
    /// Interaction logic for Frm_000403_AcessosDelete.xaml
    /// </summary>
    public partial class Frm_000403_AcessosDelete : Window
    {
        private readonly int accessId;
        private readonly string accessCod;
        private int level;
        private string cod, desig, status;

        public Frm_000403_AcessosDelete(int accessId, string accessCod)
        {
            InitializeComponent();
            this.accessId = accessId;
            this.accessCod = accessCod;
            LoadAcessos();
        }

        private void LoadAcessos()
        {
            // Dados dos acesso
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT opm_id, opm_cod, opm_descr, opm_nivel, opm_status, status_descr FROM tbl_0003_opcoesAcesso, tbl_0001_status WHERE opm_status = status_cod AND opm_id = ?";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@AcessosID", accessId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cod = reader["opm_cod"].ToString();
                                desig = reader["opm_descr"].ToString();
                                level = Convert.ToInt32(reader["opm_nivel"]);
                                status = reader["status_descr"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.Forms.MessageBox.Show("Error while loading users: " + ex.Message);
                }

                Txt_Cod.Text = cod;
                Txt_Designation.Text = desig;
                Txt_Level.Text = level.ToString();
                Txt_Status.Text = status;
            }
        }

        private bool ShowConfirmation(string message)
        {
            var confirmDialog = new Frm_0000_ConfirmDialog(message);
            confirmDialog.ShowDialog();
            return confirmDialog.Confirmed;
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            int used = 0;
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // Definição de procura de registos na tabela clientes
                    string query = "SELECT COUNT(*) FROM tbl_0004_acessos WHERE acs_cod = ?";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@acessocod", accessCod);
                        used = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    if (used > 0)
                    {
                        MessageBox.Show("Não é possível eliminar o acesso! Já foi atribuido!");
                        return;
                    }
                    else
                    {
                        if (ShowConfirmation("Tem a certeza, que deseja eliminar o acesso?"))
                        {
                            // Definição de procura de registos na tabela clientes
                            string queryDelete = "DELETE FROM tbl_0003_opcoesAcesso WHERE opm_id = ?";
                            // Define a consulta e a ligação
                            using (MySqlCommand cmdDelete = new MySqlCommand(queryDelete, conn))
                            {
                                cmdDelete.Parameters.AddWithValue("@opmID", accessId);
                                cmdDelete.ExecuteNonQuery();
                            }
                        }
                        MessageBox.Show("Acesso eliminado com sucesso!");
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
