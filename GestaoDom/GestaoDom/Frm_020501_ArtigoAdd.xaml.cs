/*
Frm_020501_ArtigoAdd.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Data ultima alteração: 14.06.2024
Versão: 1.0.1
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_020501_ArtigoAdd.xaml
    /// </summary>
    public partial class Frm_020501_ArtigoAdd : Window
    {
        private readonly string loginUserId;
        private string artFamCod;
        private string artSFCod;
        private string artGrpCod;
        private string artTercCod;

        public Frm_020501_ArtigoAdd(string loginUserId, string artFamCod, string artSFCod, string artGrpCod, string artTercCod)
        {
            InitializeComponent();

            this.loginUserId = loginUserId;
            this.artFamCod = artFamCod;
            this.artSFCod = artSFCod;
            this.artGrpCod = artGrpCod;
            this.artTercCod = artTercCod;
            LoadCbxFamilia();
        }
        private void LoadCbxFamilia()
        {
            // ComboBox familia
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT fam_codigo, fam_descr FROM tbl_0201_familias ORDER BY fam_descr";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        //cbx_Familia.Items.Clear();

                        DataRow allRow = dt.NewRow();
                        allRow["fam_codigo"] = 00; // Defina um valor que represente "Todos"
                        allRow["fam_descr"] = "Todos"; // O texto a ser exibido na ComboBox
                        dt.Rows.InsertAt(allRow, 0);
                        // Limpa a combobox antes de adicionar os itens

                        cbx_Familia.ItemsSource = dt.DefaultView;
                        cbx_Familia.DisplayMemberPath = "fam_descr";
                        cbx_Familia.SelectedValuePath = "fam_codigo";
                        //cbx_Familia.SelectedIndex = 0;
                        cbx_Familia.SelectedValue = artFamCod;
                        cbx_Familia.IsEditable = false;
                        return;
                    }
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Familia (Erro: C1054)!");
                }
            }
        }

        private void LoadCbx_SubFamilia()
        {
            // ComboBox subfamilia
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT sfm_codigo, sfm_descr FROM tbl_0202_subfamilias WHERE sfm_codfam = ? ORDER BY sfm_descr";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@sfm_codfam", artFamCod);
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        DataRow allRow = dt.NewRow();
                        allRow["sfm_codigo"] = 000;
                        allRow["sfm_descr"] = "Todos"; // O texto a ser exibido na ComboBox
                        dt.Rows.InsertAt(allRow, 0);

                        cbx_SubFamilia.ItemsSource = dt.DefaultView;
                        cbx_SubFamilia.DisplayMemberPath = "sfm_descr";
                        cbx_SubFamilia.SelectedValuePath = "sfm_codigo";
                        cbx_SubFamilia.SelectedValue = artSFCod;
                        cbx_SubFamilia.IsEditable = false;
                        //Cls_0000_VariaveisGlobais.GrupoCodSF = cbx_SubFamilia.SelectedValue.ToString();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_SubFamilias (Erro: C1055)!" + ex.Message);
                }
            }
        }

        private void LoadCbx_Grupo()
        {
            // ComboBox grupos
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT grp_codigo, grp_descr FROM tbl_0203_grupos WHERE grp_codsubfam = ? ORDER BY grp_descr";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@grp_codsfm", artSFCod);
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        DataRow allRow = dt.NewRow();
                        allRow["grp_codigo"] = 000;
                        allRow["grp_descr"] = "Todos"; // O texto a ser exibido na ComboBox
                        dt.Rows.InsertAt(allRow, 0);

                        cbx_Grupo.ItemsSource = dt.DefaultView;
                        cbx_Grupo.DisplayMemberPath = "grp_descr";
                        cbx_Grupo.SelectedValuePath = "grp_codigo";
                        cbx_Grupo.SelectedValue = artGrpCod;
                        cbx_Grupo.IsEditable = false;
                        //Cls_0000_VariaveisGlobais.GrupoCodSF = cbx_SubFamilia.SelectedValue.ToString();
                        return;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Grupo (Erro: C1056)!" + ex.Message);
                }
            }
        }

        private void LoadCbx_Terceiro()
        {
            // ComboBox terceiros
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT terc_cod, terc_descr FROM tbl_0102_terceiros ORDER BY terc_descr";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        //cmd.Parameters.AddWithValue("@grp_codsfm", Cls_0000_VariaveisGlobais.ArtigoCodSFm);
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        DataRow allRow = dt.NewRow();
                        allRow["terc_cod"] = 000;
                        allRow["terc_descr"] = "Todos"; // O texto a ser exibido na ComboBox
                        dt.Rows.InsertAt(allRow, 0);

                        cbx_Terceiro.ItemsSource = dt.DefaultView;
                        cbx_Terceiro.DisplayMemberPath = "terc_descr";
                        cbx_Terceiro.SelectedValuePath = "terc_cod";
                        cbx_Terceiro.SelectedValue = artTercCod;
                        cbx_Terceiro.IsEditable = false;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Terceiro (Erro: C1057)!" + ex.Message);
                }
            }
        }

        private void LoadCbx_Unidade()
        {
            // ComboBox terceiros
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT uni_cod, uni_descr FROM tbl_0204_unidades ORDER BY uni_descr";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);

                        DataRow allRow = dt.NewRow();
                        allRow["uni_cod"] = 000;
                        allRow["uni_descr"] = "Todos"; // O texto a ser exibido na ComboBox
                        dt.Rows.InsertAt(allRow, 0);

                        cbx_Unidade.ItemsSource = dt.DefaultView;
                        cbx_Unidade.DisplayMemberPath = "uni_descr";
                        cbx_Unidade.SelectedValuePath = "uni_cod";
                        //cbx_Unidade.SelectedValue = Cls_0000_VariaveisGlobais.ArtigoCodUni;
                        cbx_Unidade.IsEditable = false;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Unidade (Erro: C1058)!" + ex.Message);
                }
            }
        }

        private void LoadNumeracao()
        {
            // variavel para numeração
            int ultimoId = 0;
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                //Contrlo de erros
                try
                {
                    conn.Open();
                    // Seleção do código mais elevado
                    string query = "SELECT MAX(art_cod) FROM tbl_0207_artigos WHERE art_codgrupo = ? AND art_codterc = ?";
                    // Execução da consulta
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@art_codgrp", artGrpCod);
                        cmd.Parameters.AddWithValue("@art_codterc", artTercCod);
                        // Obtenção do resultado: nulo, passa a um, valor soma um
                        object result = cmd.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            ultimoId = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Erro ao carregar númeração (Erro: N1009)!" + ex.Message);
                }
                txt_Cod.Text = string.Format("{0:000000}", ultimoId + 1);
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
                    string query = "SELECT COUNT(*) FROM tbl_0207_artigos WHERE art_codgrupo = ? AND art_codterc = ? AND art_descr = ?";
                    // obtem query e ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@art_codgrp", artGrpCod);
                        cmd.Parameters.AddWithValue("@art_codterc", artTercCod);
                        cmd.Parameters.AddWithValue("@art_descr", artigo);
                        existe = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                }
                catch (Exception)
                {
                    // mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Ocorreu um erro a ligar à base de dados (Erro: V1017)!");

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


        private void Cbx_Familia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbx_SubFamilia.IsEnabled = true;
            artFamCod = cbx_Familia.SelectedValue.ToString();
            LoadCbx_SubFamilia();
        }

        private void Cbx_SubFamilia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbx_Grupo.IsEnabled = true;
            artSFCod = cbx_SubFamilia.SelectedValue.ToString();
            LoadCbx_Grupo();
        }

        private void Cbx_Grupo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbx_Terceiro.IsEnabled = true;
            artGrpCod = cbx_Grupo.SelectedValue.ToString();
            LoadCbx_Terceiro();
        }


        private void Cbx_Terceiro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cbx_Unidade.IsEnabled = true;
            artTercCod = cbx_Terceiro.SelectedValue.ToString();
            LoadCbx_Unidade();
        }

        private void Cbx_Unidade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            txt_Descr.IsEnabled = true;
            LoadNumeracao();
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
                    // Verifica se já existe
                    string artigo = txt_Descr.Text;
                    if (!ValidarArtigo(artigo))
                    {
                        // Obtem ligação
                        using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                        {
                            // Controlo de erros
                            try
                            {
                                conn.Open();
                                // Definição do insert 
                                string query = "INSERT INTO tbl_0207_artigos(art_codgrupo, art_cod, art_codterc, art_coduni, art_codigo, art_descr, art_status, art_usercreate, art_datecreate, art_timecreate, art_userlastchg, art_datelastchg, art_timelastchg) " +
                                    "                       VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                                // Definição de query e ligação
                                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                                {
                                    // Atribuição de variaveis
                                    cmd.Parameters.AddWithValue("@CodGrp", cbx_Grupo.SelectedValue);
                                    cmd.Parameters.AddWithValue("@Cod", txt_Cod.Text);
                                    cmd.Parameters.AddWithValue("@CodTerc", cbx_Terceiro.SelectedValue);
                                    cmd.Parameters.AddWithValue("@CodUni", cbx_Unidade.SelectedValue);
                                    cmd.Parameters.AddWithValue("@Codigo", cbx_Grupo.SelectedValue + txt_Cod.Text + cbx_Terceiro.SelectedValue + cbx_Unidade.SelectedValue);
                                    cmd.Parameters.AddWithValue("@Descr", txt_Descr.Text);
                                    cmd.Parameters.AddWithValue("@status", 1);
                                    cmd.Parameters.AddWithValue("@User", loginUserId);
                                    cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                    cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                    cmd.Parameters.AddWithValue("@UserLastChg", 0);
                                    cmd.Parameters.AddWithValue("@DataLastChg", 0);
                                    cmd.Parameters.AddWithValue("@HoraLastChg", 0);
                                    // execução do comando
                                    cmd.ExecuteNonQuery();
                                }
                                // Fecha o formulário                            
                                this.Close();
                                System.Windows.MessageBox.Show("Artigo inserido com exito!");
                            }
                            catch (Exception ex)
                            {
                                // mensagem de erro da ligação
                                System.Windows.MessageBox.Show("Ocorreu um erro ao ligar à base de dados (Erro: I1010)!" + ex.Message);
                                return;
                            }
                        }
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Já existe um grupo com este nome, escolha uma novo nome!");
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
