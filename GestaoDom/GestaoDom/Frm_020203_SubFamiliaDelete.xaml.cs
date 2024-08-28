/*
Frm_020203_SubFamiliaDelete.xaml.cs
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
    /// Interaction logic for Frm_020203_SubFamiliaDelete.xaml
    /// </summary>
    public partial class Frm_020203_SubFamiliaDelete : Window
    {
        private readonly string selectedSFCodFam, selectedSFCod;
        private readonly int selectedSFId;
        string SFCod = "", SFDescr = "", SFStatus = "", FCod = "", FDescr = "";
        public Frm_020203_SubFamiliaDelete(string selectedSFCodFam, string selectedSFCod, int selectedSFId)
        {
            InitializeComponent();

            this.selectedSFCodFam = selectedSFCodFam;
            this.selectedSFCod = selectedSFCod;
            this.selectedSFId = selectedSFId;
            LoadFamilias();
            LoadSubFamilias();
        }

        private void LoadFamilias()
        {
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros
                try
                {
                    conn.Open();
                    string query = "SELECT fam_id, fam_codigo, fam_descr FROM tbl_0201_familias WHERE fam_codigo = ?";

                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@fam_cod", selectedSFCodFam);
                        // Executa a consulta
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Lê os registos da tabela
                            if (reader.Read())
                            {
                                FCod = reader["fam_codigo"].ToString();
                                FDescr = reader["fam_descr"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao carregar dados (Erro C1030)!" + ex.Message);
                }
                txt_CodFam.Text = FCod;
                txt_DescrFam.Text = FDescr;
            }
        }

        private void LoadSubFamilias()
        {
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros
                try
                {
                    conn.Open();
                    string query = "SELECT sfm_id, sfm_codigo, sfm_descr, status_descr FROM tbl_0202_subfamilias, tbl_0001_status WHERE  sfm_status = status_cod AND sfm_id = ?";

                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@sfm_id", selectedSFId);
                        // Executa a consulta
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Lê os registos da tabela
                            if (reader.Read())
                            {
                                SFCod = reader["sfm_codigo"].ToString();
                                SFDescr = reader["sfm_descr"].ToString();
                                SFStatus = reader["status_descr"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao carregar dados (Erro C1031)!" + ex.Message);
                }
                txt_Cod.Text = SFCod;
                txt_Descr.Text = SFDescr;
                Txt_Status.Text = SFStatus;
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
            // Variavel para verificação de exisência de registos no terceiro
            int used = 0;
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo erros
                try
                {
                    conn.Open();
                    // Definição de procura de registos na tabela 
                    string query = "SELECT COUNT(*) FROM tbl_0203_grupos WHERE grp_codsubfam = ?";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Parametros para identificação do terceiro por código
                        cmd.Parameters.AddWithValue("@sfmCod", selectedSFCod);
                        // Executa a consulta
                        used = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    // Verifica que existem registos
                    if (used > 0)
                    {
                        // Existem registos, mensagem não pode ser anulado
                        System.Windows.MessageBox.Show("Não é possível eliminar a subfamilia! Já foi atribuido a pelo menos um registo!");
                        return;
                    }
                    // Verifica anão exstência de registos
                    else
                    {
                        // Mensagem para confirmação da eliminação
                        if (ShowConfirmation("Tem a certeza, que deseja eliminar a subfamilia?"))
                        {
                            // Definição de procura de registos na tabela terceiros
                            string queryDelete = "DELETE FROM tbl_0202_subfamilias WHERE sfm_id = ?";
                            // Define a consulta e a ligação
                            using (MySqlCommand cmdDelete = new MySqlCommand(queryDelete, conn))
                            {
                                // Parametros para identificação do terceiro por código
                                cmdDelete.Parameters.AddWithValue("@sfamID", selectedSFId);
                                // Executa a consulta
                                cmdDelete.ExecuteNonQuery();
                            }
                        }
                        System.Windows.MessageBox.Show("Subfamilia eliminado com sucesso!");
                        this.Close();
                    }
                }
                // Erro de ligação à base de dados
                catch (Exception ex)
                {
                    // Mensagem de erro de ligação à base de dados
                    System.Windows.MessageBox.Show("Erro ao ligar à base de dados (Erro: D1006)!" + ex.Message);
                }
                LoadSubFamilias();
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
