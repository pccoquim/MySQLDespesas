/*
Frm_0202_SubFamiliasManut.xaml.cs
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
    /// Interaction logic for Frm_0202_SubFamiliasManut.xaml
    /// </summary>
    public partial class Frm_0202_SubFamiliasManut : Window
    {
        public static Cls_0003_Users user = new Cls_0003_Users();
        private readonly int loginUserSequence;
        private readonly string loginUserId, loginUserType;
        private string selectedSFCodFam, selectedSFCod;
        private int selectedSFId, selectedSFIndex;

        public Frm_0202_SubFamiliasManut(int loginUserSequence, string loginUserId, string loginUserType)
        {
            InitializeComponent();
            this.loginUserSequence = loginUserSequence;
            this.loginUserId = loginUserId;
            this.loginUserType = loginUserType;
            LoadAccess();
            LoadCBXFam();
            LoadSubFamilias();
        }

        private void LoadAccess()
        {
            user.MenuAccess = Cls_0005_AccessControl.GetAccess(Convert.ToInt32(loginUserSequence));
            if (loginUserType == "Utilizador")
            {
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

        private void LoadCBXFam()
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

                        cbx_Familia.Items.Clear();

                        DataRow allRow = dt.NewRow();
                        allRow["fam_codigo"] = 00; // Defina um valor que represente "Todos"
                        allRow["fam_descr"] = "Todos"; // O texto a ser exibido na ComboBox
                        dt.Rows.InsertAt(allRow, 0);
                        // Limpa a combobox antes de adicionar os itens

                        cbx_Familia.ItemsSource = dt.DefaultView;
                        cbx_Familia.DisplayMemberPath = "fam_descr";
                        cbx_Familia.SelectedValuePath = "fam_codigo";
                        cbx_Familia.SelectedIndex = 0;
                        cbx_Familia.IsEditable = false;
                        selectedSFCodFam = cbx_Familia.SelectedValue.ToString();
                        return;
                    }
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Status (Erro: C1028)!");
                }
            }
        }

        private void LoadSubFamilias()
        {
            ObservableCollection<Cls_0202_SubFamilias> subFamilias = new ObservableCollection<Cls_0202_SubFamilias>();
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros
                try
                {
                    conn.Open();
                    var query = "";
                    if (selectedSFCodFam == "0")
                    {
                        // Definição de procura de todos os registos na tabela 
                        query = "SELECT sfm_id, sfm_codfam, sfm_cod, sfm_codigo, sfm_descr, status_descr, fam_descr FROM tbl_0202_subfamilias JOIN tbl_0001_status ON tbl_0202_subfamilias.sfm_status = tbl_0001_status.status_cod LEFT JOIN tbl_0201_familias ON tbl_0202_subfamilias.sfm_codfam = tbl_0201_familias.fam_codigo ORDER BY sfm_descr";
                    }
                    else
                    {
                        // Definição de procura de registos por familia na tabela 
                        query = "SELECT sfm_id, sfm_codfam, sfm_cod, sfm_codigo, sfm_descr, status_descr, fam_descr FROM tbl_0202_subfamilias JOIN tbl_0001_status ON tbl_0202_subfamilias.sfm_status = tbl_0001_status.status_cod LEFT JOIN tbl_0201_familias ON tbl_0202_subfamilias.sfm_codfam = tbl_0201_familias.fam_codigo WHERE sfm_codfam = ? ORDER BY sfm_descr";
                    }

                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Atribuição de variavel
                        cmd.Parameters.AddWithValue("@sfm_codfam", selectedSFCodFam);
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
                    System.Windows.MessageBox.Show("Erro ao carregar dados (Erro C1027)!" + ex.Message);
                }
            }
            listSubFamilias.ItemsSource = subFamilias;
            // Desabilitar botões por falta de seleção de item
            btn_SFEdit.Visibility = Visibility.Collapsed;
            btn_SFDelete.Visibility = Visibility.Collapsed;
        }

        private void ListSubFamilias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listSubFamilias.SelectedItem != null)
            {
                // Obtem o item selecionado (linha) da ListView
                var selectedItem = (Cls_0202_SubFamilias)listSubFamilias.SelectedItem;

                // Obtem os valores (ID column, Cod column) e guarda em variáveis
                selectedSFId = selectedItem.SF_Id;
                selectedSFIndex = listSubFamilias.Items.IndexOf(selectedItem);
                selectedSFCodFam = selectedItem.SF_CodFam;
                selectedSFCod = selectedItem.SF_Codigo;
                btn_SFEdit.Visibility = Visibility.Visible;
                btn_SFDelete.Visibility = Visibility.Visible;
            }
            else
            {
                // No item selected, reset the user_ID variable
                selectedSFId = -1;
                btn_SFEdit.Visibility = Visibility.Collapsed;
                btn_SFDelete.Visibility = Visibility.Collapsed;
            }
        }

        private void ListSubFamilias_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {

            if (listSubFamilias.SelectedItem != null)
            {
                if (loginUserType == "Utilizador")
                {
                    // Acesso a alterar
                    if (Cls_0005_AccessControl.AccessGranted("M20M010302", user.MenuAccess))
                    {
                        Btn_SFEdit_Click(sender, e);
                    }
                }
                else
                {
                    Btn_SFEdit_Click(sender, e);
                }
            }
        }

        private void Cbx_Familia_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            selectedSFCodFam = cbx_Familia.SelectedValue.ToString();
            LoadSubFamilias();
        }

        private void Btn_SFAdd_Click(object sender, RoutedEventArgs e)
        {
            if (selectedSFCodFam != "0" || selectedSFCodFam != null)
            {
                // MessageBox.Show($"{selectedSFCodFam}");
                Frm_020201_SubFamiliaAdd frm_020201_SubFamiliasAdd = new Frm_020201_SubFamiliaAdd(loginUserId, selectedSFCodFam)
                {
                    Owner = this
                };
                frm_020201_SubFamiliasAdd.ShowDialog();
                LoadSubFamilias();
            }
        }

        private void Btn_SFEdit_Click(object sender, RoutedEventArgs e)
        {
            Frm_020202_SubFamiliaEdit frm_020202_SubFamiliasEdit = new Frm_020202_SubFamiliaEdit(loginUserId, selectedSFCodFam, selectedSFId)
            {
                Owner = this,
            };
            frm_020202_SubFamiliasEdit.ShowDialog();
            LoadSubFamilias();
            listSubFamilias.SelectedIndex = selectedSFIndex;
        }

        private void Btn_SFDelete_Click(object sender, RoutedEventArgs e)
        {
            Frm_020203_SubFamiliaDelete subFamiliaDelete = new Frm_020203_SubFamiliaDelete(selectedSFCodFam, selectedSFCod, selectedSFId)
            {
                Owner = this
            };
            subFamiliaDelete.ShowDialog();
            LoadSubFamilias();
        }

        private void Btn_FClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
