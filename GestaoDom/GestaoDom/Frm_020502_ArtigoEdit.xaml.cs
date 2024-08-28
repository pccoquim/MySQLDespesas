/*
Frm_020502_ArtigoEdit.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_020502_ArtigoEdit.xaml
    /// </summary>
    public partial class Frm_020502_ArtigoEdit : Window
    {
        private string ArtCod = "", ArtDescr = "", ArtStatus = "", ArtGrpCod = "", ArtGrpDescr = "", ArtSFmCod = "", ArtSFmDescr = "", ArtFamCod = "", ArtFamDescr = "", ArtTercCod = "", ArtTercDescr = "", ArtUniCod = "", ArtUniDescr = "";
        private readonly string loginUserId, selectedArtCod, selectedArtGrpCod;
        private readonly int selectedArtId;

        public Frm_020502_ArtigoEdit(string loginUserId, string selectedArtCod, string selectedArtGrpCod, int selectedArtId)
        {
            InitializeComponent();

            this.loginUserId = loginUserId;
            this.selectedArtCod = selectedArtCod;
            this.selectedArtGrpCod = selectedArtGrpCod;
            this.selectedArtId = selectedArtId;
            LoadArtigos();
        }

        private void LoadArtigos()
        {
            // ComboBox status
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string queryStatus = "SELECT status_cod, status_descr FROM tbl_0001_status ORDER BY status_descr";
                    using (MySqlCommand cmdStatus = new MySqlCommand(queryStatus, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapterStatus = new MySqlDataAdapter(cmdStatus);
                        DataTable dtStatus = new DataTable();
                        adapterStatus.Fill(dtStatus);
                        // Limpa a combobox antes de adicionar os itens
                        cbx_Status.Items.Clear();
                        cbx_Status.ItemsSource = dtStatus.DefaultView;
                        cbx_Status.DisplayMemberPath = "status_descr";
                        cbx_Status.SelectedValuePath = "status_cod";
                        cbx_Status.SelectedIndex = -1;
                        cbx_Status.IsEditable = false;
                    }
                    // Ligação à base de dados
                    string query = "SELECT art_cod, art_descr, art_status, grp_cod, grp_descr, sfm_cod, sfm_descr, fam_codigo, fam_descr, terc_cod, terc_descr, uni_cod, uni_descr FROM tbl_0207_artigos " +
                        "LEFT JOIN tbl_0203_grupos ON tbl_0207_artigos.art_codgrupo = tbl_0203_grupos.grp_codigo " +
                        "LEFT JOIN tbl_0202_subfamilias ON tbl_0203_grupos.grp_codsubfam = tbl_0202_subfamilias.sfm_codigo " +
                        "LEFT JOIN tbl_0201_familias ON tbl_0202_subfamilias.sfm_codfam = tbl_0201_familias.fam_codigo " +
                        "LEFT JOIN tbl_0102_terceiros ON tbl_0207_artigos.art_codterc = tbl_0102_terceiros.terc_cod " +
                        "LEFT JOIN tbl_0204_unidades ON tbl_0207_artigos.art_coduni = tbl_0204_unidades.uni_cod " +
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
                                ArtStatus = reader["art_status"].ToString();
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
                cbx_Status.SelectedValue = ArtStatus;
            }
        }

        public bool ValidarArtigo(string artigo)
        {
            int existe = 0;
            bool valor;
            // obtem a ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // definição da consulta 
                    string query = "SELECT COUNT(*) FROM tbl_0207_artigos WHERE art_codgrupo = ? AND art_descr = ? AND art_id != ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@art_codgrp", selectedArtGrpCod);
                        cmd.Parameters.AddWithValue("@art_descr", artigo);
                        cmd.Parameters.AddWithValue("@art_id", selectedArtId);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception ex)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1018)!" + ex.Message);
                }

                if (existe > 0)
                {
                    valor = true;
                }
                else
                {
                    valor = false;
                }
                return valor;
            }
        }

        private bool ShowConfirmation(string message)
        {
            var confirmDialog = new Frm_0000_ConfirmDialog(message);
            confirmDialog.ShowDialog();
            return confirmDialog.Confirmed;
        }

        private void Txt_Descr_LostFocus(object sender, RoutedEventArgs e)
        {
            // Verificação de espaços em branco
            bool espacos = (txt_Descr.Text.Trim() != txt_Descr.Text);
            if (espacos)
            {
                System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim do nome, não são permitidos espaços em branco nestes locais!");
            }
        }

        private void Txt_Descr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Impedir que a tecla Enter seja inserida no campo atual
                e.Handled = true;

                // Mover o foco para o próximo campo de entrada
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                if (Keyboard.FocusedElement is UIElement element)
                    element.MoveFocus(request);
            }
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            // Verifica se a descrição está preenchida
            if (txt_Descr.Text != "")
            {
                // Verifica os espaços no inicio e fim do campo
                if (txt_Descr.Text.Trim() == txt_Descr.Text)
                {
                    // Verifica se já existe o número de conta
                    string artigo = txt_Descr.Text;
                    if (!ValidarArtigo(artigo))
                    {
                        if (txt_Descr.Text != ArtDescr || Convert.ToString(cbx_Status.SelectedValue) != ArtStatus)
                        {
                            // Obtem ligação
                            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                            {
                                // Controlo de erros
                                try
                                {
                                    conn.Open();
                                    // Definição do insert 
                                    string query = "UPDATE tbl_0207_artigos SET art_descr = ?, art_status = ?, art_userlastchg = ?, art_datelastchg = ?, art_timelastchg = ? WHERE art_id = ?";
                                    // Definição de query e ligação
                                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                    {
                                        // Atribuição de variaveis
                                        cmd.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                        cmd.Parameters.AddWithValue("@Status", cbx_Status.SelectedValue);
                                        cmd.Parameters.AddWithValue("@User", loginUserId);
                                        cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                        cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                        cmd.Parameters.AddWithValue("@id", selectedArtId);
                                        // execução do comando
                                        cmd.ExecuteNonQuery();
                                    }
                                    // Fecha o formulário                            
                                    this.Close();
                                    System.Windows.MessageBox.Show("Artigo alterado com exito!");

                                }
                                catch (Exception ex)
                                {
                                    // mensagem de erro da ligação
                                    System.Windows.MessageBox.Show("Ocorreu um erro ao ligar à base de dados (Erro: U1013)!" + ex.Message);
                                    return;
                                }
                            }
                        }
                        else
                        {
                            // Não existem alterações para gravar
                            if (ShowConfirmation("Não foram efetuadas alterações para guardar. Deseja sair das alterações?"))
                            {
                                this.Close();
                            }
                            else
                            {
                                return;
                            }
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Já existe um artigo com este nome, escolha uma novo nome!");
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("Verifique os espaços em branco no inicio ou fim do nome, não são permitidos espaços em branco nestes locais!");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("O campo: nome é de preenchimento obrigatório e não está preenchido!");
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
