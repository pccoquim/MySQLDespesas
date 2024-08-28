/*
Frm_010203_TerceiroDelete.xaml.cs
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
    /// Interaction logic for Frm_010203_TerceiroDelete.xaml
    /// </summary>
    public partial class Frm_010203_TerceiroDelete : Window
    {
        private readonly int selectedTerceiroId;
        private readonly string selectedTerceiroCod;
        private string cod, desig, tipo, morada1, morada2, cp, localidade, nif, tlf, email, status;

        public Frm_010203_TerceiroDelete(int selectedTerceiroId, string selectedTerceiroCod)
        {
            InitializeComponent();
            this.selectedTerceiroId = selectedTerceiroId;
            this.selectedTerceiroCod = selectedTerceiroCod;
            LoadTerceiros();
        }

        private void LoadTerceiros()
        {
            // Dados dos terceiros
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT terc_id, terc_cod, terc_descr, terc_codtipo, terc_morada1, terc_morada2, terc_cp, terc_localidade, terc_nif, terc_tlf, terc_email, terc_status, status_descr FROM tbl_0102_terceiros, tbl_0101_tipoterceiro, tbl_0001_status WHERE terc_codtipo = tipoterc_cod AND terc_status = status_cod AND terc_id = ?";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@tercID", selectedTerceiroId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cod = reader["terc_cod"].ToString();
                                desig = reader["terc_descr"].ToString();
                                tipo = reader["terc_codtipo"].ToString();
                                morada1 = reader["terc_morada1"].ToString();
                                morada2 = reader["terc_morada2"].ToString();
                                cp = reader["terc_cp"].ToString();
                                localidade = reader["terc_localidade"].ToString();
                                nif = reader["terc_nif"].ToString();
                                tlf = reader["terc_tlf"].ToString();
                                email = reader["terc_email"].ToString();
                                status = reader["status_descr"].ToString();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Erro ao carregar tipo de terceiros (Erro: C1010)!");
                }

                txt_Cod.Text = cod;
                txt_Descr.Text = desig;
                txt_Tipo.Text = tipo;
                txt_Morada1.Text = morada1;
                txt_Morada2.Text = morada2;
                txt_CP.Text = cp;
                txt_Localidade.Text = localidade;
                txt_NIF.Text = nif;
                txt_Tlf.Text = tlf;
                txt_Email.Text = email;
                txt_Status.Text = status;
            }
        }

        // Mensagem com resposta
        private bool ShowConfirmation(string message)
        {
            var confirmDialog = new Frm_0000_ConfirmDialog(message);
            confirmDialog.ShowDialog();
            return confirmDialog.Confirmed;
        }

        private void Btn_Eliminar_Click(object sender, RoutedEventArgs e)
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
                    string query = "SELECT COUNT(*) FROM tbl_0301_movimentosdebito WHERE fd_codterc = ?";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Parametros para identificação do terceiro por código
                        cmd.Parameters.AddWithValue("@tercCod", selectedTerceiroCod);
                        // Executa a consulta
                        used = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    // Verifica que existem registos
                    if (used > 0)
                    {
                        // Existem registos, mensagem não pode ser anulado
                        MessageBox.Show("Não é possível eliminar o terceiro! Já foi atribuido a pelo menos um registo!");
                        return;
                    }
                    // Verifica a não exstência de registos
                    else
                    {
                        // Mensagem para confirmação da eliminação
                        if (ShowConfirmation("Tem a certeza, que deseja eliminar o terceiro?"))
                        {
                            // Definição de procura de registos na tabela terceiros
                            string queryDelete = "DELETE FROM tbl_0102_terceiros WHERE terc_id = ?";
                            // Define a consulta e a ligação
                            using (MySqlCommand cmdDelete = new MySqlCommand(queryDelete, conn))
                            {
                                // Parametros para identificação do terceiro por código
                                cmdDelete.Parameters.AddWithValue("@tercID", selectedTerceiroId);
                                // Executa a consulta
                                cmdDelete.ExecuteNonQuery();
                            }
                        }
                        MessageBox.Show("Tipo de terceiro eliminado com sucesso!");
                        this.Close();
                    }
                }
                // Erro de ligação à base de dados
                catch (Exception ex)
                {
                    // Mensagem de erro de ligação à base de dados
                    MessageBox.Show("Erro ao ligar à base de dados (Erro: D1001)!" + ex.Message);
                }
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
