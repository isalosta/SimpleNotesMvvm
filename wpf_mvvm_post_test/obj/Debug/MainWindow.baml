   M S B A M L   `   `   `  ����M  Iwpf_mvvm_post_test, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null7 clr-namespace:wpf_mvvm_post_testwpf_mvvm_post_test  5   	   #    wpf_mvvm_post_test.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   2local clr-namespace:wpf_mvvm_post_test     5   	   -   5          � 
ResizeMode$  NoResize=�5   	    � Title$ Simple Notes��5
   	   $	��538��6   $	��800��6+   ���	#FF3E3E42 � DataContext 5   	   �� *Main+ 	  MODEL �� Source# [  .��5      ]� ���	#FF3E3E42$��0,0,0,-3q�6'   .+�5   
   o� $	��Top=�6   ���	#FF2D2D30.@�5      m� $��_File��6   
�������.@�5      m� $
��_New��6   
���   �6)   m� 5      $��_Save��6   
���   �6*   �� 5      m� 5      -   5        $��Configuration��5      
�������6N   �� 5      m� 5      $
��EXIT��6   
����������	#FFF100006F   5      m� 5      $��_Tools��6   
�������.@�5      m� $��Sync Now��6   
�������6-   m� 5      $��Delete Notes��6   
�������61   5      5      o� 5   
   $��Bottom=�6   
��� ���$��26��6;   .@�5      �� $
��Wrap=�6   
�������$	��186��6V   $��14(�6b   ��6,   �� *TIMER+6b   5      � 5   
   $��	0,18,0,32q�6   $	��764��6"   ���	#FF333337.+�5!      �� $
��Left=�6   $��26��61   $��
30,131,0,0q�6=    �� TextWrapping$
 Wrap=�6Q   $	��Top=�6�   $	��704��6�   ��6e   �� *TITLE+ �� UpdateSourceTrigger$ PropertyChanged=�6�   �� 5"      $
��Left=�6   $	��222��61   $��
30,176,0,0q�6>   $ WrapWithOverflow=�6R   $	��Top=�6�   $	��704��6�   ��6r   �� *	CONTENT+$ PropertyChanged=�6�   �� 5#      -   5        $
��Left=�5#      $��	30,85,0,0q�62   $	��Top=�6E   $	��342��6]   ��6i   �� *
ITEM_ARR+ �� SelectedIndex 6�   �� *
SELECTED+6�   �� 5$      $
��Left=�6   $��
392,85,0,0q�63   $
��Wrap=�6G   $	��Top=�6o   $��22��6�   $	��342��6�   $
��Bold"�6�   $��16(�6�   
���������6[   �� *ID+6�   �� 5%      -   5        $	��NEW��5%      $
��Left=�6#   $��	30,24,0,0q�6>   $	��Top=�6Q   $	��128��6i   $��40��6u   ���	#FF84C17A$��18(�6�   6�   �� 5&      -   5        $��DELETE��5&      $
��Left=�6&   $��
176,24,0,0q�6A   $	��Top=�6U   $	��128��6m   $��40��6y   ���	#FFC17A7A$��18(�6�   6�   �� 5'      -   5        $
��SAVE��5'      $
��Left=�6$   $��
322,24,0,0q�6?   $	��Top=�6S   $	��128��6k   $��40��6w   ���	#FF7ABBC1$��18(�6�   6�   5(      5)      5*      