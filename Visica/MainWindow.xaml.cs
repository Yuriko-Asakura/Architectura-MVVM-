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

namespace Visica
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
        }
        private List<Button> buttons = new List<Button>();
        private string word;
        private string guessedLetters = "";

        private void CreateButtons(string word)
        {
            this.word = word;
            int wordLength = word.Length;
            for (int i = 0; i < wordLength; i++)
            {
                Button button = new Button();
                button.Content = "";
                button.Width = 50;
                button.Height = 50;
                button.Margin = new Thickness(5);
                button.IsEnabled = false;
                buttons.Add(button);
                Grid.SetColumn(button, i);
                gridButtons.Children.Add(button);
            }
        }

        private void UpdateButtons(char letter)
        {
            guessedLetters += letter;
            for (int i = 0; i < word.Length; i++)
            {
                if (word[i] == letter)
                {
                    buttons[i].Content = letter.ToString();
                    buttons[i].IsEnabled = true;
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textBox.Text.Length > 0)
            {
                char letter = textBox.Text[0];
                if (guessedLetters.Contains(letter))
                {
                    MessageBox.Show("You already guessed this letter!");
                }
                else if (word.Contains(letter))
                {
                    UpdateButtons(letter);
                }
                else
                {
                    MessageBox.Show("This letter is not in the word!");
                }
                textBox.Text = "";
            }
        }
    }
}
