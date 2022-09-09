using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace test2
{
    public partial class Form1 : Form
    {
        public SerialPort serial_port1;
        private string device_id, sensor_id, eeprom, network, status;
        private string DataReceived;
        private string DataReceivedString;
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(serial_port1);
            form2.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string [] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
            //not connected enable comboboxes
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
            comboBox5.Enabled = true;
            //disable configure button if not connected
            button3.Enabled = false;
            LoadConfigurationSettings();
            


        }

        private void LoadConfigurationSettings()
        {
            comboBox2.Text = System.Configuration.ConfigurationManager.AppSettings["combaudrate"];
            comboBox3.Text = System.Configuration.ConfigurationManager.AppSettings["comdatabits"];
            comboBox4.Text = System.Configuration.ConfigurationManager.AppSettings["comstopbits"];
            comboBox5.Text = System.Configuration.ConfigurationManager.AppSettings["comparity"];
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                String timestamp = dateTime.ToString();
                serial_port1 = new SerialPort(comboBox1.Text, Convert.ToInt32(comboBox2.Text), (Parity)Enum.Parse(typeof(Parity), comboBox5.Text), Convert.ToInt32(comboBox3.Text), (StopBits)Enum.Parse(typeof(StopBits), comboBox4.Text));
                serial_port1.PortName = comboBox1.Text;
                serial_port1.BaudRate = Convert.ToInt32(comboBox2.Text);
                serial_port1.DataBits = Convert.ToInt32(comboBox3.Text);
                serial_port1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), comboBox4.Text);
                serial_port1.Parity = (Parity)Enum.Parse(typeof(Parity), comboBox5.Text);
                textBox1.Text += timestamp + " " + "Connecting to Serial PORT..." + comboBox1.Text + Environment.NewLine;
                serial_port1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                serial_port1.Open();
                //check whether the port is connected then function executes
                if (serial_port1.IsOpen)
                {
                    label18.Text = "CONNECTED";
                    label18.ForeColor = Color.Green;
                    textBox1.Text += timestamp + " " + "PORT" + comboBox1.Text + ": " + "is connected!" + Environment.NewLine;
                    //disable combobox if it is connected
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    comboBox5.Enabled = false;
                    button3.Enabled = false;

                    //running command for each process to get info of the devices
                    //read id

                    try
                    {
                        textBox1.Text += timestamp + ": Read Data, Please Wait...";
                        serialPort1.Write("checkAll");
                        wait(2000);
                        if (DataReceivedString == null)
                        {
                            device_id = "";
                            if (device_id == "")
                            {
                                label16.Text = "NOT AVAILABLE";
                            }
                        }
                        else
                        {
                            device_id = DataReceivedString;
                            String[] check_all = device_id.Split(',');
                            label16.Text = check_all[0];                            //read device id
                            textBox1.Text += "...";
                            progressBar1.Value = 20;
                            label17.Text = check_all[1];                            //read connected sensors
                            textBox1.Text += ":...";
                            progressBar1.Value = 40;
                            int storage_value_int = Convert.ToInt32(check_all[2]);   //read eeprom storage
                            if (storage_value_int != 0)
                            {
                                progressBar1.Value = 60;
                                textBox1.Text += "...";
                                progressBar3.Value = storage_value_int;
                                label15.Text = progressBar3.Value.ToString();
                            }
                            else
                            {
                                progressBar3.Value = 0;
                                label15.Text = progressBar3.Value.ToString();

                                // }
                            }
                            switch (check_all[3])                                    //network connection type
                            {
                                case "1":
                                    label20.Text = "Wi-Fi";
                                    break;
                                case "2":
                                    label20.Text = "LoraWAN";
                                    break;
                                case "3":
                                    label20.Text = "Sigfox";
                                    break;
                                case "4":
                                    label20.Text = "4G Network";
                                    break;
                                default:
                                    label20.Text = "Not Available";
                                    break;
                            }
                            progressBar1.Value = 80;
                            textBox1.Text = "...";
                            int connection_status_input = Convert.ToInt32(check_all[4]); //read network connection status
                            switch (connection_status_input)
                            {
                                case 0:
                                    label21.Text = "NOT CONNECTED";
                                    label21.ForeColor = Color.Red;
                                    break;
                                case 1:
                                    label21.Text = "CONNECTED";
                                    label21.ForeColor = Color.Green;
                                    break;
                                default:
                                    label21.Text = "NOT CONNECTED";
                                    label21.ForeColor = Color.Red;
                                    break;
                            }
                        }
                        textBox1.Text += "..." + Environment.NewLine;
                    }
                    catch (Exception err)
                    {
                        DateTime dateTime = DateTime.Now;
                        String timestamp = dateTime.ToString();
                        MessageBox.Show(err.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        textBox1.Text += timestamp + " " + "Error connecting to Serial PORT." + Environment.NewLine;
                        label18.ForeColor = Color.Red;
                        label18.Text = "NOT CONNECTED";
                        //not connected enable comboboxes
                        comboBox1.Enabled = true;
                        comboBox2.Enabled = true;
                        comboBox3.Enabled = true;
                        comboBox4.Enabled = true;
                        comboBox5.Enabled = true;
                        //disable configure button if not connected
                        button3.Enabled = false;

                    }
        }
        //wait function
        private void wait(int milliseconds)
        {
            var timer1 = new System.Windows.Forms.Timer();
            if (milliseconds == 0 || milliseconds < 0) return;
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

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                DataReceived = serial_port1.ReadLine();
                this.Invoke(new Action(ProcessingData));
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            

        }

        private void ProcessingData()
        {
            DataReceivedString = DataReceived.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (serial_port1.IsOpen)
            {
                DateTime dateTime = DateTime.Now;
                String timestamp = dateTime.ToString();
                serial_port1.DiscardOutBuffer();
                serial_port1.DiscardInBuffer();
                serial_port1.Close();
                serial_port1.DataReceived -= new SerialDataReceivedEventHandler(DataReceivedHandler);
                progressBar1.Value = 0;
                textBox1.Text += timestamp + " " + "PORT" + comboBox1.Text + " " + "is disconnected!" + Environment.NewLine;
                label18.ForeColor = Color.Red;
                label18.Text = "NOT CONNECTED";
                label21.ForeColor = Color.Red;
                label21.Text = "NOT CONNECTED";
                //not connected enable comboboxes
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                comboBox5.Enabled = true;
                //disable configure button if not connected
                button3.Enabled = false;
                label16.Text = "Not Available";
                label17.Text = "Not Available";
                progressBar3.Value = 0;
                label15.Text = "-";
                label20.Text = "Not Available";
                label11.Text = "-";
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
            Form3 form3 = new Form3();
            form3.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
            if (serial_port1.IsOpen)
            {
                //close connection when application exit
                serial_port1.Close();
                
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //button to save port configuration for next time. (Congfig file)
            try
            {
                var config = System.Configuration.ConfigurationManager.OpenExeConfiguration(System.Configuration.ConfigurationUserLevel.None);
                //remove the setting in config file
                config.AppSettings.Settings.Remove("combaudrate");
                config.AppSettings.Settings.Remove("comdatabits");
                config.AppSettings.Settings.Remove("comstopbits");
                config.AppSettings.Settings.Remove("comparity");

                //then add new setting to the config file
                config.AppSettings.Settings.Add("combaudrate",comboBox2.Text);
                config.AppSettings.Settings.Add("comdatabits", comboBox3.Text);
                config.AppSettings.Settings.Add("comstopbits", comboBox4.Text);
                config.AppSettings.Settings.Add("comparity", comboBox5.Text);

                config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                DateTime dateTime = DateTime.Now;
                String timestamp = dateTime.ToString();
                textBox1.Text += timestamp + " " + "New port settings are saved." + Environment.NewLine;
            }
            catch(Exception err)
            {
                MessageBox.Show(@"Saving Error." + err.Message);
            }
        }
    }
}
