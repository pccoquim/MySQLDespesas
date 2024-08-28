/*
Frm_0204_UnidadesManut.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_0204_UnidadesManut.xaml
    /// </summary>
    public partial class Frm_0204_UnidadesManut : Window
    {
        private readonly int loginUserSequence;
        private readonly string loginUserId, loginUserType;
        private int selectedUnidadeId, selectedUnidadeIndex;
        private string selectedUnidadeCod;
        public static Cls_0003_Users user = new Cls_0003_Users();
        public Frm_0204_UnidadesManut(int loginUserSequence, string loginUserId, string loginUserType)
        {
            InitializeComponent();

            this.loginUserSequence = loginUserSequence;
            this.loginUserId = loginUserId;
            this.loginUserType = loginUserType;
            LoadAccess();
            LoadUnidades();
        }

        private void LoadAccess()
        {
            user.MenuAccess = Cls_0005_AccessControl.GetAccess(Convert.ToInt32(loginUserSequence));
            if (loginUserType == "Utilizador")
            {
                // Acesso a adicionar
                if (Cls_0005_AccessControl.AccessGranted("M20M010601", user.MenuAccess))
                {
                    btn_Add.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Add.Visibility = Visibility.Collapsed;
                }
                // Acesso a alterar
                if (Cls_0005_AccessControl.AccessGranted("M20M010602", user.MenuAccess))
                {
                    btn_Edit.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Edit.Visibility = Visibility.Collapsed;
                }
                // Acesso a eliminar
                if (Cls_0005_AccessControl.AccessGranted("M20M010603", user.MenuAccess))
                {
                    btn_Delete.Visibility = Visibility.Visible;
                }
                else
                {
                    btn_Delete.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void LoadUnidades()
        {
            ObservableCollection<Cls_0204_Unidades> unidades = new ObservableCollection<Cls_0204_Unidades>();
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                // Controlo de erros
                try
                {
                    conn.Open();
                    // Definição de procura de registos na tabela 
                    string query = "SELECT uni_id, uni_cod, uni_descr, uni_peso, uni_volume, status_descr FROM tbl_0204_unidades, tbl_0001_status WHERE uni_status = status_cod ORDER BY uni_descr";
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
                                decimal peso = Convert.ToDecimal(reader["uni_peso"]);
                                decimal volume = Convert.ToDecimal(reader["uni_volume"]);

                                CultureInfo culture = CultureInfo.CreateSpecificCulture("en-US");
                                // Atribui a variaveis os dados da tabela para apresentar 
                                unidades.Add(new Cls_0204_Unidades
                                {
                                    U_Id = Convert.ToInt32(reader["uni_id"]),
                                    U_Cod = reader["uni_cod"].ToString(),
                                    U_Descr = reader["uni_descr"].ToString(),
                                    U_Peso = peso.ToString("0.0000", culture),
                                    U_Volume = volume.ToString("0.0000", culture),
                                    U_Status = reader["status_descr"].ToString(),
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
            listUnidades.ItemsSource = unidades;
            // Desabilitar botões por falta de seleção de item
            btn_Edit.Visibility = Visibility.Collapsed;
            btn_Delete.Visibility = Visibility.Collapsed;
        }

        private void ListUnidades_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listUnidades.SelectedItem != null)
            {
                // Obtem o item selecionado (linha) da ListView
                var selectedItem = (Cls_0204_Unidades)listUnidades.SelectedItem;

                // Obtem os valores (ID column, Cod column) e guarda em variáveis
                selectedUnidadeId = selectedItem.U_Id;
                selectedUnidadeCod = selectedItem.U_Cod;
                selectedUnidadeIndex = listUnidades.Items.IndexOf(selectedItem);
                btn_Edit.Visibility = Visibility.Visible;
                btn_Delete.Visibility = Visibility.Visible;
            }
            else
            {
                // No item selected, reset the user_ID variable
                selectedUnidadeId = -1;
                btn_Edit.Visibility = Visibility.Collapsed;
                btn_Delete.Visibility = Visibility.Collapsed;
            }
        }

        private void ListUnidades_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (listUnidades.SelectedItem != null)
            {
                if (loginUserType == "Utilizador")
                {
                    // Acesso a alterar
                    if (Cls_0005_AccessControl.AccessGranted("M20M010602", user.MenuAccess))
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
            Frm_020401_UnidadeAdd frm_020401_UnidadesAdd = new Frm_020401_UnidadeAdd(loginUserId)
            {
                Owner = this,
            };
            frm_020401_UnidadesAdd.ShowDialog();
            LoadUnidades();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Frm_020402_UnidadeEdit frm_020402_UnidadesEdit = new Frm_020402_UnidadeEdit(loginUserId, selectedUnidadeId)
            {
                Owner = this
            };
            frm_020402_UnidadesEdit.ShowDialog();
            LoadUnidades();
            listUnidades.SelectedIndex = selectedUnidadeIndex;
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Frm_020403_UnidadeDelete frm_020403_UnidadesDelete = new Frm_020403_UnidadeDelete(selectedUnidadeId, selectedUnidadeCod)
            {
                Owner = this
            };
            frm_020403_UnidadesDelete.ShowDialog();
            LoadUnidades();
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
