/*
Frm_010303_ContaDelete.xaml.cs
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
    /// Interaction logic for Frm_010303_ContaDelete.xaml
    /// </summary>
    public partial class Frm_010303_ContaDelete : Window
    {
        private readonly int selectedContaId;
        private readonly string selectedContaCod;
        private string cod, desig, nr, status;

        public Frm_010303_ContaDelete(int selectedContaId, string selectedContaCod)
        {
            InitializeComponent();
            this.selectedContaId = selectedContaId;
            this.selectedContaCod = selectedContaCod;
            LoadContas();
        }

        private void LoadContas()
        {
            // Dados da conta
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT cntcred_cod, cntcred_descr, cntcred_nr, cntcred_status, status_descr FROM tbl_0103_contascred, tbl_0001_status WHERE cntcred_status = status_cod AND cntcred_id = ?";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@contaID", selectedContaId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                cod = reader["cntcred_cod"].ToString();
                                desig = reader["cntcred_descr"].ToString();
                                nr = reader["cntcred_nr"].ToString();
                                status = reader["status_descr"].ToString();
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    System.Windows.Forms.MessageBox.Show("Erro ao carregar contas (Erro: C1014)!");
                }

                txt_Cod.Text = cod;
                txt_Descr.Text = desig;
                txt_Nr.Text = nr;
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
                    string query = "SELECT COUNT(*) FROM tbl_0301_movimentosdebito WHERE fd_conta = ?";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Parametros para identificação do terceiro por código
                        cmd.Parameters.AddWithValue("@contaCod", selectedContaCod);
                        // Executa a consulta
                        used = Convert.ToInt32(cmd.ExecuteScalar());
                    }

                    // Verifica que existem registos
                    if (used > 0)
                    {
                        // Existem registos, mensagem não pode ser anulado
                        MessageBox.Show("Não é possível eliminar a conta! Já foi atribuida a pelo menos um registo!");
                        return;
                    }
                    // Verifica a não exstência de registos
                    else
                    {
                        // Mensagem para confirmação da eliminação
                        if (ShowConfirmation("Tem a certeza, que deseja eliminar a conta?"))
                        {

                            // Definição de procura de registos na tabela 
                            string queryDelCntCred = "DELETE FROM tbl_0103_contascred WHERE cntcred_id = ?";
                            // Define a consulta e a ligação
                            using (MySqlCommand cmdDelCntCred = new MySqlCommand(queryDelCntCred, conn))
                            {
                                // Parametros para identificação do terceiro por código
                                cmdDelCntCred.Parameters.AddWithValue("@cntcredID", selectedContaId);
                                // Executa a consulta
                                cmdDelCntCred.ExecuteNonQuery();
                            }

                            // Definição de procura de registos na tabela 
                            string queryDelCntDeb = "DELETE FROM tbl_0103_contasdeb WHERE cntdeb_id = ?";
                            // Define a consulta e a ligação
                            using (MySqlCommand cmdDelCntDeb = new MySqlCommand(queryDelCntDeb, conn))
                            {
                                // Parametros para identificação do terceiro por código
                                cmdDelCntDeb.Parameters.AddWithValue("@cntdebID", selectedContaId);
                                // Executa a consulta
                                cmdDelCntDeb.ExecuteNonQuery();
                            }
                        }
                    }
                }
                // Erro de ligação à base de dados
                catch (Exception)
                {
                    // Mensagem de erro de ligação à base de dados
                    MessageBox.Show("Erro ao ligar à base de dados (Erro: D1003)!");
                }
                // Mensagem de alteração concluida com exito
                System.Windows.MessageBox.Show("Conta eliminada com sucesso!");
                this.Close();
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
