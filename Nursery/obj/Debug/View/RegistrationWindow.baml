   M S B A M L   `   `   `  ����B  >Nursery, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null+clr-namespace:Nursery.ViewNursery.View  5   	   5clr-namespace:Nursery.ViewModelNursery.ViewModel  5   	   -clr-namespace:Nursery.ModelNursery.Model  5   	   %    Nursery.View.RegistrationWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   ,localclr-namespace:Nursery.View     5   	   5	viewmodelclr-namespace:Nursery.ViewModel     5   	   -modelclr-namespace:Nursery.Model     5   	     � Icon$�  zpack://application:,,,/Nursery;component/Common/Google-Noto-Emoji-People-Bodyparts-11947-middle-finger-light-skin-tone.ico��5   	    � Title$% Питомник Ми-Ми-Ми��5   	   $	��600��6#   $
��1000��60    � 	Resources 5      "  Nursery.Model.ConverterInput   	Converter��%   &	          5   
   6   5       J� InputBindings 5      �� 5   
   
 �� Key$ Return��65    �� Command 6   �� *LoginButton+ �� CommandParameter 5      `� 5       `� 	Converter# [  6   .1�5      ��  �� ElementName$ 	FirstName��6   	 �� Path$
	 Text�66   �� 5      $ 
SecondName��6   $
	 Text�67   �� 5      $ LastName��6   $
	 Text�65   �� 5      $ Address��6   $
	 Text�64   �� 5      $ PhoneNumber��6   $
	 Text�68   �� 5      $ Login��6   $
	 Text�62   5      5      .��5"   
   �   defaultGrid#��[ 6   
 � ColumnDefinitions
 5#      �� 5$      $��* �6#   �� 5%      $��* �6#    � RowDefinitions 5'      �� 5(      $��1* �6    �� 5)      $��4* �6    �� 5*      $��* �6   .+�5.   
   � $��0��6   $��1��6   
 5/      �� 50      $
��0.8* �6#   �� 51      $��* �6#   �� 52      $
��0.2* �6#   .+�55      �� $��1��6   .+�57      ��   baseStackPanel#��[ 6   $��Right=�6E   .+�58      �� $��Имя:��6   �� 59      -   5         ��Name$ 	FirstName��59   F     defaultTextBox#��[ 6   6F   5:      �� 5;      #��[ 6   $��Right=�6E   .+�5<      �� $��Фамилия:��6   �� 5=      -   5        $ 
SecondName��5=   F   #��[ 6   6F   5>      �� 5?      #��[ 6   $��Right=�6E   .+�5@      �� $��Отчество:��6   �� 5A      -   5        $ LastName��5A   F   #��[ 6   6F   5B      �� 5C      #��[ 6   $��Right=�6E   .+�5D      �� $��Пол:��6   �� 5E      $��81��5G      ��5E      �� *Males+ �� SelectedItem 5F      �� *
SettedMale+��5H      �� 5I      .R�5J   "   �� ��6,   �� *Male+5K      5M      5N      �� 5[      #��[ 6   $��Right=�6E   .+�5\      �� $ ��Дата рождения:��6   (  "System.Windows.Controls.DatePicker  5]         SelectedDate 6!   �� *
DateOfBorn+   DisplayDateStart 6E   �� *	StartTime+   DisplayDateEnd 6l   �� *	EndTime+5^      �� 5_      #��[ 6   $��Right=�6E   .+�5`      �� $��Адрес:��6   �� 5a      -   5        $ Address��5a   G   #��[ 6   6G   5b      �� 5c      #��[ 6   $��Right=�6E   .+�5d      �� $"��Номер телефона:��6   �� 5e      -   5        $ PhoneNumber��5e   G   #��[ 6   ��6\   �� *	Numbers+ �� UpdateSourceTrigger$ PropertyChanged=�5f      5g      5h      �� 5i      $��Center=�6   $��0��61   $��2��6>   $��Регистрация��6R     headerLabel#��[ 6h   � 5j   
   $��1��6   $��1��6    
 5k      �� 5l      $
��0.3* �6#   $	��100��60   �� 5m      $��* �6#   �� 5n      $
��0.7* �6#   .+�5q      �� $��1��6   .+�5s      �� #��[ 6   $��Right=�6E   .+�5t      �� $&�� Придумайте логин:��6   �� 5u      -   5        $ Login��5u   F   #��[ 6   6F   5v      �� 5w      #��[ 6   $��0,10,0,0q�6E   $��Right=�6W   .+�5x      �� $(��"Придумайте пароль:��6   G� 5y      "   Nursery.Model.PasswordHelper   Attach .   defaultPasswordBox#��[ 5z         Password 5y   E   �� *Pass+ �� Mode$ TwoWay=�$ PropertyChanged=���6�   �� *
PassBorder+5z      5|      �� 5}      $	��Top=�6a   
���  ��$��Right=�6�   ��6   �� *BedPasswordText+$ PropertyChanged=�6�   �� 5~      #��[ 6   $��0,2,0,10q�6E   $��Right=�6W   .+�5      �� $&�� Повторите пароль:��6   G� 5�       . #��[ 5�       5�   E   �� *	Passrep+$ TwoWay=�$ PropertyChanged=���6�   �� *
PassBorder+5�      5�      �� 5�      $	��Top=�6g   
���  ��$��Right=�6�   ��6   �� *BedPasswordRepeatText+$ PropertyChanged=�6�   �� 5�        verticalStackPanel#��[ 6   $��Right=�6I   .+�5�      �� $5��/Введите секретный вопрос:��6   $��Center=�6@   �� 5�      -   5        $ SecretQuestion��5�       �� TextWrapping$
 Wrap=�67   $	��200��6K   $��30��6Z   " �� VerticalScrollBarVisibility$ Visible=�5�       �� AcceptsReturn . 5�      5�      �� 5�      #��[ 6   $��Right=�6I   .+�5�      �� $E��?Введите ответ на секретный вопрос:��6   �� 5�      -   5        $ SecretAnswer��5�   G   #��[ 6   $	��200��6]   5�      5�      5�      �� 5�   
   #��[ 6   $��Center=�6=   $��Center=�6X   $��2��6u   $��2��6�   .+�5�      ��   	labelLink#��[ 6   $��16(�67   $��40,10q�6E   $��	0,0,150,0q�6U   $��Center=�6h    � Cursor$
 Hand��6�   $��Отмена��6�    5�      d� 5�       d� MouseAction$ 	LeftClicke�6#    5�   '   �� *	GotoLogin+5�      �� 5�        defaultButton#��[ 6   $	��5,0q�6>   $	��150��6L   $&�� Зарегистрировать��6z   ��6[   �� *
Registrate+��5�      `� 5�      # [  6#   .1�5�      �� $ 	FirstName��6"   $
	 Text�6:   �� 5�      $ 
SecondName��6"   $
	 Text�6;   �� 5�      $ LastName��6"   $
	 Text�69   �� 5�      $ Address��6"   $
	 Text�68   �� 5�      $ PhoneNumber��6"   $
	 Text�6<   �� 5�      $ Login��6"   $
	 Text�66   �� 5�      $ SecretQuestion��6"   $
	 Text�6?   �� 5�      $ SecretAnswer��6"   $
	 Text�6=   5�      5�      5�      5�      5�      