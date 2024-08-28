/*
Frm_0201_FamiliasManut.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_0201_FamiliasManut.xaml
    /// </summary>
    public partial class Frm_0201_FamiliasManut : Window
    {
        private static readonly Cls_0003_Users user = new Cls_0003_Users();
        private readonly int loginUserSequence;
        private readonly string loginUserId, loginUserType;
        private int selectedFamId, selectedFamIndex;
        private string selectedFamCod;
        public Frm_0201_FamiliasManut(int loginUserSequence, string loginUserId, string loginUserType)
        {
            InitializeComponent();
            this.loginUserSequence = loginUserSequence;
            this.loginUserId = loginUserId;
            this.loginUserType = loginUserType;
            LoadAccess();
            LoadFamilias();
        }

        private void LoadAccess()
        {
            user.MenuAccess = Cls_0005_AccessControl.GetAccess(Convert.ToInt32(loginUserSequence));
            if (loginUserType == "Utilizador")
            {
                // Acesso a adicionar
                if (Cls_0005_AccessControl.AccessGranted("M20M010201", user.MenuAccess))
                {
                    btn_Add.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Add.Visibility = Visibility.Collapsed;
                }
                // Acesso a alterar
                if (Cls_0005_AccessControl.AccessGranted("M20M010202", user.MenuAccess))
                {
                    btn_Edit.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Edit.Visibility = Visibility.Collapsed;
                }
                // Acesso a eliminar
                if (Cls_0005_AccessControl.AccessGranted("M20M010203", user.MenuAccess))
                {
                    btn_Delete.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Delete.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void LoadFamilias()
        {
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
                    System.Windows.MessageBox.Show("Erro ao carregar dados (Erro C1020)!" + ex.Message);
                }
            }
            listFamilias.ItemsSource = familias;
            // Desabilitar botões por falta de seleção de item
            btn_Edit.Visibility = Visibility.Collapsed;
            btn_Delete.Visibility = Visibility.Collapsed;
        }

        private void ListFamilias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listFamilias.SelectedItem != null)
            {
                // Obtem o item selecionado (linha) da ListView
                var selectedItem = (Cls_0201_Familias)listFamilias.SelectedItem;

                // Obtem os valores (ID column, Cod column) e guarda em variáveis
                selectedFamId = selectedItem.F_Id;
                selectedFamCod = selectedItem.F_Cod;
                selectedFamIndex = listFamilias.Items.IndexOf(selectedItem);
                btn_Edit.Visibility = Visibility.Visible;
                btn_Delete.Visibility = Visibility.Visible;
            }
            else
            {
                // No item selected, reset the user_ID variable
                selectedFamId = -1;
                btn_Edit.Visibility = Visibility.Collapsed;
                btn_Delete.Visibility = Visibility.Collapsed;
            }
        }

        private void ListFamilias_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listFamilias.SelectedItem != null)
            {
                if (loginUserType == "Utilizador")
                {
                    // Acesso a alterar
                    if (Cls_0005_AccessControl.AccessGranted("M20M010202", user.MenuAccess))
                    {
                        Btn_Edit_Click(sender, e);
                    }
                }
                else
                {
                    Btn_Edit_Click(sender, e);
                }
            }
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            Frm_020101_FamiliaAdd frm_020101_FamiliasAdd = new Frm_020101_FamiliaAdd(loginUserId)
            {
                Owner = this
            };
            frm_020101_FamiliasAdd.ShowDialog();
            LoadFamilias();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Frm_020102_FamiliaEdit frm_020102_FamiliasEdit = new Frm_020102_FamiliaEdit(loginUserId, selectedFamId)
            {
                Owner = this
            };
            frm_020102_FamiliasEdit.ShowDialog();
            LoadFamilias();
            listFamilias.SelectedIndex = selectedFamIndex;
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Frm_020103_FamiliaDelete familiaDelete = new Frm_020103_FamiliaDelete(selectedFamId, selectedFamCod)
            {
                Owner = this
            };
            familiaDelete.ShowDialog();
            LoadFamilias();
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
