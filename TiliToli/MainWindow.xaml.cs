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
        Button[,] boardMatrix;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonSwitchEvent(object sender, RoutedEventArgs e)
        {
            //TODO: Move Button
            //TODO: Check Win
        }

        private void GameClickEvent(object sender, RoutedEventArgs e)
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
                    InitBoard();
                }

            }
            catch (FormatException)
            {
                MessageBox.Show("Számot adjon meg a tábla méretének.");
            }
        }

        private void InitBoard()
        {
            boardSizeLabel.Visibility = Visibility.Collapsed;
            boardSizeInput.Visibility = Visibility.Collapsed;
            gameButton.Visibility = Visibility.Collapsed;
            boardMatrix = new Button[boardSizeValue, boardSizeValue];
            int k = 1;

            for (int i = 0; i < boardSizeValue; i++)
            {
                grid.ColumnDefinitions.Add(new ColumnDefinition());
                grid.RowDefinitions.Add(new RowDefinition());
            }
            
            for (int i = 0; i < boardSizeValue; i++)
            {
                for (int j = 0; j < boardSizeValue; j++)
                {
                    Button newButton = new Button();
                    newButton.Name = "Button" + k.ToString();
                    newButton.Content = k.ToString();
                    Grid.SetRow(newButton, i);
                    Grid.SetColumn(newButton, j);
                    newButton.Click += ButtonSwitchEvent;
                    boardMatrix[i, j] = newButton;
                    grid.Children.Add(newButton);
                    k++;
                }
            }
            boardMatrix[boardSizeValue-1,boardSizeValue-1].Visibility = Visibility.Hidden;
        }
    }
}
