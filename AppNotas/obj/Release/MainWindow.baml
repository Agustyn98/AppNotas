   M S B A M L   `   `   `  ����   AppNotas    AppNotas.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     -     �Name$  MainWindow1�� � Title$ AppNotas��$	��650��$
��1000�� � WindowStartupLocation$ CenterScreen=� � Icon$ icons8-notepad-100.png�����	#FFFFFDFD
���   ���.  |�.  .��� -    �Name$ MainGrid��$��0,0,0,-2q����	#FFF1F1F1 � ColumnDefinitions �� �� $��0* ��� $��0* ��� $
��991* �.+��� -    ��Name$ listBoxNotes�� �� AlternationCount$ 2��$��17(�$��Medium"�}�. $
��Left=�$	��245��$��0,-0.5,-1.5,0q� K� ZIndex$ 3��$��4��$��1��$��0,0,0,31q�
���   �$��1,1,1,1q�$��Hidden=�$��Hidden=����� .R��� .+��� 	 �� Padding$	 0,0,0,4q�$��Center=�$	��224��$��10��$
��Wrap=����� *Title+���� 
 �� 
TargetType"
 ��.���  �� Property �@�FocusVisualStyle �� Value Z� ��  � ��$ 37����  � ��$ 5,0,0,0q���  � ��
 ��  ���  � �� ��  �� 
TargetType" ��.R��� $��Bd��#��z��#��z��#��z�� �� Padding# z�� J� SnapsToDevicePixels . ._��� #��z��#��z��# z  �� Triggers P�  P� Property �@��
IsSelected P� Value . . ���  �� 
TargetName$ Bd�� � �� �	#B2A0D8E6P�  �@J�	IsFocused . . ��� $ Bd�� � �� �	#99EE82EEP�  � {� .  . ��� # � �� � �� �� Triggers P�  �@��AlternationIndex$ 0��. ���  � ��
 ���� P�  �@��AlternationIndex$ 1��. ���  � �� �	#FFEBEBEB��  �� Binding �� *Pin+ �� Value 1.a���  � ��
 � � ���  � ��$ 5,0,0,0q�P�  �@��
IsSelected . . ���  � ��
 �   ���  � ��$ 5,0,0,0q��� -    ��Name$ textBoxNote��$��
249,75,2,3q�$��18(�$��3��$ 1��$��	1,2.5,0,1q�$��3��
���   �$��Arial)� � 	Resources ��%3   'J�       �� "
 J�.���  �@��Margin$ 0q� �� SelectionBrush ��  �� Color$ 	#FFFF64B4�� �� Opacity$	 0.8Q�. �/� .T�J�  �� Padding$	 0,5q�.*�.*��� $� ���� -   $ textBoxTitle��$��22(�$��249,0,167,0q�$��70��$	��Top=�$��3��$
��Bold"�$ 1��$��2��
���   �$��Verdana)�$��1q� �� AcceptsReturn .  . �/� .T�J� .*�.*��� $� ��J� .*�.*���  ��%3   'J�       �� "
 J�.���  �@��Margin$ 0q� �� $ 	#FFFF64B4��$	 0.8Q�o� -    o�Name$ MenuFile��$��3��$��Right=�$��32��$��	0,38,72,0q�$	��Top=�$��90��$��18(�$��0q�$ 1��|�.  .@�m� -    m�Name$ MenuItemFile��$��Options��  J� RenderTransformOrigin$  0.5,0.5&�$��20(�$��32��$��90��$��4,0,0,3q�$
��Bold"�
���   ���.  ���	#FFE6E6E6z�V� .~��� �� �� T� .@�m� -   $
��_New��$��35��m� -	   $��_Delete��$��35��m� -
   $
��Save��$��35��m� -   $��_Clear��$��35��m� -   $ btnSpellCheck��$��Spell c_heck��! m� IsCheckable! . " m� 	IsChecked" .  # � ToolTip$-# 'Depends on the keyboard layout you type��$��35���� m� $��_Export��$��35��.@�m� -   $��Export as .txt��$��35��m� -   $��Export as .rtf (word)��$��35��m� $��_Import��$��35��.@�m� -   $��Import .txt��$��35��m� -   $��Import .rtf (word)��$��35���� m� -   $��_Print��$��35��m� -   $��_Back up��$��35���� m� -   $��	_Settings��$��42��
���  ���� -   $ ��Name$$ comboBoxFont��$��3��$��Right=�$��32��$��0,2,57,0q�$	��Top=�$��50��$��1q�$��18(�$��5,3,2,1q�% �� MaxDropDownHeight$	% 300����.  |�.  $  0.267,0.621&�.@��� $��6��$��15(��� $��7��$��15(��� $��8��$��15(��� $��9��$��15(��� $��10��$��15(��� $��11��$��15(��� $��12��$��15(��� $��13��$��15(��� $��14��$��15(��� $��15��$��15(��� $��16��$��15(��� $��17��$��15(��� $��18��$��15(��� $��19��$��15(��� $��20��$��15(��� $��21��$��15(��� $��22��$��15(��� $��23��$��15(��� $��24��$��15(��� $��25��$��15(��� $��26��$��15(��� $��27��$��15(��� $��28��$��15(��� $��29��$��15(��� $��30��$��15(��� $��31��$��15(��� $��32��$��15(��� $��33��$��15(��� $��34��$��15(��� $��35��$��15(��� $��36��$��15(��� $��37��$��15(��� $��38��$��15(��� $��39��$��15(��� $��40��$��15(����� "
 ��.���  �@�FocusVisualStyle Z�  P�  �@J�	IsFocused . . ���  � ��$ 2,2,2,2q��� -   & ��Name$#& textBlockCountCharactersTitle��$��3��$��Right=�$��18��$��
0,47,171,0q�$
��Wrap=�$	��Top=�$��26��$��15(����	#FF2867D6$	 0q�$��RightToLeft=�$ 2��9� -   ' 9�Name$' expanderButtom��-  'System.Windows.Input.KeyboardNavigation(   TabNavigation$
( None=�$��3��$
��Info��$��Right=�$	��164��$	��Top=�$	��219��$��	0,45,13,0q���.  $��0q�$��19(�$  0.5,0.5&�$��RightToLeft=�|�.  z�V� .~��� ) �� ScaleX$	) 1.1Q�* �� ScaleY$	* 1.1Q��� �� T� .���� 
���   �$��1q�._�� ���	#FFF5F5FF$��LeftToRight=�$	��141��.+��� -   + ��Name$+ 
checkBoxE1��$��	Encrypted��$��26��$��	0,0,17,17q�$��Bottom=�$��88��$��13(���.  $��1,-2,0,0q�
�������
���   �
���   �$��2q�$ 1��$��Right=�$  0.5,0.5&�z�V� .~��� $	) 1.3Q�$	* 1.3Q��� �� T� �� -   $+ checkBoxPin��$	��Pin��$��24��$��
0,0,137,17q�$��Bottom=�$��55��$��13(���.  $��1,-2,0,0q�
�������
���   �
���   �$��2q�$ 1��$��Right=�$  0.5,0.5&�z�V� .~��� $	) 1.3Q�$	* 1.3Q��� �� T� �� -   $& textBlockModifiedDate��$��Right=�$��18��$��0,0,-0.5,110q�$
��Wrap=�$��Bottom=�$	��210��$��Arial)�$��16(�
���  ��$��LeftToRight=�$��Normal"��� -   $& textBlockCreatedDate��$��Right=�$��18��$��0,0,0,85q�$
��Wrap=�$��Bottom=�$	��210��$��Arial)�$��16(�
���  ��$��LeftToRight=�$��Normal"��� -   $"& textBlockCountCharactersText��$��Right=�$��22��$��	0,0,15,57q�$
��Wrap=�$��Bottom=�$	��195��$��16(�
����  �$��LeftToRight=�$��Arial)��� -   $$ comboBoxFont1��$��3��$��Right=�$��32��$��	0,2,112,0q�$	��Top=�$��50��$��18(���.  |�.  .@��� $��Arial�����	#FFF0F0F0$��Arial)�$��33��$	��170���� $��Verdana��$��Verdana)�$��33���� $��Segoe Print�����	#FFF0F0F0$��Segoe Print)�$��33���� $��Comic Sans MS��$��Comic Sans MS)�$��33���� $��Source Code Pro�����	#FFF0F0F0$��16(�$��Source Code Pro Medium)�$��33���� $��	Castellar��$��	Castellar)�$��33���� $��Times New Roman�����	#FFF0F0F0$��Times New Roman)�$��33������ "
 ��.���  �@�FocusVisualStyle Z�  P�  �@J�	IsFocused . . ���  � ��$ 2,2,2,2q��� -   $$ comboBoxColors��$��3��$��32��$��0,2,2,0q�$	��Top=�$��50��$��Right=���.  |�.  ���� $ Black��$ 0Q�.@��� $�� ��$
��0.01��$��1(��� $
��Blue��
����  �$��33��$��75��$��15(�$
��Bold"�$��4,4,2,1q��� $	��Red�����	#FFF0F0F0
���  ��$��33��$��15(�$
��Bold"�$��4,4,2,1q��� $��Green��
��� � �$��33��$��15(�$
��Bold"�$��4,4,2,1q��� $��Purple�����	#FFF0F0F0
���� ��$��33��$��15(�$
��Bold"�$��4,4,2,1q��� $��Black��
���   �$��33��$��15(�$
��Bold"�$��4,4,2,1q����� "
 ��.���  �@�FocusVisualStyle Z�  P�  �@J�	IsFocused . . ���  � ��$ 2,2,2,2q�9� -   $' expanderSearches��$��3��$
( None=�$��Search��$
��Left=�$	��104��$��	5.5,0,0,3q�$��Bottom=�$	��240��, 9� ExpandDirection$, Up=�$��20(���.  $ 1��$��0q�- � FocusVisualStyle- Z� ��Z� .���� 
���   �$��1q�$��0,0,8,0q�._�� ���	#FFF5F5FF.+��� -   . ��Name$. textBoxSearchAll��$
��Left=�$��30��$��53,0,0,4q�/ �� TextWrapping$
/ Wrap=�$��Bottom=�$	��170��$��16(�$��0,1,0,0q���.  $ 1��
���   �$�� Search all notes���� -    0 ��Name$0 btnSearchAll��$	��All��$
��Left=�$��29��$��4,0,0,4q�$��Bottom=�$��44����.  $ 1��
���   �#��� ��$
��Bold"�$��16(��� -!   $0 	btnSearch��$
��Note��$
��Left=�$��29��$��4,0,0,39q�$��Bottom=�$��44��$
��Bold"�
���   �#��� ��$��16(���.  $��1,1,1,2q��� -"   $. textBoxSearch��$
��Left=�$��29��$��	53,0,0,39q�$
/ Wrap=�$��Bottom=�$	��170��$�� Search in note��$��16(���.  
���   �$��0,1,0,0q��� -#   1 ��Name$1 lblSearchMatches��$�� ��$
��Left=�$��29��$��
190,0,0,39q�$��Bottom=�$
��38.5��$ 1��$��20(�$��2,0,0,1q�$
��Bold"�$��RightToLeft=�� -$   $ gridSearches��$��3��$
��Left=�$��28��$��0,0,0,3q�$��Bottom=�$	��244��
�������.+��� 
���   �$��1,0,1,1q�$
��Left=�$��28��$	��Top=�$	��245��$��-1,0,0,0q�$ -2��