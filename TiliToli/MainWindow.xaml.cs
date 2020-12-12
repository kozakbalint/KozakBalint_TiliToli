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
        Button[,] winBoardMatrix;
        Button[,] currBoardMatrix;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonSwitchEvent(object sender, RoutedEventArgs e)
        {
            ShowMatrix();
            MoveButtons(sender);
            ShowMatrix();
            //TODO: Check Win
        }

        //Show the current matrix in the console
        private void ShowMatrix()
        {
            //Only for debug purposes
            for (int i = 0; i < boardSizeValue; i++)
            {
                for (int j = 0; j < boardSizeValue; j++)
                {
                    Console.Write($"({currBoardMatrix[i,j].Content})");
                }
                Console.WriteLine();
            }
        }

        //Check the board input and call the InitBoard and MixingBoard methods 
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
                    //TODO: Mixing the board.
                }

            }
            catch (FormatException)
            {
                MessageBox.Show("Számot adjon meg a tábla méretének.");
            }
        }

        //Generate the buttons and initalize the matrices 
        private void InitBoard()
        {
            boardSizeLabel.Visibility = Visibility.Collapsed;
            boardSizeInput.Visibility = Visibility.Collapsed;
            gameButton.Visibility = Visibility.Collapsed;
            winBoardMatrix = new Button[boardSizeValue, boardSizeValue];
            currBoardMatrix = new Button[boardSizeValue, boardSizeValue];
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
                    winBoardMatrix[i, j] = newButton;
                    currBoardMatrix[i, j] = newButton;
                    grid.Children.Add(newButton);
                    k++;
                }
            }
            winBoardMatrix[boardSizeValue-1,boardSizeValue-1].Visibility = Visibility.Hidden;
        }

        //Update the buttons when two buttons switch positions.
        private void MoveButtons(object sender)
        {
            Button oneBtn = sender as Button;
            Button lastBtn = winBoardMatrix[boardSizeValue - 1, boardSizeValue - 1];
            int[] oneBtnCord = new int[2];
            int[] lastBtnCord = new int[2];
            int maxHCord = Math.Abs(Grid.GetRow(oneBtn) - Grid.GetRow(lastBtn));
            int maxVCord = Math.Abs(Grid.GetColumn(oneBtn) - Grid.GetColumn(lastBtn));
            if ((maxHCord == 1 && maxVCord == 0) || (maxHCord == 0 && maxVCord == 1))
            {
                oneBtnCord[0] = Grid.GetRow(oneBtn);
                oneBtnCord[1] = Grid.GetColumn(oneBtn);
                lastBtnCord[0] = Grid.GetRow(lastBtn);
                lastBtnCord[1] = Grid.GetColumn(lastBtn);
                for (int i = 0; i < 2; i++)
                {
                    int tmp = oneBtnCord[i];
                    oneBtnCord[i] = lastBtnCord[i];
                    lastBtnCord[i] = tmp;
                }
                Grid.SetRow(oneBtn, oneBtnCord[0]);
                Grid.SetRow(lastBtn, lastBtnCord[0]);
                Grid.SetColumn(oneBtn, oneBtnCord[1]);
                Grid.SetColumn(lastBtn, lastBtnCord[1]);
                ChangeMatrix(oneBtn, lastBtn);
            }
        }

        //Update the matrix when two buttons switch position.
        private void ChangeMatrix(Button firstBtn, Button secondBtn)
        {
            int[] firstBtnCord = new int[2];  
            int[] secondBtnCord = new int[2]; 
            for (int i = 0; i < boardSizeValue; i++)
            {
                for (int j = 0; j < boardSizeValue; j++)
                {
                    if (currBoardMatrix[i,j] == firstBtn)
                    {
                        firstBtnCord[0] = i;
                        firstBtnCord[1] = j;
                    }
                    if (currBoardMatrix[i, j] == secondBtn)
                    {
                        secondBtnCord[0] = i;
                        secondBtnCord[1] = j;
                    }
                }
            }
            currBoardMatrix[firstBtnCord[0], firstBtnCord[1]] = secondBtn;
            currBoardMatrix[secondBtnCord[0], secondBtnCord[1]] = firstBtn;
        }
    }
}
