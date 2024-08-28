<Window x:Class="GestaoDom.MainWindow"
        xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
        xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
        xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
        xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
        xmlns:local="clr-namespace:GestaoDom"
        mc:Ignorable="d"
        Icon="/Imagens/Main.png"
        WindowStartupLocation="CenterScreen"
        WindowState="Normal"
        Title="Principal"
        Background="Aqua"
        Height="450"
        Width="800"
        Loaded="MainWindow_Loaded">
    <Window.Style>
        <Style TargetType="Window">
            <Setter Property="Template">
                <Setter.Value>
                    <ControlTemplate TargetType="Window">
                        <Border Background="{TemplateBinding Background}">
                            <Grid>
                                <!-- Barra de título personalizada -->
                                <Grid Background="{TemplateBinding Background}" VerticalAlignment="Top" Height="30">
                                    <!-- Título da janela -->
                                    <TextBlock Text="{TemplateBinding Title}" VerticalAlignment="Center" HorizontalAlignment="Center"/>
                                    <!-- Botões da janela -->
                                    <StackPanel Orientation="Horizontal" HorizontalAlignment="Right" VerticalAlignment="Center" Margin="0,0,5,0">
                                        <!-- Botão Minimizar -->
                                        <Button Content="_" Width="20" Click="MinimizeButton_Click"/>
                                        <!-- Botão Maximizar -->
                                        <Button Content="[ ]" Width="20" Click="MaximizeButton_Click"/>
                                        <!-- Botão Fechar -->
                                        <Button Content="X" Width="20" Click="CloseButton_Click"/>
                                    </StackPanel>
                                </Grid>
                                <!-- Conteúdo da sua janela -->
                                <ContentPresenter Margin="0,30,0,0"/>
                            </Grid>
                        </Border>
                    </ControlTemplate>
                </Setter.Value>
            </Setter>
        </Style>
    </Window.Style>
    <Grid>
        <Menu Grid.ColumnSpan="1" 
              Margin="0,0,0,0"
              Background="#FF25C58A"
              VerticalAlignment="Top"
              Height="30">
            <MenuItem Name="M01" Header="Ficheiro" Height="30" VerticalAlignment="Center">
                <MenuItem.Icon>
                    <Image Source="/Imagens/Ficheiro.png" Width="16" Height="16"/>
                </MenuItem.Icon>
                <MenuItem Name="M01L01" Header="Login" Click="M01L01_Click">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/Login.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                </MenuItem>
                <MenuItem Name="M01O01" Header="Logout" Click="M01O01_Click">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/Logout.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                </MenuItem>
                <MenuItem Name="M01S01" Header="Sair" Click="M01S01_Click">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/Sair.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                </MenuItem>
                <Image Name="image" Height="100" Width="100"/>
            </MenuItem>
            
            <MenuItem Name="M20" Header="Entidades" Height="29">
                <MenuItem.Icon>
                    <Image Source="/Imagens/Entidades.png" Width="16" Height="16"/>
                </MenuItem.Icon>
                <MenuItem Name="M20U01" Header="Utilizadores">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/User.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                    <MenuItem Name="M20U0101" Header="Manutenção de utilizadores" Click="M20U0101_Click">
                        <MenuItem.Icon>
                            <Image Source="/Imagens/Manutencao.png" Width="16" Height="16"/>
                        </MenuItem.Icon>
                    </MenuItem>
                    <MenuItem Name="M20U0102" Header="Manutenção de acessos" Click="M20U0102_Click">
                        <MenuItem.Icon>
                            <Image Source="/Imagens/Manutencao.png" Width="16" Height="16"/>
                        </MenuItem.Icon>
                    </MenuItem>
                </MenuItem>
                <MenuItem Name="M20T01" Header="Terceiros">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/Terceiro.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                    <MenuItem Name="M20T0101" Header="Manutenção de tipos de terceiros" Click="M20T0101_Click">
                        <MenuItem.Icon>
                            <Image Source="/Imagens/TipoTerceiro.png" Width="16" Height="16"/>
                        </MenuItem.Icon>
                    </MenuItem>
                    <MenuItem Name="M20T0102" Header="Manutenção de terceiros" Click="M20T0102_Click">
                        <MenuItem.Icon>
                            <Image Source="/Imagens/Terceiro.png" Width="16" Height="16"/>
                        </MenuItem.Icon>
                    </MenuItem>
                </MenuItem>
                <MenuItem Name="M20C01" Header="Contas">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/Contas.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                    <MenuItem Name="M20C0101" Header="Manutenção de contas" Click="M20C0101_Click">
                        <MenuItem.Icon>
                            <Image Source="/Imagens/Contas.png" Width="16" Height="16"/>
                        </MenuItem.Icon>
                    </MenuItem>
                    <MenuItem Name="M20C0102" Header="Manutenção de tipos de receitas" Click="M20C0102_Click">
                        <MenuItem.Icon>
                            <Image Source="/Imagens/TipoReceita.png" Width="16" Height="16"/>
                        </MenuItem.Icon>
                    </MenuItem>
                </MenuItem>
                <MenuItem Name="M20M01" Header="Movimentação">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/Artigos.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                    <MenuItem Name="M20M0101" Header="Manutenção geral" Click="M20M0101_Click">
                        <MenuItem.Icon>
                            <Image Source="/Imagens/Artigos.png" Width="16" Height="16"/>
                        </MenuItem.Icon>
                    </MenuItem>
                    <MenuItem Name="M20M0102" Header="Manutenção de familias" Click="M20M0102_Click">
                        <MenuItem.Icon>
                            <Image Source="/Imagens/Familia.png" Width="16" Height="16"/>
                        </MenuItem.Icon>
                    </MenuItem>
                    <MenuItem Name="M20M0103" Header="Manutenção de subfamilias" Click="M20M0103_Click">
                        <MenuItem.Icon>
                            <Image Source="/Imagens/SubFamilia.png" Width="16" Height="16"/>
                        </MenuItem.Icon>
                    </MenuItem>
                    <MenuItem Name="M20M0104" Header="Manutenção de grupos" Click="M20M0104_Click">
                        <MenuItem.Icon>
                            <Image Source="/Imagens/Grupos.png" Width="16" Height="16"/>
                        </MenuItem.Icon>
                    </MenuItem>
                    <MenuItem Name="M20M0105" Header="Manutenção de artigos" Click="M20M0105_Click">
                        <MenuItem.Icon>
                            <Image Source="/Imagens/Artigos.png" Width="16" Height="16"/>
                        </MenuItem.Icon>
                    </MenuItem>
                    <MenuItem Name="M20M0106" Header="Manutenção de unidades" Click="M20M0106_Click">
                        <MenuItem.Icon>
                            <Image Source="/Imagens/Unidades.png" Width="16" Height="16"/>
                        </MenuItem.Icon>
                    </MenuItem>
                </MenuItem>
                <MenuItem Name="M20V01" Header="Viaturas">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/Viaturas.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                    <MenuItem Name="M20V0101" Header="Manutenção de viaturas" Click="M20V0101_Click">
                        <MenuItem.Icon>
                            <Image Source="/Imagens/Viaturas.png" Width="16" Height="16"/>
                        </MenuItem.Icon>
                    </MenuItem>
                </MenuItem>
            </MenuItem>

            <MenuItem Name="M25" Header="Movimentação" VerticalContentAlignment="Bottom">
                <MenuItem.Icon>
                    <Image Source="/Imagens/Movimentos.png" Width="16" Height="16"/>
                </MenuItem.Icon>
                <MenuItem Name="M25C01" Header="Créditos" Click="M25C01_Click">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/Credito.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                </MenuItem>
                <MenuItem Name="M25D01" Header="Débitos" Click="M25D01_Click">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/Debito.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                </MenuItem>
                <MenuItem Name="M25V02" Header="Registo manutenção de viaturas" Click="M25V02_Click">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/Viaturas.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                </MenuItem>
            </MenuItem>

            <MenuItem Name="M28" Header="Relatórios">
                <MenuItem.Icon>
                    <Image Source="/Imagens/Print.png" Width="16" Height="16"/>
                </MenuItem.Icon>
                <MenuItem Name="M28A0101" Header="Relatório por artigo/mês com seleção de ano" Click="M28A0101_Click">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/Artigos.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                </MenuItem>
                <MenuItem Name="M28A0102" Header="Relatório por grupo/mês com seleção de ano" Click="M28A0102_Click">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/Grupos.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                </MenuItem>
                <MenuItem Name="M28A0103" Header="Relatório por subfamilia/mês com seleção de ano" Click="M28A0103_Click">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/SubFamilia.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                </MenuItem>
                <MenuItem Name="M28A0104" Header="Relatório por familia/mês com seleção de ano" Click="M28A0104_Click">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/Familia.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                </MenuItem>
                <MenuItem Name="M28A0201" Header="Relatório de saldo/mês com seleção de ano" Click="M28A0201_Click">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/Saldo.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                </MenuItem>

            </MenuItem>

            <MenuItem Name="M30" Header="Configurações">
                <MenuItem.Icon>
                    <Image Source="/Imagens/Configuracao.png" Width="16" Height="16"/>
                </MenuItem.Icon>
                <MenuItem Name="M30D01" Header="Localização da base de dados" Click="M30D01_Click">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/DBLocal.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                </MenuItem>
                <MenuItem Name="M30D02" Header="Backup da base de dados" Click="M30D02_Click">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/Backup.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                </MenuItem>
                <MenuItem Name="M30D03" Header="Restaurar base de dados" Click="MenuItemDBRestore_Click">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/Restore.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                </MenuItem>
            </MenuItem>

            <MenuItem Name="M40" Header="Utilitários">
                <MenuItem.Icon>
                    <Image Source="/Imagens/Utilitarios.png" Width="16" Height="16"/>
                </MenuItem.Icon>
                <MenuItem Name="M40A01" Header="Alterar Palavra-passe" Click="M40A01_Click">
                    <MenuItem.Icon>
                        <Image Source="/Imagens/ChgPw.png" Width="16" Height="16"/>
                    </MenuItem.Icon>
                </MenuItem>
            </MenuItem>

            <MenuItem Name="M90" Header="Ajuda" Click="M90A01_Click">
                <MenuItem.Icon>
                    <Image Source="/Imagens/Help.png" Width="16" Height="16"/>
                </MenuItem.Icon>
            </MenuItem>
        </Menu>
        <Label Name="Lbl_UserLogin" Content="Não existe utilizador com login efetuado!" HorizontalAlignment="Left" VerticalAlignment="Bottom" Margin="10,0,0,10" Width="300"/>
    </Grid>
</Window>
Com este código está corrigida a nova barra, mas aparece a por defeito, como posso remover a que aparece por defeito?