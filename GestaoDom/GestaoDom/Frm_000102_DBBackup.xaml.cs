/*
Frm_000102_DBBackup.xaml.cs
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
    /// Interaction logic for Frm_000102_DBBackup.xaml
    /// </summary>
    public partial class Frm_000102_DBBackup : Window
    {
        public Frm_000102_DBBackup()
        {
            InitializeComponent();
        }

        private void Btn_Backup_Click(object sender, RoutedEventArgs e)
        {


            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {

                try
                {
                    conn.Open();
                    SaveFileDialog saveDialog = new SaveFileDialog
                    {
                        Filter = "Backup Files (*.sql)|*.sql|All Files (*.*)|*.*"
                    };
                    if (saveDialog.ShowDialog() == true)
                    {
                        string backupPath = saveDialog.FileName;
                        string dbName = conn.Database;
                        string backupQuery = $"BACKUP DATABASE [{dbName}] TO DISK = '{backupPath}'";

                        using (MySqlCommand cmd = new MySqlCommand(backupQuery, conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                        System.Windows.MessageBox.Show("Backup cocluido com sucesso.");
                    }
                }
                catch (Exception)
                {
                    System.Windows.MessageBox.Show("Erro durante o backup (X1003)!");
                }
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
