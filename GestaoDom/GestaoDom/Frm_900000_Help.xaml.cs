/*
Frm_900000_Help.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Versão: 1.0.0
*/
using System;
using System.Reflection;
using System.Windows;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_900000_Help.xaml
    /// </summary>
    public partial class Frm_900000_Help : Window
    {
        public Frm_900000_Help()
        {
            InitializeComponent();

            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            // Atribui a versão aos rótulos
            Lbl_VersionN.Content = version.ToString();
        }
    }
}
