/*
Frm_010103_TipoTerceiroDelete.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Windows;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_010103_TipoTerceiroDelete.xaml
    /// </summary>
    public partial class Frm_010103_TipoTerceiroDelete : Window
    {
        private readonly int selectedTipoTercId;
        private readonly string selectedTipoTercCod;
        private string cod, desig, status;

        public Frm_010103_TipoTerceiroDelete(int selectedTipoTercId, string selectedTipoTercCod)
        {
            InitializeComponent();
            this.selectedTipoTercId = selectedTipoTercId;
            this.selectedTipoTercCod = selectedTipoTercCod;
            LoadTipoTerceiros();
        }


        private void LoadTipoTerceiros()
        {
            // Dados do tipo de terceiro
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT tipoterc_id, tipoterc_cod, tipoterc_descr, tipoterc_status, status_descr FROM tbl_0101_tipoterceiro, tbl_0001_status WHERE tipoterc_status = status_cod AND tipoterc_id = ?";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@tipotercID", selectedTipoTercId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cod = reader["tipoterc_cod"].ToString();
                                desig = reader["tipoterc_descr"].ToString();
                                status = reader["status_descr"].ToString();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Erro ao carregar tipo de terceiros (Erro: C1007)!");
                }
                txt_Cod.Text = cod;
                txt_Descr.Text = desig;
                txt_Status.Text = status;
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
                    string query = "SELECT COUNT(*) FROM tbl_0102_terceiros WHERE terc_codtipo = ?";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tipotercID", selectedTipoTercCod);
                        used = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    if (used > 0)
                    {
                        MessageBox.Show("Não é possível eliminar o tipo de terceiro! Já foi atribuido a pelo menos um terceiro!");
                        return;
                    }
                    else
                    {
                        if (ShowConfirmation("Tem a certeza, que deseja eliminar o tipo de terceiro?"))
                        {
                            // Definição de procura de registos na tabela clientes
                            string queryDelete = "DELETE FROM tbl_0101_tipoterceiro WHERE tipoterc_id = ?";
                            // Define a consulta e a ligação
                            using (MySqlCommand cmdDelete = new MySqlCommand(queryDelete, conn))
                            {
                                cmdDelete.Parameters.AddWithValue("@tipotercID", selectedTipoTercId);
                                cmdDelete.ExecuteNonQuery();
                            }
                        }
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
