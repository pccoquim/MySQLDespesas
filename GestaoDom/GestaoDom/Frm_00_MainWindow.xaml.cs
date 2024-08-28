/*
#################################################################
#   Projeto:        GestaoDom                                   #
#   Arquivo:        Frm_00_MainWindow.xaml.cs                   #
#   Autor:          Paulo da Cruz Coquim                        #
#   Data:           07.06.2024                                  #
#   Data alteração: 28.08.2024                                  #
#   Versão:         1.0.2.0                                     #
#################################################################
*/

using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using Application = System.Windows.Application;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Frm_00_MainWindow : Window
    {
        public static Cls_0003_Users user = new Cls_0003_Users();
        private bool login;
        private int loginUserSequence;
        private string loginUserId, loginUserType;

        private WindowState _previousWindowState;
        private Rect _previousWindowRect;
        private bool _isMaximized;

        public Frm_00_MainWindow()
        {
            InitializeComponent();

            M20.Visibility = Visibility.Collapsed;
            M25.Visibility = Visibility.Collapsed;
            M28.Visibility = Visibility.Collapsed;
            M30.Visibility = Visibility.Collapsed;
            M40.Visibility = Visibility.Collapsed;

            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            // Atribui a versão as etiquetas
            Lbl_Version.Content = "Version:";
            Lbl_VersionN.Content = version.ToString();

            
            Closing += MainWindow_Closing;

            _previousWindowState = this.WindowState;
            _previousWindowRect = new Rect(this.Left, this.Top, this.Width, this.Height);
            _isMaximized = false;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            if (Application.Current.Windows.OfType<Frm_0002_Login>().Any() == false)
            {
                Login();
                M20.Visibility = Visibility.Collapsed;
                M25.Visibility = Visibility.Collapsed;
                M28.Visibility = Visibility.Collapsed;
                M30.Visibility = Visibility.Collapsed;
                M40.Visibility = Visibility.Collapsed;
            }
            RefreshLogin();

            // Salva estado e dimensões da janela
            _previousWindowState = this.WindowState;
            _previousWindowRect = new Rect(this.Left, this.Top, this.Width, this.Height);

            // Guarda a area de trabalho do ecrãn
            var screen = System.Windows.Forms.Screen.FromHandle(new WindowInteropHelper(this).Handle);
            var workArea = screen.WorkingArea;

            // Define o tamanho da janela para o tamanho da area de trabalho
            this.WindowState = WindowState.Normal;
            this.Left = workArea.Left;
            this.Top = workArea.Top;
            this.Width = workArea.Width;
            this.Height = workArea.Height;

            // Define que a janela é maximizada
            _isMaximized = true;
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (ShowConfirmation("Tem a certeza que deseja encerrar a aplicação?"))
            {
                Application.Current.Shutdown();
            }
            else
            {
                e.Cancel = true;
            }
        }

        // Verifica as autorizações dos utilizadores para acesso aos formulários
        public void RefreshLogin()
        {
            user.MenuAccess = Cls_0005_AccessControl.GetAccess(Convert.ToInt32(loginUserSequence));
            // Exibir o valor da variável de nome de utilizador na etiqueta
            Lbl_UserLogin.Content = loginUserId;
            if (login == false)
            {
                M01.Visibility = Visibility.Visible;
                M01L01.Visibility = Visibility.Visible;
                M01O01.Visibility = Visibility.Collapsed;
                M01S01.Visibility = Visibility.Visible;
                M20.Visibility = Visibility.Collapsed;
                M25.Visibility = Visibility.Collapsed;
                M28.Visibility = Visibility.Collapsed;
                M30.Visibility = Visibility.Collapsed;
                M40.Visibility = Visibility.Collapsed;
                M90.Visibility = Visibility.Visible;
            }
            else if (loginUserType == "Administrador")
            {
                M01.Visibility = Visibility.Visible;
                M01L01.Visibility = Visibility.Collapsed;
                M01O01.Visibility = Visibility.Visible;
                M01S01.Visibility = Visibility.Visible;
                M20.Visibility = Visibility.Visible;
                M25.Visibility = Visibility.Visible;
                M28.Visibility = Visibility.Visible;
                M30.Visibility = Visibility.Visible;
                M40.Visibility = Visibility.Visible;
                M90.Visibility = Visibility.Visible;
            }
            else if (loginUserType == "Utilizador")
            {

                M01L01.Visibility = Visibility.Collapsed;
                M01O01.Visibility = Visibility.Visible;
                M01S01.Visibility = Visibility.Visible;


                // Menu Ficheiro ############################################################################################################################################
                if (Cls_0005_AccessControl.AccessGranted("M01", user.MenuAccess))
                {
                    M01.Visibility = Visibility.Visible;
                }
                else
                {
                    M01.Visibility = Visibility.Collapsed;
                }
                // Menu Entidades ############################################################################################################################################
                if (Cls_0005_AccessControl.AccessGranted("M20", user.MenuAccess))
                {
                    M20.Visibility = Visibility.Visible;
                }
                else
                {
                    M20.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 2 Entidades/Utilizadores
                if (Cls_0005_AccessControl.AccessGranted("M20U01", user.MenuAccess))
                {
                    M20U01.Visibility = Visibility.Visible;
                }
                else
                {
                    M20U01.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 3 Entidades/Utilizadores/Manutenção utilizadores
                if (Cls_0005_AccessControl.AccessGranted("M20U0101", user.MenuAccess))
                {
                    M20U0101.Visibility = Visibility.Visible;
                }
                else
                {
                    M20U0101.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 3 Entidades/Utilizadores/Manutenção acessos
                if (Cls_0005_AccessControl.AccessGranted("M20U0102", user.MenuAccess))
                {
                    M20U0102.Visibility = Visibility.Visible;
                }
                else
                {
                    M20U0102.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 2 Entidades/Terceiros
                if (Cls_0005_AccessControl.AccessGranted("M20T01", user.MenuAccess))
                {
                    M20T01.Visibility = Visibility.Visible;
                }
                else
                {
                    M20T01.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 3 Entidades/Terceiros/Manutenção tipos terceiro
                if (Cls_0005_AccessControl.AccessGranted("M20T0101", user.MenuAccess))
                {
                    M20T0101.Visibility = Visibility.Visible;
                }
                else
                {
                    M20T0101.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 3 Entidades/Terceiros/Manutenção terceiro
                if (Cls_0005_AccessControl.AccessGranted("M20T0102", user.MenuAccess))
                {
                    M20T0102.Visibility = Visibility.Visible;
                }
                else
                {
                    M20T0102.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 2 Entidades/Contas
                if (Cls_0005_AccessControl.AccessGranted("M20C01", user.MenuAccess))
                {
                    M20C01.Visibility = Visibility.Visible;
                }
                else
                {
                    M20C01.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 3 Entidades/Contas/Manutenção contas
                if (Cls_0005_AccessControl.AccessGranted("M20C0101", user.MenuAccess))
                {
                    M20C0101.Visibility = Visibility.Visible;
                }
                else
                {
                    M20C0101.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 2 Entidades/Movimentação
                if (Cls_0005_AccessControl.AccessGranted("M20M01", user.MenuAccess))
                {
                    M20M01.Visibility = Visibility.Visible;
                }
                else
                {
                    M20M01.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 3 Entidades/Movimentação/Manutenção geral
                if (Cls_0005_AccessControl.AccessGranted("M20M0101", user.MenuAccess))
                {
                    M20M0101.Visibility = Visibility.Visible;
                }
                else
                {
                    M20M0101.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 3 Entidades/Movimentação/Manutenção familias
                if (Cls_0005_AccessControl.AccessGranted("M20M0102", user.MenuAccess))
                {
                    M20M0102.Visibility = Visibility.Visible;
                }
                else
                {
                    M20M0102.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 3 Entidades/Movimentação/Manutenção subfamilias
                if (Cls_0005_AccessControl.AccessGranted("M20M0103", user.MenuAccess))
                {
                    M20M0103.Visibility = Visibility.Visible;
                }
                else
                {
                    M20M0103.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 3 Entidades/Movimentação/Manutenção gupos
                if (Cls_0005_AccessControl.AccessGranted("M20M0104", user.MenuAccess))
                {
                    M20M0104.Visibility = Visibility.Visible;
                }
                else
                {
                    M20M0104.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 3 Entidades/Movimentação/Manutenção artigos
                if (Cls_0005_AccessControl.AccessGranted("M20M0105", user.MenuAccess))
                {
                    M20M0105.Visibility = Visibility.Visible;
                }
                else
                {
                    M20M0105.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 3 Entidades/Movimentação/Manutenção unidades
                if (Cls_0005_AccessControl.AccessGranted("M20M0106", user.MenuAccess))
                {
                    M20M0106.Visibility = Visibility.Visible;
                }
                else
                {
                    M20M0106.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 2 Entidades/Viaturas
                if (Cls_0005_AccessControl.AccessGranted("M20V01", user.MenuAccess))
                {
                    M20V01.Visibility = Visibility.Visible;
                }
                else
                {
                    M20V01.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 3 Entidades/Viaturas/Manutenção de viaturas
                if (Cls_0005_AccessControl.AccessGranted("M20C0101", user.MenuAccess))
                {
                    M20V0101.Visibility = Visibility.Visible;
                }
                else
                {
                    M20V0101.Visibility = Visibility.Collapsed;
                }

                // Menu Movimentação ############################################################################################################################################
                if (Cls_0005_AccessControl.AccessGranted("M25", user.MenuAccess))
                {
                    M25.Visibility = Visibility.Visible;
                }
                else
                {
                    M25.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 2 Movimentação/Crédito
                if (Cls_0005_AccessControl.AccessGranted("M25C01", user.MenuAccess))
                {
                    M25C01.Visibility = Visibility.Visible;
                }
                else
                {
                    M25C01.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 2 Movimentação/Débito
                if (Cls_0005_AccessControl.AccessGranted("M25D01", user.MenuAccess))
                {
                    M25D01.Visibility = Visibility.Visible;
                }
                else
                {
                    M25D01.Visibility = Visibility.Collapsed;
                }

                // Menu relatórios ###############################################################################################################################################
                if (Cls_0005_AccessControl.AccessGranted("M28", user.MenuAccess))
                {
                    M28.Visibility = Visibility.Visible;
                }
                else
                {
                    M28.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 2 Relatórios/Artigo Ano Valor
                if (Cls_0005_AccessControl.AccessGranted("M28A0101", user.MenuAccess))
                {
                    M28A0101.Visibility = Visibility.Visible;
                }
                else
                {
                    M28A0101.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 2 Relatórios/Grupo Ano Valor
                if (Cls_0005_AccessControl.AccessGranted("M28A0102", user.MenuAccess))
                {
                    M28A0102.Visibility = Visibility.Visible;
                }
                else
                {
                    M28A0102.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 2 Relatórios/Subfamilia Ano Valor
                if (Cls_0005_AccessControl.AccessGranted("M28A0103", user.MenuAccess))
                {
                    M28A0103.Visibility = Visibility.Visible;
                }
                else
                {
                    M28A0103.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 2 Relatórios/Familia Ano Valor
                if (Cls_0005_AccessControl.AccessGranted("M28A0104", user.MenuAccess))
                {
                    M28A0104.Visibility = Visibility.Visible;
                }
                else
                {
                    M28A0104.Visibility = Visibility.Collapsed;
                }

                // Menu Configurações ############################################################################################################################################
                if (Cls_0005_AccessControl.AccessGranted("M30", user.MenuAccess))
                {
                    M30.Visibility = Visibility.Visible;
                }
                else
                {
                    M30.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 2 Configurações/Localização da base de dados
                if (Cls_0005_AccessControl.AccessGranted("M30D01", user.MenuAccess))
                {
                    M30D01.Visibility = Visibility.Visible;
                }
                else
                {
                    M30D01.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 2 Configurações/Backup da base de dados
                if (Cls_0005_AccessControl.AccessGranted("M30D02", user.MenuAccess))
                {
                    M30D02.Visibility = Visibility.Visible;
                }
                else
                {
                    M30D02.Visibility = Visibility.Collapsed;
                }
                // Menu nivel 2 Configurações/Restore da base de dados
                if (Cls_0005_AccessControl.AccessGranted("M30D03", user.MenuAccess))
                {
                    M30D03.Visibility = Visibility.Visible;
                }
                else
                {
                    M30D03.Visibility = Visibility.Collapsed;
                }

                // Menu Utilitários ##################################################################################################################################################
                if (Cls_0005_AccessControl.AccessGranted("M40", user.MenuAccess))
                {
                    M40.Visibility = Visibility.Visible;
                }
                else
                {
                    M40.Visibility = Visibility.Collapsed;
                }

                // Menu Ajuda ##########################################################################################################################################################
                if (Cls_0005_AccessControl.AccessGranted("M90", user.MenuAccess))
                {
                    M90.Visibility = Visibility.Visible;
                }
                else
                {
                    M90.Visibility = Visibility.Collapsed;
                }
            }
        }

        private void Login()
        {
            Frm_0002_Login loginWindow = new Frm_0002_Login();
            loginWindow.LoginCompleted += (sender, loginData) =>
            {
                // Atualiza as variáveis
                login = loginData.Login;
                loginUserSequence = loginData.LoginUserSequence;
                loginUserId = loginData.LoginUserId;
                loginUserType = loginData.LoginUserType;
                loginWindow.Close();
            };
            loginWindow.Owner = this;
            loginWindow.ShowDialog();

            RefreshLogin();
        }

        private bool ShowConfirmation(string message)
        {
            var confirmDialog = new Frm_0000_ConfirmDialog(message);
            confirmDialog.ShowDialog();
            return confirmDialog.Confirmed;
        }

        // Entrada de menu: Ficheiro/Login
        private void M01L01_Click(object sender, RoutedEventArgs e)
        {
            Login();
        }
        // Entrada de menu: Ficheiro/Logout
        private void M01O01_Click(object sender, RoutedEventArgs e)
        {
            if (ShowConfirmation("Tem a certeza que deseja efetuar o logout?"))
            {
                login = false;
                loginUserSequence = 0;
                loginUserId = "Não existe utilizador com login efetuado!";
                loginUserType = null;
                RefreshLogin();
            }
            else
            {
                return;
            }
        }
        // Entrada de menu: Ficheiro/Sair
        private void M01S01_Click(object sender, RoutedEventArgs e)
        {
            if (ShowConfirmation("Tem a certeza que deseja encerrar a aplicação?"))
            {
                Application.Current.Shutdown();
            }
            else
            {
                return;
            }
        }
        // Entrada de menu: Entidades/Utilizadores/Manutenção de utilizadores
        private void M20U0101_Click(object sender, RoutedEventArgs e)
        {
            Frm_0003_UsersManut frm_0003_UsersManut = new Frm_0003_UsersManut(loginUserSequence, loginUserId, loginUserType);
            frm_0003_UsersManut.Show();
        }
        // Entrada de menu: Entidades/Utilizadores/Manutenção de acesso
        private void M20U0102_Click(object sender, RoutedEventArgs e)
        {
            Frm_0004_AcessosManut frm_0004_AcessosManut = new Frm_0004_AcessosManut(loginUserSequence, loginUserId, loginUserType);
            frm_0004_AcessosManut.ShowDialog();
        }
        // Entrada de menu: Entidades/Entidades/Terceiros/Manutenção de tipos de terceiro
        private void M20T0101_Click(object sender, RoutedEventArgs e)
        {
            Frm_0101_TipoTerceiroManut frm_0101_TipoTerceiroManut = new Frm_0101_TipoTerceiroManut(loginUserSequence, loginUserId, loginUserType);
            frm_0101_TipoTerceiroManut.ShowDialog();
        }
        // Entrada de menu: Entidades/Entidades/Terceiros/Manutenção de terceiros
        private void M20T0102_Click(object sender, RoutedEventArgs e)
        {
            Frm_0102_TerceirosManut frm_0102_TerceirosManut = new Frm_0102_TerceirosManut(loginUserSequence, loginUserId, loginUserType);
            frm_0102_TerceirosManut.ShowDialog();
        }
        // Entrada de menu: Entidades/Entidades/Contas/Manutenção de contas
        private void M20C0101_Click(object sender, RoutedEventArgs e)
        {
            Frm_0103_ContasManut frm_0103_ContasManut = new Frm_0103_ContasManut(loginUserSequence, loginUserId, loginUserType);
            frm_0103_ContasManut.ShowDialog();
        }
        // Entrada de menu: Entidades/Entidades/Contas/Manutenção de tipos de receita
        private void M20C0102_Click(object sender, RoutedEventArgs e)
        {
            Frm_0104_TipoReceitaManut frm_0104_TipoReceitaManut = new Frm_0104_TipoReceitaManut(loginUserSequence, loginUserId, loginUserType);
            frm_0104_TipoReceitaManut.ShowDialog();
        }
        // Entrada de menu: Entidades/Entidades/Movimentação/Manutenção geral
        private void M20M0101_Click(object sender, RoutedEventArgs e)
        {
            Frm_0200_ArtigosManut frm_0200_ArtigosManut = new Frm_0200_ArtigosManut(loginUserSequence, loginUserId, loginUserType);
            frm_0200_ArtigosManut.ShowDialog();
        }
        // Entrada de menu: Entidades/Entidades/Movimentação/Manutenção de familias
        private void M20M0102_Click(object sender, RoutedEventArgs e)
        {
            Frm_0201_FamiliasManut frm_0201_FamiliasManut = new Frm_0201_FamiliasManut(loginUserSequence, loginUserId, loginUserType);
            frm_0201_FamiliasManut.ShowDialog();
        }
        // Entrada de menu: Entidades/Entidades/Movimentação/Manutenção de subfamilias
        private void M20M0103_Click(object sender, RoutedEventArgs e)
        {
            Frm_0202_SubFamiliasManut frm_0202_SubFamiliasManut = new Frm_0202_SubFamiliasManut(loginUserSequence, loginUserId, loginUserType);
            frm_0202_SubFamiliasManut.ShowDialog();
        }
        // Entrada de menu: Entidades/Entidades/Movimentação/Manutenção de grupos
        private void M20M0104_Click(object sender, RoutedEventArgs e)
        {
            Frm_0203_GruposManut frm_0203_GruposManut = new Frm_0203_GruposManut(loginUserSequence, loginUserId, loginUserType);
            frm_0203_GruposManut.ShowDialog();
        }
        // Entrada de menu: Entidades/Entidades/Movimentação/Manutenção de artigos
        private void M20M0105_Click(object sender, RoutedEventArgs e)
        {
            Frm_0205_ArtigosManut frm_0205_ArtigosManut = new Frm_0205_ArtigosManut(loginUserSequence, loginUserId, loginUserType);
            frm_0205_ArtigosManut.ShowDialog();
        }
        // Entrada de menu: Entidades/Entidades/Movimentação/Manutenção de unidades
        private void M20M0106_Click(object sender, RoutedEventArgs e)
        {
            Frm_0204_UnidadesManut frm_0204_UnidadesManut = new Frm_0204_UnidadesManut(loginUserSequence, loginUserId, loginUserType);
            frm_0204_UnidadesManut.ShowDialog();
        }
        // Entrada de menu: Entidades/Entidades/Movimentação/Manutenção de viaturas
        private void M20V0101_Click(object sender, RoutedEventArgs e)
        {
            Frm_0206_ViaturasManut frm_0206_ViaturasManut = new Frm_0206_ViaturasManut(loginUserSequence, loginUserId, loginUserType);
            frm_0206_ViaturasManut.ShowDialog();
        }
        // Entrada de menu: Movimentação/Créditos
        private void M25C01_Click(object sender, RoutedEventArgs e)
        {
            Frm_0400_CreditosManut frm_0400_CreditosManut = new Frm_0400_CreditosManut(loginUserSequence, loginUserId, loginUserType);
            frm_0400_CreditosManut.ShowDialog();
        }
        // Entrada de menu: Movimentação/Débitos
        private void M25D01_Click(object sender, RoutedEventArgs e)
        {
            Frm_0301_DebitosCabManut frm_0301_DebitosCabManut = new Frm_0301_DebitosCabManut(loginUserSequence, loginUserId, loginUserType);
            frm_0301_DebitosCabManut.ShowDialog();
        }
        // registo de manutenção de viaturas
        private void M25V02_Click(object sender, RoutedEventArgs e)
        {
            Frm_0801_RegManutViaturas frm_0801_RegManutViaturas = new Frm_0801_RegManutViaturas(loginUserSequence, loginUserId, loginUserType);
            frm_0801_RegManutViaturas.ShowDialog();
        }


        // Entrada de menu: Relatórios
        // Relatório por artigo/ano/mês
        private void M28A0101_Click(object sender, RoutedEventArgs e)
        {
            Frm_200001_RelatorioArtAno frm_200001_RelatorioArtAno = new Frm_200001_RelatorioArtAno(loginUserSequence, loginUserId, loginUserType);
            frm_200001_RelatorioArtAno.ShowDialog();
        }
        // Relatório por grupo/ano/mês
        private void M28A0102_Click(object sender, RoutedEventArgs e)
        {
            Frm_200002_RelatorioGrupoAno frm_200002_RelatorioGrupoAno = new Frm_200002_RelatorioGrupoAno(loginUserSequence, loginUserId, loginUserType);
            frm_200002_RelatorioGrupoAno.ShowDialog();
        }
        // Relatório por subfamilia/ano/mês
        private void M28A0103_Click(object sender, RoutedEventArgs e)
        {
            Frm_200003_RelatorioSubFamAno frm_200003_RelatorioSubFamAno = new Frm_200003_RelatorioSubFamAno(loginUserSequence, loginUserId, loginUserType);
            frm_200003_RelatorioSubFamAno.ShowDialog();
        }
        // Relatório por familia/ano/mês
        private void M28A0104_Click(object sender, RoutedEventArgs e)
        {
            Frm_200004_RelatorioFamAno frm_200004_RelatorioFamAno = new Frm_200004_RelatorioFamAno(loginUserSequence, loginUserId, loginUserType);
            frm_200004_RelatorioFamAno.ShowDialog();
        }
        // Relatório de saldo por ano/mês
        private void M28A0201_Click(object sender, RoutedEventArgs e)
        {
            Frm_200101_SaldoAnoMesCredDeb frm_200101_SaldoAnoMesCredDeb = new Frm_200101_SaldoAnoMesCredDeb(loginUserSequence, loginUserId, loginUserType);
            frm_200101_SaldoAnoMesCredDeb.ShowDialog();
        }

        // Entrada de menu: Configurações/Localização da base de dados
        private void M30D01_Click(object sender, RoutedEventArgs e)
        {
            Frm_000101_ConfigConnection frm_000101_ConfigConnection = new Frm_000101_ConfigConnection();
            frm_000101_ConfigConnection.Show();
        }
        // Entrada de menu: Configurações/Backup da base de dados
        private void M30D02_Click(object sender, RoutedEventArgs e)
        {
            Frm_000102_DBBackup frm_000102_DBBackup = new Frm_000102_DBBackup();
            frm_000102_DBBackup.Show();
        }
        // Entrada de menu: Configurações/Restore da base de dados
        private void MenuItemDBRestore_Click(object sender, RoutedEventArgs e)
        {
            Frm_000103_DBRestore frm_000103_DBRestore = new Frm_000103_DBRestore();
            frm_000103_DBRestore.Show();
        }

        private void M90A01_Click(object sender, RoutedEventArgs e)
        {
            Frm_900000_Help frm_900000_Help = new Frm_900000_Help();
            frm_900000_Help.ShowDialog();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isMaximized)
            {
                // Restore the previous state and dimensions

                this.WindowState = _previousWindowState;
                this.Left = _previousWindowRect.Left;
                this.Top = _previousWindowRect.Top;
                this.Width = _previousWindowRect.Width;
                this.Height = _previousWindowRect.Height;
                _isMaximized = false;
                // MaximizeButton.Content = "□⃣";
            }
            else
            {
                // Save the current state and dimensions
                _previousWindowState = this.WindowState;
                _previousWindowRect = new Rect(this.Left, this.Top, this.Width, this.Height);

                // Get the current screen work area
                var screen = System.Windows.Forms.Screen.FromHandle(new WindowInteropHelper(this).Handle);
                var workArea = screen.WorkingArea;

                // Set the window size to the work area size
                this.WindowState = WindowState.Normal; // Ensure the window is in normal state before resizing
                this.Left = workArea.Left;
                this.Top = workArea.Top;
                this.Width = workArea.Width;
                this.Height = workArea.Height;

                // Set the flag to indicate the window is maximized
                _isMaximized = true;

            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        // Entrada de menu: Utilitários/Alterar a palavra-passe
        private void M40A01_Click(object sender, RoutedEventArgs e)
        {
            Frm_000304_SelfChgPw frm_000303_SelfChgPw = new Frm_000304_SelfChgPw(loginUserSequence, loginUserId);
            frm_000303_SelfChgPw.ShowDialog();
        }
    }
}
