/*
Frm_0103_ContasManut.xaml.cs
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
    /// Interaction logic for Frm_0103_ContasManut.xaml
    /// </summary>
    public partial class Frm_0103_ContasManut : Window
    {
        private static readonly Cls_0003_Users user = new Cls_0003_Users();
        private readonly int loginUserSequence;
        private readonly string loginUserId, loginUserType;
        private int selectedContaId, selectedContaIndex;
        private string selectedContaCod;

        public Frm_0103_ContasManut(int loginUserSequence, string loginUserId, string loginUserType)
        {
            InitializeComponent();
            this.loginUserSequence = loginUserSequence;
            this.loginUserId = loginUserId;
            this.loginUserType = loginUserType;
            LoadAccess();
            LoadContas();
        }

        // Acessos às funcionalidades
        private void LoadAccess()
        {
            user.MenuAccess = Cls_0005_AccessControl.GetAccess(Convert.ToInt32(loginUserSequence));
            if (loginUserType == "Utilizador")
            {
                // Acesso a adicionar
                if (Cls_0005_AccessControl.AccessGranted("M20C010101", user.MenuAccess))
                {
                    btn_Add.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Add.Visibility = Visibility.Collapsed;
                }
                // Acesso a alterar
                if (Cls_0005_AccessControl.AccessGranted("M20C010102", user.MenuAccess))
                {
                    btn_Edit.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Edit.Visibility = Visibility.Collapsed;
                }
                // Acesso a eliminar
                if (Cls_0005_AccessControl.AccessGranted("M20C010103", user.MenuAccess))
                {
                    btn_Delete.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Delete.Visibility = Visibility.Collapsed;
                }
            }
            // Desabilitar botões por falta de seleção de item
        }

        private void LoadContas()
        {
            ObservableCollection<Cls_0103_Contas> contas = new ObservableCollection<Cls_0103_Contas>();
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros
                try
                {
                    conn.Open();
                    // Definição de procura de registos na tabela 
                    string query = "SELECT cntcred_id, cntcred_cod, cntcred_descr, cntcred_nr, status_descr FROM tbl_0103_contascred, tbl_0001_status WHERE cntcred_status = status_cod AND cntcred_cod != 0 ORDER BY cntcred_descr";
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
                                contas.Add(new Cls_0103_Contas
                                {
                                    Id = Convert.ToInt32(reader["cntcred_id"]),
                                    Cod = reader["cntcred_cod"].ToString(),
                                    Descr = reader["cntcred_descr"].ToString(),
                                    Nr = reader["cntcred_nr"].ToString(),
                                    Status = reader["status_descr"].ToString(),
                                });
                            }
                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Erro ao carregar dados (Erro C1011)!");
                }
            }
            listContas.ItemsSource = contas;
            // Desabilitar botões por falta de seleção de item
            btn_Edit.Visibility = Visibility.Collapsed;
            btn_Delete.Visibility = Visibility.Collapsed;
        }

        private void ListContas_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listContas.SelectedItem != null)
            {
                // Obtem o item selecionado (linha) da ListView
                var selectedItem = (Cls_0103_Contas)listContas.SelectedItem;

                // Obtem os valores (ID column, Cod column) e guarda em variáveis
                selectedContaId = selectedItem.Id;
                selectedContaCod = selectedItem.Cod;
                selectedContaIndex = listContas.Items.IndexOf(selectedItem);
                btn_Edit.Visibility = Visibility.Visible;
                btn_Delete.Visibility = Visibility.Visible;
            }
            else
            {
                // No item selected, reset the user_ID variable
                selectedContaId = -1;
                btn_Edit.Visibility = Visibility.Collapsed;
                btn_Delete.Visibility = Visibility.Collapsed;
            }
        }

        private void ListContas_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listContas.SelectedItem != null)
            {
                if (loginUserType == "Utilizador")
                {
                    // Acesso a alterar
                    if (Cls_0005_AccessControl.AccessGranted("M20T010102", user.MenuAccess))
                    {
                        Btn_Edit_Click(sender, e);
                        LoadContas();
                    }
                }
                else
                {
                    Btn_Edit_Click(sender, e);
                    LoadContas();
                }
            }
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            Frm_010301_ContaAdd frm_010301_ContasAdd = new Frm_010301_ContaAdd(loginUserId)
            {
                Owner = this
            };
            frm_010301_ContasAdd.ShowDialog();
            LoadContas();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Frm_010302_ContaEdit frm_010301_ContasEdit = new Frm_010302_ContaEdit(loginUserId, selectedContaId)
            {
                Owner = this
            };
            frm_010301_ContasEdit.ShowDialog();
            LoadContas();
            listContas.SelectedIndex = selectedContaIndex;
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Frm_010303_ContaDelete contaDelete = new Frm_010303_ContaDelete(selectedContaId, selectedContaCod)
            {
                Owner = this
            };
            contaDelete.ShowDialog();
            LoadContas();
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
