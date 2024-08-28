/*
Frm_020403_UnidadeDelete.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Globalization;
using System.Windows;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_020403_UnidadeDelete.xaml
    /// </summary>
    public partial class Frm_020403_UnidadeDelete : Window
    {
        private readonly int selectedUnidadeId;
        private readonly string selectedUnidadeCod;
        string U_Cod = "", U_Descr = "", U_Peso = "", U_Volume = "", U_Status = "";

        public Frm_020403_UnidadeDelete(int selectedUnidadeId, string selectedUnidadeCod)
        {
            InitializeComponent();

            this.selectedUnidadeId = selectedUnidadeId;
            this.selectedUnidadeCod = selectedUnidadeCod;
            LoadUnidades();
        }

        private void LoadUnidades()
        {
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros
                try
                {
                    conn.Open();
                    // Definição de procura de registos na tabela 
                    string query = "SELECT uni_id, uni_cod, uni_descr, uni_peso, uni_volume, status_descr FROM tbl_0204_unidades, tbl_0001_status WHERE uni_status = status_cod AND uni_id = ?";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@UniID", selectedUnidadeId);
                        // Executa a consulta
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Lê os registos da tabela
                            if (reader.Read())
                            {
                                U_Cod = reader["uni_cod"].ToString();
                                U_Descr = reader["uni_descr"].ToString();
                                // Recupera os valores decimais
                                decimal peso = Convert.ToDecimal(reader["uni_peso"]);
                                decimal volume = Convert.ToDecimal(reader["uni_Volume"]);

                                // Define a cultura para usar o ponto como separador decimal
                                CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");

                                // Formata os valores decimais com o ponto como separador decimal
                                U_Peso = peso.ToString("0.0000", culture);
                                U_Volume = volume.ToString("0.0000", culture);
                                U_Status = reader["status_descr"].ToString();

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao carregar dados (Erro C1043)!" + ex.Message);
                }
                txt_Cod.Text = U_Cod;
                txt_Descr.Text = U_Descr;
                txt_Peso.Text = U_Peso;
                txt_Volume.Text = U_Volume;
                txt_Status.Text = U_Status;
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
                    string query = "SELECT COUNT(*) FROM tbl_0207_artigos WHERE art_coduni = ?";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Parametros para identificação por código
                        cmd.Parameters.AddWithValue("@uni_cod", selectedUnidadeCod);
                        // Executa a consulta
                        used = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    // Verifica que existem registos
                    if (used > 0)
                    {
                        // Existem registos, mensagem não pode ser anulado
                        System.Windows.MessageBox.Show("Não é possível eliminar a unidade! Já foi atribuido a pelo menos um registo!");
                        return;
                    }
                    // Verifica a não exstência de registos
                    else
                    {
                        // Mensagem para confirmação da eliminação
                        if (ShowConfirmation("Tem a certeza, que deseja eliminar a unidade?"))
                        {
                            // Definição de procura de registos na tabela terceiros
                            string queryDelete = "DELETE FROM tbl_0204_unidades WHERE uni_id = ?";
                            // Define a consulta e a ligação
                            using (MySqlCommand cmdDelete = new MySqlCommand(queryDelete, conn))
                            {
                                // Parametros para identificação do terceiro por código
                                cmdDelete.Parameters.AddWithValue("@uniID", selectedUnidadeId);
                                // Executa a consulta
                                cmdDelete.ExecuteNonQuery();
                            }
                            // Fecha o formulário                            
                            this.Close();
                            System.Windows.MessageBox.Show("Unidade eliminada com sucesso!");
                        }
                    }
                }
                // Erro de ligação à base de dados
                catch (Exception)
                {
                    // Mensagem de erro de ligação à base de dados
                    System.Windows.MessageBox.Show("Erro ao ligar à base de dados (Erro: D1007)!");
                }
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
