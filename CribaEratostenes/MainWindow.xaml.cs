using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CribaEratostenes
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public SolidColorBrush ColorPrimers = new SolidColorBrush(Colors.Purple);
        int[,] matriu;
        List<int> nombresPrimers;
        int mida;
        int delay = 100;

        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// genera la matriu amb els nombres del 1 al mida*mida
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GenerateMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(matrixSizeTextBox.Text, out int matrixSize))
                GenerarMatriu(matrixSize);
        }

        /// <summary>
        /// genera la matriu amb els nombres del 1 al mida*mida
        /// </summary>
        /// <param name="_size"></param>
        private void GenerarMatriu(int _size)
        {
            mida = _size;
            matriu = new int[mida, mida];
            int count = 1;

            for (int i = 0; i < mida; i++)
                for (int j = 0; j < mida; j++)
                    matriu[i, j] = count++;

            matrixGrid.Children.Clear();
            matrixGrid.RowDefinitions.Clear();
            matrixGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < mida; i++)
            {
                matrixGrid.RowDefinitions.Add(new RowDefinition());
                matrixGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < mida; i++)
            {
                for (int j = 0; j < mida; j++)
                {
                    Border border = new Border
                    {
                        Background = Brushes.LightGray,
                        BorderBrush = Brushes.Gray,
                        BorderThickness = new Thickness(1)
                    };

                    TextBlock textBlock = new TextBlock
                    {
                        Text = matriu[i, j].ToString(),
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };

                    border.Child = textBlock;

                    Grid.SetRow(border, i);
                    Grid.SetColumn(border, j);

                    matrixGrid.Children.Add(border);
                }
            }
        }

        /// <summary>
        /// Canvia el color dels nombres que son primers
        /// </summary>
        /// <param name="numero"></param>
        /// <param name="color"></param>
        private void CanviaColorDeNumero(int numero, SolidColorBrush color)
        {
            foreach (var border in matrixGrid.Children.OfType<Border>())
            {
                TextBlock textBlock = (TextBlock)border.Child;
                if (int.TryParse(textBlock.Text, out int currentNumber) && currentNumber == numero)
                {
                    border.Background = color;
                }
            }
        }

        /// <summary>
        /// Busca els nombres primers amb l'algorisme de la criba d'eratostenes
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BuscaPrimers(object sender, RoutedEventArgs e)
        {
            Thread a = new Thread(() => Eratostenes());
            a.Start();
            a.Join();
        }

        /// <summary>
        /// Algorisme de la criba d'eratostenes
        /// </summary>
        private async void Eratostenes()
        {
            int limit = mida * mida;
            nombresPrimers = new List<int>();
            bool[] isPrime = new bool[limit + 1];
            matrixGrid.Dispatcher.InvokeAsync(new Action(() => CanviaColorDeNumero(2, ColorPrimers)));

            for (int i = 2; i <= limit; i++)
            {
                if (!isPrime[i])
                {
                    nombresPrimers.Add(i);

                    for (int j = i * i; j <= limit; j += i)
                    {
                        isPrime[j] = true;
                        await Task.Delay(delay);
                        await matrixGrid.Dispatcher.InvokeAsync(new Action(() => CanviaColorDeNumero(j, ColorsEratostenes.Brushes[i])));
                    }
                    await Task.Delay(delay);
                    await matrixGrid.Dispatcher.InvokeAsync((Action)delegate {
                        TextBlock textBlock = new TextBlock
                        {
                            Text = i.ToString(),
                            Margin = new Thickness(0, 0, 0, 5)
                        };
                        stckPrimers.Children.Add(textBlock);

                        CanviaColorDeNumero(i, ColorPrimers);
                    });
                }
            }
        }
    }

    /// <summary>
    /// Classe per a triar els colors
    /// </summary>
    public static class ColorsEratostenes
    {
        public static List<SolidColorBrush> Brushes = new List<SolidColorBrush>()
            {
            new SolidColorBrush(Colors.Red),
            new SolidColorBrush(Colors.Green),
            new SolidColorBrush(Colors.Blue),
            new SolidColorBrush(Colors.Orange),
            new SolidColorBrush(Colors.Yellow),
            new SolidColorBrush(Colors.Cyan),
            new SolidColorBrush(Colors.Magenta),
            new SolidColorBrush(Colors.Brown),
            new SolidColorBrush(Colors.Teal),
            new SolidColorBrush(Colors.Pink),
            new SolidColorBrush(Colors.Gray),
            new SolidColorBrush(Colors.DarkGreen),
            new SolidColorBrush(Colors.DarkBlue),
            new SolidColorBrush(Colors.DarkOrange)
        };
    }
}

