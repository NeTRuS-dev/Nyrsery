   M S B A M L   `   `   `  ����B  >Nursery, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Nursery.ViewNursery.View  5   	       Nursery.View.ClientView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Nursery.View     5   	     � Icon$�  zpack://application:,,,/Nursery;component/Common/Google-Noto-Emoji-People-Bodyparts-11947-middle-finger-light-skin-tone.ico��5   	    � Title$ 
ClientView��5
   	   $	��600��6   $
��1000��6)   .��5      �    defaultGrid#��[  6    � RowDefinitions 5   
   �� 5      $��1* �6   �� 5      $
��1.5* �6    � ColumnDefinitions 5   
   �� 5      $
��1.4* �6   �� 5      $��4* �6   .+�5   
   �   menuGrid#��[ 6   .+�5      �� $��Center=�6   .+�5      �� $:��4Вы клиент фирмы ООО 'Ми-Ми-Ми'��6     defaultLabel#��[ 6A   $��Center=�6g   �� 5       � Cursor$
 Hand��6     	labelLink#��[ 6&   $��Center=�6I   $/��)Редактировать профиль��6g    J� InputBindings 5      d� 5       d� MouseAction$ 	LeftClicke�6'    �� Command 5   '   �� *GoToEditter+5      �� 5      $
 Hand��6   #��[ 6&   $��Center=�6J   $.��(Устроиться в питомник��6h    5      d� 5       $ 	LeftClicke�6'    5!   '   �� *	GetaJob+5#      �� 5%        verticalStackPanel#��[ 6   .+�5&      �� $��Питомцы��6     defaultButton#��[ 6M   $
��35,0q�6t   $	��0,7q�6�   $	��150��6�   ��6/   �� *	PetsFrame+6�   �� 5'      $��Мои питомцы��6   #��[ 6S   $
��10,0q�6z   $	��0,7q�6�   ��63   �� *MyPetsFrame+6�   5)      5*      5+       � 5,   
   $��1��6   $��2��5-        � NavigationUIVisibility$ Hidden=�5/      $��0��51      ��5.      �� *CurrentPage+	 J� Opacity	 50      �� *FrameOpacity+51       � 52   
   $��0��6   $ Hidden=�55      $��1��57      ��54      �� *CurrentPageHelper+	 56      �� *FrameOpacity+57      59      5:      