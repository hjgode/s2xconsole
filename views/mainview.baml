   M S B A M L   `   `   `  ���� H  DS2XConsole, Version=8.70.3.163, Culture=neutral, PublicKeyToken=null9!clr-namespace:S2XConsole.CommandsS2XConsole.Commands  'clr-namespace:S2XConsole
S2XConsole  3clr-namespace:S2XConsole.ViewsS2XConsole.Views      S2XConsole.Views.MainView   W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     /c!clr-namespace:S2XConsole.Commands     (appclr-namespace:S2XConsole     /viewclr-namespace:S2XConsole.Views     -     � Icon$6  0/S2XConsole;component/Images/ScanNGo_Icon_V1.ico��$	��600��$	��800�� � SizeToContent$ WidthAndHeight=� � Title �� $  S2XConsole.Properties.Settings   Default �� Source# Z  �� Path$ AppDisplayName� � 	Resources *   $S2XConsole.Commands.CommandReference   ExitCommandReference   Command  PreviousCommandReference  NextCommandReference��%|   &	        &	      &	 @        �� *ExitCommand+   �� *PreviousCommand+   �� *NextCommand+ J� CommandBindings )  #System.Windows.Input.CommandBinding  -   	   Command$
	 Help��
 J� InputBindings
 �� 
 �� Key$ X�� �� 	Modifiers$ Controlf� �� Command# [  �� $
 Left��# [ �� $ Right��# [ �� $ Up��# [ �� $
 Down��# [ .��]� .+�o� $	��Top=�.@�m� $��_Help��.@�m� -    m�Name$ menuItemHelp��$
��Help�� m� Command$
 Help�� m� InputGestureText$ F1��m� -   $��About���� $	��Top=�����  �� 
StartPoint$ 0.5,0.5&� �� EndPoint$ 1,0.5&�.���  � Color$ White�� � Offset$	 0.0Q�� $ 	#FF00417B��$	 1.0Q�.+��� $��20(�$��10,10q����� # Z $ AppDisplayName��� $��0, 20q�$��14(�$��Center=����� $ CurrentPage.Tag�� $��Bottom=�$
��Auto�� � RowDefinitions �� $
��Auto ��� $
��Auto �.+��� $��0�� �� Orientation$ 
Horizontal=�$��Center=�$��5q�$��Bottom=�.+��� $��5q�$��30��$��70������ *PreviousCommand+{��� *IsNotFirstPage+.��
Previous�� $��5q�$��30��$��70������ *NextCommand+{��� *IsNotLastPage+.��Next�� $
��20,5q�$��30��$��70������ *ExitCommand+.��Close�� $��1��$ 
Horizontal=�$
��Auto��$��Center=�$��Center=�.+��� $	��2,0q�.��Step�� $	��2,0q����� $ CurrentPageIndex��� $	��2,0q�.��of�� $	��2,0q����� $ 
TotalPages�(   "S2XConsole.Views.PageContainerView  $��Right=�$
��Auto��$
��Auto��$��Center=�$��Center=����� $ CurrentPage�