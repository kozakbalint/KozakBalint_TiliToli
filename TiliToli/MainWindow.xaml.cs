using System;
using System.Collections.Generic;
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

namespace TiliToli
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int boardSizeValue = 0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Game_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                boardSizeValue = Int32.Parse(boardSizeInput.Text);
                if (boardSizeValue < 3)
                {
                    MessageBox.Show("Nagyobb számot adj meg.");
                }
                else if (boardSizeValue > 10)
                {
                    MessageBox.Show("Kisebb számot adj meg.");
                }
                else
                {
                    //TODO: Game method.
                }

            }
            catch (FormatException)
            {
                MessageBox.Show("Számot adjon meg a tábla méretének.");
            }
        }
    }
}
