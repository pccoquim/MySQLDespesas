/*
Frm_0004_AcessosManut.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.1
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
    /// Interaction logic for Frm_0004_AcessosManut.xaml
    /// </summary>
    public partial class Frm_0004_AcessosManut : Window
    {
        private static readonly Cls_0003_Users user = new Cls_0003_Users();
        private readonly int loginUserSequence;
        private readonly string loginUserId;
        private readonly string loginUserType;
        private int accessId, accessIndex;
        private string accessCod;

        public Frm_0004_AcessosManut(int loginUserSequence, string loginUserId, string loginUserType)
        {
            InitializeComponent();
            this.loginUserSequence = loginUserSequence;
            this.loginUserId = loginUserId;
            this.loginUserType = loginUserType;
            LoadAccess();
            Btn_Add.Visibility = Visibility.Collapsed;
            Btn_Edit.Visibility = Visibility.Collapsed;
            Btn_Delete.Visibility = Visibility.Collapsed;
            LoadAccessGrantedAdd();
        }

        private void LoadAccessGrantedAdd()
        {
            user.MenuAccess = Cls_0005_AccessControl.GetAccess(loginUserSequence);
            if (loginUserType == "Utilizador")
            {
                // Acesso a adicionar acessos
                if (Cls_0005_AccessControl.AccessGranted("M20U010201", user.MenuAccess))
                {
                    Btn_Add.Visibility = Visibility.Visible;
                }
                else
                {
                    Btn_Add.Visibility = Visibility.Hidden;
                }
            }
            else if (loginUserType == "Administrador")
            {
                Btn_Add.Visibility = Visibility.Visible;
            }
        }

        private void LoadAccessGranted()
        {
            user.MenuAccess = Cls_0005_AccessControl.GetAccess(Convert.ToInt32(loginUserSequence));
            if (loginUserType == "Utilizador")
            {
                // Acesso a adicionar acessos
                if (Cls_0005_AccessControl.AccessGranted("M20U010201", user.MenuAccess))
                {
                    Btn_Add.Visibility = Visibility.Visible;
                }
                else
                {
                    Btn_Add.Visibility = Visibility.Collapsed;
                }
                // Acesso a alterar acessos
                if (Cls_0005_AccessControl.AccessGranted("M20U010202", user.MenuAccess))
                {
                    Btn_Edit.Visibility = Visibility.Visible;
                }
                else
                {
                    Btn_Edit.Visibility = Visibility.Collapsed;
                }
                // Acesso a eliminar acessos
                if (Cls_0005_AccessControl.AccessGranted("M20U010203", user.MenuAccess))
                {
                    Btn_Delete.Visibility = Visibility.Visible;
                }
                else
                {
                    Btn_Delete.Visibility = Visibility.Collapsed;
                }
            }
            else if (loginUserType == "Administrador")
            {
                Btn_Add.Visibility = Visibility.Visible;
                Btn_Edit.Visibility = Visibility.Visible;
                Btn_Delete.Visibility = Visibility.Visible;
            }
        }
        private void LoadAccess()
        {
            ObservableCollection<Cls_0004_Access> access = new ObservableCollection<Cls_0004_Access>();

            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT opm_id, opm_cod, opm_descr, opm_nivel, status_descr FROM tbl_0003_opcoesAcesso, tbl_0001_status WHERE opm_status = status_cod ORDER BY opm_cod";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                access.Add(new Cls_0004_Access
                                {
                                    Id = Convert.ToInt32(reader["opm_id"]),
                                    Cod = reader["opm_cod"].ToString(),
                                    Desig = reader["opm_descr"].ToString(),
                                    Level = Convert.ToInt32(reader["opm_nivel"]),
                                    Status = reader["status_descr"].ToString(),
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while loading acessos: " + ex.Message);
                }
            }
            Lst_Acessos.ItemsSource = access;

            Btn_Edit.Visibility = Visibility.Collapsed;
            Btn_Delete.Visibility = Visibility.Collapsed;

        }

        private void Lst_Acessos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Lst_Acessos.SelectedItem != null)
            {
                // Get the selected item (row) from the ListView
                var selectedItem = (Cls_0004_Access)Lst_Acessos.SelectedItem;

                accessIndex = Lst_Acessos.Items.IndexOf(selectedItem);
                accessId = selectedItem.Id;
                accessCod = selectedItem.Cod;
                Btn_Edit.Visibility = Visibility.Visible;
                Btn_Delete.Visibility = Visibility.Visible;
                LoadAccessGranted();
            }
            else
            {
                // No item selected, reset the user_ID variable
                accessId = -1;
                Btn_Edit.Visibility = Visibility.Collapsed;
                Btn_Delete.Visibility = Visibility.Collapsed;
            }
        }

        private void Lst_Acessos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Lst_Acessos.SelectedItem != null)
            {
                if (loginUserType == "Utilizador")
                {
                    // Acesso a alterar
                    if (Cls_0005_AccessControl.AccessGranted("M20U010202", user.MenuAccess))
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
            Frm_000401_AcessosAdd frm_000401_AcessosAdd = new Frm_000401_AcessosAdd(loginUserId)
            {
                Owner = this,
            };
            frm_000401_AcessosAdd.ShowDialog();
            LoadAccess();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Frm_000402_AcessosEdit frm_000402_AcessosEdit = new Frm_000402_AcessosEdit(loginUserId, accessId)
            {
                Owner = this,
            };
            frm_000402_AcessosEdit.ShowDialog();
            LoadAccess();
            Lst_Acessos.SelectedIndex = accessIndex;
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Frm_000403_AcessosDelete acessosDelete = new Frm_000403_AcessosDelete(accessId, accessCod)
            {
                Owner = this,
            };
            acessosDelete.ShowDialog();
            LoadAccess();
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}