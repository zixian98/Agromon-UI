using System;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Windows.Forms;


namespace test2
{
    public partial class Form1 : Form
    {
        //Variable Definition
        public SerialPort serial_port1; //Serial Port 1 Variable
        public string device_id, sensor_id, eeprom, network, status; //Variable to store string data
        public string DataReceived; //Variable to store RX data
        public string DataReceivedString; //Varialbe to store RX data converted to string
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
                    //disable combobox if it is connected
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    comboBox5.Enabled = false;
                    button3.Enabled = true;
                    label16.Text = "Refreshing";
                    label16.ForeColor = Color.Blue;
                    label20.Text = "Refreshing";
                    label20.ForeColor = Color.Blue;
                    label21.Text = "Refreshing";
                    label21.ForeColor = Color.Blue;
                    textBox2.Text = "Refreshing";
                    textBox2.ForeColor = Color.Blue;
                    textBox3.Text = "Refreshing";
                    textBox3.ForeColor = Color.Blue;
                    textBox4.Text = "Refreshing";
                    textBox4.ForeColor = Color.Blue;
                    //running command for each process to get info of the devices
                    //read id

                    try
                    {
                        //textBox1.Text += timestamp + ": Read Data, Please Wait..." + Environment.NewLine;
                        serialPort1.Write("0xAA 0x55" + "\r\n");
                        wait(2000);
                        serialPort1.Write("0xAA 0x55" + "\r\n");
                        wait(2000);
                        if (DataReceivedString == null)
                        {
                            device_id = "";
                            if (device_id == "")
                            {
                                label16.Text = "Not Avaialble";
                                label20.Text = "Not Avaialble";
                                textBox2.Text = "Not Avaialble";
                                textBox3.Text = "Not Avaialble";
                                textBox4.Text = "Not Avaialble";
                                label18.Text = "Not Connected";
                                label18.ForeColor = Color.DarkRed;
                                label21.Text = "Not Connected";
                                label21.ForeColor = Color.DarkRed;
                                progressBar1.Style = ProgressBarStyle.Blocks;
                                progressBar1.Value = 100;
                            }
                        }
                        else
                        {
                            //device_id = DataReceivedString;
                            //String[] check_all = device_id.Split(',');
                            //label16.Text = check_all[0];                            //read device id
                            //textBox1.Text += "...";

                            //switch (check_all[3])                                   //network connection type
                            //{
                            //    case "1":
                            //        label20.Text = "Wi-Fi";
                            //        break;
                            //    case "2":
                            //        label20.Text = "LoraWAN";
                            //        break;
                            //    case "3":
                            //        label20.Text = "Sigfox";
                            //        break;
                            //    case "4":
                            //        label20.Text = "4G Network";
                            //        break;
                            //    default:
                            //        label20.Text = "Not Available";
                            //        break;
                            //}
                            //progressBar1.Value = 80;
                            //textBox1.Text = "...";
                            //int connection_status_input = Convert.ToInt32(check_all[4]); //read network connection status
                            //switch (connection_status_input)
                            //{
                            //    case 0:
                            //        label21.Text = "NOT CONNECTED";
                            //        label21.ForeColor = Color.Red;
                            //        break;
                            //    case 1:
                            //        label21.Text = "CONNECTED";
                            //        label21.ForeColor = Color.Green;
                            //        break;
                            //    default:
                            //        label21.Text = "NOT CONNECTED";
                            //        label21.ForeColor = Color.Red;
                            //        break;
                            //}

                            //String[] sensors = DataReceivedString.Split(',');
                            //if (sensors[6] == "0")
                            //{
                            //    int input1A = Convert.ToInt32(sensors[5]);
                            //    switch (input1A)
                            //    {
                            //        case 128:
                            //            textBox2.Text = "Soil pH";
                            //            break;
                            //        case 64:
                            //            textBox2.Text = "Soil Moisture and Temperature";
                            //            break;
                            //        case 32:
                            //            textBox2.Text = "Soil Electrical Conductivity";
                            //            break;
                            //        case 16:
                            //            textBox2.Text = "Soil Salinity";
                            //            break;
                            //        case 8:
                            //            textBox2.Text = "Soil Temperature, Moisture, Salinity and EC";
                            //            break;
                            //        case 4:
                            //            textBox2.Text = "Soil NPK (Nitrogen-PhosphorousPotassium)";
                            //            break;
                            //        case 2:
                            //            textBox2.Text = "Water pH Sensor";
                            //            break;
                            //        case 1:
                            //            textBox2.Text = "Water Dissolved Oxygen Sensor";
                            //            break;
                            //    }
                            //}
                            //if (sensors[5] == "0")
                            //{
                            //    int input1B = Convert.ToInt32(sensors[6]);
                            //    switch (input1B)
                            //    {
                            //        case 128:
                            //            textBox2.Text = "Water Ammonia Sensor";
                            //            break;
                            //        case 64:
                            //            textBox2.Text = "Water Turbidity Sensor";
                            //            break;
                            //        case 32:
                            //            textBox2.Text = "Water Salinity Sensor";
                            //            break;
                            //        case 16:
                            //            textBox2.Text = "Ultrasonic Level Meter";
                            //            break;
                            //        case 8:
                            //            textBox2.Text = "Pneumatic Level Meter";
                            //            break;
                            //        case 4:
                            //            textBox2.Text = "Radar Level Meter";
                            //            break;

                            //        default:
                            //            textBox2.Text = "Not Available";
                            //            break;

                            //    }
                            //}

                            //if (sensors[8] == "0")
                            //{
                            //    int input2A = Convert.ToInt32(sensors[7]);
                            //    switch (input2A)
                            //    {
                            //        case 128:
                            //            textBox3.Text = "Soil pH";
                            //            break;
                            //        case 64:
                            //            textBox3.Text = "Soil Moisture and Temperature";
                            //            break;
                            //        case 32:
                            //            textBox3.Text = "Soil Electrical Conductivity";
                            //            break;
                            //        case 16:
                            //            textBox3.Text = "Soil Salinity";
                            //            break;
                            //        case 8:
                            //            textBox3.Text = "Soil Temperature, Moisture, Salinity and EC";
                            //            break;
                            //        case 4:
                            //            textBox3.Text = "Soil NPK (Nitrogen-PhosphorousPotassium)";
                            //            break;
                            //        case 2:
                            //            textBox3.Text = "Water pH Sensor";
                            //            break;
                            //        case 1:
                            //            textBox3.Text = "Water Dissolved Oxygen Sensor";
                            //            break;
                            //    }
                            //}

                            //if (sensors[7] == "0")
                            //{
                            //    int input2B = Convert.ToInt32(sensors[8]);
                            //    switch (input2B)
                            //    {
                            //        case 128:
                            //            textBox3.Text = "Water Ammonia Sensor";
                            //            break;
                            //        case 64:
                            //            textBox3.Text = "Water Turbidity Sensor";
                            //            break;
                            //        case 32:
                            //            textBox3.Text = "Water Salinity Sensor";
                            //            break;
                            //        case 16:
                            //            textBox3.Text = "Ultrasonic Level Meter";
                            //            break;
                            //        case 8:
                            //            textBox3.Text = "Pneumatic Level Meter";
                            //            break;
                            //        case 4:
                            //            textBox3.Text = "Radar Level Meter";
                            //            break;

                            //        default:
                            //            textBox3.Text = "Not Available";
                            //            break;

                            //    }
                            //}

                            //if (sensors[10] == "0")
                            //{
                            //    int input2A = Convert.ToInt32(sensors[9]);
                            //    switch (input2A)
                            //    {
                            //        case 128:
                            //            textBox4.Text = "Soil pH";
                            //            break;
                            //        case 64:
                            //            textBox4.Text = "Soil Moisture and Temperature";
                            //            break;
                            //        case 32:
                            //            textBox4.Text = "Soil Electrical Conductivity";
                            //            break;
                            //        case 16:
                            //            textBox4.Text = "Soil Salinity";
                            //            break;
                            //        case 8:
                            //            textBox4.Text = "Soil Temperature, Moisture, Salinity and EC";
                            //            break;
                            //        case 4:
                            //            textBox4.Text = "Soil NPK (Nitrogen-PhosphorousPotassium)";
                            //            break;
                            //        case 2:
                            //            textBox4.Text = "Water pH Sensor";
                            //            break;
                            //        case 1:
                            //            textBox4.Text = "Water Dissolved Oxygen Sensor";
                            //            break;
                            //    }
                            //}

                            //if (sensors[9] == "0")
                            //{
                            //    int input2B = Convert.ToInt32(sensors[10]);
                            //    switch (input2B)
                            //    {
                            //        case 128:
                            //            textBox4.Text = "Water Ammonia Sensor";
                            //            break;
                            //        case 64:
                            //            textBox4.Text = "Water Turbidity Sensor";
                            //            break;
                            //        case 32:
                            //            textBox4.Text = "Water Salinity Sensor";
                            //            break;
                            //        case 16:
                            //            textBox4.Text = "Ultrasonic Level Meter";
                            //            break;
                            //        case 8:
                            //            textBox4.Text = "Pneumatic Level Meter";
                            //            break;
                            //        case 4:
                            //            textBox4.Text = "Radar Level Meter";
                            //            break;

                            //        default:
                            //            textBox4.Text = "Not Available";
                            //            break;
                            //    }
                            //}
                        }
                    }
                    catch (Exception)
                    {

                    }
                    textBox1.Text += timestamp + ": " + "PORT " + comboBox1.Text + " " + "Sucessfull Load Data!" + Environment.NewLine;
                    progressBar1.Value = 100;
                    button3.Enabled = true;
                    button1.Enabled = false;
                    button2.Enabled = true;
                    label18.Text = "CONNECTED";
                    progressBar1.Style = ProgressBarStyle.Blocks;
                    progressBar1.Value = 100;
                    label18.ForeColor = Color.Green;
                    label16.ForeColor = Color.Black;
                    label20.ForeColor = Color.Black;
                    textBox2.ForeColor = Color.Black;
                    textBox3.ForeColor = Color.Black;
                    textBox4.ForeColor = Color.Black;
                    label21.ForeColor = Color.Green;
                }
            }
        }

        //These Items will load when form is load.
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
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

        //Load COM Port Configurations from XML File
        private void LoadConfigurationSettings()
        {
            comboBox2.Text = System.Configuration.ConfigurationManager.AppSettings["combaudrate"];
            comboBox3.Text = System.Configuration.ConfigurationManager.AppSettings["comdatabits"];
            comboBox4.Text = System.Configuration.ConfigurationManager.AppSettings["comstopbits"];
            comboBox5.Text = System.Configuration.ConfigurationManager.AppSettings["comparity"];
        }

        //Button to establish connection to COM port
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                serial_port1 = new SerialPort(comboBox1.Text, Convert.ToInt32(comboBox2.Text), (Parity)Enum.Parse(typeof(Parity), comboBox5.Text), Convert.ToInt32(comboBox3.Text), (StopBits)Enum.Parse(typeof(StopBits), comboBox4.Text));
                serialPort1.PortName = comboBox1.Text;
                serialPort1.BaudRate = Convert.ToInt32(comboBox2.Text);
                serialPort1.DataBits = Convert.ToInt32(comboBox3.Text);
                serialPort1.StopBits = (StopBits)Enum.Parse(typeof(StopBits), comboBox4.Text);
                serialPort1.Parity = (Parity)Enum.Parse(typeof(Parity), comboBox5.Text);
                serialPort1.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                serialPort1.Open();
                //check whether the port is connected then function executes
                if (serialPort1.IsOpen)
                {
                    progressBar1.Visible = true;
                    progressBar1.Style = ProgressBarStyle.Marquee;
                    label18.Text = "CONNECTING";
                    label18.ForeColor = Color.Red;
                    //disable combobox if it is connected
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    comboBox5.Enabled = false;
                    button3.Enabled = false;
                    button2.Enabled = true;
                    button5.Enabled = false;

                    // When software established connection with Agromon hardware
                    // Send 0xAA 0x55 \r\n to Agromon to enter Configuration mode automatically before the 10s timeout.
                    try
                    {
                        //wait(1000);
                        //serialPort1.Write("ªU\r\n"); //Send Cofiguration Mode Command
                        //wait(2000); // Wait for 2s for the response, to solve the code running too fast before getting the response
                        textBox1.Text += "TX: "  + DataReceivedString + "\r\n" + Environment.NewLine;
                        if(textBox1.Text == "AGROMON READY\r\nOK\r\n")
                        {
                            textBox1.Text += "Information Send: OK" + Environment.NewLine;
                            serialPort1.Write("ªU");
                            textBox1.Text += "RX: " + serialPort1 + "\r\n" + Environment.NewLine;
                        }
                        textBox1.Text += DataReceivedString + "\r\n" + Environment.NewLine;
                        wait(1000);

                        if (DataReceivedString == null)
                        {
                            device_id = "";
                            if (device_id == "")
                            {
                                label16.Text = "Not Avaialble";
                                label20.Text = "Not Avaialble";
                                textBox2.Text = "Not Avaialble";
                                textBox3.Text = "Not Avaialble";
                                textBox4.Text = "Not Avaialble";
                                label18.Text = "Not Connected";
                                label18.ForeColor = Color.DarkRed;
                                label21.Text = "Not Connected";
                                label21.ForeColor = Color.DarkRed;
                                progressBar1.Style = ProgressBarStyle.Blocks;
                                progressBar1.Value = 100;
                            }
                        }
                        else
                        {
                            label18.Text = "CONNECTED";
                            label18.ForeColor = Color.Green;
                            //device_id = DataReceivedString;
                            //String[] check_all = device_id.Split(',');
                            //label16.Text = check_all[0];                             //read device id
                            //textBox1.Text += "...";

                            //switch (check_all[3])                                    //network connection type
                            //{
                            //    case "1":
                            //        label20.Text = "Wi-Fi";
                            //        break;
                            //    case "2":
                            //        label20.Text = "LoraWAN";
                            //        break;
                            //    case "3":
                            //        label20.Text = "Sigfox";
                            //        break;
                            //    case "4":
                            //        label20.Text = "4G Network";
                            //        break;
                            //    default:
                            //        label20.Text = "Not Available";
                            //        break;
                            //}
                            ////progressBar1.Value = 80;
                            //textBox1.Text = "...";
                            //int connection_status_input = Convert.ToInt32(check_all[4]); //read network connection status
                            //switch (connection_status_input)
                            //{
                            //    case 0:
                            //        label21.Text = "NOT CONNECTED";
                            //        label21.ForeColor = Color.Red;
                            //        break;
                            //    case 1:
                            //        label21.Text = "CONNECTED";
                            //        label21.ForeColor = Color.Green;
                            //        break;
                            //    default:
                            //        label21.Text = "NOT CONNECTED";
                            //        label21.ForeColor = Color.Red;
                            //        break;
                            //}
                            //if (DataReceivedString == null)
                            //{
                            //    string test = " ";
                            //    if (test == " ")
                            //    {

                            //    }
                            //}
                            //else
                            //{
                            //    String[] sensors = DataReceivedString.Split(',');
                            //    if (sensors[6] == "0")
                            //    {
                            //        int input1A = Convert.ToInt32(sensors[5]);
                            //        switch (input1A)
                            //        {
                            //            case 128:
                            //                textBox2.Text = "Soil pH";
                            //                break;
                            //            case 64:
                            //                textBox2.Text = "Soil Moisture and Temperature";
                            //                break;
                            //            case 32:
                            //                textBox2.Text = "Soil Electrical Conductivity";
                            //                break;
                            //            case 16:
                            //                textBox2.Text = "Soil Salinity";
                            //                break;
                            //            case 8:
                            //                textBox2.Text = "Soil Temperature, Moisture, Salinity and EC";
                            //                break;
                            //            case 4:
                            //                textBox2.Text = "Soil NPK (Nitrogen-PhosphorousPotassium)";
                            //                break;
                            //            case 2:
                            //                textBox2.Text = "Water pH Sensor";
                            //                break;
                            //            case 1:
                            //                textBox2.Text = "Water Dissolved Oxygen Sensor";
                            //                break;
                            //        }
                            //    }
                            //    if (sensors[5] == "0")
                            //    {
                            //        int input1B = Convert.ToInt32(sensors[6]);
                            //        switch (input1B)
                            //        {
                            //            case 128:
                            //                textBox2.Text = "Water Ammonia Sensor";
                            //                break;
                            //            case 64:
                            //                textBox2.Text = "Water Turbidity Sensor";
                            //                break;
                            //            case 32:
                            //                textBox2.Text = "Water Salinity Sensor";
                            //                break;
                            //            case 16:
                            //                textBox2.Text = "Ultrasonic Level Meter";
                            //                break;
                            //            case 8:
                            //                textBox2.Text = "Pneumatic Level Meter";
                            //                break;
                            //            case 4:
                            //                textBox2.Text = "Radar Level Meter";
                            //                break;

                            //            default:
                            //                textBox2.Text = "Not Available";
                            //                break;

                            //        }
                            //    }

                            //    if (sensors[8] == "0")
                            //    {
                            //        int input2A = Convert.ToInt32(sensors[7]);
                            //        switch (input2A)
                            //        {
                            //            case 128:
                            //                textBox3.Text = "Soil pH";
                            //                break;
                            //            case 64:
                            //                textBox3.Text = "Soil Moisture and Temperature";
                            //                break;
                            //            case 32:
                            //                textBox3.Text = "Soil Electrical Conductivity";
                            //                break;
                            //            case 16:
                            //                textBox3.Text = "Soil Salinity";
                            //                break;
                            //            case 8:
                            //                textBox3.Text = "Soil Temperature, Moisture, Salinity and EC";
                            //                break;
                            //            case 4:
                            //                textBox3.Text = "Soil NPK (Nitrogen-PhosphorousPotassium)";
                            //                break;
                            //            case 2:
                            //                textBox3.Text = "Water pH Sensor";
                            //                break;
                            //            case 1:
                            //                textBox3.Text = "Water Dissolved Oxygen Sensor";
                            //                break;
                            //        }
                            //    }

                            //    if (sensors[7] == "0")
                            //    {
                            //        int input2B = Convert.ToInt32(sensors[8]);
                            //        switch (input2B)
                            //        {
                            //            case 128:
                            //                textBox3.Text = "Water Ammonia Sensor";
                            //                break;
                            //            case 64:
                            //                textBox3.Text = "Water Turbidity Sensor";
                            //                break;
                            //            case 32:
                            //                textBox3.Text = "Water Salinity Sensor";
                            //                break;
                            //            case 16:
                            //                textBox3.Text = "Ultrasonic Level Meter";
                            //                break;
                            //            case 8:
                            //                textBox3.Text = "Pneumatic Level Meter";
                            //                break;
                            //            case 4:
                            //                textBox3.Text = "Radar Level Meter";
                            //                break;

                            //            default:
                            //                textBox3.Text = "Not Available";
                            //                break;

                            //        }
                            //    }

                            //    if (sensors[10] == "0")
                            //    {
                            //        int input2A = Convert.ToInt32(sensors[9]);
                            //        switch (input2A)
                            //        {
                            //            case 128:
                            //                textBox4.Text = "Soil pH";
                            //                break;
                            //            case 64:
                            //                textBox4.Text = "Soil Moisture and Temperature";
                            //                break;
                            //            case 32:
                            //                textBox4.Text = "Soil Electrical Conductivity";
                            //                break;
                            //            case 16:
                            //                textBox4.Text = "Soil Salinity";
                            //                break;
                            //            case 8:
                            //                textBox4.Text = "Soil Temperature, Moisture, Salinity and EC";
                            //                break;
                            //            case 4:
                            //                textBox4.Text = "Soil NPK (Nitrogen-PhosphorousPotassium)";
                            //                break;
                            //            case 2:
                            //                textBox4.Text = "Water pH Sensor";
                            //                break;
                            //            case 1:
                            //                textBox4.Text = "Water Dissolved Oxygen Sensor";
                            //                break;
                            //        }
                            //    }

                            //    if (sensors[9] == "0")
                            //    {
                            //        int input2B = Convert.ToInt32(sensors[10]);
                            //        switch (input2B)
                            //        {
                            //            case 128:
                            //                textBox4.Text = "Water Ammonia Sensor";
                            //                break;
                            //            case 64:
                            //                textBox4.Text = "Water Turbidity Sensor";
                            //                break;
                            //            case 32:
                            //                textBox4.Text = "Water Salinity Sensor";
                            //                break;
                            //            case 16:
                            //                textBox4.Text = "Ultrasonic Level Meter";
                            //                break;
                            //            case 8:
                            //                textBox4.Text = "Pneumatic Level Meter";
                            //                break;
                            //            case 4:
                            //                textBox4.Text = "Radar Level Meter";
                            //                break;

                            //            default:
                            //                textBox4.Text = "Not Available";
                            //                break;
                            //        }
                            //    }
                            //}
                            //serialPort1.Write("0x06" + "\r\n");
                            //wait(2000);
                            //if (DataReceivedString == null)
                            //{
                            //    String names = "";
                            //    if (names == "")
                            //    {

                            //    }
                            //}
                            //else
                            //{
                            //    textBox1.Text += timestamp + ":" + DataReceivedString + Environment.NewLine;
                            //}
                            //textBox1.Text += timestamp +  "..." + Environment.NewLine;
                            //progressBar1.Style = ProgressBarStyle.Blocks;
                            //progressBar1.Value = 100;
                            //label18.Text = "CONNECTED";
                            //label18.ForeColor = Color.Green;
                        }
                    }
                    catch (Exception)
                    {

                    }

                    button3.Enabled = true;
                    button1.Enabled = false;
                    label18.Text = "CONNECTED";
                    label18.ForeColor = Color.Green;

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please select the serial PORT or refresh if none serial PORT detected", "Message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                label18.ForeColor = Color.Red;
                label18.Text = "NOT CONNECTED";
                //Enable Comboboxes when disconnected to COM Port.
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                comboBox5.Enabled = true;
                button1.Enabled = true;
                button5.Enabled = true;
                button3.Enabled = false;
                button2.Enabled = false;

            }
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

        //Button to refresh COM port available.
        private void button5_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            String timestamp = dateTime.ToString();
            if (!serialPort1.IsOpen)
            {

                try
                {
                    comboBox1.Items.Clear();
                    comboBox1.Text = "Select Port";
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

        //Button to disconnect COM port connection
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
                button2.Enabled = false;
                button1.Enabled = true;
                button5.Enabled = true;
                label16.Text = "Not Available";
                label20.Text = "Not Available";
            }
            else
            {
                DateTime dateTime = DateTime.Now;
                String timestamp = dateTime.ToString();
                textBox1.Text += timestamp + " " + "No Serial Port is connected!" + Environment.NewLine;

            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serialPort1.IsOpen)
            {
                serialPort1.DiscardOutBuffer();
                serialPort1.DiscardInBuffer();
                serialPort1.DataReceived -= new SerialDataReceivedEventHandler(DataReceivedHandler);
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
                config.AppSettings.Settings.Add("combaudrate", comboBox2.Text);
                config.AppSettings.Settings.Add("comdatabits", comboBox3.Text);
                config.AppSettings.Settings.Add("comstopbits", comboBox4.Text);
                config.AppSettings.Settings.Add("comparity", comboBox5.Text);

                config.Save(System.Configuration.ConfigurationSaveMode.Modified);
                System.Configuration.ConfigurationManager.RefreshSection("appSettings");
                DateTime dateTime = DateTime.Now;
                String timestamp = dateTime.ToString();
                textBox1.Text += timestamp + " " + "New port settings are saved." + Environment.NewLine;
            }
            catch (Exception err)
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
                //if (portss == 0 )
                //{
                //    textBox1.Text += timestamp + ": " + "No Serial Port Detected " + portss+ Environment.NewLine;
                //}
                ////this code displays the values in a list form in the textbox
                //else
                //{
                foreach (var s in portList)
                {
                    textBox1.Text += timestamp + " " + s.ToString() + "\r\n" + "detected " + Environment.NewLine;
                }
                //}
            }
            //this code only gets the com port number
            string[] ports = SerialPort.GetPortNames();
            comboBox1.Items.AddRange(ports);
        }

    }
}


