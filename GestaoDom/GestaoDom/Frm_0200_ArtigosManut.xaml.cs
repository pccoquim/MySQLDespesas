/*
Frm_0200_ArtigosManut.xaml.cs
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
    /// Interaction logic for Frm_0200_ArtigosManut.xaml
    /// </summary>
    public partial class Frm_0200_ArtigosManut : Window
    {
        private static readonly Cls_0003_Users user = new Cls_0003_Users();
        private readonly int loginUserSequence;
        private readonly string loginUserId, loginUserType;

        public Frm_0200_ArtigosManut(int loginUserSequence, string loginUserId, string loginUserType)
        {
            InitializeComponent();
            this.loginUserSequence = loginUserSequence;
            this.loginUserId = loginUserId;
            this.loginUserType = loginUserType;
            LoadAccess();
        }

        // Acessos às funcionalidades
        private void LoadAccess()
        {
            user.MenuAccess = Cls_0005_AccessControl.GetAccess(Convert.ToInt32(loginUserSequence));
            if (loginUserType == "Utilizador")
            {
                // Acesso á aba 1
                if (Cls_0005_AccessControl.AccessGranted("M20M0102", user.MenuAccess))
                {
                    TabItem tabFamilias = tabControl.Items[0] as TabItem;
                    tabFamilias.Visibility = Visibility.Visible;
                }
                else
                {
                    TabItem tabFamilias = tabControl.Items[0] as TabItem;
                    tabFamilias.Visibility = Visibility.Collapsed;
                }
                // Acesso á aba 2
                if (Cls_0005_AccessControl.AccessGranted("M20M0103", user.MenuAccess))
                {
                    TabItem tabSubfamilias = tabControl.Items[1] as TabItem;
                    tabSubfamilias.Visibility = Visibility.Visible;
                }
                else
                {
                    TabItem tabSubfamilias = tabControl.Items[1] as TabItem;
                    tabSubfamilias.Visibility = Visibility.Collapsed;
                }
                // Acesso á aba 3
                if (Cls_0005_AccessControl.AccessGranted("M20M0104", user.MenuAccess))
                {
                    TabItem tabGrupos = tabControl.Items[2] as TabItem;
                    tabGrupos.Visibility = Visibility.Visible;
                }
                else
                {
                    TabItem tabGrupos = tabControl.Items[2] as TabItem;
                    tabGrupos.Visibility = Visibility.Collapsed;
                }
                // Acesso á aba 4
                if (Cls_0005_AccessControl.AccessGranted("M20M0105", user.MenuAccess))
                {
                    TabItem tabUnidades = tabControl.Items[3] as TabItem;
                    tabUnidades.Visibility = Visibility.Visible;
                }
                else
                {
                    TabItem tabUnidades = tabControl.Items[3] as TabItem;
                    tabUnidades.Visibility = Visibility.Collapsed;
                }
                // Acesso á aba 5
                if (Cls_0005_AccessControl.AccessGranted("M20M0106", user.MenuAccess))
                {
                    TabItem tabArtigos = tabControl.Items[4] as TabItem;
                    tabArtigos.Visibility = Visibility.Visible;
                }
                else
                {
                    TabItem tabArtigos = tabControl.Items[4] as TabItem;
                    tabArtigos.Visibility = Visibility.Collapsed;
                }

                // Familias - Aba 1
                // Acesso a adicionar
                if (Cls_0005_AccessControl.AccessGranted("M20M010201", user.MenuAccess))
                {
                    btn_FAdd.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_FAdd.Visibility = Visibility.Collapsed;
                }
                // Acesso a alterar
                if (Cls_0005_AccessControl.AccessGranted("M20M010202", user.MenuAccess))
                {
                    btn_FEdit.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_FEdit.Visibility = Visibility.Collapsed;
                }
                // Acesso a eliminar
                if (Cls_0005_AccessControl.AccessGranted("M20M010203", user.MenuAccess))
                {
                    btn_FDelete.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_FDelete.Visibility = Visibility.Collapsed;
                }

                // SubFamilias - Aba 2
                // Acesso a adicionar
                if (Cls_0005_AccessControl.AccessGranted("M20M010301", user.MenuAccess))
                {
                    btn_SFAdd.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_SFAdd.Visibility = Visibility.Collapsed;
                }
                // Acesso a alterar
                if (Cls_0005_AccessControl.AccessGranted("M20M010302", user.MenuAccess))
                {
                    btn_SFEdit.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_SFEdit.Visibility = Visibility.Collapsed;
                }
                // Acesso a eliminar
                if (Cls_0005_AccessControl.AccessGranted("M20M010303", user.MenuAccess))
                {
                    btn_SFDelete.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_SFDelete.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            System.Windows.Controls.TabControl tabControl = sender as System.Windows.Controls.TabControl;
            if (!blockTabControlSelection && tabControl != null && tabControl.SelectedItem != null)
            {
                TabItem selectedTab = tabControl.SelectedItem as TabItem;
                if (selectedTab != null)
                {
                    if (selectedTab.Header.ToString() == "Famílias")
                    {
                        LoadFamilias();
                    }
                    else if (selectedTab.Header.ToString() == "Subfamílias")
                    {
                        LoadSubFamilias();
                    }
                    else if (selectedTab.Header.ToString() == "Grupos")
                    {
                        // Carregar dados para a aba de Artigos
                    }
                    else if (selectedTab.Header.ToString() == "Unidades")
                    {
                        // Carregar dados para a aba de Artigos
                    }
                    else if (selectedTab.Header.ToString() == "Artigos")
                    {
                        // Carregar dados para a aba de Artigos
                    }
                }
            }
        }

        private bool blockTabControlSelection = false;

        private bool ShowConfirmation(string message)
        {
            var confirmDialog = new Frm_0000_ConfirmDialog(message);
            confirmDialog.ShowDialog();
            return confirmDialog.Confirmed;
        }
        /* ############################################################################################################################################################################################### */
        // Aba Familias
        private int selectedFamId, selectedFamIndex;
        private string selectedFamCod;
        private void LoadFamilias()
        {
            if (blockTabControlSelection)
            {
                return;
            }
            ObservableCollection<Cls_0201_Familias> familias = new ObservableCollection<Cls_0201_Familias>();
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros
                try
                {
                    conn.Open();
                    // Definição de procura de registos na tabela 
                    string query = "SELECT fam_id, fam_codigo, fam_descr, status_descr FROM tbl_0201_familias, tbl_0001_status WHERE fam_status = status_cod ORDER BY fam_descr";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa a consulta
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Lê os registos da tabela
                            while (reader.Read())
                            {
                                // Atribui a variaveis os dados da tabela para apresentar 
                                familias.Add(new Cls_0201_Familias
                                {
                                    F_Id = Convert.ToInt32(reader["fam_id"]),
                                    F_Cod = reader["fam_codigo"].ToString(),
                                    F_Descr = reader["fam_descr"].ToString(),
                                    F_Status = reader["status_descr"].ToString(),
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao carregar dados (Erro C1021)!" + ex.Message);
                }
            }
            listFamilias.ItemsSource = familias;
            // Desabilitar botões por falta de seleção de item
            btn_FEdit.Visibility = Visibility.Collapsed;
            btn_FDelete.Visibility = Visibility.Collapsed;
        }

        private void ListFamilias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            blockTabControlSelection = false;
            if (!blockTabControlSelection && listFamilias.SelectedItem != null)
            {
                blockTabControlSelection = true;
                listFamilias.SelectedItem = e.AddedItems[0];
                // Obtem o item selecionado (linha) da ListView
                var selectedItem = (Cls_0201_Familias)listFamilias.SelectedItem;

                // Obtem os valores (ID column, Cod column) e guarda em variáveis
                selectedFamId = selectedItem.F_Id;
                selectedFamCod = selectedItem.F_Cod;
                selectedFamIndex = listFamilias.Items.IndexOf(selectedItem);
                btn_FEdit.Visibility = Visibility.Visible;
                btn_FDelete.Visibility = Visibility.Visible;
                // blockTabControlSelection = false;
            }
            else
            {
                // Sem item selecionado, reset da variávem ID
                selectedFamId = -1;
                btn_FEdit.Visibility = Visibility.Collapsed;
                btn_FDelete.Visibility = Visibility.Collapsed;
            }
        }

        private void ListFamilias_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (loginUserType == "Utilizador")
            {
                // Acesso a alterar
                if (Cls_0005_AccessControl.AccessGranted("M20M010202", user.MenuAccess))
                {
                    Btn_FEdit_Click(sender, e);
                }
            }
            else
            {
                Btn_FEdit_Click(sender, e);
            }
        }

        private void Btn_FAdd_Click(object sender, RoutedEventArgs e)
        {
            Frm_020101_FamiliaAdd frm_020101_FamiliasAdd = new Frm_020101_FamiliaAdd(loginUserId)
            {
                Owner = this,
                Topmost = true,
            };
            frm_020101_FamiliasAdd.ShowDialog();
            LoadFamilias();
        }

        private void Btn_FEdit_Click(object sender, RoutedEventArgs e)
        {
            Frm_020102_FamiliaEdit frm_020102_FamiliasEdit = new Frm_020102_FamiliaEdit(loginUserId, selectedFamId)
            {
                Owner = this,
                Topmost = true,
            };
            frm_020102_FamiliasEdit.ShowDialog();
            blockTabControlSelection = false;
            LoadFamilias();
            listFamilias.SelectedIndex = selectedFamIndex;
        }

        private void Btn_FDelete_Click(object sender, RoutedEventArgs e)
        {
            Frm_020103_FamiliaDelete familiaDelete = new Frm_020103_FamiliaDelete(selectedFamId, selectedFamCod)
            {
                Owner = this,
                Topmost = true,
            };
            familiaDelete.ShowDialog();
            LoadFamilias();
        }

        private void Btn_FClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        /* ############################################################################################################################################################################################### */
        // Aba subfamilias
        private int selectedSFId, selectedSFIndex;
        private string selectedSFCod;
        private void LoadSubFamilias()
        {

            // ComboBox status
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

                        cbx_Familia.Items.Clear();

                        DataRow allRow = dt.NewRow();
                        allRow["fam_codigo"] = -1; // Defina um valor que represente "Todos"
                        allRow["fam_descr"] = "Todos"; // O texto a ser exibido na ComboBox
                        dt.Rows.InsertAt(allRow, 0);
                        // Limpa a combobox antes de adicionar os itens

                        cbx_Familia.ItemsSource = dt.DefaultView;
                        cbx_Familia.DisplayMemberPath = "fam_descr";
                        cbx_Familia.SelectedValuePath = "fam_codigo";
                        cbx_Familia.SelectedIndex = 0;
                        cbx_Familia.IsEditable = false;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    //System.Windows.MessageBox.Show("Erro de componentes da cbx_Status (Erro: C1022)!");
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }
            }

            if (blockTabControlSelection)
            {
                return;
            }
            ObservableCollection<Cls_0202_SubFamilias> subFamilias = new ObservableCollection<Cls_0202_SubFamilias>();
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros
                try
                {
                    // Definição de procura de registos na tabela 
                    string query = "SELECT sfm_id, sfm_codfam, sfm_cod, sfm_codigo, sfm_descr, status_descr, fam_descr FROM tbl_0202_subfamilias JOIN tbl_0001_status ON tbl_0202_subfamilias.sfm_status = tbl_0001_status.status_cod LEFT JOIN tbl_0201_familias ON tbl_0202_subfamilias.sfm_codfam = tbl_0201_familias.fam_codigo ORDER BY sfm_descr";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa a consulta
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Lê os registos da tabela
                            while (reader.Read())
                            {
                                // Atribui a variaveis os dados da tabela para apresentar 
                                subFamilias.Add(new Cls_0202_SubFamilias
                                {
                                    SF_Id = Convert.ToInt32(reader["sfm_id"]),
                                    SF_CodFam = reader["sfm_codfam"].ToString(),
                                    SF_DescrFam = reader["fam_descr"].ToString(),
                                    SF_Cod = reader["sfm_cod"].ToString(),
                                    SF_Codigo = reader["sfm_codigo"].ToString(),
                                    SF_Descr = reader["sfm_descr"].ToString(),
                                    SF_Status = reader["status_descr"].ToString(),
                                }); ;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao carregar dados (Erro C1025)!" + ex.Message);
                }
            }
            listSubFamilias.ItemsSource = subFamilias;
            // Desabilitar botões por falta de seleção de item
            btn_FEdit.Visibility = Visibility.Collapsed;
            btn_FDelete.Visibility = Visibility.Collapsed;
        }

        private void ListSubFamilias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            blockTabControlSelection = false;
            if (!blockTabControlSelection && listFamilias.SelectedItem != null)
            {
                blockTabControlSelection = true;
                listFamilias.SelectedItem = e.AddedItems[0];
                // Obtem o item selecionado (linha) da ListView
                var selectedItem = (Cls_0201_Familias)listFamilias.SelectedItem;

                // Obtem os valores (ID column, Cod column) e guarda em variáveis
                selectedSFId = selectedItem.F_Id;
                selectedSFCod = selectedItem.F_Cod;
                selectedSFIndex = listFamilias.Items.IndexOf(selectedItem);
                btn_SFEdit.Visibility = Visibility.Visible;
                btn_SFDelete.Visibility = Visibility.Visible;
            }
            else
            {
                // Sem item selecionado, reset da variávem ID
                selectedSFId = -1;
                btn_SFEdit.Visibility = Visibility.Collapsed;
                btn_SFDelete.Visibility = Visibility.Collapsed;
            }
        }

        private void ListSubFamilias_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

        }

        private void Btn_SFAdd_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_SFEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Btn_SFDelete_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
