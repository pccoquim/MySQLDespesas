   M S B A M L   `   `   `  ���� D  @GestaoDom, Version=1.0.0.3, Culture=neutral, PublicKeyToken=null%clr-namespace:GestaoDom	GestaoDom  0    *GestaoDom.Frm_03010201_DebitoCabEditAddDet   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     )localclr-namespace:GestaoDom       � Icon$  /Imagens/Debito.png�� � WindowStartupLocation$ CenterScreen=� � Title$; 5Adição de detalhes de debitos - Seleção de artigo��$	��450��$	��800�� � 	Resources  �� 
TargetType �� Property �� Value��%2   '��       �� " ��.���  � ��$ Stretch=�.��� $��	0,0,120,0q�.+�� -   $��largura��$y�Hidden=� � ColumnDefinitions �� $
��0.0* ��� $
��0.0* ��� $
��0.5* ��� $
��0.2* �.+��� -   $��largurawidth0��$��0���� -   $��largurawidth1��$��1���� -   $��largurawidth2��$��2���� -   $��largurawidth3��$��3���� -   $��ListArtigos��$	��Top=�$
��Auto=�$��Disabled=� �� AlternationCount$ 2��$��
10,10,0,10q�	 �� View	 �� .P��� 
 �� Width
 ��  �� ElementName$ largurawidth0�� �� Path$ ActualWidth����� 
 � Tag$ ID��.��ID �� CellTemplate �� .R���  �� TextAlignment$ Center=����� *Id+�� 
 �� $ largurawidth1��$ ActualWidth����� $	 Cod��.��Cod �� .R��� $ Center=����� *Cod+�� 
 �� $ largurawidth2��$ ActualWidth����� $ Descr��.��Artigo �� .R��� ���� *Artigo+�� 
 �� $ largurawidth3��$ ActualWidth����� $ Unidade��.��	Unidade �� .R��� ���� *	Unidade+ ��%{   '��       �� " ��.���  � ��
 ��·���  � ��
 �   ���  � ��$ Center=���  � ��$ Center=��� -   $��Btn_AddLinha��$��Right=�$	��Top=�$��Center=�$��0, 10, -110, 0q�$	��100��$��40��.���� .+��� $��	Adicionar��$��Center=��� $��linha��$��Center=��� -   $��	Btn_Close��$��Fechar��$��Right=�$��Bottom=�$��0, 0, -110,10q�$	��100��$��40��