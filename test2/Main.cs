using System;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;
using Application = System.Windows.Forms.Application;
using WMPLib;
using System.Windows;
using static System.Net.Mime.MediaTypeNames;

namespace test2
{
    public partial class Main : Form
    {
        public SerialPort serial_port2;
        public string DataReceived;
        public string DataReceivedString, sensor_id;
        private Encoding serialPortEncoding;
        public Main()
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
            DateTime dateTime = DateTime.Now;
            String timestamp = dateTime.ToString("hh:mm:ss");
            //Define timestamp for command log.
            if (serialPort2.IsOpen)
            {
                serialPort2.DiscardOutBuffer();
                serialPort2.DiscardInBuffer();
                //******************************START OF WIFI SETTING******************************************
                //IF Wi-Fi radiobutton is checked, 
                int wifisetting_state;
                string a, b, c, d, f;
                if (radioButton1.Checked == true && textBox1.Text.Length != 0 && textBox2.Text.Length != 0)
                {
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
                        serialPort2.DiscardOutBuffer();
                        serialPort2.DiscardInBuffer();
                        wifisetting_state += 1;
                        if (wifisetting_state == 1)
                        {
                            serialPort2.Write("WIFISET" + "\r\n"); //Write WIFISET to Agromon to initialise WIFI setup.
                            richTextBox1.Text += "<TX>" + " " + timestamp + " " + "WIFISET" + " " + "<CR><LF>" + Environment.NewLine;
                            wait(2000);
                            a = serialPort2.ReadLine();
                            string[] DataReceivedStrings1 = a.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + DataReceivedStrings1[0] + " " + "<CR><LF>" + Environment.NewLine;
                            wait(2000);
                            if (DataReceivedStrings1[0] == "OK")
                            {
                                serialPort2.Write("SSID" + "\r\n"); //Write SSID to Agromon to initialise SSID setup.
                                richTextBox1.Text += "<TX>" + " " + timestamp + " " + "SSID" + " " + "<CR><LF>" + Environment.NewLine;
                                wait(1000);
                                b = serialPort2.ReadLine();
                                string[] DataReceivedStrings2 = b.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                richTextBox1.Text += "<RX>" + " " + timestamp + " " + DataReceivedStrings2[0] + " " + "<CR><LF>" + Environment.NewLine;
                                if (DataReceivedStrings2[0] == "OK")
                                {
                                    wifisetting_state = 2;
                                    if (wifisetting_state == 2)
                                    {
                                        serialPort2.Write(textBox1.Text); //Write SSID name to Agromon
                                        richTextBox1.Text += "<TX>" + " " + timestamp + " " + textBox1.Text + " " + "<CR><LF>" + Environment.NewLine;
                                        wait(1000);
                                        c = serialPort2.ReadLine();
                                        string[] DataReceivedStrings3 = c.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + DataReceivedStrings3[0] + " " + "<CR><LF>" + Environment.NewLine;
                                        if (DataReceivedStrings3[0] == "OK")
                                        {
                                            wifisetting_state += 1;
                                            if (wifisetting_state == 3)
                                            {
                                                wait(3000);
                                                serialPort2.Write("PASSWORD"); //Write PASSWORD  to Agromon to initialise PASSWORD setup.
                                                richTextBox1.Text += "<TX>" + " " + timestamp + " " + "PASSWORD" + " " + "<CR><LF>" + Environment.NewLine;
                                                wait(1000);
                                                d = serialPort2.ReadLine();
                                                string[] DataReceivedStrings4 = d.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                                richTextBox1.Text += "<RX>" + " " + timestamp + " " + DataReceivedStrings4[0] + " " + "<CR><LF>" + Environment.NewLine;
                                                if (DataReceivedStrings4[0] == "OK")
                                                {
                                                    wifisetting_state += 1;
                                                    if (wifisetting_state == 4)
                                                    {
                                                        serialPort2.Write(textBox2.Text); //Write password  to Agromon
                                                        richTextBox1.Text += "<TX>" + " " + timestamp + " " + textBox2.Text + " " + "<CR><LF>" + Environment.NewLine;
                                                        wait(1000);
                                                        f = serialPort2.ReadLine();
                                                        string[] DataReceivedStrings5 = f.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + DataReceivedStrings5[0] + " " + "<CR><LF>" + Environment.NewLine;
                                                        if (DataReceivedStrings5[0] == "OK")
                                                        {
                                                            wifisetting_state = 0; //Wi-Fi Setup Done
                                                            richTextBox1.Text += "<TX>" + " " + timestamp + " WIFISET DONE " + "<CR><LF>" + Environment.NewLine;
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
                    string message = "Are you sure to add the network?";
                    string title = "Confirm";
                    wifisetting_state = 0;
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    //Button Result : IF Yes then start network configuration setup
                    DialogResult result = MessageBox.Show(message, title, buttons);
                    if (result == DialogResult.Yes)
                    {
                        serialPort2.DiscardOutBuffer();
                        serialPort2.DiscardInBuffer();

                        int sigfox_setup_state = 0;
                        int frequency_selection = comboBox2.SelectedIndex;
                        serialPort2.Write("SIGFOXSET"); //Write SIGFOXSET to initialise Sigfox Setting
                        richTextBox1.Text += "<TX>" + " " + timestamp + " " + "SIGFOXSET" + " " + "<CR><LF>" + Environment.NewLine;
                        a = serialPort2.ReadLine();
                        string[] DataReceivedStrings1 = a.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        richTextBox1.Text += "<TX>" + " " + timestamp + " " + DataReceivedStrings1[0] + " " + "<CR><LF>" + Environment.NewLine;
                        wait(1000);
                        if (DataReceivedStrings1[0] == "OK")
                        {
                            sigfox_setup_state += 1;
                            if (sigfox_setup_state == 1)
                            {
                                switch (frequency_selection)
                                {
                                    case 0:
                                        serialPort2.Write("RC1"); //Write RC1  to Agromon to initialise setup RC1 frequency setup.
                                        richTextBox1.Text += "<TX>" + " " + timestamp + " " + "RC1" + " " + "<CR><LF>" + Environment.NewLine;
                                        b = serialPort2.ReadLine();
                                        string[] DataReceivedStrings2 = b.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        wait(3000);
                                        if (DataReceivedStrings2[0] == "OK")
                                        {
                                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + DataReceivedStrings2[0] + " " + "<CR><LF>" + Environment.NewLine;
                                            wait(300);
                                            richTextBox1.Text += "<TX>" + " " + timestamp + " SIGFOXSET DONE " + "<CR><LF>" + Environment.NewLine;
                                        }
                                        else
                                        {
                                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + "ERROR" + " " + "<CR><LF>" + Environment.NewLine;
                                        }
                                        break;
                                    case 1:
                                        serialPort2.Write("RC2"); //Write RC1  to Agromon to initialise setup RC1 frequency setup.
                                        richTextBox1.Text += "<TX>" + " " + timestamp + " " + "RC2" + " " + "<CR><LF>" + Environment.NewLine;
                                        b = serialPort2.ReadLine();
                                        string[] DataReceivedStrings3 = b.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        wait(3000);
                                        if (DataReceivedStrings3[0] == "OK")
                                        {
                                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + DataReceivedStrings3[0] + " " + "<CR><LF>" + Environment.NewLine;
                                            wait(300);
                                            richTextBox1.Text += "<TX>" + " " + timestamp + " SIGFOXSET DONE " + "<CR><LF>" + Environment.NewLine;
                                        }
                                        else
                                        {
                                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + "ERROR" + " " + "<CR><LF>" + Environment.NewLine;
                                        }
                                        break;
                                    case 2:
                                        serialPort2.Write("RC3"); //Write RC1  to Agromon to initialise setup RC1 frequency setup.
                                        richTextBox1.Text += "<TX>" + " " + timestamp + " " + "RC3" + " " + "<CR><LF>" + Environment.NewLine;
                                        b = serialPort2.ReadLine();
                                        string[] DataReceivedStrings4 = b.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        wait(3000);
                                        if (DataReceivedStrings4[0] == "OK")
                                        {
                                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + DataReceivedStrings4[0] + " " + "<CR><LF>" + Environment.NewLine;
                                            wait(300);
                                            richTextBox1.Text += "<TX>" + " " + timestamp + " SIGFOXSET DONE " + "<CR><LF>" + Environment.NewLine;
                                        }
                                        else
                                        {
                                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + "ERROR" + " " + "<CR><LF>" + Environment.NewLine;
                                        }
                                        break;
                                    case 3:
                                        serialPort2.Write("RC4"); //Write RC1  to Agromon to initialise setup RC1 frequency setup.
                                        richTextBox1.Text += "<TX>" + " " + timestamp + " " + "RC4" + " " + "<CR><LF>" + Environment.NewLine;
                                        b = serialPort2.ReadLine();
                                        string[] DataReceivedStrings5 = b.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        wait(3000);
                                        if (DataReceivedStrings5[0] == "OK")
                                        {
                                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + DataReceivedStrings5[0] + " " + "<CR><LF>" + Environment.NewLine;
                                            wait(300);
                                            richTextBox1.Text += "<TX>" + " " + timestamp + " SIGFOXSET DONE " + "<CR><LF>" + Environment.NewLine;
                                        }
                                        else
                                        {
                                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + "ERROR" + " " + "<CR><LF>" + Environment.NewLine;
                                        }
                                        break;
                                    case 4:
                                        serialPort2.Write("RC5"); //Write RC1  to Agromon to initialise setup RC1 frequency setup.
                                        richTextBox1.Text += "<TX>" + " " + timestamp + " " + "RC5" + " " + "<CR><LF>" + Environment.NewLine;
                                        b = serialPort2.ReadLine();
                                        string[] DataReceivedStrings6 = b.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        wait(3000);
                                        if (DataReceivedStrings6[0] == "OK")
                                        {
                                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + DataReceivedStrings6[0] + " " + "<CR><LF>" + Environment.NewLine;
                                            wait(300);
                                            richTextBox1.Text += "<TX>" + " " + timestamp + " SIGFOXSET DONE " + "<CR><LF>" + Environment.NewLine;
                                        }
                                        else
                                        {
                                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + "ERROR" + " " + "<CR><LF>" + Environment.NewLine;
                                        }
                                        break;
                                    case 5:
                                        serialPort2.Write("RC6"); //Write RC1  to Agromon to initialise setup RC1 frequency setup.
                                        richTextBox1.Text += "<TX>" + " " + timestamp + " " + "RC6" + " " + "<CR><LF>" + Environment.NewLine;
                                        b = serialPort2.ReadLine();
                                        string[] DataReceivedStrings7 = b.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        wait(3000);
                                        if (DataReceivedStrings7[0] == "OK")
                                        {
                                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + DataReceivedStrings7[0] + " " + "<CR><LF>" + Environment.NewLine;
                                            wait(300);
                                            richTextBox1.Text += "<TX>" + " " + timestamp + " SIGFOXSET DONE " + "<CR><LF>" + Environment.NewLine;
                                        }
                                        else
                                        {
                                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + "ERROR" + " " + "<CR><LF>" + Environment.NewLine;
                                        }
                                        break;
                                    case 6:
                                        serialPort2.Write("RC7"); //Write RC1  to Agromon to initialise setup RC1 frequency setup.
                                        richTextBox1.Text += "<TX>" + " " + timestamp + " " + "RC7" + " " + "<CR><LF>" + Environment.NewLine;
                                        b = serialPort2.ReadLine();
                                        string[] DataReceivedStrings8 = b.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        wait(3000);
                                        if (DataReceivedStrings8[0] == "OK")
                                        {
                                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + DataReceivedStrings8[0] + " " + "<CR><LF>" + Environment.NewLine;
                                            wait(300);
                                            richTextBox1.Text += "<TX>" + " " + timestamp + " SIGFOXSET DONE " + "<CR><LF>" + Environment.NewLine;
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
                }
                //******************************END OF SIGFOX SETTING**************************************

                //******************************START OF LoRaWAN SETTING**************************************
                if (radioButton3.Checked == true)
                {

                }
                //******************************END OF LoRaWAN SETTING**************************************

                //******************************START OF 4G SETTING**************************************
                if (radioButton4.Checked == true)
                {
                    string message = "Are you sure to add the network?";
                    string title = "Confirm";
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    //Button Result : IF Yes then start network configuration setup
                    DialogResult result = MessageBox.Show(message, title, buttons);
                    if (result == DialogResult.Yes)
                    {
                        serialPort2.DiscardOutBuffer();
                        serialPort2.DiscardInBuffer();
                        int lte_selection = comboBox3.SelectedIndex;
                        serialPort2.Write("LTSET "); //Write 4G to initialise 4G Setting
                        richTextBox1.Text += "<TX>" + " " + timestamp + " " + "LTSET" + " " + "<CR><LF>" + Environment.NewLine;
                        a = serialPort2.ReadLine();
                        string[] DataReceivedStrings1 = a.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        richTextBox1.Text += "<TX>" + " " + timestamp + " " + DataReceivedStrings1[0] + " " + "<CR><LF>" + Environment.NewLine;
                        wait(1000);
                        if (DataReceivedStrings1[0] == "OK")
                        {
                            switch (lte_selection)
                            {
                                case 0:
                                    serialPort2.Write("SIM"); //Write SIM  to Agromon to initialise setup SIM frequency setup.
                                    wait(2000);
                                    serialPort2.Write("SIM"); 
                                    wait(2000);
                                    richTextBox1.Text += "<TX>" + " " + timestamp + " " + "SIM" + " " + "<CR><LF>" + Environment.NewLine;
                                    b = serialPort2.ReadLine();
                                    string[] DataReceivedStrings2 = b.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                    wait(3000);
                                    if (DataReceivedStrings2[0] == "OK")
                                    {
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + DataReceivedStrings2[0] + " " + "<CR><LF>" + Environment.NewLine;
                                        wait(300);
                                        richTextBox1.Text += "<TX>" + " " + timestamp + " 4GSET DONE " + "<CR><LF>" + Environment.NewLine;
                                    }
                                    else
                                    {
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + "ERROR" + " " + "<CR><LF>" + Environment.NewLine;
                                    }
                                    break;

                                case 1:
                                    serialPort2.Write("eSIM"); //Write eSIM  to Agromon to initialise setup eSIM frequency setup.
                                    richTextBox1.Text += "<TX>" + " " + timestamp + " " + "eSIM" + " " + "<CR><LF>" + Environment.NewLine;
                                    b = serialPort2.ReadLine();
                                    string[] DataReceivedStrings3 = b.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                    wait(3000);
                                    if (DataReceivedStrings3[0] == "OK")
                                    {
                                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + DataReceivedStrings3[0] + " " + "<CR><LF>" + Environment.NewLine;
                                        wait(300);
                                        richTextBox1.Text += "<TX>" + " " + timestamp + " 4G SET DONE " + "<CR><LF>" + Environment.NewLine;
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
                //******************************END OF 4G SETTING*****************************************//
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
            groupBox16.Enabled = true; //Enable Groupbox 15
            groupBox15.Enabled = false; //Enable Groupbox 16
            tabControl2.SelectedIndex = 2; //Select LoraWAN Tab = 2
        }

        // 4G/LTE Radiobutton : Selected Condition
        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            groupBox13.Enabled = false; //Enable Groupbox 13
            groupBox14.Enabled = false; //Enable Groupbox 14
            groupBox16.Enabled = false; //Enable Groupbox 15
            groupBox15.Enabled = true; //Enable Groupbox 16
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
        private void LoadConfigurationSettings()
        {
            baudrate.Text = System.Configuration.ConfigurationManager.AppSettings["combaudrate"];
            databits.Text = System.Configuration.ConfigurationManager.AppSettings["comdatabits"];
            stopbits.Text = System.Configuration.ConfigurationManager.AppSettings["comstopbits"];
            parity.Text = System.Configuration.ConfigurationManager.AppSettings["comparity"];
        }


        //Form Load : Initialise Elements
        private void Main_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Maximized;
            string[] ports = SerialPort.GetPortNames();
            com_port.Items.AddRange(ports);
            //not connected enable comboboxes
            com_port.Enabled = true;
            baudrate.Enabled = true;
            databits.Enabled = true;
            stopbits.Enabled = true;
            sensor1_cofig_page.Enabled = true;
            sensor2_cofig_page.Enabled = false;
            sensor3_cofig_page.Enabled = false;
            groupBox13.Enabled = false; //Enable Groupbox 13
            groupBox14.Enabled = false; //Enable Groupbox 14
            groupBox16.Enabled = false; //Enable Groupbox 15
            groupBox15.Enabled = false; //Enable Groupbox 16
            parity.Enabled = true;
            radioButton3.Enabled = false;
            //disable configure button if not connected
            disconnect_button.Enabled = false;
            LoadConfigurationSettings();
            this.textBox1.AutoSize = true;

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
            string a, c;
            DateTime dateTime = DateTime.Now;
            String timestamp = dateTime.ToString("hh:mm:ss");
            if (serialPort2.IsOpen)
            {
                serialPort2.DiscardOutBuffer();
                serialPort2.DiscardInBuffer();
                try
                {
                    serialPort2.Write("OK"); //Write OK to Agromon to read network setting.
                    richTextBox1.Text += "<TX>" + " " + timestamp + " " + "READ" + " " + "<CR><LF>" + Environment.NewLine;
                    wait(2000);
                    a = serialPort2.ReadLine();
                    string[] DataReceivedStrings1 = a.Split(' ');
                    if (DataReceivedStrings1[0] == "NO")
                    {
                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + "NO DATA AVAILABLE" + " " + "<CR><LF>" + Environment.NewLine;
                    }
                    else
                    {
                        label16.Text = DataReceivedStrings1[0];
                        string[] b = DataReceivedStrings1[0].Split('\r');
                        richTextBox1.Text += "<RX>" + " " + timestamp + " " + b[0] + " " + "<CR><LF>" + Environment.NewLine;
                        wait(2000);
                        if(b[0] == "WIFI")
                        {
                            serialPort2.Write("WIFI");
                            c = serialPort2.ReadLine();
                            wait(100);
                            label17.Text = c;
                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + c + " " + "<CR><LF>" + Environment.NewLine;
                        }
                        if (b[0] == "SIGFOX")
                        {
                            serialPort2.Write("SIGFOX");
                            wait(100);
                            c = serialPort2.ReadLine();
                            label38.Text = c;
                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + c + " " + "<CR><LF>" + Environment.NewLine;
                        }
                        if (b[0] == "LORAWAN")
                        {
                            serialPort2.Write("LORAWAN");
                            wait(100);
                            c = serialPort2.ReadLine();
                            label17.Text = c;
                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + c + " " + "<CR><LF>" + Environment.NewLine;
                        }
                        if (b[0] == "4G")
                        {
                            serialPort2.Write("LTE4G");
                            wait(100);
                            c = serialPort2.ReadLine();
                            label36.Text = c;
                            richTextBox1.Text += "<RX>" + " " + timestamp + " " + c + " " + "<CR><LF>" + Environment.NewLine;
                        }
                    }
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
        private void Main_FormClosed(object sender, FormClosedEventArgs e)
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

        private void connect_button_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                String timestamp = dateTime.ToString("hh:mm:ss");
                serial_port2 = new SerialPort(com_port.Text, Convert.ToInt32(baudrate.Text), (Parity)Enum.Parse(typeof(Parity), parity.Text), Convert.ToInt32(databits.Text), (StopBits)Enum.Parse(typeof(StopBits), stopbits.Text));
                serialPort2.PortName = com_port.Text;
                serialPort2.BaudRate = Convert.ToInt32(baudrate.Text);
                serialPort2.DataBits = Convert.ToInt32(databits.Text);
                serialPort2.StopBits = (StopBits)Enum.Parse(typeof(StopBits), stopbits.Text);
                serialPort2.Parity = (Parity)Enum.Parse(typeof(Parity), parity.Text);
                serialPort2.Open();

                if (serialPort2.IsOpen)
                {
                    //string k;
                    com_status.Text = "CONNECTING";
                    com_status.ForeColor = Color.Red;
                    //disable combobox if it is connected
                    com_port.Enabled = false;
                    baudrate.Enabled = false;
                    databits.Enabled = false;
                    stopbits.Enabled = false;
                    parity.Enabled = false;
                    refresh_button.Enabled = false;
                    disconnect_button.Enabled = true;
                    //wait(5000);
                    // When software established connection with Agromon hardware
                    // Send 0xAA 0x55 \r\n to Agromon to enter Configuration mode automatically before the 10s timeout.string a;
                    //    serialPort2.Write("config");
                    //    wait(2000);
                    //    log.Text += "<TX>" + " " + timestamp + " " + "Conne\\" + " " + "<CR><LF>" + Environment.NewLine;
                    //    wait(1000);
                    //    k = serialPort2.ReadLine();
                    //    string[] DataReceivedStrings1 = k.Split(' ');
                }
                com_status.Text = "CONNECTED";
                com_status.ForeColor = Color.Green;
                network_name.Text = "NOT AVAILABLE";
                sensor.Text = "NOT AVAILABLE";
                sensor2.Text = "NOT AVAILABLE";
                sensor3.Text = "NOT AVAILABLE";
                connect_button.Enabled = false;
            }
            catch (Exception)
            {
                MessageBox.Show("Please select the serial PORT or refresh if none serial PORT detected", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                com_status.ForeColor = Color.Red;
                com_status.Text = "NOT CONNECTED";
                //Enable Comboboxes when disconnected to COM Port.
                com_port.Enabled = true;
                baudrate.Enabled = true;
                databits.Enabled = true;
                stopbits.Enabled = true;
                com_port.Enabled = true;
                connect_button.Enabled = true;
                refresh_button.Enabled = true;
                disconnect_button.Enabled = false;

            }
        }
        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {

            try
            {
                DataReceived = serialPort2.ReadLine();
                this.Invoke(new Action(ProcessingData));
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void refresh_button_Click(object sender, EventArgs e)
        {

            DateTime dateTime = DateTime.Now;
            String timestamp = dateTime.ToString("hh:mm:ss");
            if (!serialPort2.IsOpen)
            {

                try
                {
                    com_port.Items.Clear();
                    com_port.Text = "Select COM Port";
                    log.Clear();
                    getAvailablePorts();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Serial Port is Busy Or Open", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Close The Current Port Before Refreshing", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void ProcessingData()
        {
            DataReceivedString = DataReceived.ToString();
        }

        private void disconnect_button_Click(object sender, EventArgs e)
        {

            if (serialPort2.IsOpen)
            {
                DateTime dateTime = DateTime.Now;
                String timestamp = dateTime.ToString("hh:mm:ss");
                serialPort2.DiscardOutBuffer();
                serialPort2.DiscardInBuffer();
                serialPort2.Close();
                serialPort2.DataReceived -= new SerialDataReceivedEventHandler(DataReceivedHandler);
                log.Text += timestamp + " " + "PORT" + com_port.Text + " " + "is disconnected!" + Environment.NewLine;
                com_status.ForeColor = Color.Red;
                com_status.Text = "NOT CONNECTED";
                network_status.ForeColor = Color.Red;
                network_status.Text = "NOT CONNECTED";
                //not connected enable comboboxes
                com_port.Enabled = true;
                baudrate.Enabled = true;
                databits.Enabled = true;
                stopbits.Enabled = true;
                parity.Enabled = true;
                //disable configure button if not connected
                disconnect_button.Enabled = false;
                connect_button.Enabled = true;
                refresh_button.Enabled = true;
                network_name.Text = "Not Available";
            }
            else
            {
                DateTime dateTime = DateTime.Now;
                String timestamp = dateTime.ToString();
                textBox1.Text += timestamp + " " + "No Serial Port is connected!" + Environment.NewLine;

            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            About form3 = new About();
            form3.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            var website = MessageBox.Show("Redirect to Agromon Website", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if(website == DialogResult.Yes)
            { 
                System.Diagnostics.Process.Start("https://www.wondernica.com/agromon.html");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            string a;
            DateTime dateTime = DateTime.Now;
            String timestamp = dateTime.ToString("hh:mm:ss");
            if (serialPort2.IsOpen)
            {
                string message = "Are you sure to reset all the network settings?";
                string title = "Confirm";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                //Button Result : IF Yes then start network configuration setup
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    serialPort2.DiscardOutBuffer();
                    serialPort2.DiscardInBuffer();
                    serialPort2.Write("RESET"); //Write RESET to Agromon to reset network setting.
                    richTextBox1.Text += "<TX>" + " " + timestamp + " " + "RESET ALL" + " " + "<CR><LF>" + Environment.NewLine;
                    wait(2000);
                    a = serialPort2.ReadLine();
                    string[] DataReceivedStrings1 = a.Split(Environment.NewLine.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    richTextBox1.Text += "<RX>" + " " + timestamp + " " + DataReceivedStrings1[0] + " <CR><LF>" + Environment.NewLine;
                    wait(2000);
                    label17.Text = "NOT AVAILABLE";
                    label36.Text = "NOT AVAILABLE";
                    label38.Text = "NOT AVAILABLE";
                }
            }
        }

        void getAvailablePorts()
        {
            DateTime dateTime = DateTime.Now;
            String timestamp = dateTime.ToString();
            //this code gets the name of the port and port number
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PnPEntity WHERE Caption like '%(COM%'"))
            {
                var portnames = SerialPort.GetPortNames();
                var ports2 = searcher.Get().Cast<ManagementBaseObject>().ToList().Select(p => p["Caption"].ToString());

                var portList = portnames.Select(n => n + " - " + ports2.FirstOrDefault(s => s.Contains(n))).ToList();
                foreach (var s in portList)
                {
                    log.Text += timestamp + " " + s.ToString() + "\r\n" + "detected " + Environment.NewLine;
                }
                //}
            }
            //this code only gets the com port number
            string[] ports = SerialPort.GetPortNames();
            com_port.Items.AddRange(ports);
        }
    }
}

