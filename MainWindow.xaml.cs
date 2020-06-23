using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace File_Iterator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

             
        }

       
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Properties.Settings1.Default.Save();
        }

        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }


        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void btnMaximize_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Maximized)
                WindowState = WindowState.Normal;
            else
                WindowState = WindowState.Maximized;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.CheckKeyword(sender as RichTextBox, "while", Colors.Purple, 0);
            this.CheckKeyword(sender as RichTextBox, "if", Colors.Green, 0);
        }
        private void CheckKeyword(RichTextBox box, string word, Color color, int startIndex)
        {
            //if (Rchtxt.Text.Contains(word))
            //{
            //    int index = -1;
            //    int selectStart = Rchtxt.SelectionStart;

            //    while ((index = Rchtxt.Text.IndexOf(word, (index + 1))) != -1)
            //    {
            //        //Rchtxt.select((index + startIndex), word.Length);
            //        //Rchtxt.SelectionColor = color;
            //        //Rchtxt.Select(selectStart, 0);
            //        //Rchtxt.SelectionColor = Colors.Black;
            //    }
        }

    }
}
