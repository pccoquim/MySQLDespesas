/*
Frm_0104_TipoReceitaManut.xaml.cs
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
    /// Interaction logic for Frm_0104_TipoReceitaManut.xaml
    /// </summary>
    public partial class Frm_0104_TipoReceitaManut : Window
    {
        private static readonly Cls_0003_Users user = new Cls_0003_Users();
        private readonly int loginUserSequence;
        private readonly string loginUserId, loginUserType;
        private int selectedTipoReceitaId, selectedTipoReceitaIndex;
        private string selectedTipoReceitaCod;

        public Frm_0104_TipoReceitaManut(int loginUserSequence, string loginUserId, string loginUserType)
        {
            InitializeComponent();
            this.loginUserSequence = loginUserSequence;
            this.loginUserId = loginUserId;
            this.loginUserType = loginUserType;
            LoadAccess();
            LoadTipoReceita();
        }

        // Acessos às funcionalidades
        private void LoadAccess()
        {
            user.MenuAccess = Cls_0005_AccessControl.GetAccess(Convert.ToInt32(loginUserSequence));
            if (loginUserType == "Utilizador")
            {
                // Acesso a adicionar
                if (Cls_0005_AccessControl.AccessGranted("M20C010201", user.MenuAccess))
                {
                    btn_Add.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Add.Visibility = Visibility.Collapsed;
                }
                // Acesso a alterar
                if (Cls_0005_AccessControl.AccessGranted("M20C010202", user.MenuAccess))
                {
                    btn_Edit.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Edit.Visibility = Visibility.Collapsed;
                }
                // Acesso a eliminar
                if (Cls_0005_AccessControl.AccessGranted("M20C010203", user.MenuAccess))
                {
                    btn_Delete.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Delete.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void LoadTipoReceita()
        {
            ObservableCollection<Cls_0104_TipoReceita> tipoReceita = new ObservableCollection<Cls_0104_TipoReceita>();
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros
                try
                {
                    conn.Open();
                    // Definição de procura de registos na tabela 
                    string query = "SELECT tr_id, tr_cod, tr_descr, status_descr FROM tbl_0104_tiporeceita, tbl_0001_status WHERE tr_status = status_cod ORDER BY tr_descr";
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
                                tipoReceita.Add(new Cls_0104_TipoReceita
                                {
                                    Id = Convert.ToInt32(reader["tr_id"]),
                                    Cod = reader["tr_cod"].ToString(),
                                    Descr = reader["tr_descr"].ToString(),
                                    Status = reader["status_descr"].ToString(),
                                });
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Erro ao carregar dados (Erro C1017)!");
                }
            }
            listTipoReceita.ItemsSource = tipoReceita;
            // Desabilitar botões por falta de seleção de item
            btn_Edit.Visibility = Visibility.Collapsed;
            btn_Delete.Visibility = Visibility.Collapsed;
        }
        private void ListTipoReceita_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listTipoReceita.SelectedItem != null)
            {
                // Obtem o item selecionado (linha) da ListView
                var selectedItem = (Cls_0104_TipoReceita)listTipoReceita.SelectedItem;

                // Obtem os valores (ID column, Cod column) e guarda em variáveis
                selectedTipoReceitaId = selectedItem.Id;
                selectedTipoReceitaCod = selectedItem.Cod;
                selectedTipoReceitaIndex = listTipoReceita.Items.IndexOf(selectedItem);
                btn_Edit.Visibility = Visibility.Visible;
                btn_Delete.Visibility = Visibility.Visible;
            }
            else
            {
                // No item selected, reset the user_ID variable
                selectedTipoReceitaId = -1;
                btn_Edit.Visibility = Visibility.Collapsed;
                btn_Delete.Visibility = Visibility.Collapsed;
            }
        }

        private void ListTipoReceita_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listTipoReceita.SelectedItem != null)
            {
                if (loginUserType == "Utilizador")
                {
                    // Acesso a alterar
                    if (Cls_0005_AccessControl.AccessGranted("M20C010202", user.MenuAccess))
                    {
                        Btn_Edit_Click(sender, e);
                        LoadTipoReceita();
                    }
                }
                else
                {
                    Btn_Edit_Click(sender, e);
                    LoadTipoReceita();
                }
            }
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            Frm_010401_TipoReceitaAdd frm_010401_TipoReceitaAdd = new Frm_010401_TipoReceitaAdd(loginUserId)
            {
                Owner = this
            };
            frm_010401_TipoReceitaAdd.ShowDialog();
            LoadTipoReceita();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Frm_010402_TipoReceitaEdit frm_010402_TipoReceitaEdit = new Frm_010402_TipoReceitaEdit(loginUserId, selectedTipoReceitaId)
            {
                Owner = this
            };
            frm_010402_TipoReceitaEdit.ShowDialog();
            LoadTipoReceita();
            listTipoReceita.SelectedIndex = selectedTipoReceitaIndex;
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Frm_010403_TipoReceitaDelete tipoReceitaDelete = new Frm_010403_TipoReceitaDelete(selectedTipoReceitaId, selectedTipoReceitaCod)
            {
                Owner = this
            };
            tipoReceitaDelete.ShowDialog();
            LoadTipoReceita();
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
