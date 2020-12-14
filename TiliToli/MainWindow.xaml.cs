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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        #region Global Variables
        /// <value>Holds the board size.</value>
        int boardSizeValue = 0;
        /// <value>Holds the winning board positions.</value>
        Button[,] winBoardMatrix;
        /// <value>Holds the current board positions.</value>
        Button[,] currBoardMatrix;
        /// <value>An index variable.</value>
        int k = 1;
        /// <value>Holds the last button.</value>
        Button lastBtn;
        /// <value>Holds the current step count.</value>
        int steps = 0;
        /// <value>Holds the step count label</value>
        Label stepLbl = new Label();
        /// <value>Holds the new game button.</value>
        Button newGamBtn = new Button();
        #endregion

        #region Button Click events
        /// <summary>
        /// It's called when any button clicked in the game. Switch two buttons and check whether the player won.
        /// </summary>
        /// <param name="sender">The button which was clicked.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void ButtonSwitchEvent(object sender, RoutedEventArgs e)
        {
            MoveButtons(sender);
            CheckWin();
        }
        /// <summary>
        /// It's called when the restart game button clicked. Resets the global variables and init the game.
        /// </summary>
        /// <param name="sender">The button which was clicked.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
        private void RestartGameEvent(object sender, RoutedEventArgs e)
        {
            k = 1;
            steps = 0;
            grid.Children.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            InitBoard();
        }
        /// <summary>
        /// It's called when the game button clicked. Get the boardSizeValue from the boardSizeInput and init the board.
        /// </summary>
        /// <param name="sender">The button which was clicked.</param>
        /// <param name="e">Contains state information and event data associated with a routed event.</param>
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
        #endregion

        #region Void Methods
        /// <summary>
        /// Initialize the board.
        /// </summary>
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
            grid.RowDefinitions.Add(new RowDefinition());
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
            newGamBtn.Name = "newGamButton"; newGamBtn.Content = "Új játék"; newGamBtn.HorizontalAlignment = HorizontalAlignment.Right; newGamBtn.VerticalAlignment = VerticalAlignment.Bottom; newGamBtn.Margin = new Thickness(0, 0, 10, 10); newGamBtn.FontSize = 14; newGamBtn.Width = 75; newGamBtn.Click += RestartGameEvent;
            Grid.SetRow(newGamBtn, boardSizeValue);
            Grid.SetColumn(newGamBtn, boardSizeValue - 1);
            grid.Children.Add(newGamBtn);
            stepLbl.Name = "stepLabel"; stepLbl.Content = "Lépések száma: "; stepLbl.HorizontalAlignment = HorizontalAlignment.Left; stepLbl.Margin = new Thickness(10, 0, 0, 10); stepLbl.VerticalAlignment = VerticalAlignment.Bottom; stepLbl.Width = 130; stepLbl.FontSize = 14;
            Grid.SetRow(stepLbl, boardSizeValue);
            Grid.SetColumn(stepLbl, 0);
            Grid.SetColumnSpan(stepLbl, boardSizeValue - 1);
            grid.Children.Add(stepLbl);
            MixingBoard();
        }

        /// <summary>
        /// Mix the board positions.
        /// </summary>
        private void MixingBoard()
        {
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

        /// <summary>
        /// Change two positions in the matrix.
        /// </summary>
        private void ChangeMatrix(Button firstBtn, Button secondBtn)
        {
            int[] firstBtnCord = FindMatrixPos(firstBtn);
            int[] secondBtnCord = FindMatrixPos(secondBtn);
            currBoardMatrix[firstBtnCord[0], firstBtnCord[1]] = secondBtn;
            currBoardMatrix[secondBtnCord[0], secondBtnCord[1]] = firstBtn;
        }

        /// <summary>
        /// Change two Buttons on the UI and two positions on the matrix.  
        /// </summary>
        /// <param name="sender">The button which was clicked</param>
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
                steps++;
                stepLbl.Content = $"Lépések száma: {steps}";
            }
        }

        /// <summary>
        /// Change two Buttons on the UI.
        /// </summary>
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

        /// <summary>
        /// Compare the two matrices. If match end the game.
        /// </summary>
        private void CheckWin()
        {
            var equal =
            Enumerable.Range(0, winBoardMatrix.Rank).All(dimension => winBoardMatrix.GetLength(dimension) == currBoardMatrix.GetLength(dimension)) &&
            winBoardMatrix.Cast<Button>().SequenceEqual(currBoardMatrix.Cast<Button>());
            if (equal)
            {
                for (int i = 0; i < boardSizeValue; i++)
                {
                    for (int j = 0; j < boardSizeValue; j++)
                    {
                        currBoardMatrix[i, j].IsEnabled = false;
                    }
                }
                MessageBox.Show("Nyertel");
            }
        }
        /// <summary>
        /// Show the current matrix positions in the console.
        /// </summary>
        private void ShowMatrix()
        {
            for (int i = 0; i < boardSizeValue; i++)
            {
                for (int j = 0; j < boardSizeValue; j++)
                {
                    Console.Write($"({currBoardMatrix[i, j].Content})");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
        #endregion

        #region Return Methods
        /// <summary>
        /// Get a button x and y positions.
        /// </summary>
        /// <returns>An int array with a length of two, which contains the Button x and y positions.</returns>
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

        /// <summary>
        /// Find a button by position.
        /// </summary>
        /// <param name="x">A matrix x position.</param>
        /// <param name="y">A matrix y position.</param>
        /// <returns></returns>
        private Button FindButton(int x, int y)
        {
            Button button = currBoardMatrix[x, y];
            return button;
        }

        /// <summary>
        /// Get a button wich is appropiate for the mixing.
        /// </summary>
        /// <returns>A random button, wich is meets the conditions.</returns>
        private Button FindARandomPossibleSwitch(Button button)
        {
            Random r = new Random();
            List<Button> possibleSwitches = new List<Button>();
            int[] buttonPos = FindMatrixPos(button);
            if (buttonPos[0] == 0 && buttonPos[1] == 0)
            {
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1] + 1));
                possibleSwitches.Add(FindButton(buttonPos[0] + 1, buttonPos[1]));
                return possibleSwitches[r.Next(0, 2)];
            }
            else if (buttonPos[0] == boardSizeValue - 1 && buttonPos[1] == boardSizeValue - 1)
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
            else if (buttonPos[0] == 0 && buttonPos[1] == boardSizeValue - 1)
            {
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1] - 1));
                possibleSwitches.Add(FindButton(buttonPos[0] + 1, buttonPos[1]));
                return possibleSwitches[r.Next(0, 2)];
            }
            else if (buttonPos[0] == 0)
            {
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1] - 1));
                possibleSwitches.Add(FindButton(buttonPos[0] + 1, buttonPos[1]));
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1] + 1));
                return possibleSwitches[r.Next(0, 3)];
            }
            else if (buttonPos[1] == 0)
            {
                possibleSwitches.Add(FindButton(buttonPos[0] - 1, buttonPos[1]));
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1] + 1));
                possibleSwitches.Add(FindButton(buttonPos[0] + 1, buttonPos[1]));
                return possibleSwitches[r.Next(0, 3)];
            }
            else if (buttonPos[0] == boardSizeValue - 1)
            {
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1] - 1));
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1] + 1));
                possibleSwitches.Add(FindButton(buttonPos[0] - 1, buttonPos[1]));
                return possibleSwitches[r.Next(0, 3)];
            }
            else if (buttonPos[1] == boardSizeValue - 1)
            {
                possibleSwitches.Add(FindButton(buttonPos[0] - 1, buttonPos[1]));
                possibleSwitches.Add(FindButton(buttonPos[0] + 1, buttonPos[1]));
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1] - 1));
                return possibleSwitches[r.Next(0, 3)];
            }
            else
            {
                possibleSwitches.Add(FindButton(buttonPos[0] - 1, buttonPos[1]));
                possibleSwitches.Add(FindButton(buttonPos[0] + 1, buttonPos[1]));
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1] - 1));
                possibleSwitches.Add(FindButton(buttonPos[0], buttonPos[1] + 1));
                return possibleSwitches[r.Next(0, 4)];
            }
        }
        #endregion
    }
}
