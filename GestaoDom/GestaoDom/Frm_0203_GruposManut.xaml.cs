/*
Frm_0203_GruposManut.xaml.cs
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
    /// Interaction logic for Frm_0203_GruposManut.xaml
    /// </summary>
    public partial class Frm_0203_GruposManut : Window
    {
        private readonly int loginUserSequence;
        private readonly string loginUserId, loginUserType;
        private string grupoCodFam, grupoCodSF;
        private int selectedGrupoId, selectedGrupoIndex;
        private string selectedGrupoCodFam, selectedGrupoCodSF, selectedGrupoCod;
        public static Cls_0003_Users user = new Cls_0003_Users();

        public Frm_0203_GruposManut(int loginUserSequence, string loginUserId, string loginUserType)
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
                if (Cls_0005_AccessControl.AccessGranted("M20M010401", user.MenuAccess))
                {
                    btn_GRPAdd.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_GRPAdd.Visibility = Visibility.Collapsed;
                }
                // Acesso a alterar
                if (Cls_0005_AccessControl.AccessGranted("M20M010402", user.MenuAccess))
                {
                    btn_GRPEdit.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_GRPEdit.Visibility = Visibility.Collapsed;
                }
                // Acesso a eliminar
                if (Cls_0005_AccessControl.AccessGranted("M20M010403", user.MenuAccess))
                {
                    btn_GRPDelete.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_GRPDelete.Visibility = Visibility.Collapsed;
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
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Familia (Erro: C1034)!");
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
                    if (grupoCodFam != "0" || grupoCodFam != "")
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
                        cmd.Parameters.AddWithValue("@sfm_codfam", grupoCodFam);
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
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_SubFamilias (Erro: C1035)!" + ex.Message);
                }
            }
        }

        private void LoadGrupos()
        {
            string codQuery = "";
            ObservableCollection<Cls_0203_Grupos> grupos = new ObservableCollection<Cls_0203_Grupos>();
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros
                try
                {
                    conn.Open();
                    var query = "";
                    if (grupoCodFam == "0" || grupoCodFam == null)
                    {
                        // Definição de procura de todos os registos na tabela 
                        query = "SELECT grp_id, grp_codsubfam, grp_cod, grp_codigo, grp_descr, status_descr, sfm_descr FROM tbl_0203_grupos JOIN tbl_0001_status ON tbl_0203_grupos.grp_status = tbl_0001_status.status_cod LEFT JOIN tbl_0202_subfamilias ON tbl_0203_grupos.grp_codsubfam = tbl_0202_subfamilias.sfm_codigo ORDER BY sfm_descr, grp_descr";
                    }
                    else
                    {
                        if (grupoCodSF == "0" || grupoCodSF == null)
                        {
                            codQuery = grupoCodFam;
                            // Definição de procura de registos por familia na tabela 
                            query = "SELECT grp_id, grp_codsubfam, grp_cod, grp_codigo, grp_descr, status_descr, sfm_descr, LEFT(grp_codsubfam, 2) AS cod_select FROM tbl_0203_grupos JOIN tbl_0001_status ON tbl_0203_grupos.grp_status = tbl_0001_status.status_cod LEFT JOIN tbl_0202_subfamilias ON tbl_0203_grupos.grp_codsubfam = tbl_0202_subfamilias.sfm_codigo WHERE LEFT(grp_codsubfam, 2) = ? ORDER BY sfm_descr";
                        }
                        else
                        {
                            codQuery = grupoCodSF;
                            query = "SELECT grp_id, grp_codsubfam, grp_cod, grp_codigo, grp_descr, status_descr, sfm_descr, LEFT(grp_codsubfam, 2) AS cod_select FROM tbl_0203_grupos JOIN tbl_0001_status ON tbl_0203_grupos.grp_status = tbl_0001_status.status_cod LEFT JOIN tbl_0202_subfamilias ON tbl_0203_grupos.grp_codsubfam = tbl_0202_subfamilias.sfm_codigo WHERE grp_codsubfam = ? ORDER BY sfm_descr";
                        }
                    }

                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@grp_codQuery", codQuery);
                        // Executa a consulta
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Lê os registos da tabela
                            while (reader.Read())
                            {
                                // Atribui a variaveis os dados da tabela para apresentar 
                                grupos.Add(new Cls_0203_Grupos
                                {
                                    GRP_Id = Convert.ToInt32(reader["grp_id"]),
                                    GRP_CodSubFam = reader["grp_codsubfam"].ToString(),
                                    GRP_DescrSubFam = reader["sfm_descr"].ToString(),
                                    GRP_Cod = reader["grp_cod"].ToString(),
                                    GRP_Codigo = reader["grp_codigo"].ToString(),
                                    GRP_Descr = reader["grp_descr"].ToString(),
                                    GRP_Status = reader["status_descr"].ToString(),
                                }); ;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao carregar dados (Erro C1036)!" + ex.Message);
                }
            }
            listGrupos.ItemsSource = grupos;
            // Desabilitar botões por falta de seleção de item
            btn_GRPEdit.Visibility = Visibility.Collapsed;
            btn_GRPDelete.Visibility = Visibility.Collapsed;
        }

        private void Cbx_Familia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbx_Familia.SelectedIndex != 0)
            {
                cbx_SubFamilia.Visibility = Visibility.Visible;
                lbl_SubFamilia.Visibility = Visibility.Visible;
                grupoCodFam = cbx_Familia.SelectedValue.ToString();
                selectedGrupoCodFam = cbx_Familia.SelectedValue.ToString();
                Loadcbx_SubFamilia();
                LoadGrupos();
            }
            else
            {
                LoadGrupos();
            }
        }

        private void Cbx_SubFamilia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbx_SubFamilia.SelectedIndex != 0)
            {
                grupoCodSF = cbx_SubFamilia.SelectedValue.ToString();
                LoadGrupos();
            }
        }

        private void ListGrupos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listGrupos.SelectedItem != null)
            {
                // Obtem o item selecionado (linha) da ListView
                var selectedItem = (Cls_0203_Grupos)listGrupos.SelectedItem;

                // Obtem os valores (ID column, Cod column) e guarda em variáveis
                selectedGrupoId = selectedItem.GRP_Id;
                selectedGrupoIndex = listGrupos.Items.IndexOf(selectedItem);
                selectedGrupoCodSF = selectedItem.GRP_CodSubFam;
                selectedGrupoCod = selectedItem.GRP_Codigo;
                btn_GRPEdit.Visibility = Visibility.Visible;
                btn_GRPDelete.Visibility = Visibility.Visible;
            }
            else
            {
                // No item selected, reset the user_ID variable
                selectedGrupoId = -1;
                btn_GRPEdit.Visibility = Visibility.Collapsed;
                btn_GRPDelete.Visibility = Visibility.Collapsed;
            }
        }

        private void ListGrupos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listGrupos.SelectedItem != null)
            {
                if (loginUserType == "Utilizador")
                {
                    // Acesso a alterar
                    if (Cls_0005_AccessControl.AccessGranted("M20M010402", user.MenuAccess))
                    {
                        Btn_GRPEdit_Click(sender, e);
                    }
                }
                else
                {
                    Btn_GRPEdit_Click(sender, e);
                }
            }
        }

        private void Btn_GRPAdd_Click(object sender, RoutedEventArgs e)
        {
            Frm_020301_GrupoAdd frm_020301_GruposAdd = new Frm_020301_GrupoAdd(loginUserId, selectedGrupoCodFam, selectedGrupoCodSF)
            {
                Owner = this
            };
            frm_020301_GruposAdd.ShowDialog();
            LoadGrupos();
        }

        private void Btn_GRPEdit_Click(object sender, RoutedEventArgs e)
        {
            Frm_020302_GrupoEdit frm_020302_GruposEdit = new Frm_020302_GrupoEdit(loginUserId, selectedGrupoCodSF, selectedGrupoCod, selectedGrupoId)
            {
                Owner = this
            };
            frm_020302_GruposEdit.ShowDialog();
            LoadGrupos();
            listGrupos.SelectedIndex = selectedGrupoIndex;
        }

        private void Btn_GRPDelete_Click(object sender, RoutedEventArgs e)
        {
            Frm_020303_GrupoDelete frm_020303_GruposDelete = new Frm_020303_GrupoDelete(selectedGrupoCod, selectedGrupoId)
            {
                Owner = this
            };
            frm_020303_GruposDelete.ShowDialog();
            LoadGrupos();
        }

        private void Btn_GRPClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
