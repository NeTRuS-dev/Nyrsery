   M S B A M L   `   `   `  ����B  >Nursery, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Nursery.ViewNursery.View  5   	   5clr-namespace:Nursery.ViewModelNursery.ViewModel  5   	       Nursery.View.AboutView   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Nursery.View     5   	   5	viewmodelclr-namespace:Nursery.ViewModel     5   	     � WindowStartupLocation$  CenterScreen=�5       � Icon$� zpack://application:,,,/Nursery;component/Common/Google-Noto-Emoji-People-Bodyparts-11947-middle-finger-light-skin-tone.ico��5	   	    � 
ResizeMode$ NoResize=�5
   	    � Title$ О программе��5   	   $	��600��6   $	��800��6*   .��5      �    menuGrid#��[  6   ��5   
   �� 5       �� ImageSource$? 9pack://application:,,,/Nursery;component/Common/About.jpg��6    j� Stretch$ UniformToFill=�6a    �� Opacity$	 0.9Q�6y    � ColumnDefinitions 5   
   �� 5      $��1* �6   �� 5      $��1* �6   �� 5      $��2* �6    � RowDefinitions 5   
   �� 5      $��1* �6   �� 5      $��3* �6   �� 5      $��1* �6   .+�5   
   �� $��0��6   $��2��6   $��О программе��6-   $��Center=�6C   $��26(�6a   $
��Bold"�6o   
������$��Center=�6�   	 J� Effect	 5      3  -System.Windows.Media.Effects.DropShadowEffect  5      6%   5       �� 5!   
   $��2��6   $��1��6%   $��Center=�62   .+�5"      �� $��Ми-Ми-Ми��6   $��22(�6)   
�������
 �� TextAlignment$
 Center=�6J   $��0,0,0,20q�6a   	 5#        5$         Opacity$ 1Q�6'      
BlurRadius$ 5Q�64      ShadowDepth$ 0Q�6C      Color$ Black��6S   6c   5&      �� 5'      $
 Center=�6   
�������$��Center=�6C   $��Center=�6`   $
��Wrap=�6{   $����Мы приветствуем вас в нашей программе. Данная программа была разработана специально для вас и вашего удобства. Мы верим, что наше творение поможет вам сделать правильный выбор услуги и получить полное удовлетворение от ее получения.��6�   $
��30,0q�6�  $��20(�6�    defaultTextBlock#��[ 6�  	 5(        5)      $ 1Q�6'   $ 5Q�64   $ 0Q�6C   $ Black��6S   6c   5,      �� 5-      $&�� Телефон: +7-928-810-84-92��6   
�������$��20(�6N   $
 Center=�6]   $
��0,20q�6t   	 5.        5/      $ 1Q�6'   $ 5Q�64   $ 0Q�6C   $ Black��6S   6c   51      52      �� 53   
   $��2��6   $��2��6%   $��Center=�62   .+�54      �� $��Понятно��6   $��Center=�6(   $	��Top=�6E     defaultButton#��[ 6}   $��40��6�   ��6]   �� *CloseWindow+6�   56      57      59      