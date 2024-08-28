/*
Frm_020503_ArtigoDelete.xaml.cs
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
    /// Interaction logic for Frm_020503_ArtigoDelete.xaml
    /// </summary>
    public partial class Frm_020503_ArtigoDelete : Window
    {
        string ArtCod = "", ArtDescr = "", ArtStatus = "", ArtGrpCod = "", ArtGrpDescr = "", ArtSFmCod = "", ArtSFmDescr = "", ArtFamCod = "", ArtFamDescr = "", ArtTercCod = "", ArtTercDescr = "", ArtUniCod = "", ArtUniDescr = "";
        private readonly int selectedArtId;
        private readonly string selectedArtCod;
        public Frm_020503_ArtigoDelete(int selectedArtId, string selectedArtCod)
        {
            InitializeComponent();

            this.selectedArtId = selectedArtId;
            this.selectedArtCod = selectedArtCod;
            LoadArtigos();
        }

        private void LoadArtigos()
        {
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros
                try
                {
                    conn.Open();
                    string query = "SELECT art_cod, art_descr, status_descr, grp_cod, grp_descr, sfm_cod, sfm_descr, fam_codigo, fam_descr, terc_cod, terc_descr, uni_cod, uni_descr FROM tbl_0207_artigos " +
                        "LEFT JOIN tbl_0203_grupos ON tbl_0207_artigos.art_codgrupo = tbl_0203_grupos.grp_codigo " +
                        "LEFT JOIN tbl_0202_subfamilias ON tbl_0203_grupos.grp_codsubfam = tbl_0202_subfamilias.sfm_codigo " +
                        "LEFT JOIN tbl_0201_familias ON tbl_0202_subfamilias.sfm_codfam = tbl_0201_familias.fam_codigo " +
                        "LEFT JOIN tbl_0102_terceiros ON tbl_0207_artigos.art_codterc = tbl_0102_terceiros.terc_cod " +
                        "LEFT JOIN tbl_0204_unidades ON tbl_0207_artigos.art_coduni = tbl_0204_unidades.uni_cod " +
                        "LEFT JOIN tbl_0001_status ON tbl_0207_artigos.art_status = tbl_0001_status.status_cod " +
                        "WHERE art_codigo = ?";

                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@art_cod", selectedArtCod);
                        // Executa a consulta
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Lê os registos da tabela
                            if (reader.Read())
                            {
                                ArtFamCod = reader["fam_codigo"].ToString();
                                ArtFamDescr = reader["fam_descr"].ToString();
                                ArtSFmCod = reader["sfm_cod"].ToString();
                                ArtSFmDescr = reader["sfm_descr"].ToString();
                                ArtGrpCod = reader["grp_cod"].ToString();
                                ArtGrpDescr = reader["grp_descr"].ToString();
                                ArtTercCod = reader["terc_cod"].ToString();
                                ArtTercDescr = reader["terc_descr"].ToString();
                                ArtUniCod = reader["uni_cod"].ToString();
                                ArtUniDescr = reader["uni_descr"].ToString();
                                ArtCod = reader["art_cod"].ToString();
                                ArtDescr = reader["art_descr"].ToString();
                                ArtStatus = reader["status_descr"].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao carregar dados (Erro C1060)!" + ex.Message);
                }
                // Preenchimento dos campos
                txt_CodFam.Text = ArtFamCod;
                txt_DescrFam.Text = ArtFamDescr;
                txt_CodSFm.Text = ArtSFmCod;
                txt_DescrSFm.Text = ArtSFmDescr;
                txt_CodGrp.Text = ArtGrpCod;
                txt_DescrGrp.Text = ArtGrpDescr;
                txt_CodTerc.Text = ArtTercCod;
                txt_DescrTerc.Text = ArtTercDescr;
                txt_CodUni.Text = ArtUniCod;
                txt_DescrUni.Text = ArtUniDescr;
                txt_Cod.Text = ArtCod;
                txt_Descr.Text = ArtDescr;
                txt_Status.Text = ArtStatus;
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
                    string query = "SELECT COUNT(*) FROM tbl_0302_movimentosdebito_det WHERE md_codartigo = ?";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Parametros para identificação por código
                        cmd.Parameters.AddWithValue("@Art_cod", selectedArtCod);
                        // Executa a consulta
                        used = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    // Verifica que existem registos
                    if (used > 0)
                    {
                        // Existem registos, mensagem não pode ser anulado
                        System.Windows.MessageBox.Show("Não é possível eliminar o artigo! Já foi atribuido a pelo menos um registo!");
                        return;
                    }
                    // Verifica a não exstência de registos
                    else
                    {
                        // Mensagem para confirmação da eliminação
                        if (ShowConfirmation("Tem a certeza, que deseja eliminar o artigo?"))
                        {
                            // Definição de procura de registos na tabela terceiros
                            string queryDelete = "DELETE FROM tbl_0207_artigos WHERE art_id = ?";
                            // Define a consulta e a ligação
                            using (MySqlCommand cmdDelete = new MySqlCommand(queryDelete, conn))
                            {
                                // Parametros para identificação do terceiro por código
                                cmdDelete.Parameters.AddWithValue("@ArtID", selectedArtId);
                                // Executa a consulta
                                cmdDelete.ExecuteNonQuery();
                            }
                            // Fecha o formulário                            
                            this.Close();
                            System.Windows.MessageBox.Show("Artigo eliminado com sucesso!");
                        }
                    }
                }
                // Erro de ligação à base de dados
                catch (Exception)
                {
                    // Mensagem de erro de ligação à base de dados
                    System.Windows.MessageBox.Show("Erro ao ligar à base de dados (Erro: D1010)!");
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
