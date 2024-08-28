/*
Frm_0005_AddAcessos.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.1
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Windows;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_0005_AddAcessos.xaml
    /// </summary>
    public partial class Frm_0005_AddAcessos : Window
    {
        private readonly string loginUserId;
        private readonly int selectedUserId;
        public Frm_0005_AddAcessos(int selectedUserId, string loginUserId)
        {
            InitializeComponent();
            this.loginUserId = loginUserId;
            this.selectedUserId = selectedUserId;
            LoadUser();
            List<Cls_0005_AccessControl> listaAcessos = Cls_0005_AccessControl.GetAccess(selectedUserId);
            LoadAcessos(listaAcessos);
        }

        private void LoadUser()
        {
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    string query = "SELECT user_id, user_userID FROM tbl_0002_users WHERE user_id = ?";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@userID", selectedUserId);
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                int iduser = reader.GetInt32(reader.GetOrdinal("user_id"));
                                string userId = reader.GetString(reader.GetOrdinal("user_userID"));

                                lbl_IDUser.Content = iduser;
                                lbl_UserID.Content = userId.ToString();
                            }
                            else
                            {
                                MessageBox.Show("Erro ao selecionar!");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ocorreu um erro a ligar à base de dados: " + ex.Message);
                    return;
                }
            }
        }

        private void LoadAcessos(List<Cls_0005_AccessControl> listaAcessos)
        {
            try
            {
                foreach (var item in listaAcessos)
                {
                    listAcessos.Items.Add(new Cls_0007_AccessData
                    {
                        OptionId = item.OptionId,
                        OptionCod = item.OptionCod,
                        OptionDesig = item.OptionDesig,
                        OptionLevel = item.OptionLevel,
                        IdReg = item.IdReg,
                        Acs_userid = Convert.ToInt32(selectedUserId),
                        OptionAccess = item.OptionAccess,
                        Access = item.OptionAccess
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro de conversão" + ex.Message);
            }
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            var listaAcessos = new List<Cls_0005_AccessControl>();
            try
            {
                foreach (Cls_0007_AccessData acessoData in listAcessos.Items)
                {
                    // MessageBox.Show($"{acessoData.acesso} - {acessoData.opcaoAcesso}");
                    if (acessoData.Access != acessoData.OptionAccess)
                    {
                        listaAcessos.Add(new Cls_0005_AccessControl(
                            acessoData.OptionId,
                            acessoData.OptionCod,
                            acessoData.IdReg,
                            acessoData.OptionAccess = acessoData.Access));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            var userid = Convert.ToInt32(lbl_IDUser.Content);
            if (listaAcessos.Count > 0)
            {
                if (new Cls_0005_AccessControl().Save(userid, loginUserId, listaAcessos) == "Ok")
                {
                    MessageBox.Show("Acessos gravados com sucesso!");
                }
                else
                {
                    MessageBox.Show("Não foi possivel gravar! Por favor, tente mais tarde!");
                }
            }
            this.Close();
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
