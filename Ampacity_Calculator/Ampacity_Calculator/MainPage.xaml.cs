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
            double number;

            // Condition turns true if a invalid input is entered
            bool error_flag = false;

            // Brush colors for borders
            SolidColorBrush RedBrush = new SolidColorBrush(Windows.UI.Colors.Red);
            SolidColorBrush noColor = null;

            // If input is valid, assign no border, else assign red border, loops through the user inputs
            for (int i = 0; i <= 3; i++)
            {
                if (Double.TryParse(user_inputs[i].Text, out number))
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
            // If code finds errors with your inputs, it will output an error message
            if (error_flag == true)
            {
                ResultsVD.Text = "";
                ResultsVDper.Text = "";
                chosenSize.Text = "";
                powerloss.Text = "";
                error_message.Text = "*There are errors with your inputs.";
                return;
            }
            else
            {
                error_message.Text = "";
            }

            // Get the length of the wire and convert to double
            double length=Convert.ToDouble( user_inputs[0].Text);
            
            // Get the current and convert to double
            double I = Convert.ToDouble(user_inputs[2].Text);
            
            // Get the system voltage and convert to double
            double system_voltage = Convert.ToDouble(user_inputs[1].Text);

            double desired_VD = Convert.ToDouble(user_inputs[3].Text);

            // 30 degrees C ambient, Single insulated conductors, 0-2000 V, 72 degrees C conductor rating
            string[] wire_size_key = {"14 AWG", "12 AWG", "10 AWG", "8 AWG", "6 AWG", "4 AWG", "3 AWG" , "2 AWG", "1 AWG", "1/0 AWG","2/0 AWG","3/0 AWG","4/0 AWG", "250 kcmil", "300 kcmil", "350 kcmil", "400 kcmil" , "500 kcmil" ,"600 kcmil", "750 kcmil"} ;
            double[] Cu_ampacity_key = {30,35,50,70,95,125,145,170,195,230,265,310,360,405,445,505,545,620,690,785};
            double[] Al_ampacity_key= {0,30,40,55,75,100,115,135,155,180,210,240,280,315,350,395,425,485,540,620}; 

            // 600 V cables, 3 phase, 60 Hz, 75 degrees C
            double[] Cu_res_key = {3.1,2.0,1.2,0.78,0.49,0.31,0.25,0.19,0.15,0.12,0.10,0.077,0.062,0.052,0.044,0.038,0.033,0.027,0.023,0.019};
            double[] Al_res_key = {0,3,2,2.0,1.3,0.81,0.51,0.40,0.32,0.25,0.20,0.16,0.13,0.10,0.085,0.071,0.061,0.054,0.043,0.036,0.029};

            // Condition for when the correct wire size is found matching desired voltage drop
            bool correct_wire_size=false;

            // Doubles to store the voltage drop and percent voltage drop
            double VD=0;
            double percent_VD=0;
            double power=0;

            // Arrays to set desired ampacity/res key to either Al or Cu
            double[] ampacity_key;
            double[] res_key;

            // If index is 0, Cu is choosen, else Al is choosen
            if (material_index == 0)
            {
                ampacity_key = Cu_ampacity_key;
                res_key = Cu_res_key;
            }
            else
            {
                ampacity_key = Al_ampacity_key;
                res_key = Al_res_key;
            }

            // Loop variable
            int j=0;

            for (j=0; correct_wire_size == false && j<=19; j++)
            {
                if (ampacity_key[j] > I)
                {
                    // Calculate the voltage drop and percent VD of the system 
                    VD = (2 * length * res_key[j] * I) / 1000;
                    percent_VD = (VD / system_voltage) * 100;

                    if (percent_VD <= desired_VD)
                    {
                        correct_wire_size = true;
                        power=I*I*(res_key[j]/1000)*2*length;
                        break;
                    }
                }  
            }
            if (correct_wire_size == true)
            {
                // Return the results to the text boxes for the user to see
                ResultsVD.Text = "Voltage Drop= " + Math.Round( VD,3) + " V";
                ResultsVDper.Text = "Percent VD= " + Math.Round(percent_VD,3) + "%";
                chosenSize.Text = "Wire Size: " + wire_size_key[j];
                powerloss.Text = "Power Loss: " + power/1000 + " kW";
            }

            else{
                // Output error message, clear the values for the results 
                error_message.Text = "*Your voltage drop could not be met or too much current exists.";
                ResultsVD.Text = "";
                ResultsVDper.Text = "";
                chosenSize.Text = "";
                powerloss.Text = "";
            }
        }
    }
}
