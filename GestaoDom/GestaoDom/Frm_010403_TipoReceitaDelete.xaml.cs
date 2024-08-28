/*
Frm_010403_TipoReceitaDelete.xaml.cs
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
    /// Interaction logic for Frm_010403_TipoReceitaDelete.xaml
    /// </summary>
    public partial class Frm_010403_TipoReceitaDelete : Window
    {
        private readonly int selectedTipoReceitaId;
        private readonly string selectedTipoReceitaCod;
        private string cod, desig, status;
        public Frm_010403_TipoReceitaDelete(int selectedTipoReceitaId, string selectedTipoReceitaCod)
        {
            InitializeComponent();

            this.selectedTipoReceitaId = selectedTipoReceitaId;
            this.selectedTipoReceitaCod = selectedTipoReceitaCod;
            LoadTipoReceitas();
        }

        private void LoadTipoReceitas()
        {
            // Dados dos tios de receita
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT tr_cod, tr_descr, status_descr FROM tbl_0104_tiporeceita, tbl_0001_status WHERE tr_status = status_cod AND tr_id = ?";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@trID", selectedTipoReceitaId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cod = reader["tr_cod"].ToString();
                                desig = reader["tr_descr"].ToString();
                                status = reader["status_descr"].ToString();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Erro ao carregar dados (Erro: C1019)!");
                }
                txt_Cod.Text = cod;
                txt_Descr.Text = desig;
                Txt_Status.Text = status;
            }
        }


        // Mensagem com resposta
        private bool ShowConfirmation(string message)
        {
            var confirmDialog = new Frm_0000_ConfirmDialog(message);
            confirmDialog.ShowDialog();
            return confirmDialog.Confirmed;
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            // Variavel para verificação de exisência de registos 
            int used = 0;
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo erros
                try
                {
                    conn.Open();
                    // Definição de procura de registos na tabela 
                    string query = "SELECT COUNT(*) FROM tbl_0402_movimentoscredito WHERE mc_codtiporeceita = ?";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Parametros para identificação do terceiro por código
                        cmd.Parameters.AddWithValue("@tipoReceitaCod", selectedTipoReceitaCod);
                        // Executa a consulta
                        used = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    // Verifica que existem registos
                    if (used > 0)
                    {
                        // Existem registos, mensagem não pode ser anulado
                        MessageBox.Show("Não é possível eliminar o tipo de receita! Já foi atribuida a pelo menos um registo!");
                        return;
                    }
                    // Verifica a não exstência de registos
                    else
                    {
                        // Mensagem para confirmação da eliminação
                        if (ShowConfirmation("Tem a certeza, que deseja eliminar o tipo de receita?"))
                        {
                            // Definição de procura de registos na tabela 
                            string queryDelete = "DELETE FROM tbl_0104_tiporeceita WHERE tr_id = ?";
                            // Define a consulta e a ligação
                            using (MySqlCommand cmdDelete = new MySqlCommand(queryDelete, conn))
                            {
                                // Parametros para identificação do registo
                                cmdDelete.Parameters.AddWithValue("@tiporeceitadID", selectedTipoReceitaId);
                                // Executa a consulta
                                cmdDelete.ExecuteNonQuery();
                            }
                        }
                    }
                }
                // Erro de ligação à base de dados
                catch (Exception)
                {
                    // Mensagem de erro de ligação à base de dados
                    MessageBox.Show("Erro ao ligar à base de dados (Erro: D1004)!");
                }
                // Mensagem de alteração concluida com exito
                System.Windows.MessageBox.Show("Tipo de receita eliminado com sucesso!");
                this.Close();
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
