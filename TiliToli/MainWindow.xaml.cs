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
        int k = 1;
        Button lastBtn;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonSwitchEvent(object sender, RoutedEventArgs e)
        {
            MoveButtons(sender);
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
            Console.WriteLine();
        }

        //Check the board input and call the InitBoard method.
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

        //Generate the buttons and initalize the matrices 
        private void InitBoard()
        {
            boardSizeLabel.Visibility = Visibility.Collapsed;
            boardSizeInput.Visibility = Visibility.Collapsed;
            gameButton.Visibility = Visibility.Collapsed;
            winBoardMatrix = new Button[boardSizeValue, boardSizeValue];
            currBoardMatrix = new Button[boardSizeValue, boardSizeValue];

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
            lastBtn = winBoardMatrix[boardSizeValue - 1, boardSizeValue - 1];
            lastBtn.Visibility = Visibility.Hidden;
            MixingBoard();
            
        }

        //Update the buttons when two buttons switch positions.
        private void MoveButtons(object sender)
        {
            Button oneBtn = sender as Button;
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

        private void MoveButtons(Button firstBtn, Button secondBtn)
        { 
            int[] firstBtnCord = new int[2];
            int[] secondBtnCord = new int[2];
            int maxHCord = Math.Abs(Grid.GetRow(firstBtn) - Grid.GetRow(secondBtn));
            int maxVCord = Math.Abs(Grid.GetColumn(firstBtn) - Grid.GetColumn(secondBtn));
            if ((maxHCord == 1 && maxVCord == 0) || (maxHCord == 0 && maxVCord == 1))
            {
                firstBtnCord[0] = Grid.GetRow(firstBtn);
                firstBtnCord[1] = Grid.GetColumn(firstBtn);
                secondBtnCord[0] = Grid.GetRow(secondBtn);
                secondBtnCord[1] = Grid.GetColumn(secondBtn);
                for (int i = 0; i < 2; i++)
                {
                    int tmp = firstBtnCord[i];
                    firstBtnCord[i] = secondBtnCord[i];
                    secondBtnCord[i] = tmp;
                }
                Grid.SetRow(firstBtn, firstBtnCord[0]);
                Grid.SetRow(secondBtn, secondBtnCord[0]);
                Grid.SetColumn(firstBtn, firstBtnCord[1]);
                Grid.SetColumn(secondBtn, secondBtnCord[1]);
            }
        }

        //Return with button positions 
        private int[] FindMatrixPos(Button button)
        {
            int[] buttonCords = new int[2];
            for (int i = 0; i < boardSizeValue; i++)
            {
                for (int j = 0; j < boardSizeValue; j++)
                {
                    if (currBoardMatrix[i, j] == button)
                    {
                        buttonCords[0] = i;
                        buttonCords[1] = j;
                    }
                }
            }
            return buttonCords;
        }

        private Button FindButton(int x, int y)
        {
            Button button = currBoardMatrix[x, y];
            return button;
        }


        //Update the matrix when two buttons switch position.
        private void ChangeMatrix(Button firstBtn, Button secondBtn)
        {
            int[] firstBtnCord = FindMatrixPos(firstBtn);
            int[] secondBtnCord = FindMatrixPos(secondBtn);
            currBoardMatrix[firstBtnCord[0], firstBtnCord[1]] = secondBtn;
            currBoardMatrix[secondBtnCord[0], secondBtnCord[1]] = firstBtn;
        }

        private Button FindARandomPossibleSwitch(Button button)
        {
            Random r = new Random();
            List<Button> possibleSwitches = new List<Button>();
            int[] buttonPos = FindMatrixPos(button);
            if (buttonPos[0] == 0 && buttonPos[1] == 0)
            {
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1]+1));
                possibleSwitches.Add(FindButton(buttonPos[0]+1, buttonPos[1]));
                return possibleSwitches[r.Next(0,2)];
            }
            else if (buttonPos[0] == boardSizeValue-1 && buttonPos[1] == boardSizeValue-1)
            {
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1] - 1));
                possibleSwitches.Add(FindButton(buttonPos[0] - 1, buttonPos[1]));
                return possibleSwitches[r.Next(0, 2)];
            }
            else if (buttonPos[0] == boardSizeValue - 1 && buttonPos[1] == 0)
            {
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1] + 1));
                possibleSwitches.Add(FindButton(buttonPos[0] - 1, buttonPos[1]));
                return possibleSwitches[r.Next(0, 2)];
            } 
            else if (buttonPos[1] == 0 && buttonPos[1] == boardSizeValue - 1) 
            {
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1] - 1));
                possibleSwitches.Add(FindButton(buttonPos[0] + 1, buttonPos[1]));
                return possibleSwitches[r.Next(0, 2)];
            }
            else if (buttonPos[0] == 0)
            {
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1]-1));
                possibleSwitches.Add(FindButton(buttonPos[0]+1, buttonPos[1]));
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1]+1));
                return possibleSwitches[r.Next(0, 3)];
            }
            else if (buttonPos[1] == 0)
            {
                possibleSwitches.Add(FindButton(buttonPos[0]-1, buttonPos[1]));
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1]+1));
                possibleSwitches.Add(FindButton(buttonPos[0]+1, buttonPos[1]));
                return possibleSwitches[r.Next(0, 3)];
            }
            else if (buttonPos[0] == boardSizeValue - 1)
            {
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1]-1));
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1]+1));
                possibleSwitches.Add(FindButton(buttonPos[0]-1, buttonPos[1]));
                return possibleSwitches[r.Next(0, 3)];
            }
            else if (buttonPos[1] == boardSizeValue - 1)
            {
                possibleSwitches.Add(FindButton(buttonPos[0]-1, buttonPos[1]));
                possibleSwitches.Add(FindButton(buttonPos[0]+1, buttonPos[1]));
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1]-1));
                return possibleSwitches[r.Next(0, 3)];
            }
            else
            {
                possibleSwitches.Add(FindButton(buttonPos[0]-1, buttonPos[1]));
                possibleSwitches.Add(FindButton(buttonPos[0]+1, buttonPos[1]));
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1]-1));
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1]+1));
                return possibleSwitches[r.Next(0, 4)];
            }
        }

        private void MixingBoard()
        {
            Random r = new Random();
            Button secondBtn = FindARandomPossibleSwitch(lastBtn);
            Button beforeBtn = secondBtn;
            for (int i = 0; i < 15; i++)
            {
                do
                {
                    secondBtn = FindARandomPossibleSwitch(lastBtn);
                } while (secondBtn == beforeBtn);
                beforeBtn = secondBtn;
                ChangeMatrix(lastBtn, secondBtn);
                MoveButtons(secondBtn, lastBtn);
                ShowMatrix();
            }
        }
    }
}
