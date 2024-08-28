/*
Frm_0205_ArtigosManut.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_0205_ArtigosManut.xaml
    /// </summary>
    public partial class Frm_0205_ArtigosManut : Window
    {
        private readonly int loginUserSequence;
        private readonly string loginUserId, loginUserType;
        private static readonly Cls_0003_Users user = new Cls_0003_Users();
        private string artFamCod, artSFCod, artGrpCod, artTercCod;
        private int selectedArtId, selectedArtIndex;
        private string selectedArtCod, selectedArtGrpCod;
        private string ArtCodConsulta = "";
        int Consulta = 0;
        public Frm_0205_ArtigosManut(int loginUserSequence, string loginUserId, string loginUserType)
        {
            InitializeComponent();
            this.loginUserSequence = loginUserSequence;
            this.loginUserId = loginUserId;
            this.loginUserType = loginUserType;
            LoadAccess();
            Loadcbx_Familia();
        }

        private void LoadAccess()
        {
            user.MenuAccess = Cls_0005_AccessControl.GetAccess(Convert.ToInt32(loginUserSequence));
            if (loginUserType == "Utilizador")
            {
                // Acesso a adicionar
                if (Cls_0005_AccessControl.AccessGranted("M20M010501", user.MenuAccess))
                {
                    btn_ArtAdd.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_ArtAdd.Visibility = Visibility.Collapsed;
                }
                // Acesso a alterar
                if (Cls_0005_AccessControl.AccessGranted("M20M010502", user.MenuAccess))
                {
                    btn_ArtEdit.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_ArtEdit.Visibility = Visibility.Collapsed;
                }
                // Acesso a eliminar
                if (Cls_0005_AccessControl.AccessGranted("M20M010503", user.MenuAccess))
                {
                    btn_ArtDelete.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_ArtDelete.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Loadcbx_Familia()
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

                        DataRow allRow = dt.NewRow();
                        allRow["fam_codigo"] = 00; // Define um valor que represente "Todos"
                        allRow["fam_descr"] = "Todos"; // O texto a ser exibido na ComboBox
                        dt.Rows.InsertAt(allRow, 0);

                        cbx_Familia.ItemsSource = dt.DefaultView;
                        cbx_Familia.DisplayMemberPath = "fam_descr";
                        cbx_Familia.SelectedValuePath = "fam_codigo";
                        cbx_Familia.SelectedIndex = 0;
                        cbx_Familia.IsEditable = false;
                        return;
                    }
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Familia (Erro: C1049)!");
                }
            }
        }

        private void Loadcbx_SubFamilia()
        {
            // ComboBox subfamilia
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    var query = "";
                    if (artFamCod != "0" || artFamCod != "")
                    {
                        query = "SELECT sfm_codigo, sfm_descr FROM tbl_0202_subfamilias WHERE sfm_codfam = ? ORDER BY sfm_descr";

                    }
                    else
                    {
                        query = "SELECT sfm_codigo, sfm_descr FROM tbl_0202_subfamilias ORDER BY sfm_descr";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@Art_codfam", artFamCod);
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
                        cbx_SubFamilia.SelectedIndex = 0;
                        cbx_SubFamilia.IsEditable = false;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_SubFamilias (Erro: C1050)!" + ex.Message);
                }
            }
        }

        private void Loadcbx_Grupo()
        {
            // ComboBox grupo
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    var query = "";
                    if (artSFCod != "0" || artSFCod != "")
                    {
                        query = "SELECT grp_codigo, grp_descr FROM tbl_0203_grupos WHERE grp_codsubfam = ? ORDER BY grp_descr";

                    }
                    else
                    {
                        query = "SELECT grp_codigo, grp_descr FROM tbl_0203_grupos ORDER BY grp_descr";
                    }

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@Art_codsfm", artSFCod);
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
                        cbx_Grupo.SelectedIndex = 0;
                        cbx_Grupo.IsEditable = false;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_grupos (Erro: C1051!" + ex.Message);
                }
            }
        }

        private void Loadcbx_Terceiro()
        {
            // ComboBox terceiro
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT terc_cod,terc_descr FROM tbl_0102_terceiros ORDER BY terc_descr";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
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
                        cbx_Terceiro.SelectedIndex = 0;
                        cbx_Terceiro.IsEditable = false;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_terceiros (Erro: C1052!" + ex.Message);
                }
            }
        }

        private void LoadArtigos()
        {
            ObservableCollection<Cls_0205_Artigos> artigos = new ObservableCollection<Cls_0205_Artigos>();
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros
                try
                {
                    conn.Open();
                    var query = "";
                    if (Consulta == 0)
                    {
                        // Definição de procura de todos os registos na tabela 
                        query = "SELECT art_id, art_codgrupo, art_cod, art_codigo, art_descr, art_coduni, art_codterc, status_descr, grp_descr, terc_descr, uni_descr " +
                            "FROM tbl_0207_artigos JOIN tbl_0001_status ON tbl_0207_artigos.art_status = tbl_0001_status.status_cod " +
                            "LEFT JOIN tbl_0203_grupos ON tbl_0207_artigos.art_codgrupo = tbl_0203_grupos.grp_codigo " +
                            "LEFT JOIN tbl_0202_subfamilias ON tbl_0203_grupos.grp_codsubfam = tbl_0202_subfamilias.sfm_codigo " +
                            "LEFT JOIN tbl_0201_familias ON tbl_0202_subfamilias.sfm_codfam = tbl_0201_familias.fam_codigo " +
                            "LEFT JOIN tbl_0102_terceiros ON tbl_0207_artigos.art_codterc = tbl_0102_terceiros.terc_cod " +
                            "LEFT JOIN tbl_0204_unidades ON tbl_0207_artigos.art_coduni = tbl_0204_unidades.uni_cod " +
                            "ORDER BY grp_descr, art_descr";
                    }
                    if (Consulta == 1)
                    {
                        ArtCodConsulta = artFamCod;
                        // Definição de procura de todos os registos na tabela 
                        query = "SELECT art_id, art_codgrupo, art_cod, art_codigo, art_descr, art_coduni, art_codterc, status_descr, grp_descr, terc_descr, uni_descr " +
                            "FROM tbl_0207_artigos JOIN tbl_0001_status ON tbl_0207_artigos.art_status = tbl_0001_status.status_cod " +
                            "LEFT JOIN tbl_0203_grupos ON tbl_0207_artigos.art_codgrupo = tbl_0203_grupos.grp_codigo " +
                            "LEFT JOIN tbl_0202_subfamilias ON tbl_0203_grupos.grp_codsubfam = tbl_0202_subfamilias.sfm_codigo " +
                            "LEFT JOIN tbl_0201_familias ON tbl_0202_subfamilias.sfm_codfam = tbl_0201_familias.fam_codigo " +
                            "LEFT JOIN tbl_0102_terceiros ON tbl_0207_artigos.art_codterc = tbl_0102_terceiros.terc_cod " +
                            "LEFT JOIN tbl_0204_unidades ON tbl_0207_artigos.art_coduni = tbl_0204_unidades.uni_cod " +
                            "WHERE LEFT(art_codigo, 2) = ? " +
                            "ORDER BY grp_descr, art_descr";
                    }
                    if (Consulta == 2)
                    {
                        ArtCodConsulta = artSFCod;
                        // Definição de procura de todos os registos na tabela 
                        query = "SELECT art_id, art_codgrupo, art_cod, art_codigo, art_descr, art_coduni, art_codterc, status_descr, grp_descr, terc_descr, uni_descr " +
                            "FROM tbl_0207_artigos JOIN tbl_0001_status ON tbl_0207_artigos.art_status = tbl_0001_status.status_cod " +
                            "LEFT JOIN tbl_0203_grupos ON tbl_0207_artigos.art_codgrupo = tbl_0203_grupos.grp_codigo " +
                            "LEFT JOIN tbl_0202_subfamilias ON tbl_0203_grupos.grp_codsubfam = tbl_0202_subfamilias.sfm_codigo " +
                            "LEFT JOIN tbl_0201_familias ON tbl_0202_subfamilias.sfm_codfam = tbl_0201_familias.fam_codigo " +
                            "LEFT JOIN tbl_0102_terceiros ON tbl_0207_artigos.art_codterc = tbl_0102_terceiros.terc_cod " +
                            "LEFT JOIN tbl_0204_unidades ON tbl_0207_artigos.art_coduni = tbl_0204_unidades.uni_cod " +
                            "WHERE LEFT(art_codigo, 5) = ? " +
                            "ORDER BY grp_descr, art_descr";
                    }
                    if (Consulta == 3)
                    {
                        ArtCodConsulta = artGrpCod;
                        // Definição de procura de todos os registos na tabela 
                        query = "SELECT art_id, art_codgrupo, art_cod, art_codigo, art_descr, art_coduni, art_codterc, status_descr, grp_descr, terc_descr, uni_descr " +
                            "FROM tbl_0207_artigos JOIN tbl_0001_status ON tbl_0207_artigos.art_status = tbl_0001_status.status_cod " +
                            "LEFT JOIN tbl_0203_grupos ON tbl_0207_artigos.art_codgrupo = tbl_0203_grupos.grp_codigo " +
                            "LEFT JOIN tbl_0202_subfamilias ON tbl_0203_grupos.grp_codsubfam = tbl_0202_subfamilias.sfm_codigo " +
                            "LEFT JOIN tbl_0201_familias ON tbl_0202_subfamilias.sfm_codfam = tbl_0201_familias.fam_codigo " +
                            "LEFT JOIN tbl_0102_terceiros ON tbl_0207_artigos.art_codterc = tbl_0102_terceiros.terc_cod " +
                            "LEFT JOIN tbl_0204_unidades ON tbl_0207_artigos.art_coduni = tbl_0204_unidades.uni_cod " +
                            "WHERE LEFT(art_codigo, 8) = ? " +
                            "ORDER BY grp_descr, art_descr";
                    }
                    if (Consulta == 4)
                    {
                        ArtCodConsulta = artGrpCod;
                        // Definição de procura de todos os registos na tabela 
                        query = "SELECT art_id, art_codgrupo, art_cod, art_codigo, art_descr, art_coduni, art_codterc, status_descr, grp_descr, terc_descr, uni_descr " +
                            "FROM tbl_0207_artigos JOIN tbl_0001_status ON tbl_0207_artigos.art_status = tbl_0001_status.status_cod " +
                            "LEFT JOIN tbl_0203_grupos ON tbl_0207_artigos.art_codgrupo = tbl_0203_grupos.grp_codigo " +
                            "LEFT JOIN tbl_0202_subfamilias ON tbl_0203_grupos.grp_codsubfam = tbl_0202_subfamilias.sfm_codigo " +
                            "LEFT JOIN tbl_0201_familias ON tbl_0202_subfamilias.sfm_codfam = tbl_0201_familias.fam_codigo " +
                            "LEFT JOIN tbl_0102_terceiros ON tbl_0207_artigos.art_codterc = tbl_0102_terceiros.terc_cod " +
                            "LEFT JOIN tbl_0204_unidades ON tbl_0207_artigos.art_coduni = tbl_0204_unidades.uni_cod " +
                            "WHERE LEFT(art_codigo, 8) = ? AND art_codterc = ? " +
                            "ORDER BY grp_descr, art_descr";
                    }

                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@Art_codConsulta", ArtCodConsulta);
                        cmd.Parameters.AddWithValue("@Art_codTerc", artTercCod);
                        // Executa a consulta
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Lê os registos da tabela
                            while (reader.Read())
                            {
                                // Atribui a variaveis os dados da tabela para apresentar 
                                artigos.Add(new Cls_0205_Artigos
                                {
                                    Art_Id = Convert.ToInt32(reader["art_id"]),
                                    Art_CodGrp = reader["art_codgrupo"].ToString(),
                                    Art_DescrGrp = reader["grp_descr"].ToString(),
                                    Art_Cod = reader["art_cod"].ToString(),
                                    Art_Codigo = reader["art_codigo"].ToString(),
                                    Art_Descr = reader["art_descr"].ToString(),
                                    Art_CodTerc = reader["art_codterc"].ToString(),
                                    Art_DescrTerc = reader["terc_descr"].ToString(),
                                    Art_CodUni = reader["art_coduni"].ToString(),
                                    Art_DescrUni = reader["uni_descr"].ToString(),
                                    Art_Status = reader["status_descr"].ToString(),
                                }); ;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao carregar dados (Erro C1053)!" + ex.Message);
                }
            }
            listArtigos.ItemsSource = artigos;
            // Desabilitar botões por falta de seleção de item
            btn_ArtEdit.Visibility = Visibility.Collapsed;
            btn_ArtDelete.Visibility = Visibility.Collapsed;
        }


        private void Cbx_Familia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbx_Familia.SelectedIndex == 0)
            {
                Consulta = 0;
                LoadArtigos();
            }
            else
            {
                Consulta = 1;
                artFamCod = cbx_Familia.SelectedValue.ToString();
                LoadArtigos();
                cbx_SubFamilia.Visibility = Visibility.Visible;
                lbl_SubFamilia.Visibility = Visibility.Visible;
                Loadcbx_SubFamilia();
            }
        }

        private void Cbx_SubFamilia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbx_SubFamilia.SelectedIndex == 0)
            {
                Consulta = 1;
                LoadArtigos();
            }
            else
            {
                Consulta = 2;
                artSFCod = cbx_SubFamilia.SelectedValue.ToString();
                LoadArtigos();
                cbx_Grupo.Visibility = Visibility.Visible;
                lbl_Grupo.Visibility = Visibility.Visible;
                Loadcbx_Grupo();
            }
        }

        private void Cbx_Grupo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbx_Grupo.SelectedIndex == 0)
            {
                Consulta = 2;
                LoadArtigos();
            }
            else
            {
                Consulta = 3;
                artGrpCod = cbx_Grupo.SelectedValue.ToString();
                LoadArtigos();
                cbx_Terceiro.Visibility = Visibility.Visible;
                lbl_Terceiro.Visibility = Visibility.Visible;
                Loadcbx_Terceiro();
            }
        }

        private void Cbx_Terceiro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbx_Terceiro.SelectedIndex == 0)
            {
                Consulta = 3;
                LoadArtigos();
            }
            else
            {
                Consulta = 4;
                artTercCod = cbx_Terceiro.SelectedValue.ToString();
                LoadArtigos();
            }
        }

        private void ListArtigos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listArtigos.SelectedItem != null)
            {
                // Obtem o item selecionado (linha) da ListView
                var selectedItem = (Cls_0205_Artigos)listArtigos.SelectedItem;

                // Obtem os valores (ID column, Cod column) e guarda em variáveis
                selectedArtId = selectedItem.Art_Id;
                selectedArtCod = selectedItem.Art_Codigo;
                selectedArtIndex = listArtigos.Items.IndexOf(selectedItem);
                selectedArtGrpCod = selectedItem.Art_CodGrp;
                btn_ArtEdit.Visibility = Visibility.Visible;
                btn_ArtDelete.Visibility = Visibility.Visible;
            }
            else
            {
                // No item selected, reset the user_ID variable
                selectedArtId = -1;
                btn_ArtEdit.Visibility = Visibility.Collapsed;
                btn_ArtDelete.Visibility = Visibility.Collapsed;
            }
        }

        private void ListArtigos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listArtigos.SelectedItem != null)
            {
                if (loginUserType == "Utilizador")
                {
                    // Acesso a alterar
                    if (Cls_0005_AccessControl.AccessGranted("M20M010502", user.MenuAccess))
                    {
                        Btn_ArtEdit_Click(sender, e);
                    }
                }
                else
                {
                    Btn_ArtEdit_Click(sender, e);
                }
            }
        }

        private void Btn_ArtAdd_Click(object sender, RoutedEventArgs e)
        {
            Frm_020501_ArtigoAdd frm_020501_ArtigosAdd = new Frm_020501_ArtigoAdd(loginUserId, artFamCod, artSFCod, artGrpCod, artTercCod)
            {
                Owner = this
            };
            frm_020501_ArtigosAdd.ShowDialog();
            LoadArtigos();
        }

        private void Btn_ArtEdit_Click(object sender, RoutedEventArgs e)
        {
            Frm_020502_ArtigoEdit frm_020502_ArtigoEdit = new Frm_020502_ArtigoEdit(loginUserId, selectedArtCod, selectedArtGrpCod, selectedArtId)
            {
                Owner = this
            };
            frm_020502_ArtigoEdit.ShowDialog();
            LoadArtigos();
            listArtigos.SelectedIndex = selectedArtIndex;
        }

        private void Btn_ArtDelete_Click(object sender, RoutedEventArgs e)
        {
            Frm_020503_ArtigoDelete frm_020503_ArtigosDelete = new Frm_020503_ArtigoDelete(selectedArtId, selectedArtCod)
            {
                Owner = this
            };
            frm_020503_ArtigosDelete.ShowDialog();
            LoadArtigos();
        }

        private void Btn_ArtClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
