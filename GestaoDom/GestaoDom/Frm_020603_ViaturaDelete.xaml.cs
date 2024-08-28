/*
Frm_020503_ViaturaDelete.xaml.cs
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
    /// Interaction logic for Frm_020503_ViaturaDelete.xaml
    /// </summary>
    public partial class Frm_020603_ViaturaDelete : Window
    {
        private readonly int selectedViaturaId;
        private readonly string selectedViaturaCod;
        string Cod = "", Descr = "", Matricula = "", Status = "";


        public Frm_020603_ViaturaDelete(int selectedViaturaId, string selectedViaturaCod)
        {
            InitializeComponent();

            this.selectedViaturaId = selectedViaturaId;
            this.selectedViaturaCod = selectedViaturaCod;
            LoadViatura();
        }

        private void LoadViatura()
        {
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros
                try
                {
                    conn.Open();
                    // Definição de procura de registos na tabela 
                    string query = "SELECT vtr_id, vtr_cod, vtr_descr, vtr_matricula, status_descr FROM tbl_0205_viaturas, tbl_0001_status WHERE vtr_status = status_cod AND vtr_id = ?";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@vtrID", selectedViaturaId);
                        // Executa a consulta
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Lê os registos da tabela
                            if (reader.Read())
                            {
                                Cod = reader["vtr_cod"].ToString();
                                Descr = reader["vtr_descr"].ToString();
                                Matricula = reader["vtr_matricula"].ToString();
                                Status = reader["status_descr"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao carregar dados (Erro C1043)!" + ex.Message);
                }
                txt_Cod.Text = Cod;
                txt_Descr.Text = Descr;
                txt_Matricula.Text = Matricula;
                txt_Status.Text = Status;
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
                    string query = "SELECT COUNT(*) FROM tbl_0302_movimentosdebito_det WHERE md_codviatura = ?";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Parametros para identificação por código
                        cmd.Parameters.AddWithValue("@vtr_cod", selectedViaturaCod);
                        // Executa a consulta
                        used = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    // Verifica que existem registos
                    if (used > 0)
                    {
                        // Existem registos, mensagem não pode ser anulado
                        System.Windows.MessageBox.Show("Não é possível eliminar a viatura! Já foi atribuido a pelo menos um registo!");
                        return;
                    }
                    // Verifica anão exstência de registos
                    else
                    {
                        // Mensagem para confirmação da eliminação
                        if (ShowConfirmation("Tem a certeza, que deseja eliminar a viatura?"))
                        {
                            // Definição de procura de registos na tabela terceiros
                            string queryDelete = "DELETE FROM tbl_0205_viaturas WHERE vtr_id = ?";
                            // Define a consulta e a ligação
                            using (MySqlCommand cmdDelete = new MySqlCommand(queryDelete, conn))
                            {
                                // Parametros para identificação do terceiro por código
                                cmdDelete.Parameters.AddWithValue("@vtrID", selectedViaturaId);
                                // Executa a consulta
                                cmdDelete.ExecuteNonQuery();
                            }
                            // Fecha o formulário                            
                            this.Close();
                            System.Windows.MessageBox.Show("Viatura eliminada com sucesso!");
                        }
                    }
                }
                // Erro de ligação à base de dados
                catch (Exception)
                {
                    // Mensagem de erro de ligação à base de dados
                    System.Windows.MessageBox.Show("Erro ao ligar à base de dados (Erro: D1009)!");
                }
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
