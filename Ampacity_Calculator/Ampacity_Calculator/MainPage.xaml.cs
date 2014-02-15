using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace Ampacity_Calculator
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : Ampacity_Calculator.Common.LayoutAwarePage
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Get the values the user inputs into the text boxes array
            TextBox[] user_inputs;
            user_inputs = new TextBox[4];

            user_inputs[0] = wire_length;
            user_inputs[1] = voltage;
            user_inputs[2] = current;
            user_inputs[3] = volt_drop_percent;

            // Gets the material index, 0 for Cu, 1 for Al
            int material_index = material.SelectedIndex;

            // Gets the phase index, 0 for single, 1 for three
            int phase_index = phases.SelectedIndex;

            // Variable if number is valid input
            int number;

            //
            bool error_flag = false;

            // Brush colors for borders
            SolidColorBrush RedBrush = new SolidColorBrush(Windows.UI.Colors.Red);
            SolidColorBrush noColor = null;

            // If input is valid, assign no border, else assign red border, loops through the user inputs
            for (int i = 0; i <= 3; i++)
            {
                if (Int32.TryParse(user_inputs[i].Text, out number))
                {
                    user_inputs[i].BorderBrush = noColor;
                    

                }
                else
                {
                    user_inputs[i].BorderBrush = RedBrush;
                    if (error_flag == false)
                    {
                        error_flag = true;
                    }
                }

            }
            if (error_flag == true)
            {
                error_message.Text = "*There are errors with your inputs.";
            }
            else
            {
                error_message.Text = "";
            }
        }
    }
}
