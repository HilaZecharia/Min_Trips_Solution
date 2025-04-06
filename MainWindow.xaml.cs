using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Asio_Home_Assignment
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ITripCalculation tripCalc;
        private const string BagNamePrefix = "txtBoxweightOfBag_";

        public MainWindow()
        {
            InitializeComponent();
            tripCalc = new TripCalculation();
        }
        private async void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int numBags = int.Parse(txtNumBags.Text);
                List<Bag> bagWeights = new List<Bag>();
                var bagsWeightList = bagsPanel.Children.OfType<StackPanel>() 
                .SelectMany(stackPanel => stackPanel.Children.OfType<TextBox>())
                .Where(txtBox => txtBox.Name.Contains(BagNamePrefix)).ToList();

                foreach (var txtBox in bagsWeightList)
                {
                    try
                    {
                        bagWeights.Add(new Bag() { Weight = double.Parse(txtBox.Text) });
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred: {ex.Message}");
                    }
                }

                var result = await Task.Run(() => tripCalc.CalculateTrips(bagWeights));
                ShowResult(result); //display the result in new window
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private async void btnEnterBagsWeightAsync_Click(object sender, RoutedEventArgs e)
        {
            int numBags;
            try
            {
                if (int.TryParse(txtNumBags.Text, out numBags))
                {
                    bagsPanel.Children.Clear();

                    List<UIElement> newControls = new List<UIElement>();
                    await Task.Run(() =>
                    {
                        // This runs on a background thread
                        List<string> bagWeights = new List<string>();

                        for (int i = 0; i < numBags; i++)
                            bagWeights.Add($"Please enter bag {i + 1} weight:");

                        //marshal the creation of UI elements back to the UI thread
                        Dispatcher.Invoke(() =>
                        {
                            Button btnCalculate = new Button() { Content = "Calculate trip", Width = 130, VerticalAlignment = VerticalAlignment.Top, Margin = new Thickness(0, 20, 0, 0) };
                            btnCalculate.Click += btnCalculate_Click;
                            // Dynamically create textboxes for entering bag weights
                            for (int i = 0; i < bagWeights.Count; i++)
                            {
                                StackPanel stackPanel = new StackPanel()
                                {
                                    Orientation = Orientation.Horizontal,
                                    VerticalAlignment = VerticalAlignment.Center,
                                };

                                Label lblWeight = new Label()
                                {
                                    Content = bagWeights[i],
                                    VerticalAlignment = VerticalAlignment.Center,
                                    HorizontalAlignment = HorizontalAlignment.Left,
                                };
                                TextBox weightTextBox = new TextBox() { Width = 100, VerticalAlignment = VerticalAlignment.Center, Name = $"{BagNamePrefix + i.ToString()}", HorizontalAlignment = HorizontalAlignment.Left };
                                stackPanel.Children.Add(lblWeight);
                                stackPanel.Children.Add(weightTextBox);
                                bagsPanel.Children.Add(stackPanel);
                            }
                            bagsPanel.Children.Add(btnCalculate);
                        });
                    });
                }
                else
                {
                    MessageBox.Show("Please enter integer number");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }
        private void ShowResult(Tuple<int, List<Trip>> result)
        {
            int numTrips = result.Item1;
            List<Trip> trips = result.Item2;
            string resultText = $"Minimum number of trips: {numTrips}\n";

            for (int i = 0; i < trips.Count; i++)
            {
                resultText += $"Trip number {i + 1} includes Bags with weights : {string.Join(", ", trips[i].Bags.Select(b=>b.Weight))}\n";
            }
            ShowResultWin resultWin = new ShowResultWin(resultText);
            resultWin.ShowDialog(); // Show the popup window as a modal dialog
        }
    }
}