   M S B A M L   `   `   `  ����   
S2XConsole&     S2XConsole.Views.LoadSettingView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     
  � Tag$7  1Software Download Location or SmartSystems Folder��.���  � RowDefinitions �� �� �� �� �� �� �� �� ��  � ColumnDefinitions �� $
��Auto ��� �� $
��Auto �.+��� -   $��labelUrl��$��0��$��0��$��10, 5q� � ToolTip$8 2Requires a SmartSystems client of 3.44 or greater.��.��Load Software from URL�� -   $��labelFolder��$��1��$��0��$
��10,5q�$8 2Requires a SmartSystems client of 3.44 or greater.��.��%#Place device in SmartSystems folder�� -    ��Name$ comboBoxUrl��$��0��$��1��$8 2Requires a SmartSystems client of 3.44 or greater.��$
��10,5q�$	��300�� �� 
IsEditable .  �� StaysOpenOnEdit .   �� SelectedIndex$ -1��$��1�� �� IsTextSearchEnabled .  	 �� IsTextSearchCaseSensitive	 . ���� 
 �� Path$
 UrlAutoComplete��� -   $ comboBoxFolder��$��1��$��1��$8 2Requires a SmartSystems client of 3.44 or greater.��$
��10,5q�$	��200�� .  . $
��Auto��$��2�� . 	 . ���� $
 FolderAutoComplete��� -   $��checkBoxDestination��$#��Specify Destination on Device��$��2��$��0��$
��15,5q�$��2��$y�	Collapsed=� d� 	IsChecked �� $
 DestinationSpecified� �� Mode$ OneWayToSource=��� -    ��Name$ labelSoftwareDest��$��3��$��0��$
��10,5q�$y�	Collapsed=�.��Software Destination�� -    ��Name$ textboxSoftwareDest��$��3��$��1��$
��10,5q�$y�	Collapsed=�.������ $
 Destination�$ OneWayToSource=� �� UpdateSourceTrigger$ PropertyChanged=�.���� -   $ labelTextFile��$��4��$��0��$
��10,5q�$y�Visible=�.��	Text File�� -	   $ textboxTextFile��$��4��$��1��$
��10,5q�$y�Visible=� �� 
IsReadOnly . .������ $
 TextFile�$ TwoWay=�$ PropertyChanged=�.���� -
    ��Name$ buttonBrowseTextFile��$��4��$��2��$
��10,5q�.��Browse�� -   $ labelTextFileDest��$��5��$��0��$
��10,5q�$y�Visible=�.��Text File Destination�� -   $ textboxTextFileDest��$��5��$��1��$
��10,5q�$y�Visible=�.������ $
 TextFileDestination�$ OneWayToSource=�$ PropertyChanged=�.���� -   $ labelOta��$��6��$��0��$
��10,5q�$y�Visible=�.��Load Update from URL�� -   $ comboBoxOta��$��6��$��1��$
��10,5q�$	��300�� .  .  $ -1��$��1�� . 	 . ���� $
 UrlAutoComplete��� -   $ labelLoadText��$��7��$��0��$
��10,5q�$y�Visible=�.��Load Text File from URL�� -   $ comboBoxLoadText��$��7��$��1��$
��10,5q�$	��300�� .  .  $ -1��$��1�� . 	 . ���� $
 UrlAutoComplete��� -   $ labelDestTextFromUrl��$��8��$��0��$
��10,5q�$y�Visible=�$(��"Destination for Text File from URL���� -   $  textboxDestTextFileFromUrl��$��8��$��1��$
��10,5q�$y�Visible=�.������ $ 
 TextFileFromUrlDestination�$ OneWayToSource=�$ PropertyChanged=�.��