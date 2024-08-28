/*
Frm_0101_TipoTerceiroManut.xaml.cs
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
    /// Interaction logic for Frm_0101_TipoTerceiroManut.xaml
    /// </summary>
    public partial class Frm_0101_TipoTerceiroManut : Window
    {
        private static readonly Cls_0003_Users user = new Cls_0003_Users();
        private readonly int loginUserSequence;
        private readonly string loginUserId, loginUserType;
        private int selectedTipoTercId, selectedTipoTercIndex;
        private string selectedTipoTercCod;

        public Frm_0101_TipoTerceiroManut(int loginUserSequence, string loginUserId, string loginUserType)
        {
            InitializeComponent();
            this.loginUserSequence = loginUserSequence;
            this.loginUserId = loginUserId;
            this.loginUserType = loginUserType;
            LoadAccess();
            LoadTipoTerceiro();
        }

        private void LoadAccess()
        {
            user.MenuAccess = Cls_0005_AccessControl.GetAccess(Convert.ToInt32(loginUserSequence));
            if (loginUserType == "Utilizador")
            {
                // Acesso a adicionar acessos
                if (Cls_0005_AccessControl.AccessGranted("M20T010101", user.MenuAccess))
                {
                    btn_Add.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Add.Visibility = Visibility.Collapsed;
                }
                // Acesso a alterar acessos
                if (Cls_0005_AccessControl.AccessGranted("M20T010102", user.MenuAccess))
                {
                    btn_Edit.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Edit.Visibility = Visibility.Collapsed;
                }
                // Acesso a eliminar acessos
                if (Cls_0005_AccessControl.AccessGranted("M20T010103", user.MenuAccess))
                {
                    btn_Delete.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Delete.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void LoadTipoTerceiro()
        {
            ObservableCollection<Cls_0101_TipoTerceiro> tipoTerceiro = new ObservableCollection<Cls_0101_TipoTerceiro>();

            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT tipoterc_id, tipoterc_cod, tipoterc_descr, status_descr FROM tbl_0101_tipoterceiro, tbl_0001_status WHERE tipoterc_status = status_cod ORDER BY tipoterc_cod";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                tipoTerceiro.Add(new Cls_0101_TipoTerceiro
                                {
                                    ID = Convert.ToInt32(reader["tipoterc_id"]),
                                    Cod = reader["tipoterc_cod"].ToString(),
                                    Descr = reader["tipoterc_descr"].ToString(),
                                    Status = reader["status_descr"].ToString(),
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar tipos de terceiros: " + ex.Message);
                }
            }
            listTipoTerceiro.ItemsSource = tipoTerceiro;

            btn_Edit.Visibility = Visibility.Collapsed;
            btn_Delete.Visibility = Visibility.Collapsed;
        }

        private void ListTipoTerceiro_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listTipoTerceiro.SelectedItem != null)
            {
                // Get the selected item (row) from the ListView
                var selectedItem = (Cls_0101_TipoTerceiro)listTipoTerceiro.SelectedItem;

                // Get the value of the first column (ID column) and store it in the user_ID variable
                selectedTipoTercId = selectedItem.ID; // Replace "ID" with the actual property name in your data item
                selectedTipoTercCod = selectedItem.Cod;
                selectedTipoTercIndex = listTipoTerceiro.Items.IndexOf(selectedItem);
                btn_Edit.Visibility = Visibility.Visible;
                btn_Delete.Visibility = Visibility.Visible;
            }
            else
            {
                // No item selected, reset the user_ID variable
                selectedTipoTercId = -1;
                btn_Edit.Visibility = Visibility.Collapsed;
                btn_Delete.Visibility = Visibility.Collapsed;
            }
        }

        private void ListTipoTerceiro_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listTipoTerceiro.SelectedItem != null)
            {
                Btn_Edit_Click(sender, e);
            }
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            Frm_010101_TipoTerceiroAdd frm_010101_TipoTerceiroAdd = new Frm_010101_TipoTerceiroAdd(loginUserId);
            frm_010101_TipoTerceiroAdd.ShowDialog();
            LoadTipoTerceiro();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            int used = 0;
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // Definição de procura de registos na tabela clientes
                    string query = "SELECT terc_codtipo FROM tbl_0102_terceiros WHERE terc_codtipo = ?";
                    // Define a consulta e a ligação
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@tipotercID", selectedTipoTercCod);
                        object result = cmd.ExecuteScalar();
                        used = result != null ? 1 : 0;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao ligar à base de dados: " + ex.Message);
                }
            }
            if (used > 0)
            {
                MessageBox.Show("Não é possível alterar o tipo de terceiro! Já foi atribuido a pelo menos um terceiro!");
                return;
            }
            else
            {
                if (loginUserType == "Utilizador")
                {
                    // Acesso a alterar
                    if (Cls_0005_AccessControl.AccessGranted("M20T010102", user.MenuAccess))
                    {
                        Frm_010102_TipoTerceiroEdit frm_010102_TipoTerceiroEdit = new Frm_010102_TipoTerceiroEdit(loginUserId, selectedTipoTercId)
                        {
                            Owner = this
                        };
                        frm_010102_TipoTerceiroEdit.ShowDialog();
                        LoadTipoTerceiro();
                        listTipoTerceiro.SelectedIndex = selectedTipoTercIndex;
                    }
                }
                else
                {
                    Frm_010102_TipoTerceiroEdit frm_010102_TipoTerceiroEdit = new Frm_010102_TipoTerceiroEdit(loginUserId, selectedTipoTercId)
                    {
                        Owner = this
                    };
                    frm_010102_TipoTerceiroEdit.ShowDialog();
                    LoadTipoTerceiro();
                    listTipoTerceiro.SelectedIndex = selectedTipoTercIndex;
                }
            }
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Frm_010103_TipoTerceiroDelete frm_010103_TipoTerceiroDelete = new Frm_010103_TipoTerceiroDelete(selectedTipoTercId, selectedTipoTercCod)
            {
                Owner = this
            };
            frm_010103_TipoTerceiroDelete.ShowDialog();
            LoadTipoTerceiro();
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
