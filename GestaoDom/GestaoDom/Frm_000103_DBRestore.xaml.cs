/*
Frm_000103_DBRestore.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.1
*/
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Windows;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_000103_DBRestore.xaml
    /// </summary>
    public partial class Frm_000103_DBRestore : Window
    {
        public Frm_000103_DBRestore()
        {
            InitializeComponent();
        }

        private void Btn_Restore_Click(object sender, RoutedEventArgs e)
        {
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    OpenFileDialog openDialog = new OpenFileDialog
                    {
                        Filter = "Backup Files (*.bak)|*.bak|All Files (*.*)|*.*"
                    };
                    if (openDialog.ShowDialog() == true)
                    {
                        string backupPath = openDialog.FileName;
                        string dbName = "OrcDom";
                        string restoreQuery = $"USE master RESTORE DATABASE [{dbName}] FROM DISK = '{backupPath}' WITH REPLACE";

                        using (MySqlCommand cmd = new MySqlCommand(restoreQuery, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        System.Windows.MessageBox.Show("Restauração concluida com sucesso.");
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro na restauração (X1002)!" + ex.Message);
                }
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
