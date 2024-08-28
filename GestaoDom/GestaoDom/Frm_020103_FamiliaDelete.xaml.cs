/*
Frm_020103_FamiliaDelete.xaml.cs
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
    /// Interaction logic for Frm_020103_FamiliaDelete.xaml
    /// </summary>
    public partial class Frm_020103_FamiliaDelete : Window
    {
        private readonly int selectedFamId;
        private readonly string selectedFamCod;
        string cod = "", desig = "", status = "";
        public Frm_020103_FamiliaDelete(int selectedFamId, string selectedFamCod)
        {
            InitializeComponent();

            this.selectedFamId = selectedFamId;
            this.selectedFamCod = selectedFamCod;
            LoadFamilia();
        }

        private void LoadFamilia()
        {
            // Dados da familia
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT fam_codigo, fam_descr, status_descr FROM tbl_0201_familias, tbl_0001_status WHERE fam_status = status_cod AND fam_id = ?";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@famID", selectedFamId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cod = reader["fam_codigo"].ToString();
                                desig = reader["fam_descr"].ToString();
                                status = reader["status_descr"].ToString();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Erro ao carregar dados (Erro: C1023)!");
                }

                txt_Cod.Text = cod;
                txt_Descr.Text = desig;
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
            // Variavel para verificação de exisência de registos no terceiro
            int used = 0;
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo erros
                try
                {
                    conn.Open();
                    // Definição de procura de registos na tabela tbl_0301_movimentosdebito
                    string query = "SELECT COUNT(*) FROM tbl_0202_subfamilias WHERE sfm_codfam = ?";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Parametros para identificação do terceiro por código
                        cmd.Parameters.AddWithValue("@famCod", selectedFamCod);
                        // Executa a consulta
                        used = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    // Verifica que existem registos
                    if (used > 0)
                    {
                        // Existem registos, mensagem não pode ser anulado
                        System.Windows.MessageBox.Show("Não é possível eliminar a familia! Já foi atribuido a pelo menos um registo!");
                        return;
                    }
                    // Verifica anão exstência de registos
                    else
                    {
                        // Mensagem para confirmação da eliminação
                        if (ShowConfirmation("Tem a certeza, que deseja eliminar a familia?"))
                        {
                            // Definição de procura de registos na tabela terceiros
                            string queryDelete = "DELETE FROM tbl_0201_familias WHERE fam_id = ?";
                            // Define a consulta e a ligação
                            using (MySqlCommand cmdDelete = new MySqlCommand(queryDelete, conn))
                            {
                                // Parametros para identificação do terceiro por código
                                cmdDelete.Parameters.AddWithValue("@famID", selectedFamId);
                                // Executa a consulta
                                cmdDelete.ExecuteNonQuery();
                            }
                        }
                        MessageBox.Show("Familia eliminada com sucesso!");
                        this.Close();
                    }
                }
                // Erro de ligação à base de dados
                catch (Exception)
                {
                    // Mensagem de erro de ligação à base de dados
                    System.Windows.MessageBox.Show("Erro ao ligar à base de dados (Erro: D1005)!");
                }
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
