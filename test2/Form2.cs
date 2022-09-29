using System;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Threading;
using Application = System.Windows.Forms.Application;

namespace test2
{
    public partial class Form2 : Form
    {
        static SerialPort serial_portform2; //define to get the port data from serial.port 1 in form1
        private string ReceivedData; //to receive data
        private string ReceivedDataString; //receive data in string format
        private Encoding serialPortEncoding;
        public Form2(SerialPort serial_port1)
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
            textBox2.MaxLength = 100;
            serial_portform2 = serial_port1;
        }

        //Serial Encoding ASCII
        private void SerialEncoding()
        {
            serialPortEncoding = Encoding.GetEncoding("us-ASCII");
            serial_portform2.Encoding = serialPortEncoding;
        }
        

        //Button : Add Network Type and Save Configuration to Agromon
        private void button3_Click(object sender, EventArgs e)
        {
            //Define timestamp for command log.
            if (serial_portform2.IsOpen)
            {
                //******************************START OF WIFI SETTING******************************************
                //IF Wi-Fi radiobutton is checked, 
                if (radioButton1.Checked == true && textBox1.Text.Length != 0 && textBox2.Text.Length != 0)
                {
                    DateTime dateTime = DateTime.Now;
                    String timestamp = dateTime.ToString();
                    //Received Data
                    //Alert Message
                    string message = "Are you sure to add the network?";
                    string title = "Confirm";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    //Button Result : IF Yes then start network configuration setup
                    DialogResult result = MessageBox.Show(message, title, buttons);
                    if (result == DialogResult.Yes)
                    {
                        serial_portform2.WriteLine("WIFISET" + "\r\n"); //Write WIFISET to Agromon to initialise WIFI setup.
                        richTextBox1.Text += "<TX>" + " " + timestamp + " " + "WIFISET" + " " + "<CR><LF>" + Environment.NewLine;
                        wait(2000);
                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + ReceivedData + " " + "<CR><LF>" + Environment.NewLine;
                        
                    }
                }
                //******************************END OF WIFI SETTING******************************************
            }
        }

        //Clear Button: To clear all the selection.
        private void button1_Click_1(object sender, EventArgs e)
        {
            //Deselect all radiobutton
            if (radioButton1.Checked)
            {
                radioButton1.Checked = false;
            }
            if (radioButton2.Checked)
            {
                radioButton2.Checked = false;
            }
            if (radioButton3.Checked)
            {
                radioButton3.Checked = false;
            }
            if (radioButton4.Checked)
            {
                radioButton4.Checked = false;
            }

            //Clear SSID Textbox
            textBox1.Clear();
            //Clear Wi-Fi password textbox
            textBox2.Clear();
            //Clear RC Selection
            comboBox2.Text = "Select RC";
            //Clear SIM Selection
            comboBox3.Text = "Select SIM";

            //Default
            groupBox13.Enabled = false; //Enable Groupbox 13
            groupBox14.Enabled = false; //Disable Groupbox 14
            groupBox15.Enabled = false; //Disable Groupbox 15
            groupBox16.Enabled = false; //Disable Groupbox 16
            tabControl2.SelectedIndex = 0; //Select Wi-Fi Tab = 0
        }

        //Wi-Fi Radiobutton : Selected Condition
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            groupBox13.Enabled = true; //Enable Groupbox 13
            groupBox14.Enabled = false; //Disable Groupbox 14
            groupBox15.Enabled = false; //Disable Groupbox 15
            groupBox16.Enabled = false; //Disable Groupbox 16
            tabControl2.SelectedIndex = 0; //Select Wi-Fi Tab = 0
        }

        //Sigfox Radiobutton : Selected Condition
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            groupBox13.Enabled = false; //Disable Groupbox 13
            groupBox14.Enabled = true; //Enable Groupbox 14
            groupBox15.Enabled = false; //Disable Groupbox 15
            groupBox16.Enabled = false; //Disable Groupbox 16
            tabControl2.SelectedIndex = 1; //Select Sigfox Tab = 1
        }

        //LoraWAN Radiobutton : Selected Condition
        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            groupBox13.Enabled = false; //Enable Groupbox 13
            groupBox14.Enabled = false; //Enable Groupbox 14
            groupBox15.Enabled = true; //Enable Groupbox 15
            groupBox16.Enabled = false; //Enable Groupbox 16
            tabControl2.SelectedIndex = 2; //Select LoraWAN Tab = 2
        }

        // 4G/LTE Radiobutton : Selected Condition
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            groupBox13.Enabled = false; //Enable Groupbox 13
            groupBox14.Enabled = false; //Enable Groupbox 14
            groupBox15.Enabled = false; //Enable Groupbox 15
            groupBox16.Enabled = true; //Enable Groupbox 16
            tabControl2.SelectedIndex = 3; //Select LoraWAN Tab = 3
        }

        //Clear button: To clear the selection of sensor 1 in combobox1
        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.Text = String.Empty;
            comboBox1.Text = "Select Sensor Type";
        }

        //Clear button: To clear the selection of sensor 2 in combobox4
        private void button5_Click(object sender, EventArgs e)
        {
            comboBox4.Text = String.Empty;
            comboBox4.Text = "Select Sensor Type";
        }

        //Clear button: To clear the selection of sensor 3 in combobox6
        private void button7_Click(object sender, EventArgs e)
        {
            comboBox6.Text = String.Empty;
            comboBox6.Text = "Select Sensor Type";
        }

        //Form Load : Initialise Elements
        private void Form2_Load(object sender, EventArgs e)
        {
            SerialEncoding(); //Serial Encoding Function
            if (serial_portform2.IsOpen)
            {
                //Define Encoding for Serial Port
                //serial_portform2.Encoding = Encoding.ASCII;
                //IF AVAILABLE: Display Data IF NOT AVAILABLE: Display "Not Available"
                //*****Wi-FI Information*****
                //Label 17 Display SSID
                label17.Text = "Not Available";
                //*****Sigfox Information*****
                //Label 38 Display Radio Configuration (RC)
                label38.Text = "Not Available";
                //*****LoraWAN Information*****
                // Label 50 Display Uplink 1 Frequency
                label50.Text = "Not Available";
                // Label 51 Display Uplink 2 Frequency
                label51.Text = "Not Available";
                // Label 52 Display Uplink 3 Frequency
                label52.Text = "Not Available";
                // Label 53 Display Uplink 4 Frequency
                label53.Text = "Not Available";
                // Label 54 Display Uplink 5 Frequency
                label54.Text = "Not Available";
                // Label 55 Display Uplink 6 Frequency
                label55.Text = "Not Available";
                // Label 56 Display Uplink 7 Frequency
                label56.Text = "Not Available";
                // Label 57 Display Uplink 8 Frequency
                label57.Text = "Not Available";
                // Label 58 Display Uplink 9 Frequency
                label58.Text = "Not Available";
                // Label 59 Display Uplink 10 Frequency
                label59.Text = "Not Available";
                // Label 60 Display Downlink Frequency
                label60.Text = "Not Available";
                //*****4G/LTE Information*****
                // Label 36 Display SIM Type
                label36.Text = "Not Avaialble";

                //LoraWAN Frequency Setup Default Settings (Checkbox)
                //Uplink Frequency Setup (Checkbox 1-10)
                checkBox1.Checked = true;
                checkBox2.Checked = true;
                checkBox3.Checked = true;
                checkBox4.Checked = true;
                checkBox5.Checked = true;
                checkBox6.Checked = true;
                checkBox7.Checked = true;
                checkBox8.Checked = true;
                checkBox9.Checked = true;
                checkBox10.Checked = true;
                //Downlink Frequency Setup (Checkbox 11)
                checkBox11.Checked = true;


                //CHECK if any radiobutton is selected.
                if (!radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked && !radioButton4.Checked)
                {
                    groupBox13.Enabled = false; //Enable Groupbox 13
                    groupBox14.Enabled = false; //Enable Groupbox 14
                    groupBox15.Enabled = false; //Enable Groupbox 15
                    groupBox16.Enabled = false; //Enable Groupbox 16
                    tabControl2.SelectedIndex = 0; //Select Default Tab
                }
            }



        }

        //Test Button: To test the network configuration of Agromon
        private void button20_Click(object sender, EventArgs e)
        {
            if (serial_portform2.IsOpen)
            {
                //This button function has not implemented yet.
            }
        }

        //Read Button : To read the network configuration of Agromon
        private void button13_Click(object sender, EventArgs e)
        {
            if (serial_portform2.IsOpen)
            {
                try
                {
                    //This button function has not implemented yet.
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }

            }
        }

        //This is a "wait" function to solve code run faster than the data transmission response from the Agromon
        private void wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;

            // Console.WriteLine("start wait timer");
            timer1.Interval = milliseconds;
            timer1.Enabled = true;
            timer1.Start();

            timer1.Tick += (s, e) =>
            {
                timer1.Enabled = false;
                timer1.Stop();
                // Console.WriteLine("stop wait timer");
            };

            while (timer1.Enabled)
            {
                Application.DoEvents();
            }
        }

        //Add Sensor Button : Sensor 1
        private void button4_Click(object sender, EventArgs e)
        {
            //This button function has not implemented yet.
        }

        //Add Sensor Button : Sensor 2
        private void button6_Click(object sender, EventArgs e)
        {
            //This button function has not implemented yet.
        }

        //Add Sensor Button : Sensor 3
        private void button8_Click(object sender, EventArgs e)
        {
            //This button function has not implemented yet.
        }

        //Read Sensor Button : Sensor 3
        private void button16_Click(object sender, EventArgs e)
        {
            //This button function has not implemented yet.
        }

        //Reset Sensor Button : Sensor 3
        private void button9_Click(object sender, EventArgs e)
        {
            //This button function has not implemented yet.
        }

        //Read Sensor Button : Sensor 2
        private void button15_Click(object sender, EventArgs e)
        {
            //This button function has not implemented yet.
        }

        //Reset Sensor Button : Sensor 2
        private void button10_Click(object sender, EventArgs e)
        {
            //This button function has not implemented yet.
        }

        //Read Sensor Button : Sensor 1
        private void button14_Click(object sender, EventArgs e)
        {
            //This button function has not implemented yet.
        }

        //Read Sensor Button : Sensor 1
        private void button11_Click(object sender, EventArgs e)
        {
            //This button function has not implemented yet.
        }

        //Form Close: Disconnect COM Port
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serial_portform2.IsOpen)
            {
                serial_portform2.DiscardOutBuffer();
                serial_portform2.DiscardInBuffer();
            }
        }

        //Rich Text Box 1 Auto Scroll when Data Received
        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            // scroll it automatically
            richTextBox1.ScrollToCaret();
        }

        //Rich Text Box 2 Auto Scroll when Data Received
        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            richTextBox2.SelectionStart = richTextBox2.Text.Length;
            // scroll it automatically
            richTextBox2.ScrollToCaret();
        }

        //Rich Text Box 3 Auto Scroll when Data Received
        private void richTextBox3_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            richTextBox3.SelectionStart = richTextBox3.Text.Length;
            // scroll it automatically
            richTextBox3.ScrollToCaret();
        }

        //Rich Text Box 4 Auto Scroll when Data Received
        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            richTextBox4.SelectionStart = richTextBox4.Text.Length;
            // scroll it automatically
            richTextBox4.ScrollToCaret();
        }

        
    }

}
