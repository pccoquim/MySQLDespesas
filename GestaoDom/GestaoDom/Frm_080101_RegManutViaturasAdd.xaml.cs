/*
Frm_080101_RegManutViariasAdd.xaml.cs
Autor: Paulo da Cruz Coquim
Data: 07.06.2024
Data ultima alteração: 14.06.2024
Versão: 1.0.1
*/
using MySql.Data.MySqlClient;
using MySqlConnector;
using System;
using System.Data;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace GestaoDom
{
    /// <summary>
    /// Interaction logic for Frm_080101_RegManutViaturasAdd.xaml
    /// </summary>
    public partial class Frm_080101_RegManutViaturasAdd : Window
    {
        private readonly string loginUserId;
        private int number;
        public Frm_080101_RegManutViaturasAdd(string loginUserId)
        {
            InitializeComponent();

            this.loginUserId = loginUserId;
            Load_Cbx_Viatura();
        }

        private void Load_Cbx_Viatura()
        {
            // ComboBox terceiros
            // Obtem ligação
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                try
                {
                    conn.Open();
                    // defenição da consulta
                    string query = "SELECT vtr_cod, vtr_matricula, vtr_descr FROM tbl_0205_viaturas WHERE vtr_status = 1 ORDER BY vtr_descr";

                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Executa o comando e obtém o resultado
                        MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        Cbx_Viatura.ItemTemplate = CreateItemTemplate();
                        Cbx_Viatura.ItemsSource = dt.DefaultView;
                        Cbx_Viatura.SelectedValuePath = "vtr_cod";
                        Cbx_Viatura.SelectedIndex = -1;
                        Cbx_Viatura.IsEditable = false;
                        return;
                    }
                }
                catch (Exception ex)
                {
                    System.Windows.MessageBox.Show("Erro de componentes da cbx_Viatura (Erro: C1081)!" + ex.Message);
                }
            }
        }

        private DataTemplate CreateItemTemplate()
        {
            DataTemplate template = new DataTemplate();

            FrameworkElementFactory stackPanelFactory = new FrameworkElementFactory(typeof(StackPanel));
            stackPanelFactory.SetValue(StackPanel.OrientationProperty, Orientation.Horizontal);

            FrameworkElementFactory matriculaTextBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            matriculaTextBlockFactory.SetBinding(TextBlock.TextProperty, new Binding("vtr_matricula"));
            stackPanelFactory.AppendChild(matriculaTextBlockFactory);

            FrameworkElementFactory descrTextBlockFactory = new FrameworkElementFactory(typeof(TextBlock));
            descrTextBlockFactory.SetBinding(TextBlock.TextProperty, new Binding("vtr_descr"));
            stackPanelFactory.AppendChild(descrTextBlockFactory);

            template.VisualTree = stackPanelFactory;

            return template;
        }

        private void Cbx_Viatura_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Impedir que a tecla Enter seja inserida no campo atual
                e.Handled = true;

                // Mover o foco para o próximo campo de entrada
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                if (Keyboard.FocusedElement is UIElement element)
                    element.MoveFocus(request);
            }
        }

        private void Txt_Descr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Impedir que a tecla Enter seja inserida no campo atual
                e.Handled = true;

                // Mover o foco para o próximo campo de entrada
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                if (Keyboard.FocusedElement is UIElement element)
                    element.MoveFocus(request);
            }
        }

        private void Txt_Data_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!IsValidDate(Txt_Data.Text))
            {
                MessageBox.Show("A data inserida não é válida. Use o formato dd/mm/yyyy.", "Erro de formato", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsValidDate(string date)
        {
            // Verifica se a data é válida usando DateTime.TryParseExact.
            return DateTime.TryParseExact(date, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out _);
        }

        private void Txt_Data_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Impedir que a tecla Enter seja inserida no campo atual
                e.Handled = true;

                // Mover o foco para o próximo campo de entrada
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                if (Keyboard.FocusedElement is UIElement element)
                    element.MoveFocus(request);
            }
        }

        private void Txt_Data_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (sender is TextBox textBox)
            {
                var text = textBox.Text.Replace(".", "");
                if (text.Length >= 2 && text.Length < 4)
                {
                    textBox.Text = text.Insert(2, ".");
                    textBox.Select(textBox.Text.Length, 0);
                }
                else if (text.Length >= 4)
                {
                    textBox.Text = text.Insert(2, ".").Insert(5, ".");
                    textBox.Select(textBox.Text.Length, 0);
                }
            }
        }

        private void Txt_Km_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Impedir que a tecla Enter seja inserida no campo atual
                e.Handled = true;

                // Mover o foco para o próximo campo de entrada
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                if (Keyboard.FocusedElement is UIElement element)
                    element.MoveFocus(request);
            }
        }

        private void Txt_Km_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Verifica se o texto inserido é um número inteiro
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true; // Impede a entrada de caracteres não numéricos
            }
        }

        private void Ckb_Oleo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Impedir que a tecla Enter seja inserida no campo atual
                e.Handled = true;

                // Mover o foco para o próximo campo de entrada
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                if (Keyboard.FocusedElement is UIElement element)
                    element.MoveFocus(request);
            }
        }

        private void Ckb_FiltroOleo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Impedir que a tecla Enter seja inserida no campo atual
                e.Handled = true;

                // Mover o foco para o próximo campo de entrada
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                if (Keyboard.FocusedElement is UIElement element)
                    element.MoveFocus(request);
            }
        }

        private void Ckb_FiltroAr_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Impedir que a tecla Enter seja inserida no campo atual
                e.Handled = true;

                // Mover o foco para o próximo campo de entrada
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                if (Keyboard.FocusedElement is UIElement element)
                    element.MoveFocus(request);
            }
        }

        private void Txt_Efetuado_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Impedir que a tecla Enter seja inserida no campo atual
                e.Handled = true;

                // Mover o foco para o próximo campo de entrada
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                if (Keyboard.FocusedElement is UIElement element)
                    element.MoveFocus(request);
            }
        }

        private void Txt_KmProxima_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                // Impedir que a tecla Enter seja inserida no campo atual
                e.Handled = true;

                // Mover o foco para o próximo campo de entrada
                TraversalRequest request = new TraversalRequest(FocusNavigationDirection.Next);
                if (Keyboard.FocusedElement is UIElement element)
                    element.MoveFocus(request);
            }
        }

        private void Txt_KmProxima_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Verifica se o texto inserido é um número inteiro
            if (!int.TryParse(e.Text, out _))
            {
                e.Handled = true; // Impede a entrada de caracteres não numéricos
            }
        }

        private void Numeracao()
        {
            // variavel para numeração
            int ultimoId = 0;
            // Ligação à base de dados
            using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
            {
                //Contrlo de erros
                try
                {
                    conn.Open();
                    // Seleção do código mais elevado
                    string query = "SELECT MAX(vtm_mid) FROM tbl_0901_viaturasmanutencao";
                    // Execução da consulta
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        // Obtenção do resultado: nulo, passa a um
                        object result = cmd.ExecuteScalar();
                        if (result != DBNull.Value)
                        {
                            ultimoId = Convert.ToInt32(result);
                        }
                    }
                }
                catch (Exception)
                {
                    // Mensagem de erro da ligação
                    System.Windows.MessageBox.Show("Erro ao carregar númeração!");
                }
                // Incremento de unidade ao valor obtido
                number = ultimoId + 1;
            }
        }

        private void Btn_Save_Click(object sender, RoutedEventArgs e)
        {
            Numeracao();

            object viatura = Cbx_Viatura.SelectedValue;

            // Conversão da data
            string dataTextBox = Txt_Data.Text;
            string dataDB = "";

            if (DateTime.TryParseExact(dataTextBox, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dataFormatada))
            {
                dataDB = dataFormatada.ToString("yyyyMMdd");
            }
            else
            {
                System.Windows.MessageBox.Show("A data está num formato incorreto!");
            }

            int km;

            if (Txt_Km.Text != "")
            {
                km = Convert.ToInt32(Txt_Km.Text); ;
            }
            else
            {
                km = 0;
            }

            bool oleo;

            if (Ckb_Oleo.IsChecked == true)
            {
                oleo = true;
            }
            else
            {
                oleo = false;
            }

            bool filtroOleo;

            if (Ckb_FiltroOleo.IsChecked == true)
            {
                filtroOleo = true;
            }
            else
            {
                filtroOleo = false;
            }

            bool filtroAr;

            if (Ckb_FiltroAr.IsChecked == true)
            {
                filtroAr = true;
            }
            else
            {
                filtroAr = false;
            }

            int kmProxima;
            if (Txt_KmProxima.Text != "")
            {
                kmProxima = Convert.ToInt32(Txt_KmProxima.Text);
            }
            else
            {
                kmProxima = 0;
            }

            if (viatura != null)
            {
                if (Txt_Descr.Text != "")
                {
                    // Obtem ligação
                    using (MySqlConnection conn = Cls_0001_DBConnection.GetConnection())
                    {
                        // Controlo de erros insert
                        try
                        {
                            conn.Open();
                            // Definição do insert 
                            string query = "INSERT INTO tbl_0901_viaturasmanutencao(vtm_mid, vtm_codviatura, vtm_descr, vtm_datamanut, vtm_km, vtm_oleo, vtm_filtrooleo, vtm_filtroar, vtm_efetuado, vtm_kmproxima, vtm_status, vtm_usercreate, vtm_datecreate, vtm_timecreate, vtm_userlstchg, vtm_datelstchg, vtm_timelstchg) " +
                                "                       VALUES(?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)";
                            // Definição de query e ligação
                            using (MySqlCommand cmd = new MySqlCommand(query, conn))
                            {
                                // Atribuição de variaveis
                                cmd.Parameters.AddWithValue("@MId", number);
                                cmd.Parameters.AddWithValue("@CodViatura", Cbx_Viatura.SelectedValue);
                                cmd.Parameters.AddWithValue("@Descr", Txt_Descr.Text);
                                cmd.Parameters.AddWithValue("@DataManut", dataDB);
                                cmd.Parameters.AddWithValue("@Km", km);
                                cmd.Parameters.AddWithValue("@Oleo", oleo);
                                cmd.Parameters.AddWithValue("@FiltroOleo", filtroOleo);
                                cmd.Parameters.AddWithValue("@FiltroAr", filtroAr);
                                cmd.Parameters.AddWithValue("@Efetuado", Txt_Efetuado.Text);
                                cmd.Parameters.AddWithValue("@KmProxima", kmProxima);
                                cmd.Parameters.AddWithValue("@status", 1);
                                cmd.Parameters.AddWithValue("@User", loginUserId);
                                cmd.Parameters.AddWithValue("@Data", Cls_0002_ActualDateTime.Date);
                                cmd.Parameters.AddWithValue("@Hora", Cls_0002_ActualDateTime.Time);
                                cmd.Parameters.AddWithValue("@UserLstChg", 0);
                                cmd.Parameters.AddWithValue("@DataLstChg", 0);
                                cmd.Parameters.AddWithValue("@HoraLstChg", 0);
                                // execução do comando
                                cmd.ExecuteNonQuery();
                            }
                            // Fecha o formulário                            
                            this.Close();
                            System.Windows.MessageBox.Show("Manutenção a viatura inserida com sucesso! Referência da manutenção: " + number);
                        }
                        catch (Exception ex)
                        {
                            // mensagem de erro da ligação
                            System.Windows.MessageBox.Show("Ocorreu um erro ao ligar à base de dados!" + ex.Message);
                            return;
                        }
                    }
                }
                else
                {
                    System.Windows.MessageBox.Show("A descrição não está preenchida! Por favor preencha ao campo da descrição!");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Não está selecionada nenhuma viatura! Por favor selecione uma viatura!");
            }
        }

        // Abre o formulário para adicionar componentes
        private void Btn_Components_Click(object sender, RoutedEventArgs e)
        {
            object viatura = Cbx_Viatura.SelectedValue;

            string data = Txt_Data.Text;

            int km;

            if (Txt_Km.Text != "")
            {
                km = Convert.ToInt32(Txt_Km.Text); ;
            }
            else
            {
                km = 0;
            }

            bool oleo;

            if (Ckb_Oleo.IsChecked == true)
            {
                oleo = true;
            }
            else
            {
                oleo = false;
            }

            bool filtroOleo;

            if (Ckb_FiltroOleo.IsChecked == true)
            {
                filtroOleo = true;
            }
            else
            {
                filtroOleo = false;
            }

            bool filtroAr;

            if (Ckb_FiltroAr.IsChecked == true)
            {
                filtroAr = true;
            }
            else
            {
                filtroAr = false;
            }

            int kmProxima;
            if (Txt_KmProxima.Text != "")
            {
                kmProxima = Convert.ToInt32(Txt_KmProxima.Text);
            }
            else
            {
                kmProxima = 0;
            }

            if (viatura != null)
            {
                if (Txt_Descr.Text != "")
                {
                    string viat = Cbx_Viatura.SelectedValue.ToString();
                    string descr = Txt_Descr.Text;
                    string efetuado = Txt_Efetuado.Text;
                    Frm_08010101_ManutViaturasComponents componets = new Frm_08010101_ManutViaturasComponents(loginUserId, viat, data, descr, km, oleo, filtroOleo, filtroAr, efetuado, kmProxima);
                    componets.ShowDialog();
                }
                else
                {
                    System.Windows.MessageBox.Show("A descrição não está preenchida! Por favor preencha ao campo da descrição!");
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Não está selecionada nenhuma viatura! Por favor selecione uma viatura!");
            }
        }

        private void Btn_Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
