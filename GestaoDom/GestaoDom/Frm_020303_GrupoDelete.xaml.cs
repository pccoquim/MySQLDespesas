/*
Frm_020303_GrupoDelete.xaml.cs
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
    /// Interaction logic for Frm_020303_GrupoDelete.xaml
    /// </summary>
    public partial class Frm_020303_GrupoDelete : Window
    {
        private readonly string selectedGrupoCod;
        private readonly int selectedGrupoId;

        public Frm_020303_GrupoDelete(string selectedGrupoCod, int selectedGrupoId)
        {
            InitializeComponent();

            this.selectedGrupoCod = selectedGrupoCod;
            this.selectedGrupoId = selectedGrupoId;
            LoadGrupos();
        }

        private void LoadGrupos()
        {
            string GRPFamCod = "", GRPFamDescr = "", GRPSFCod = "", GRPSFDescr = "", GRPCod = "", GRPDescr = "", GRPStatus = "";
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros
                try
                {
                    conn.Open();
                    string query = "SELECT grp_cod, grp_descr, status_descr, sfm_cod, sfm_descr, fam_codigo, fam_descr FROM tbl_0203_grupos LEFT JOIN tbl_0001_status ON tbl_0203_grupos.grp_status = tbl_0001_status.status_cod LEFT JOIN tbl_0202_subfamilias ON tbl_0203_grupos.grp_codsubfam = tbl_0202_subfamilias.sfm_codigo" +
                        " LEFT JOIN tbl_0201_familias ON tbl_0202_subfamilias.sfm_codfam = tbl_0201_familias.fam_codigo WHERE grp_codigo = ?";

                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@grp_cod", selectedGrupoCod);
                        // Executa a consulta
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Lê os registos da tabela
                            if (reader.Read())
                            {
                                GRPCod = reader["grp_cod"].ToString();
                                GRPDescr = reader["grp_descr"].ToString();
                                GRPStatus = reader["status_descr"].ToString();
                                GRPSFCod = reader["sfm_cod"].ToString();
                                GRPSFDescr = reader["sfm_descr"].ToString();
                                GRPFamCod = reader["fam_codigo"].ToString();
                                GRPFamDescr = reader["fam_descr"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao carregar dados (Erro C1039)!" + ex.Message);
                }
                // Preenchimento dos campos
                txt_CodFam.Text = GRPFamCod;
                txt_DescrFam.Text = GRPFamDescr;
                txt_CodSubFam.Text = GRPSFCod;
                txt_DescrSubFam.Text = GRPSFDescr;
                txt_Cod.Text = GRPCod;
                txt_Descr.Text = GRPDescr;
                txt_Status.Text = GRPStatus;
            }
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
                    string query = "SELECT COUNT(*) FROM tbl_0207_artigos WHERE art_codgrupo = ?";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Parametros para identificação por código
                        cmd.Parameters.AddWithValue("@grp_cod", selectedGrupoCod);
                        // Executa a consulta
                        used = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    // Verifica que existem registos
                    if (used > 0)
                    {
                        // Existem registos, mensagem não pode ser anulado
                        System.Windows.MessageBox.Show("Não é possível eliminar o grupo! Já foi atribuido a pelo menos um registo!");
                        return;
                    }
                    // Verifica anão exstência de registos
                    else
                    {
                        // Mensagem para confirmação da eliminação
                        if (ShowConfirmation("Tem a certeza, que deseja eliminar o grupo?"))
                        {
                            // Definição de procura de registos na tabela terceiros
                            string queryDelete = "DELETE FROM tbl_0203_grupos WHERE grp_id = ?";
                            // Define a consulta e a ligação
                            using (MySqlCommand cmdDelete = new MySqlCommand(queryDelete, conn))
                            {
                                // Parametros para identificação do terceiro por código
                                cmdDelete.Parameters.AddWithValue("@GRPID", selectedGrupoId);
                                // Executa a consulta
                                cmdDelete.ExecuteNonQuery();
                            }
                            // Fecha o formulário                            
                            this.Close();
                            System.Windows.MessageBox.Show("Grupo eliminado com secesso!");
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

        private bool ShowConfirmation(string message)
        {
            var confirmDialog = new Frm_0000_ConfirmDialog(message);
            confirmDialog.ShowDialog();
            return confirmDialog.Confirmed;
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
