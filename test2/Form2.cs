using System;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;
using Application = System.Windows.Forms.Application;

namespace test2
{
    public partial class Form2 : Form
    {
        private string ReceivedData; //to receive data
        private string ReceivedDataString; //receive data in string format
        private Encoding serialPortEncoding;
        public Form2()
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
            textBox2.MaxLength = 100;
        }


        //Serial Encoding ASCII
        private void SerialEncoding()
        {
            serialPortEncoding = Encoding.GetEncoding("us-ASCII");
            serialPort2.Encoding = serialPortEncoding;
        }


        //Button : Add Network Type and Save Configuration to Agromon
        private void button3_Click(object sender, EventArgs e)
        {
            //Define timestamp for command log.
            if (serialPort2.IsOpen)
            {
                //******************************START OF WIFI SETTING******************************************
                //IF Wi-Fi radiobutton is checked, 
                int wifisetting_state;
                if (radioButton1.Checked == true && textBox1.Text.Length != 0 && textBox2.Text.Length != 0)
                {
                    DateTime dateTime = DateTime.Now;
                    String timestamp = dateTime.ToString();
                    //Received Data
                    //Alert Message
                    string message = "Are you sure to add the network?";
                    string title = "Confirm";
                    wifisetting_state = 0;
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    //Button Result : IF Yes then start network configuration setup
                    DialogResult result = MessageBox.Show(message, title, buttons);
                    if (result == DialogResult.Yes)
                    {
                        wifisetting_state += 1;
                        if (wifisetting_state == 1)
                        {
                            serialPort2.Write("WIFISET" + "\r\n"); //Write WIFISET to Agromon to initialise WIFI setup.
                            richTextBox1.Text += "<TX>" + " " + timestamp + " " + "WIFISET" + " " + "<CR><LF>" + Environment.NewLine;
                            wait(3000);
                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + ReceivedData + " " + "<CR><LF>" + Environment.NewLine;
                            if (ReceivedData == "OK")
                            {
                                serialPort2.Write("SSID" + "\r\n"); //Write SSID to Agromon to initialise SSID setup.
                                richTextBox1.Text += "<TX>" + " " + timestamp + " " + "SSID" + " " + "<CR><LF>" + Environment.NewLine;
                                wait(3000);
                                richTextBox1.Text += "<RX>" + " " + timestamp + " " + ReceivedData + " " + "<CR><LF>" + Environment.NewLine;
                                if (ReceivedData == "OK")
                                {
                                    wifisetting_state += 1;
                                    if (wifisetting_state == 2)
                                    {
                                        serialPort2.Write(textBox1.Text + "\r\n"); //Write SSID name to Agromon to initialise setup.
                                        richTextBox1.Text += "<TX>" + " " + timestamp + " " + textBox1.Text + " " + "<CR><LF>" + Environment.NewLine;
                                        wait(3000);
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + ReceivedData + " " + "<CR><LF>" + Environment.NewLine;
                                        if (String.Equals("OK", ReceivedData))
                                        {
                                            wifisetting_state += 1;
                                            if (wifisetting_state == 3)
                                            {
                                                serialPort2.Write("PASSWORD" + "\r\n"); //Write PASSWORD  to Agromon to initialise PASSWORD setup.
                                                richTextBox1.Text += "<TX>" + " " + timestamp + " " + "PASSWORD" + " " + "<CR><LF>" + Environment.NewLine;
                                                wait(3000);
                                                richTextBox1.Text += "<RX>" + " " + timestamp + " " + ReceivedData + " " + "<CR><LF>" + Environment.NewLine;
                                                if (String.Equals("OK", ReceivedData))
                                                {
                                                    wifisetting_state += 1;
                                                    if (wifisetting_state == 4)
                                                    {
                                                        serialPort2.Write(textBox2.Text + "\r\n"); //Write password  to Agromon to initialise setup.
                                                        richTextBox1.Text += "<TX>" + " " + timestamp + " " + textBox2.Text + " " + "<CR><LF>" + Environment.NewLine;
                                                        wait(3000);
                                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + ReceivedData + " " + "<CR><LF>" + Environment.NewLine;
                                                        if (String.Equals("OK", ReceivedData))
                                                        {
                                                            wifisetting_state = 0; //Wi-Fi Setup Done
                                                        }
                                                        else
                                                        {
                                                            wifisetting_state = 0;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        wifisetting_state = 0;
                                                    }
                                                }
                                                else
                                                {
                                                    wifisetting_state = 0;
                                                }
                                            }
                                            else
                                            {
                                                wifisetting_state = 0;
                                            }

                                        }
                                        else
                                        {
                                            wifisetting_state = 0;
                                        }
                                    }
                                    else
                                    {
                                        wifisetting_state = 0;
                                    }
                                }
                                else
                                {
                                    wifisetting_state = 0;
                                }

                            }
                            else
                            {
                                wifisetting_state = 0;
                            }
                        }
                        else
                        {
                            wifisetting_state = 0;
                        }

                    }
                }
                //******************************END OF WIFI SETTING******************************************

                //******************************START OF SIGFOX SETTING**************************************
                if (radioButton2.Checked == true && comboBox2.Text != "Select RC")
                {
                    DateTime dateTime = DateTime.Now;
                    String timestamp = dateTime.ToString();
                    int sigfox_setup_state = 0;
                    int frequency_selection = comboBox2.SelectedIndex;
                    serialPort2.Write("SIGFOXSET" + "\r\n"); //Write SIGFOXSET to initialise Sigfox Setting
                    richTextBox1.Text += "<TX>" + " " + timestamp + " " + "SIGFOXSET" + " " + "<CR><LF>" + Environment.NewLine;
                    wait(3000);
                    if (String.Equals("OK", ReceivedData))
                    {
                        sigfox_setup_state += 1;
                        if (sigfox_setup_state == 1)
                        {
                            switch (frequency_selection)
                            {
                                case 0:
                                    serialPort2.Write("RC1" + "\r\n"); //Write RC1  to Agromon to initialise setup RC1 frequency setup.
                                    richTextBox1.Text += "<TX>" + " " + timestamp + " " + "RC1" + " " + "<CR><LF>" + Environment.NewLine;
                                    wait(3000);
                                    if (String.Equals("OK", ReceivedData))
                                    {
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + "OK" + " " + "<CR><LF>" + Environment.NewLine;
                                    }
                                    else
                                    {
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + "ERROR" + " " + "<CR><LF>" + Environment.NewLine;
                                    }
                                    break;
                                case 1:
                                    serialPort2.Write("RC2" + "\r\n"); //Write RC2  to Agromon to initialise setup RC2 frequency setup.
                                    richTextBox1.Text += "<TX>" + " " + timestamp + " " + "RC2" + " " + "<CR><LF>" + Environment.NewLine;
                                    wait(3000);
                                    if (String.Equals("OK", ReceivedData))
                                    {
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + "OK" + " " + "<CR><LF>" + Environment.NewLine;
                                    }
                                    else
                                    {
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + "ERROR" + " " + "<CR><LF>" + Environment.NewLine;
                                    }
                                    break;
                                case 2:
                                    serialPort2.Write("RC3" + "\r\n"); //Write RC3  to Agromon to initialise setup RC3 frequency setup.
                                    richTextBox1.Text += "<TX>" + " " + timestamp + " " + "RC3" + " " + "<CR><LF>" + Environment.NewLine;
                                    wait(3000);
                                    if (String.Equals("OK", ReceivedData))
                                    {
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + "OK" + " " + "<CR><LF>" + Environment.NewLine;
                                    }
                                    else
                                    {
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + "ERROR" + " " + "<CR><LF>" + Environment.NewLine;
                                    }
                                    break;
                                case 3:
                                    serialPort2.Write("RC4" + "\r\n"); //Write RC4  to Agromon to initialise setup RC4 frequency setup.
                                    richTextBox1.Text += "<TX>" + " " + timestamp + " " + "RC4" + " " + "<CR><LF>" + Environment.NewLine;
                                    wait(3000);
                                    if (String.Equals("OK", ReceivedData))
                                    {
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + "OK" + " " + "<CR><LF>" + Environment.NewLine;
                                    }
                                    else
                                    {
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + "ERROR" + " " + "<CR><LF>" + Environment.NewLine;
                                    }
                                    break;
                                case 4:
                                    serialPort2.Write("RC5" + "\r\n"); //Write RC5  to Agromon to initialise setup RC5 frequency setup.
                                    richTextBox1.Text += "<TX>" + " " + timestamp + " " + "RC5" + " " + "<CR><LF>" + Environment.NewLine;
                                    wait(3000);
                                    if (String.Equals("OK", ReceivedData))
                                    {
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + "OK" + " " + "<CR><LF>" + Environment.NewLine;
                                    }
                                    else
                                    {
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + "ERROR" + " " + "<CR><LF>" + Environment.NewLine;
                                    }
                                    break;
                                case 5:
                                    serialPort2.Write("RC6" + "\r\n"); //Write RC6  to Agromon to initialise setup RC6 frequency setup.
                                    richTextBox1.Text += "<TX>" + " " + timestamp + " " + "RC6" + " " + "<CR><LF>" + Environment.NewLine;
                                    wait(3000);
                                    if (String.Equals("OK", ReceivedData))
                                    {
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + "OK" + " " + "<CR><LF>" + Environment.NewLine;
                                    }
                                    else
                                    {
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + "ERROR" + " " + "<CR><LF>" + Environment.NewLine;
                                    }
                                    break;
                                case 6:
                                    serialPort2.Write("RC7" + "\r\n"); //Write RC7  to Agromon to initialise setup RC7 frequency setup.
                                    richTextBox1.Text += "<TX>" + " " + timestamp + " " + "RC7" + " " + "<CR><LF>" + Environment.NewLine;
                                    wait(3000);
                                    if (String.Equals("OK", ReceivedData))
                                    {
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + "OK" + " " + "<CR><LF>" + Environment.NewLine;
                                    }
                                    else
                                    {
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + "ERROR" + " " + "<CR><LF>" + Environment.NewLine;
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }

                }
                //******************************END OF SIGFOX SETTING**************************************

                //******************************START OF LoraWAN SETTING**************************************
                if (radioButton3.Checked == true)
                {

                }
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
            if (serialPort2.IsOpen)
            {
                //Define Encoding for Serial Port
                //serialPort2.Encoding = Encoding.ASCII;
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
            if (serialPort2.IsOpen)
            {
                //This button function has not implemented yet.
            }
        }

        //Read Button : To read the network configuration of Agromon
        private void button13_Click(object sender, EventArgs e)
        {
            if (serialPort2.IsOpen)
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
            if (serialPort2.IsOpen)
            {
                serialPort2.DiscardOutBuffer();
                serialPort2.DiscardInBuffer();
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
