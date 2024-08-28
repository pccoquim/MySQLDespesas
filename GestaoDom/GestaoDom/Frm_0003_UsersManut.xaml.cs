/*
Frm_0003_UsersManut.xaml.cs
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
    /// Lógica interna para Frm_0003_UsersManut.xaml
    /// </summary>
    public partial class Frm_0003_UsersManut : System.Windows.Window
    {
        private static readonly Cls_0003_Users user = new Cls_0003_Users();
        private readonly int loginUserSequence;
        private readonly string loginUserId;
        private readonly string loginUserType;
        private int selectedUserId, selectedUserIndex;
        private string selectedUserName;
        private string selectedUserType;
        public Frm_0003_UsersManut(int loginUserSequence, string loginUserId, string loginUserType)
        {
            InitializeComponent();
            this.loginUserSequence = loginUserSequence;
            this.loginUserId = loginUserId;
            this.loginUserType = loginUserType;
            LoadUsers();
            LoadUserAccess();
        }

        private void LoadUserAccess()
        {
            user.MenuAccess = Cls_0005_AccessControl.GetAccess(Convert.ToInt32(loginUserSequence));
            if (loginUserType == "Utilizador")
            {
                // Acesso a adicionar utilizadores
                if (Cls_0005_AccessControl.AccessGranted("M20U010101", user.MenuAccess))
                {
                    Btn_Add.Visibility = Visibility.Visible;
                }
                else
                {
                    Btn_Add.Visibility = Visibility.Collapsed;
                }
                // Acesso a alterar utilizadores
                if (Cls_0005_AccessControl.AccessGranted("M20U010102", user.MenuAccess))
                {
                    Btn_Edit.Visibility = Visibility.Visible;
                }
                else
                {
                    Btn_Edit.Visibility = Visibility.Collapsed;
                }
                // Acesso a eliminar utilizadores
                if (Cls_0005_AccessControl.AccessGranted("M20U010103", user.MenuAccess))
                {
                    Btn_Delete.Visibility = Visibility.Visible;
                }
                else
                {
                    Btn_Delete.Visibility = Visibility.Collapsed;
                }
                // Acesso a alterar assword a utilizadores
                if (Cls_0005_AccessControl.AccessGranted("M20U010104", user.MenuAccess))
                {
                    Btn_ChgPw.Visibility = Visibility.Visible;
                }
                else
                {
                    Btn_ChgPw.Visibility = Visibility.Collapsed;
                }
                // Acesso a adicionar acessos a utilizadores
                if (Cls_0005_AccessControl.AccessGranted("M20U010105", user.MenuAccess))
                {
                    Btn_AddAcessos.Visibility = Visibility.Visible;
                }
                else
                {
                    Btn_AddAcessos.Visibility = Visibility.Collapsed;
                }
            }
        }

        // Utilizadores #################################################################################################################################################
        private void LoadUsers()
        {
            ObservableCollection<Cls_0003_Users> users = new ObservableCollection<Cls_0003_Users>();

            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT user_id, user_userID, user_name, user_password, user_type, user_status, user_chgpw, user_pwcount, status_descr FROM tbl_0002_users, tbl_0001_status WHERE user_status = status_cod  ORDER BY user_userID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                users.Add(new Cls_0003_Users
                                {
                                    ID = Convert.ToInt32(reader["user_id"]),
                                    UserID = reader["user_userID"].ToString(),
                                    Name = reader["user_name"].ToString(),
                                    Password = reader["user_password"].ToString(),
                                    Type = reader["user_type"].ToString(),
                                    Status = reader["status_descr"].ToString(),
                                    ChangePassword = Convert.ToBoolean(reader["user_chgpw"]),
                                    PasswordCount = Convert.ToInt32(reader["user_pwcount"]),
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while loading users: " + ex.Message);
                }
            }
            Lst_Users.ItemsSource = users;
        }

        private void Lst_Users_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Lst_Users.SelectedItem != null)
            {
                // Get the selected item (row) from the ListView
                var selectedItem = (Cls_0003_Users)Lst_Users.SelectedItem;

                // Get the value of the first column (ID column) and store it in the user_ID variable
                selectedUserId = selectedItem.ID; // Replace "ID" with the actual property name in your data item
                selectedUserName = selectedItem.UserID;
                selectedUserIndex = Lst_Users.Items.IndexOf(selectedItem);
                selectedUserType = selectedItem.Type;

                if (selectedUserName != "Admin")
                {
                    if (selectedUserType != "Administrador")
                    {
                        Btn_Edit.Visibility = Visibility.Visible;
                        Btn_ChgPw.Visibility = Visibility.Visible;
                        Btn_Delete.Visibility = Visibility.Visible;
                        Btn_AddAcessos.Visibility = Visibility.Visible;
                        Lst_Acessos.Visibility = Visibility.Visible;
                        LoadUserAccess();
                        Lbl_UserAccess.Content = "Acessos do utilizador selecionado";
                        LoadAccess();
                    }
                    else
                    {
                        Btn_Edit.Visibility = Visibility.Visible;
                        Btn_ChgPw.Visibility = Visibility.Visible;
                        Btn_Delete.Visibility = Visibility.Visible;
                        Btn_AddAcessos.Visibility = Visibility.Collapsed;
                        LoadUserAccess();
                        Lbl_UserAccess.Content = "Os utilizadores tipo Administrador, tem acesso a todo o software";
                        Lst_Acessos.Visibility = Visibility.Collapsed;
                    }

                }
                else
                {
                    Btn_Edit.Visibility = Visibility.Collapsed;
                    Btn_ChgPw.Visibility = Visibility.Collapsed;
                    Btn_Delete.Visibility = Visibility.Collapsed;
                    Btn_AddAcessos.Visibility = Visibility.Collapsed;
                    Lbl_UserAccess.Content = "O utilizador Admin, tem acesso a todo o software";
                    Lst_Acessos.Visibility = Visibility.Collapsed;
                }
            }
            else
            {
                // No item selected, reset the user_ID variable
                selectedUserId = -1;
                Btn_Add.Visibility = Visibility.Visible;
                Btn_Edit.Visibility = Visibility.Collapsed;
                Btn_ChgPw.Visibility = Visibility.Collapsed;
                Btn_Delete.Visibility = Visibility.Collapsed;
                Btn_AddAcessos.Visibility = Visibility.Collapsed;
            }
        }

        private void Lst_Users_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (Lst_Users.SelectedItem != null)
            {
                if (loginUserType == "Administrador")
                {
                    if (selectedUserName != "Admin")
                    {
                        Btn_Edit_Click(sender, e);
                        LoadUsers();
                        Lst_Users.SelectedIndex = selectedUserIndex;
                    }
                    else
                    {
                        MessageBox.Show("O utilizador Admin é um utilizador de sistema e não pode ser alterado (à excepção da palavra-passe, no menu utilitários)!");
                        return;
                    }
                }
                else if (loginUserType == "Utilizador")
                {
                    // Acesso a alterar utilizadores
                    if (Cls_0005_AccessControl.AccessGranted("M20U010102", user.MenuAccess))
                    {
                        if (selectedUserName != "Admin")
                        {
                            Btn_Edit_Click(sender, e);
                            LoadUsers();
                            Lst_Users.SelectedIndex = selectedUserIndex;
                        }
                        else
                        {
                            MessageBox.Show("O utilizador Admin é um utilizador de sistema e não pode ser alterado (à excepção da palavra-passe, no menu utilitários)!");
                            return;
                        }
                    }
                    else
                    {

                    }
                }
            }
        }

        // Acessos ######################################################################################################################################################

        public void LoadAccess()
        {
            ObservableCollection<Cls_0006_AccessAssigned> acessos = new ObservableCollection<Cls_0006_AccessAssigned>();
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT acs_id, acs_userID, acs_cod,  acs_acesso, opm_descr, opm_nivel FROM tbl_0004_acessos, tbl_0003_opcoesAcesso WHERE acs_cod = opm_cod AND acs_userID = ? ORDER BY acs_userID";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@acsuserid", selectedUserId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                acessos.Add(new Cls_0006_AccessAssigned
                                {
                                    Id = Convert.ToInt32(reader["acs_id"]),
                                    UserId = reader["acs_userID"].ToString(),
                                    Cod = reader["acs_cod"].ToString(),
                                    Access = Convert.ToBoolean(reader["acs_acesso"]),
                                    Desig = reader["opm_descr"].ToString(),
                                    Level = Convert.ToInt32(reader["opm_nivel"]),
                                });
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error while loading users: " + ex.Message);
                }
            }
            Lst_Acessos.ItemsSource = acessos;
        }

        // Botões #########################################################################################################################################################
        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Btn_Add_Click(object sender, RoutedEventArgs e)
        {
            Frm_000301_UserAdd frm_000301_UsersAdd = new Frm_000301_UserAdd(loginUserId)
            {
                Owner = this
            };
            frm_000301_UsersAdd.ShowDialog();
            LoadUsers();
        }

        private void Btn_Edit_Click(object sender, RoutedEventArgs e)
        {
            if (selectedUserName != "Admin")
            {
                Frm_000302_UserEdit frm_000302_UsersEdit = new Frm_000302_UserEdit(selectedUserId, loginUserId)
                {
                    Owner = this
                };
                frm_000302_UsersEdit.ShowDialog();
                LoadUsers();
                Lst_Users.SelectedIndex = selectedUserIndex;
            }
            else
            {
                MessageBox.Show("O utilizador Admin é um utilizador de sistema e não pode ser alterado (à excepção da palavra-passe, no menu utilitários)!");
                return;
            }
        }

        private void Btn_Delete_Click(object sender, RoutedEventArgs e)
        {
            Frm_000303_UserDelete userDelete = new Frm_000303_UserDelete(selectedUserId)
            {
                Owner = this
            };
            userDelete.ShowDialog();
            LoadUsers();
        }

        private void Btn_ChgPw_Click(object sender, RoutedEventArgs e)
        {
            Frm_000306_AdminChgUserPw ChgPw = new Frm_000306_AdminChgUserPw(selectedUserId, selectedUserName, loginUserId)
            {
                Owner = this
            };
            ChgPw.ShowDialog();
            Lst_Users.SelectedIndex = selectedUserIndex;
        }

        private void Btn_AddAcessos_Click(object sender, RoutedEventArgs e)
        {
            Frm_0005_AddAcessos frm_0005_AddAcessos = new Frm_0005_AddAcessos(selectedUserId, loginUserId)
            {
                Owner = this
            };
            frm_0005_AddAcessos.ShowDialog();
            LoadAccess();
        }
    }
}
