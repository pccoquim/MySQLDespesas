/*
Frm_0102_TerceirosManut.xaml.cs
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
    /// Interaction logic for Frm_0102_TerceirosManut.xaml
    /// </summary>
    public partial class Frm_0102_TerceirosManut : Window
    {
        private static readonly Cls_0003_Users user = new Cls_0003_Users();
        private readonly int loginUserSequence;
        private readonly string loginUserId;
        private readonly string loginUserType;
        private int selectedTerceiroId, selectedTerceiroIndex;
        private string selectedTerceiroCod;

        public Frm_0102_TerceirosManut(int loginUserSequence, string loginUserId, string loginUserType)
        {
            InitializeComponent();
            this.loginUserSequence = loginUserSequence;
            this.loginUserId = loginUserId;
            this.loginUserType = loginUserType;
            LoadTerceiros();
            LoadAccess();
        }

        // Acessos às funcionalidades
        private void LoadAccess()
        {
            user.MenuAccess = Cls_0005_AccessControl.GetAccess(Convert.ToInt32(loginUserSequence));
            if (loginUserType == "Utilizador")
            {
                // Acesso a adicionar acessos
                if (Cls_0005_AccessControl.AccessGranted("M20T010201", user.MenuAccess))
                {
                    btn_Add.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Add.Visibility = Visibility.Collapsed;
                }
                // Acesso a alterar acessos
                if (Cls_0005_AccessControl.AccessGranted("M20T010202", user.MenuAccess))
                {
                    btn_Edit.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Edit.Visibility = Visibility.Collapsed;
                }
                // Acesso a eliminar acessos
                if (Cls_0005_AccessControl.AccessGranted("M20T010203", user.MenuAccess))
                {
                    btn_Delete.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Delete.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void LoadTerceiros()
        {
            ObservableCollection<Cls_0102_Terceiro> terceiro = new ObservableCollection<Cls_0102_Terceiro>();

            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT terc_id, terc_cod, terc_descr, terc_codtipo, terc_morada1, terc_morada2, terc_cp, terc_localidade, terc_nif, terc_tlf, terc_email, tipoterc_Descr, status_descr FROM tbl_0102_terceiros, tbl_0101_tipoterceiro, tbl_0001_status WHERE terc_codtipo = tipoterc_cod AND terc_status = status_cod ORDER BY terc_descr";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                terceiro.Add(new Cls_0102_Terceiro
                                {
                                    ID = Convert.ToInt32(reader["terc_id"]),
                                    Cod = reader["terc_cod"].ToString(),
                                    Descr = reader["terc_descr"].ToString(),
                                    Tipo = reader["tipoterc_descr"].ToString(),
                                    Morada1 = reader["terc_morada1"].ToString(),
                                    Morada2 = reader["terc_morada2"].ToString(),
                                    CP = reader["terc_cp"].ToString(),
                                    Localidade = reader["terc_localidade"].ToString(),
                                    NIF = reader["terc_nif"].ToString(),
                                    Tlf = reader["terc_tlf"].ToString(),
                                    Email = reader["terc_email"].ToString(),
                                    Status = reader["status_descr"].ToString(),
                                });
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Erro ao carregar terceiros (Erro C1005)!");
                }
            }
            listTerceiro.ItemsSource = terceiro;
            // Desabilitar botões por falta de seleção de item
            btn_Edit.Visibility = Visibility.Collapsed;
            btn_Delete.Visibility = Visibility.Collapsed;
        }

        private void ListTerceiro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listTerceiro.SelectedItem != null)
            {
                // Get the selected item (row) from the ListView
                var selectedItem = (Cls_0102_Terceiro)listTerceiro.SelectedItem;

                // Get the value of the first column (ID column) and store it in the user_ID variable
                selectedTerceiroId = selectedItem.ID; // Replace "ID" with the actual property name in your data item
                selectedTerceiroCod = selectedItem.Cod;
                selectedTerceiroIndex = listTerceiro.Items.IndexOf(selectedItem);
                btn_Edit.Visibility = Visibility.Visible;
                btn_Delete.Visibility = Visibility.Visible;
            }
            else
            {
                // No item selected, reset the user_ID variable
                selectedTerceiroId = -1;
                btn_Edit.Visibility = Visibility.Collapsed;
                btn_Delete.Visibility = Visibility.Collapsed;
            }
        }

        private void ListTerceiro_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listTerceiro.SelectedItem != null)
            {
                if (loginUserType == "Utilizador")
                {
                    // Acesso a alterar
                    if (Cls_0005_AccessControl.AccessGranted("M20T010102", user.MenuAccess))
                    {
                        Btn_Edit_Click(sender, e);
                    }
                }
                else
                {
                    Btn_Edit_Click(sender, e);
                    listTerceiro.SelectedIndex = selectedTerceiroIndex;
                }
            }
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            Frm_010201_TerceiroAdd frm_010201_TerceirosAdd = new Frm_010201_TerceiroAdd(loginUserId)
            {
                Owner = this
            };
            frm_010201_TerceirosAdd.ShowDialog();
            LoadTerceiros();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Frm_010202_TerceiroEdit terceiroEdit = new Frm_010202_TerceiroEdit(loginUserId, selectedTerceiroId)
            {
                Owner = this
            };
            terceiroEdit.ShowDialog();
            LoadTerceiros();
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Frm_010203_TerceiroDelete terceiroDelete = new Frm_010203_TerceiroDelete(selectedTerceiroId, selectedTerceiroCod)
            {
                Owner = this,
            };
            terceiroDelete.ShowDialog();
            LoadTerceiros();
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
