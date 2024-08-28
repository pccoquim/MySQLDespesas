/*
Frm_0400_CreditosManut.xaml.cs
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
    /// Interaction logic for Frm_0400_CreditosManut.xaml
    /// </summary>
    public partial class Frm_0400_CreditosManut : Window
    {
        public static Cls_0003_Users user = new Cls_0003_Users();
        private readonly int loginUserSequence;
        private readonly string loginUserId, loginUserType;
        private int selectedCredId, selectedCredIndex, selectedCredRef;

        public Frm_0400_CreditosManut(int loginUserSequence, string loginUserId, string loginUserType)
        {
            InitializeComponent();
            this.loginUserSequence = loginUserSequence;
            this.loginUserId = loginUserId;
            this.loginUserType = loginUserType;
            LoadAccess();
            LoadCreditos();
        }

        private void LoadAccess()
        {
            user.MenuAccess = Cls_0005_AccessControl.GetAccess(Convert.ToInt32(loginUserSequence));
            if (loginUserType == "Utilizador")
            {
                // Acesso a adicionar 
                if (Cls_0005_AccessControl.AccessGranted("M25C0101", user.MenuAccess))
                {
                    Btn_Add.Visibility = Visibility.Visible;
                }
                else
                {
                    Btn_Add.Visibility = Visibility.Collapsed;
                }
                // Acesso a alterar 
                if (Cls_0005_AccessControl.AccessGranted("M25C0102", user.MenuAccess))
                {
                    Btn_Edit.Visibility = Visibility.Visible;
                }
                else
                {
                    Btn_Edit.Visibility = Visibility.Collapsed;
                }
                // Acesso a eliminar 
                if (Cls_0005_AccessControl.AccessGranted("M25C0103", user.MenuAccess))
                {
                    Btn_Delete.Visibility = Visibility.Visible;
                }
                else
                {
                    Btn_Delete.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void LoadCreditos()
        {
            ObservableCollection<Cls_0400_Creditos> creditos = new ObservableCollection<Cls_0400_Creditos>();

            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT mc_id, mc_cred_id, mc_codterc, mc_numerodoc, mc_datamov, mc_codtiporeceita, mc_contacredito, mc_contadebito, mc_valor, mc_transf, mc_status, terc_descr, tr_descr, cntcred_descr, cntdeb_descr, status_descr " +
                        "FROM tbl_0402_movimentoscredito " +
                        "LEFT JOIN tbl_0102_terceiros ON tbl_0402_movimentoscredito.mc_codterc = tbl_0102_terceiros.terc_cod " +
                        "LEFT JOIN tbl_0104_tiporeceita ON tbl_0402_movimentoscredito.mc_codtiporeceita = tbl_0104_tiporeceita.tr_cod " +
                        "LEFT JOIN tbl_0103_contascred ON tbl_0402_movimentoscredito.mc_contacredito = tbl_0103_contascred.cntcred_cod " +
                        "LEFT JOIN tbl_0103_contasdeb ON tbl_0402_movimentoscredito.mc_contadebito = tbl_0103_contasdeb.cntdeb_cod " +
                        "LEFT JOIN tbl_0001_status ON tbl_0402_movimentoscredito.mc_status = tbl_0001_status.status_cod " +
                        "ORDER BY mc_cred_id DESC";

                    using (MySqlCommand command = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string dataDB = reader["mc_datamov"].ToString();

                                if (DateTime.TryParseExact(dataDB, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataFormatada))
                                {
                                    string dataFormatadaParaUI = dataFormatada.ToString("dd.MM.yyyy");

                                    creditos.Add(new Cls_0400_Creditos
                                    {
                                        Id = Convert.ToInt32(reader["mc_id"]),
                                        Ref = Convert.ToInt32(reader["mc_cred_id"]),
                                        Transf = Convert.ToBoolean(reader["mc_transf"]),
                                        DescrTipoReceita = reader["tr_descr"].ToString(),
                                        DescrTerc = reader["terc_descr"].ToString(),
                                        NumDoc = reader["mc_numerodoc"].ToString(),
                                        Data = dataFormatadaParaUI, // Aqui, definimos a data formatada para a propriedade "Data".
                                        DescrContaO = reader["cntdeb_descr"].ToString(),
                                        DescrContaD = reader["cntcred_descr"].ToString(),
                                        Valor = Convert.ToDecimal(reader["mc_valor"]),
                                        Status = reader["status_descr"].ToString(),
                                    });
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show("Data inválida!");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao carregar dados (C1063) " + ex.Message);
                }
            }
            Lst_Creditos.ItemsSource = creditos;
            Btn_Edit.Visibility = Visibility.Collapsed;
            Btn_Delete.Visibility = Visibility.Collapsed;
        }

        private void Lst_Creditos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Lst_Creditos.SelectedItem != null)
            {
                // Obtem o item selecionado (linha) da ListView
                var selectedItem = (Cls_0400_Creditos)Lst_Creditos.SelectedItem;

                // Obtem os valores (ID column, Cod column) e guarda em variáveis
                selectedCredId = selectedItem.Id;
                selectedCredRef = selectedItem.Ref;
                selectedCredIndex = Lst_Creditos.Items.IndexOf(selectedItem);
                Btn_Edit.Visibility = Visibility.Visible;
                Btn_Delete.Visibility = Visibility.Visible;
            }
            else
            {
                // No item selected, reset the user_ID variable
                selectedCredId = -1;
                Btn_Edit.Visibility = Visibility.Collapsed;
                Btn_Delete.Visibility = Visibility.Collapsed;
            }
        }

        private void Lst_Creditos_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Lst_Creditos.SelectedItem != null)
            {
                if (loginUserType == "Utilizador")
                {
                    // Acesso a alterar
                    if (Cls_0005_AccessControl.AccessGranted("M25C0102", user.MenuAccess))
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
            Frm_040001_CreditoAdd creditoAdd = new Frm_040001_CreditoAdd(loginUserId)
            {
                Owner = this
            };
            creditoAdd.ShowDialog();
            LoadCreditos();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            Frm_040002_CreditoEdit creditoEdit = new Frm_040002_CreditoEdit(loginUserId, selectedCredId)
            {
                Owner = this
            };
            creditoEdit.ShowDialog();
            LoadCreditos();
            Lst_Creditos.SelectedIndex = selectedCredIndex;
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Frm_040003_CreditoDelete creditoDelete = new Frm_040003_CreditoDelete(selectedCredId)
            {
                Owner = this,
            };
            creditoDelete.ShowDialog();
            LoadCreditos();
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
