using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.IO;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.Reflection.Emit;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolBar;


namespace test2
{
    public partial class Form1 : Form
    {
        public SerialPort serial_port1;
        public string device_id, sensor_id, eeprom, network, status;
        public string DataReceived;
        public string DataReceivedString;
        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.DiscardOutBuffer();
                serialPort1.DiscardInBuffer();
                Form2 form2 = new Form2(serialPort1);
                form2.ShowDialog();
                if (serialPort1.IsOpen)
                {
                    DateTime dateTime = DateTime.Now;
                    String timestamp = dateTime.ToString();
                    progressBar1.Visible = true;
                    progressBar1.Style = ProgressBarStyle.Marquee;
                    label18.Text = "REFRESHING";
                    label18.ForeColor = Color.Blue;
                    textBox1.Text += timestamp + ":" + "PORT " + comboBox1.Text + ": " + "is refresing!" + Environment.NewLine;
                    //disable combobox if it is connected
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    comboBox5.Enabled = false;
                    button3.Enabled = true;
                    label16.Text = "Refreshing";
                    label16.ForeColor = Color.Blue;
                    label15.Text = "0";
                    label15.ForeColor = Color.Blue;
                    label17.Text = "0";
                    label17.ForeColor = Color.Blue;
                    label20.Text = "Refreshing";
                    label20.ForeColor = Color.Blue;
                    label21.Text = "Refreshing";
                    label21.ForeColor = Color.Blue;
                    //running command for each process to get info of the devices
                    //read id

                    try
                    {
                        textBox1.Text += timestamp + ": Read Data, Please Wait...";
                        serialPort1.Write("checkAll");
                        wait(2000);
                        serialPort1.Write("checkAll");
                        wait(2000);
                        if (DataReceivedString == null)
                        {
                            device_id = "";
                            if (device_id == "")
                            {
                                label16.Text = "Not Avaialble";
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
                    }
                    catch (Exception err)
                    {

                    }
                    textBox1.Text += timestamp + ": " + "PORT " + comboBox1.Text + " " + "Sucessfull Load Data!" + Environment.NewLine;
                    progressBar1.Value = 100;
                    button3.Enabled = false;
                    button1.Enabled = false;
                    button2.Enabled = true;
                    label18.Text = "CONNECTED";
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    progressBar1.Value = 100;
                    label18.ForeColor = Color.Green;
                    label16.ForeColor = Color.Black;
                    label15.ForeColor = Color.Black;
                    label17.ForeColor = Color.Black;
                    label20.ForeColor = Color.Black;
                    label21.ForeColor = Color.Green;
                }
            }
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
            button2.Enabled = false;
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
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                serialPort1.DataBits = Convert.ToInt32(comboBox3.Text);
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), comboBox4.Text);
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), comboBox5.Text);
                textBox1.Text += timestamp + " " + "Connecting to Serial PORT..." + comboBox1.Text + Environment.NewLine;
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                serialPort1.Open();
                //check whether the port is connected then function executes
                if (serialPort1.IsOpen)
                {
                    progressBar1.Visible = true;
                    progressBar1.Style = ProgressBarStyle.Marquee;
                    label18.Text = "CONNECTING";
                    label18.ForeColor = Color.Red;
                    textBox1.Text += timestamp + ":" + "PORT " + comboBox1.Text + ": " + "is connecting" + Environment.NewLine;
                    wait(1000);
                    textBox1.Text += timestamp + ":" + "PORT " + comboBox1.Text + ": " + "is do checking data routine" + Environment.NewLine;
                    //disable combobox if it is connected
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    comboBox5.Enabled = false;
                    button3.Enabled = false;
                    button2.Enabled = true;
                    button5.Enabled = false;

                    //running command for each process to get info of the devices
                    //read id

                    try
                    {
                        textBox1.Text += timestamp + ": Read Data, Please Wait...";
                        serialPort1.Write("checkAll");
                        wait(2000);
                        serialPort1.Write("checkAll");
                        wait(2000);
                        if (DataReceivedString == null)
                        {
                            device_id = "";
                            if (device_id == "")
                            {
                                label16.Text = "Not Avaialble";
                            }
                        }
                        else
                        {
                            device_id = DataReceivedString;
                            String[] check_all = device_id.Split(',');
                            label16.Text = check_all[0];                            //read device id
                            textBox1.Text += "...";
                            //progressBar1.Value = 20;
                            label17.Text = check_all[1];                            //read connected sensors
                            textBox1.Text += ":...";
                            //progressBar1.Value = 40;
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
                            //progressBar1.Value = 80;
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
                            if (DataReceivedString == null)
                            {
                                string test = " ";
                                if (test == " ")
                                {

                                }
                            }
                            else
                            {
                                String[] sensors = DataReceivedString.Split(',');
                                if (sensors[6] == "0")
                                {
                                    int input1A = Convert.ToInt32(sensors[5]);
                                    switch (input1A)
                                    {
                                        case 128:
                                            textBox2.Text = "Soil pH";
                                            break;
                                        case 64:
                                            textBox2.Text = "Soil Moisture and Temperature";
                                            break;
                                        case 32:
                                            textBox2.Text = "Soil Electrical Conductivity";
                                            break;
                                        case 16:
                                            textBox2.Text = "Soil Salinity";
                                            break;
                                        case 8:
                                            textBox2.Text = "Soil Temperature, Moisture, Salinity and EC";
                                            break;
                                        case 4:
                                            textBox2.Text = "Soil NPK (Nitrogen-PhosphorousPotassium)";
                                            break;
                                        case 2:
                                            textBox2.Text = "Water pH Sensor";
                                            break;
                                        case 1:
                                            textBox2.Text = "Water Dissolved Oxygen Sensor";
                                            break;
                                    }
                                }
                                if (sensors[5] == "0")
                                {
                                    int input1B = Convert.ToInt32(sensors[6]);
                                    switch (input1B)
                                    {
                                        case 128:
                                            textBox2.Text = "Water Ammonia Sensor";
                                            break;
                                        case 64:
                                            textBox2.Text = "Water Turbidity Sensor";
                                            break;
                                        case 32:
                                            textBox2.Text = "Water Salinity Sensor";
                                            break;
                                        case 16:
                                            textBox2.Text = "Ultrasonic Level Meter";
                                            break;
                                        case 8:
                                            textBox2.Text = "Pneumatic Level Meter";
                                            break;
                                        case 4:
                                            textBox2.Text = "Radar Level Meter";
                                            break;

                                        default:
                                            textBox2.Text = "Not Available";
                                            break;

                                    }
                                }

                                if (sensors[8] == "0")
                                {
                                    int input2A = Convert.ToInt32(sensors[7]);
                                    switch (input2A)
                                    {
                                        case 128:
                                            textBox3.Text = "Soil pH";
                                            break;
                                        case 64:
                                            textBox3.Text = "Soil Moisture and Temperature";
                                            break;
                                        case 32:
                                            textBox3.Text = "Soil Electrical Conductivity";
                                            break;
                                        case 16:
                                            textBox3.Text = "Soil Salinity";
                                            break;
                                        case 8:
                                            textBox3.Text = "Soil Temperature, Moisture, Salinity and EC";
                                            break;
                                        case 4:
                                            textBox3.Text = "Soil NPK (Nitrogen-PhosphorousPotassium)";
                                            break;
                                        case 2:
                                            textBox3.Text = "Water pH Sensor";
                                            break;
                                        case 1:
                                            textBox3.Text = "Water Dissolved Oxygen Sensor";
                                            break;
                                    }
                                }

                                if (sensors[7] == "0")
                                {
                                    int input2B = Convert.ToInt32(sensors[8]);
                                    switch (input2B)
                                    {
                                        case 128:
                                            textBox3.Text = "Water Ammonia Sensor";
                                            break;
                                        case 64:
                                            textBox3.Text = "Water Turbidity Sensor";
                                            break;
                                        case 32:
                                            textBox3.Text = "Water Salinity Sensor";
                                            break;
                                        case 16:
                                            textBox3.Text = "Ultrasonic Level Meter";
                                            break;
                                        case 8:
                                            textBox3.Text = "Pneumatic Level Meter";
                                            break;
                                        case 4:
                                            textBox3.Text = "Radar Level Meter";
                                            break;

                                        default:
                                            textBox3.Text = "Not Available";
                                            break;

                                    }
                                }

                                if (sensors[10] == "0")
                                {
                                    int input2A = Convert.ToInt32(sensors[9]);
                                    switch (input2A)
                                    {
                                        case 128:
                                            textBox4.Text = "Soil pH";
                                            break;
                                        case 64:
                                            textBox4.Text = "Soil Moisture and Temperature";
                                            break;
                                        case 32:
                                            textBox4.Text = "Soil Electrical Conductivity";
                                            break;
                                        case 16:
                                            textBox4.Text = "Soil Salinity";
                                            break;
                                        case 8:
                                            textBox4.Text = "Soil Temperature, Moisture, Salinity and EC";
                                            break;
                                        case 4:
                                            textBox4.Text = "Soil NPK (Nitrogen-PhosphorousPotassium)";
                                            break;
                                        case 2:
                                            textBox4.Text = "Water pH Sensor";
                                            break;
                                        case 1:
                                            textBox4.Text = "Water Dissolved Oxygen Sensor";
                                            break;
                                    }
                                }

                                if (sensors[9] == "0")
                                {
                                    int input2B = Convert.ToInt32(sensors[10]);
                                    switch (input2B)
                                    {
                                        case 128:
                                            textBox4.Text = "Water Ammonia Sensor";
                                            break;
                                        case 64:
                                            textBox4.Text = "Water Turbidity Sensor";
                                            break;
                                        case 32:
                                            textBox4.Text = "Water Salinity Sensor";
                                            break;
                                        case 16:
                                            textBox4.Text = "Ultrasonic Level Meter";
                                            break;
                                        case 8:
                                            textBox4.Text = "Pneumatic Level Meter";
                                            break;
                                        case 4:
                                            textBox4.Text = "Radar Level Meter";
                                            break;

                                        default:
                                            textBox4.Text = "Not Available";
                                            break;
                                    }
                                }
                            }
                            textBox1.Text += "..." + Environment.NewLine;
                            label18.Text = "CONNECTED";
                            label18.ForeColor = Color.Green;
                            progressBar1.Style = ProgressBarStyle.Blocks;
                            progressBar1.Value = 100;
                        }
                    }
                    catch (Exception err)
                    {

                    }
                    textBox1.Text += timestamp + ": " + "PORT" + comboBox1.Text + " " + "Sucessfull Load Data!" + Environment.NewLine;                    
                    button3.Enabled = true;
                    button1.Enabled = false;
                }
            }
            catch (Exception err)
            {
                DateTime dateTime = DateTime.Now;
                String timestamp = dateTime.ToString();
                MessageBox.Show("Please select the serial PORT or refresh if none serial PORT detected", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox1.Text += timestamp + " " + "Error connecting to Serial PORT." + Environment.NewLine;
                label18.ForeColor = Color.Red;
                label18.Text = "NOT CONNECTED";
                //not connected enable comboboxes
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                comboBox5.Enabled = true;
                button1.Enabled = true;
                button5.Enabled = false;
                button3.Enabled = false;

            }
        }

        //sensor read funcion
        private void sensor_read()
        {
           

        }
        //wait function
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

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                DataReceived = serialPort1.ReadLine();
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

        private void button5_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            String timestamp = dateTime.ToString();
            if (!serialPort1.IsOpen)
            {

                try
                {
                    comboBox1.Items.Clear();
                    textBox1.Clear();
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                DateTime dateTime = DateTime.Now;
                String timestamp = dateTime.ToString();
                serialPort1.DiscardOutBuffer();
                serialPort1.DiscardInBuffer();
                serialPort1.Close();
                serialPort1.DataReceived -= new SerialDataReceivedEventHandler(DataReceivedHandler);
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
                button1.Enabled = false;
                label16.Text = "Not Available";
                label17.Text = "Not Available";
                progressBar3.Value = 0;
                label20.Text = "Not Available";
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
            if (serialPort1.IsOpen)
            {
                //close connection when application exit
                serialPort1.Close();
                
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
                //this code displays the values in a list form in the textbox
                foreach(var s in portList)
                {
                    textBox1.Text += timestamp + " " + s.ToString() + "\r\n" + "detected" + Environment.NewLine;
                }
            }
            //this code only gets the com port number
            string[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
        }

    }
}


