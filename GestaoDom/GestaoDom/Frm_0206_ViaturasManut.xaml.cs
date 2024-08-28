/*
Frm_0206_ViaturasManut.xaml.cs
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
    /// Interaction logic for Frm_0206_ViaturasManut.xaml
    /// </summary>
    public partial class Frm_0206_ViaturasManut : Window
    {
        private static readonly Cls_0003_Users user = new Cls_0003_Users();
        private readonly int loginUserSequence;
        private readonly string loginUserId, loginUserType;
        private int selectedViaturaId, selectedViaturaIndex;
        private string selectedViaturaCod;

        public Frm_0206_ViaturasManut(int loginUserSequence, string loginUserId, string loginUserType)
        {
            InitializeComponent();
            this.loginUserSequence = loginUserSequence;
            this.loginUserId = loginUserId;
            this.loginUserType = loginUserType;
            LoadAccess();
            LoadViaturas();
        }

        private void LoadAccess()
        {
            user.MenuAccess = Cls_0005_AccessControl.GetAccess(Convert.ToInt32(loginUserSequence));
            if (loginUserType == "Utilizador")
            {
                // Acesso a adicionar
                if (Cls_0005_AccessControl.AccessGranted("M20V010101", user.MenuAccess))
                {
                    btn_Add.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Add.Visibility = Visibility.Collapsed;
                }
                // Acesso a alterar
                if (Cls_0005_AccessControl.AccessGranted("M20V010102", user.MenuAccess))
                {
                    btn_Edit.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Edit.Visibility = Visibility.Collapsed;
                }
                // Acesso a eliminar
                if (Cls_0005_AccessControl.AccessGranted("M20V010103", user.MenuAccess))
                {
                    btn_Delete.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Delete.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void LoadViaturas()
        {
            ObservableCollection<Cls_0206_Viaturas> viaturas = new ObservableCollection<Cls_0206_Viaturas>();
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros
                try
                {
                    conn.Open();
                    // Definição de procura de registos na tabela 
                    string query = "SELECT vtr_id, vtr_cod, vtr_descr, vtr_matricula, status_descr FROM tbl_0205_viaturas, tbl_0001_status WHERE vtr_status = status_cod ORDER BY vtr_descr";
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
                                viaturas.Add(new Cls_0206_Viaturas
                                {
                                    Id = Convert.ToInt32(reader["vtr_id"]),
                                    Cod = reader["vtr_cod"].ToString(),
                                    Descr = reader["vtr_descr"].ToString(),
                                    Matricula = reader["vtr_matricula"].ToString(),
                                    Status = reader["status_descr"].ToString(),
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro ao carregar dados (Erro C1045)!" + ex.Message);
                }
            }
            listViaturas.ItemsSource = viaturas;
            // Desabilitar botões por falta de seleção de item
            btn_Edit.Visibility = Visibility.Collapsed;
            btn_Delete.Visibility = Visibility.Collapsed;
        }

        private void ListViaturas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViaturas.SelectedItem != null)
            {
                // Obtem o item selecionado (linha) da ListView
                var selectedItem = (Cls_0206_Viaturas)listViaturas.SelectedItem;

                // Obtem os valores (ID column, Cod column) e guarda em variáveis
                selectedViaturaId = selectedItem.Id;
                selectedViaturaCod = selectedItem.Cod;
                selectedViaturaIndex = listViaturas.Items.IndexOf(selectedItem);
                btn_Edit.Visibility = Visibility.Visible;
                btn_Delete.Visibility = Visibility.Visible; ;
            }
            else
            {
                // No item selected, reset the user_ID variable
                selectedViaturaId = -1;
                btn_Edit.Visibility = Visibility.Collapsed;
                btn_Delete.Visibility = Visibility.Collapsed;
            }
        }

        private void ListViaturas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listViaturas.SelectedItem != null)
            {
                if (loginUserType == "Utilizador")
                {
                    // Acesso a alterar
                    if (Cls_0005_AccessControl.AccessGranted("M20V010102", user.MenuAccess))
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
            Frm_020601_ViaturaAdd viaturaAdd = new Frm_020601_ViaturaAdd(loginUserId)
            {
                Owner = this
            };
            viaturaAdd.ShowDialog();
            LoadViaturas();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Frm_020602_ViaturaEdit viaturaEdit = new Frm_020602_ViaturaEdit(loginUserId, selectedViaturaId)
            {
                Owner = this
            };
            viaturaEdit.ShowDialog();
            LoadViaturas();
            listViaturas.SelectedIndex = selectedViaturaIndex;
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Frm_020603_ViaturaDelete viaturaDelete = new Frm_020603_ViaturaDelete(selectedViaturaId, selectedViaturaCod)
            {
                Owner = this
            };
            viaturaDelete.ShowDialog();
            LoadViaturas();
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
