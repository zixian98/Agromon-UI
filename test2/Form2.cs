﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO.Ports;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;


namespace test2
{
    public partial class Form2 : Form
    {
        SerialPort serial_portform2; //define to get the port data from serial.port 1 in form1
        public string dataReceivedForm2;
        public string dataReceivedStringForm2;
        private string sensor_id3, sensor_id2, sensor_id1, net_id, sensor1, sensor2, sensor3;
        private string wifi_available;
        private string sigfox_available;
        private string lorawan_available;
        private string fourthgeneration_network_available;
        private string[] input1_command_first_eight_sensors = { "1A", "2A", "3A", "4A", "5A", "6A", "7A", "8A" }; //system command for setup input 1 first eight sensors
        private string[] input1_command_second_eight_sensors = { "9A", "10A", "11A", "12A", "13A", "14A" }; //system command for setup input 1 second eight sensors
        private string[] input2_command_first_eight_sensors = { "1B", "2B", "3B", "4B", "5B", "6B", "7B", "8B" }; //system command for setup input 2 first eight sensors
        private string[] input2_command_second_eight_sensors = { "9B", "10B", "11B", "12B", "13B", "14B" }; //system command for setup input 2 second eight sensors
        private string[] input3_command_first_eight_sensors = { "1C", "2C", "3C", "4C", "5C", "6C", "7C", "8C" }; //system command for setup input 3 first eight sensors
        private string[] input3_command_second_eight_sensors = { "9C", "10C", "11C", "12C", "13C", "14C" }; //system command for setup input 3 second eight sensors
        private string[] network_type_command_setup = { "N-128", "N-64", "N-32", "N-16" }; //system command network setup
        public Form2(SerialPort serial_port1)
        {
            InitializeComponent();
            textBox2.PasswordChar = '*';
            textBox2.MaxLength = 100;
            serial_portform2 = serial_port1;
            serial_portform2.DataReceived += Serial_portform2_DataReceived;
        }

        private void Serial_portform2_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                dataReceivedForm2 = serial_portform2.ReadLine();
                this.Invoke(new Action(ProcessingDataForm2));
            }
            catch (Exception)
            {
                //catch error
            }
        }

        private void ProcessingDataForm2()
        {
            dataReceivedStringForm2 = dataReceivedForm2.ToString();
            Console.Write(dataReceivedStringForm2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            String timestamp = dateTime.ToString();

            if (radioButton1.Checked == true && textBox1.Text.Length != 0 && textBox2.Text.Length != 0)
            {
                string selected_network = radioButton1.Text;
                string ssid_inserted = textBox1.Text;
                string wifi_password = textBox2.Text;
                string message = "Are you sure to add the network?";
                string title = "Confirm";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {

                    //if all information and correct and the process is success
                    //call regarding command to send instruction to the device in order for
                    //further processsing (select network type, add configuration and save it in EEPROM)
                    //FOR WI-FI
                    try
                    { 
                        serial_portform2.Write("network");
                        wait(2000);
                        serial_portform2.Write("network");
                        wait(2000);
                        if (dataReceivedStringForm2 == null)
                        {
                            sensor1 = "";
                            if (sensor1 == "")
                            {
                                label16.Text = "Not Avaialble";
                            }
                        }
                        else
                        {
                            wifi_available = dataReceivedForm2;
                            int wifi_available_int = Convert.ToInt32(wifi_available);
                            if (wifi_available_int == 0)
                            {
                                serial_portform2.WriteLine(network_type_command_setup[0]); //N-128 System Command + sending wifi ssid and password
                                label16.Text = selected_network;
                                label17.Text = ssid_inserted;
                                textBox3.Text += timestamp + " " + "The device is connected to" + " " + selected_network + Environment.NewLine;
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

                                textBox1.Text = String.Empty;
                                textBox2.Text = String.Empty;
                                groupBox1.Enabled = false;
                                button12.Enabled = true;
                            }
                            if (wifi_available_int != 0)
                            {
                                textBox3.Text += timestamp + ": " + "There is network detected. Please reset" + " " + Environment.NewLine;
                                button12.Enabled = true;
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }
                }
            }

            else if (radioButton2.Checked == true)
            {
                string selected_network = radioButton2.Text;
                string title = "Confirm";
                string message = "Confirm the selection";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    //if all information and correct and the process is success
                    //call regarding command to send instruction to the device in order for
                    //further processsing (select network type, add configuration and save it in EEPROM)
                    //FOR SIGFOX
                    try
                    {
                        serial_portform2.Write("network");
                        wait(2000);
                        serial_portform2.Write("network");
                        wait(2000);
                        if (dataReceivedStringForm2 == null)
                        {
                            sensor1 = "";
                            if (sensor1 == "")
                            {
                                label16.Text = "Not Avaialble";
                            }
                        }
                        else
                        {
                            sigfox_available = dataReceivedForm2;
                            int sigfox_available_int = Convert.ToInt32(sigfox_available);
                            if (sigfox_available_int == 0)
                            {
                                serial_portform2.WriteLine(network_type_command_setup[1]); //N-64 System Command
                                label16.Text = selected_network;
                                textBox3.Text += timestamp + " " + "The device is connected to" + " " + selected_network + Environment.NewLine;
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
                                groupBox1.Enabled = false;
                                button12.Enabled = true;
                            }
                            if (sigfox_available_int != 0)
                            {
                                textBox3.Text += timestamp + ": " + "There is network detected. Please reset" + " " + Environment.NewLine;
                                button12.Enabled = true;
                            }

                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }
                }
            }

            if (radioButton3.Checked == true)
            {
                string selected_network = radioButton3.Text;
                string title = "Confirm";
                string message = "Confirm the selection";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {


                    //if all information and correct and the process is success
                    //call regarding command to send instruction to the device in order for
                    //further processsing (select network type, add configuration and save it in EEPROM)
                    //FOR LoraWAN
                    try
                    {
                        serial_portform2.Write("network");
                        wait(2000);
                        serial_portform2.Write("network");
                        wait(2000);
                        if (dataReceivedStringForm2 == null)
                        {
                            sensor1 = "";
                            if (sensor1 == "")
                            {
                                label16.Text = "Not Avaialble";
                            }
                        }
                        else
                        {
                            lorawan_available = dataReceivedForm2;
                            int lorawan_available_int = Convert.ToInt32(lorawan_available);
                            if (lorawan_available_int == 0)
                            {
                                try
                                {
                                    serial_portform2.WriteLine(network_type_command_setup[2]); //N-32 System Command
                                    label16.Text = selected_network;
                                    textBox3.Text += timestamp + " " + "The device is connected to" + " " + selected_network + Environment.NewLine;
                                    groupBox1.Enabled = false;
                                    button12.Enabled = true;
                                }
                                catch (Exception err)
                                {
                                    MessageBox.Show(err.Message);
                                }
                            }
                            if (lorawan_available_int != 0)
                            {
                                textBox3.Text += timestamp + ": " + "There is network detected. Please reset" + " " + Environment.NewLine;
                                button12.Enabled = true;
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }
                }
            }

            if (radioButton4.Checked == true)
            {
                string selected_network = radioButton4.Text;
                string message = "Do you want to add the network?";
                string title = "Confirm";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    //if all information and correct and the process is success
                    //call regarding command to send instruction to the device in order for
                    //further processsing (select network type, add configuration and save it in EEPROM)
                    //FOR 4G Network
                    try
                    {
                        serial_portform2.Write("network");
                        wait(2000);
                        serial_portform2.Write("network");
                        wait(2000);
                        if (dataReceivedStringForm2 == null)
                        {
                            sensor1 = "";
                            if (sensor1 == "")
                            {
                                label16.Text = "Not Avaialble";
                            }
                        }
                        else
                        {
                            fourthgeneration_network_available = dataReceivedForm2;
                            int fourthgeneration_network_available_int = Convert.ToInt32(fourthgeneration_network_available);
                            if (fourthgeneration_network_available_int == 0)
                            {
                                try
                                {
                                    serial_portform2.WriteLine(network_type_command_setup[3]); //N-16 System Command
                                    label16.Text = selected_network;
                                    textBox3.Text += timestamp + " " + "The device is connected to" + " " + selected_network + Environment.NewLine;
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
                                    groupBox1.Enabled = false;
                                    button12.Enabled = true;
                                }
                                catch (Exception err)
                                {
                                    MessageBox.Show(err.Message);
                                }
                            }
                            if (fourthgeneration_network_available_int != 0)
                            {
                                textBox3.Text += timestamp + ": " + "There is network detected. Please reset" + " " + Environment.NewLine;
                                button12.Enabled = true;
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }
                }
            }

            else
            {
                string message_error_msg = "Please complete the selection.";
                string title_error_msg = "Error";
                MessageBox.Show(message_error_msg, title_error_msg);
                button12.Enabled = false;
                groupBox1.Enabled = true;
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
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

            textBox1.Text = String.Empty;
            textBox2.Text = String.Empty;
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label2.Visible = true;
            textBox1.Visible = true;
            label3.Visible = true;
            textBox2.Visible = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label2.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            textBox2.Visible = false;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            label2.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            textBox2.Visible = false;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            label2.Visible = false;
            textBox1.Visible = false;
            label3.Visible = false;
            textBox2.Visible = false;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            comboBox1.Text = String.Empty;
            comboBox1.Text = "Select Sensor Type";
        }

        private void button4_Click(object sender, EventArgs e)                       //Sensor 1 Configuration Setup Submit Button
        {
            DateTime dateTime = DateTime.Now;
            String timestamp = dateTime.ToString();
            if (comboBox1.SelectedItem != null)
            {
                string message = "Do you want to add the sensor?";
                string title = "Confirm";
                label25.Text = "Not Available";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                //labeling changed when sensor added.
                if (result == DialogResult.Yes)
                {
                    string selected_sensor = comboBox1.Text;
                    //define selected index to system command to add sensors
                    //this is a section to add sensor configuration to input1
                    int selected_index_sensor1 = comboBox1.SelectedIndex;
                    try
                    {
                        textBox4.Text += timestamp + " Reading Data, Please Wait..." + Environment.NewLine;
                        serial_portform2.Write("read");
                        wait(2000);
                        serial_portform2.Write("read");
                        wait(2000);
                        if (dataReceivedStringForm2 == null)
                        {
                            sensor1 = "";
                            if (sensor1 == "")
                            {
                                label25.Text = "Not Avaialble";
                            }
                        }
                        else
                        {
                            sensor1 = dataReceivedStringForm2;
                            String[] sen1 = sensor1.Split(',');
                            int sen1a = Convert.ToInt32(sen1[0]);
                            int sen1b = Convert.ToInt32(sen1[1]);
                            if (sen1a == 0 && sen1b == 0)
                            {
                                textBox4.Text += timestamp + " Writing Data, Please Wait..." + Environment.NewLine;
                                switch (selected_index_sensor1)
                                {
                                    case 0:
                                        //Soil pH sensor
                                        serial_portform2.WriteLine(input1_command_first_eight_sensors[0]);
                                        wait(1000); //wait for data to process
                                        label25.Text = selected_sensor;
                                        textBox4.Text += timestamp + " " + selected_sensor + " is added to Input 1." + Environment.NewLine;
                                        button11.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox1.Text = "Select Sensor Type";
                                        break;
                                    case 1:
                                        //Soil Moisture and Temperature sensor
                                        serial_portform2.WriteLine(input1_command_first_eight_sensors[1]);
                                        wait(1000); //wait for data to process
                                        label25.Text = selected_sensor;
                                        textBox4.Text += timestamp + " " + selected_sensor + " is added to Input 1." + Environment.NewLine;
                                        button11.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox1.Text = "Select Sensor Type";
                                        break;
                                    case 2:
                                        //Soil Electrical Conductivity sensor
                                        serial_portform2.WriteLine(input1_command_first_eight_sensors[2]);
                                        label25.Text = selected_sensor;
                                        textBox4.Text += timestamp + " " + selected_sensor + " is added to Input 1." + Environment.NewLine;
                                        button11.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox1.Text = "Select Sensor Type";
                                        break;
                                    case 3:
                                        //Soil Salinity sensor
                                        serial_portform2.WriteLine(input1_command_first_eight_sensors[3]);
                                        wait(1000); //wait for data to process
                                        label25.Text = selected_sensor;
                                        textBox4.Text += timestamp + " " + selected_sensor + " is added to Input 1." + Environment.NewLine;
                                        button11.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox1.Text = "Select Sensor Type";
                                        break;
                                    case 4:
                                        //Soil Temperature,Moisture, Salinity and EC sensor
                                        serial_portform2.WriteLine(input1_command_first_eight_sensors[4]);
                                        wait(1000); //wait for data to process
                                        label25.Text = selected_sensor;
                                        textBox4.Text += timestamp + " " + selected_sensor + " is added to Input 1." + Environment.NewLine;
                                        button11.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox1.Text = "Select Sensor Type";
                                        break;
                                    case 5:
                                        //Soil NPK (Nitrogen-Phosphorous Potassium) sensor
                                        serial_portform2.WriteLine(input1_command_first_eight_sensors[5]);
                                        wait(1000); //wait for data to process
                                        label25.Text = selected_sensor;
                                        textBox4.Text += timestamp + " " + selected_sensor + " is added to Input 1." + Environment.NewLine;
                                        button11.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox1.Text = "Select Sensor Type";
                                        break;
                                    case 6:
                                        //Water pH Sensor 
                                        serial_portform2.WriteLine(input1_command_first_eight_sensors[6]);
                                        wait(1000); //wait for data to process
                                        label25.Text = selected_sensor;
                                        textBox4.Text += timestamp + " " + selected_sensor + " is added to Input 1." + Environment.NewLine;
                                        button11.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox1.Text = "Select Sensor Type";
                                        break;
                                    case 7:
                                        //Water Dissolved Oxygen Sensor
                                        serial_portform2.WriteLine(input1_command_first_eight_sensors[7]);
                                        wait(1000); //wait for data to process
                                        label25.Text = selected_sensor;
                                        textBox4.Text += timestamp + " " + selected_sensor + " is added to Input 1." + Environment.NewLine;
                                        button11.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox1.Text = "Select Sensor Type";
                                        break;
                                    case 8:
                                        //Water Ammonia Sensor
                                        serial_portform2.WriteLine(input1_command_first_eight_sensors[0]);
                                        wait(1000); //wait for data to process
                                        label25.Text = selected_sensor;
                                        textBox4.Text += timestamp + " " + selected_sensor + " is added to Input 1." + Environment.NewLine;
                                        button11.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox1.Text = "Select Sensor Type";
                                        break;
                                    case 9:
                                        //Water Turbidity Sensor
                                        serial_portform2.WriteLine(input1_command_second_eight_sensors[1]);
                                        wait(1000); //wait for data to process
                                        label25.Text = selected_sensor;
                                        textBox4.Text += timestamp + " " + selected_sensor + " is added to Input 1." + Environment.NewLine;
                                        button11.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox1.Text = "Select Sensor Type";
                                        break;
                                    case 10:
                                        //Water Salinity Sensor
                                        serial_portform2.WriteLine(input1_command_second_eight_sensors[2]);
                                        wait(1000); //wait for data to process
                                        label25.Text = selected_sensor;
                                        textBox4.Text += timestamp + " " + selected_sensor + " is added to Input 1." + Environment.NewLine;
                                        button11.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox1.Text = "Select Sensor Type";
                                        break;
                                    case 11:
                                        //Ultrasonic Level Meter
                                        serial_portform2.WriteLine(input1_command_second_eight_sensors[3]);
                                        wait(1000); //wait for data to process
                                        label25.Text = selected_sensor;
                                        textBox4.Text += timestamp + " " + selected_sensor + " is added to Input 1." + Environment.NewLine;
                                        button11.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox1.Text = "Select Sensor Type";
                                        break;
                                    case 12:
                                        //Pneumatic Level Meter
                                        serial_portform2.WriteLine(input1_command_second_eight_sensors[4]);
                                        wait(1000); //wait for data to process
                                        label25.Text = selected_sensor;
                                        textBox4.Text += timestamp + " " + selected_sensor + " is added to Input 1." + Environment.NewLine;
                                        button11.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox1.Text = "Select Sensor Type";
                                        break;
                                    case 13:
                                        //Radar Level Meter
                                        serial_portform2.WriteLine(input1_command_second_eight_sensors[5]);
                                        wait(1000); //wait for data to process
                                        label25.Text = selected_sensor;
                                        textBox4.Text += timestamp + " " + selected_sensor + " is added to Input 1." + Environment.NewLine;
                                        button11.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox1.Text = "Select Sensor Type";
                                        break;
                                    default:
                                        textBox4.Text += timestamp + " " + "Error Adding Sensor Input 1." + Environment.NewLine;
                                        comboBox1.Text = "Select Sensor Type";
                                        break;

                                }
                            }
                            else
                            {
                                textBox4.Text += timestamp + " " + "Input 1 Already Have Sensor..." + Environment.NewLine;
                                wait(100);
                                textBox4.Text += timestamp + " " + "Please Reset" + Environment.NewLine;
                                button11.Enabled = true;
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }
                }

            }
            else
            {
                string message_error_msg = "Complete the Option to Add Sensor";
                string title_error_msg = "Error";
                MessageBox.Show(message_error_msg, title_error_msg);
                button11.Enabled = false;
                groupBox2.Enabled = true;
                comboBox1.Text = "Select Sensor Type";
            }



        }

        private void button6_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            String timestamp = dateTime.ToString();
            if (comboBox4.SelectedItem != null)
            {
                string message = "Do you want to add the sensor?";
                string title = "Confirm";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    string selected_sensor = comboBox4.Text;
                    //define selected index to system command to add sensors
                    //this is a section to add sensor configuration to input3
                    int selected_index_sensor2 = comboBox4.SelectedIndex;
                    try
                    {
                        textBox5.Text += timestamp + " Reading Data, Please Wait..." + Environment.NewLine;
                        serial_portform2.Write("read");
                        wait(2000);
                        serial_portform2.Write("read");
                        wait(2000);
                        if (dataReceivedStringForm2 == null)
                        {
                            sensor2 = "";
                            if (sensor2 == "")
                            {
                                label21.Text = "Not Avaialble";
                            }
                        }
                        else
                        {
                            sensor2 = dataReceivedStringForm2;
                            String[] sen2 = sensor2.Split(',');
                            int sen2a = Convert.ToInt32(sen2[2]);
                            int sen2b = Convert.ToInt32(sen2[3]);
                            if (sen2a == 0 && sen2b == 0)
                            {
                                textBox5.Text += timestamp + " Writing Data, Please Wait..." + Environment.NewLine;
                                switch (selected_index_sensor2)
                                {
                                    case 0:
                                        //Soil pH sensor
                                        serial_portform2.WriteLine(input2_command_first_eight_sensors[0]);
                                        wait(1000); //wait for data to process
                                        label21.Text = selected_sensor;
                                        textBox5.Text += timestamp + " " + selected_sensor + " is added to Input 2." + Environment.NewLine;
                                        button10.Enabled = true;
                                        groupBox5.Enabled = false;
                                        comboBox4.Text = "Select Sensor Type";
                                        break;
                                    case 1:
                                        //Soil Moisture and Temperature sensor
                                        serial_portform2.WriteLine(input2_command_first_eight_sensors[1]);
                                        wait(1000); //wait for data to process
                                        label21.Text = selected_sensor;
                                        textBox5.Text += timestamp + " " + selected_sensor + " is added to Input 2." + Environment.NewLine;
                                        button10.Enabled = true;
                                        groupBox5.Enabled = false;
                                        comboBox4.Text = "Select Sensor Type";
                                        break;
                                    case 2:
                                        //Soil Electrical Conductivity sensor
                                        serial_portform2.WriteLine(input2_command_first_eight_sensors[2]);
                                        wait(1000); //wait for data to process
                                        label21.Text = selected_sensor;
                                        textBox5.Text += timestamp + " " + selected_sensor + " is added to Input 2." + Environment.NewLine;
                                        button10.Enabled = true;
                                        groupBox5.Enabled = false;
                                        comboBox4.Text = "Select Sensor Type";
                                        break;
                                    case 3:
                                        //Soil Salinity sensor
                                        serial_portform2.WriteLine(input2_command_first_eight_sensors[3]);
                                        wait(1000); //wait for data to process
                                        label21.Text = selected_sensor;
                                        textBox5.Text += timestamp + " " + selected_sensor + " is added to Input 2." + Environment.NewLine;
                                        button10.Enabled = true;
                                        groupBox5.Enabled = false;
                                        comboBox4.Text = "Select Sensor Type";
                                        break;
                                    case 4:
                                        //Soil Temperature,Moisture, Salinity and EC sensor
                                        serial_portform2.WriteLine(input2_command_first_eight_sensors[4]);
                                        wait(1000); //wait for data to process
                                        label21.Text = selected_sensor;
                                        textBox5.Text += timestamp + " " + selected_sensor + " is added to Input 2." + Environment.NewLine;
                                        button10.Enabled = true;
                                        groupBox5.Enabled = false;
                                        comboBox4.Text = "Select Sensor Type";
                                        break;
                                    case 5:
                                        //Soil NPK (Nitrogen-Phosphorous Potassium) sensor
                                        serial_portform2.WriteLine(input2_command_first_eight_sensors[5]);
                                        wait(1000); //wait for data to process
                                        label21.Text = selected_sensor;
                                        textBox5.Text += timestamp + " " + selected_sensor + " is added to Input 2." + Environment.NewLine;
                                        button10.Enabled = true;
                                        groupBox5.Enabled = false;
                                        comboBox4.Text = "Select Sensor Type";
                                        break;
                                    case 6:
                                        //Water pH Sensor 
                                        serial_portform2.WriteLine(input2_command_first_eight_sensors[6]);
                                        wait(1000); //wait for data to process
                                        label21.Text = selected_sensor;
                                        textBox5.Text += timestamp + " " + selected_sensor + " is added to Input 2." + Environment.NewLine;
                                        button10.Enabled = true;
                                        groupBox5.Enabled = false;
                                        comboBox4.Text = "Select Sensor Type";
                                        break;
                                    case 7:
                                        //Water Dissolved Oxygen Sensor
                                        serial_portform2.WriteLine(input2_command_first_eight_sensors[7]);
                                        wait(1000); //wait for data to process
                                        label21.Text = selected_sensor;
                                        textBox5.Text += timestamp + " " + selected_sensor + " is added to Input 2." + Environment.NewLine;
                                        button10.Enabled = true;
                                        groupBox5.Enabled = false;
                                        comboBox4.Text = "Select Sensor Type";
                                        break;
                                    case 8:
                                        //Water Ammonia Sensor
                                        serial_portform2.WriteLine(input2_command_second_eight_sensors[0]);
                                        wait(1000); //wait for data to process
                                        label21.Text = selected_sensor;
                                        textBox5.Text += timestamp + " " + selected_sensor + " is added to Input 2." + Environment.NewLine;
                                        button10.Enabled = true;
                                        groupBox5.Enabled = false;
                                        comboBox4.Text = "Select Sensor Type";
                                        break;
                                    case 9:
                                        //Water Turbidity Sensor
                                        serial_portform2.WriteLine(input2_command_second_eight_sensors[1]);
                                        wait(1000); //wait for data to process
                                        label21.Text = selected_sensor;
                                        textBox5.Text += timestamp + " " + selected_sensor + " is added to Input 2." + Environment.NewLine;
                                        button10.Enabled = true;
                                        groupBox5.Enabled = false;
                                        comboBox4.Text = "Select Sensor Type";
                                        break;
                                    case 10:
                                        //Water Salinity Sensor
                                        serial_portform2.WriteLine(input2_command_second_eight_sensors[2]);
                                        wait(1000); //wait for data to process
                                        label21.Text = selected_sensor;
                                        textBox5.Text += timestamp + " " + selected_sensor + " is added to Input 2." + Environment.NewLine;
                                        button10.Enabled = true;
                                        groupBox5.Enabled = false;
                                        comboBox4.Text = "Select Sensor Type";
                                        break;
                                    case 11:
                                        //Ultrasonic Level Meter
                                        serial_portform2.WriteLine(input2_command_second_eight_sensors[3]);
                                        wait(1000); //wait for data to process
                                        label21.Text = selected_sensor;
                                        textBox5.Text += timestamp + " " + selected_sensor + " is added to Input 2." + Environment.NewLine;
                                        button10.Enabled = true; ;
                                        groupBox5.Enabled = false;
                                        comboBox4.Text = "Select Sensor Type";
                                        break;
                                    case 12:
                                        //Pneumatic Level Meter
                                        serial_portform2.WriteLine(input2_command_second_eight_sensors[4]);
                                        wait(1000); //wait for data to process
                                        label21.Text = selected_sensor;
                                        textBox5.Text += timestamp + " " + selected_sensor + " is added to Input 2." + Environment.NewLine;
                                        button10.Enabled = true;
                                        groupBox5.Enabled = false;
                                        comboBox4.Text = "Select Sensor Type";
                                        break;
                                    case 13:
                                        //Radar Level Meter
                                        serial_portform2.WriteLine(input2_command_second_eight_sensors[5]);
                                        wait(1000); //wait for data to process
                                        label21.Text = selected_sensor;
                                        textBox5.Text += timestamp + " " + selected_sensor + " is added to Input 2." + Environment.NewLine;
                                        button10.Enabled = true;
                                        groupBox5.Enabled = false;
                                        comboBox4.Text = "Select Sensor Type";
                                        break;
                                    default:
                                        textBox5.Text += timestamp + " " + "Error Adding Sensor Input 2." + Environment.NewLine;
                                        comboBox4.Text = "Select Sensor Type";
                                        break;

                                }
                            }
                            else
                            {
                                textBox5.Text += timestamp + " " + "Input 2 Already Have Sensor..." + Environment.NewLine;
                                wait(100);
                                textBox5.Text += timestamp + " " + "Please Reset" + Environment.NewLine;
                                button10.Enabled = true;
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }


                }

            }
            else
            {
                string message_error_msg = "Complete the Option to Add Sensor";
                string title_error_msg = "Error";
                MessageBox.Show(message_error_msg, title_error_msg);
                button10.Enabled = false;
                groupBox5.Enabled = true;
                comboBox4.Text = "Select Sensor Type";
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            comboBox4.Text = String.Empty;
            comboBox4.Text = "Select Sensor Type";
        }

        private void button8_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            String timestamp = dateTime.ToString();
            if (comboBox6.SelectedItem != null)
            {
                string message = "Do you want to add the sensor?";
                string title = "Confirm";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    string selected_sensor = comboBox6.Text;


                    //define selected index to system command to add sensors
                    //this is a section to add sensor configuration to input1
                    int selected_index = comboBox6.SelectedIndex;
                    try
                    {
                        textBox6.Text += timestamp + " Reading Data, Please Wait..." + Environment.NewLine;
                        serial_portform2.Write("read");
                        wait(2000);
                        serial_portform2.Write("read");
                        wait(2000);
                        if (dataReceivedStringForm2 == null)
                        {
                            sensor3 = "";
                            if (sensor3 == "")
                            {
                                label24.Text = "Not Avaialble";
                            }
                        }
                        else
                        {
                            sensor3 = dataReceivedStringForm2;
                            String[] sen3 = sensor3.Split(',');
                            int sen3a = Convert.ToInt16(sen3[4]);
                            int sen3b = Convert.ToInt16(sen3[5]);
                            if (sen3a == 0 && sen3b == 0)
                            {
                                textBox6.Text += timestamp + " Writing Data, Please Wait..." + Environment.NewLine;
                                switch (selected_index)
                                {
                                    case 0:
                                        //Soil pH sensor
                                        serial_portform2.WriteLine(input3_command_first_eight_sensors[0]);
                                        wait(1000); //wait for data to process
                                        label24.Text = selected_sensor;
                                        textBox6.Text += timestamp + " " + selected_sensor + " is added to Input 3." + Environment.NewLine;
                                        button9.Enabled = true;
                                        groupBox6.Enabled = true;
                                        comboBox6.Text = "Select Sensor Type";
                                        break;
                                    case 1:
                                        //Soil Moisture and Temperature sensor
                                        serial_portform2.WriteLine(input3_command_first_eight_sensors[1]);
                                        wait(1000); //wait for data to process
                                        label24.Text = selected_sensor;
                                        textBox6.Text += timestamp + " " + selected_sensor + " is added to Input 3." + Environment.NewLine;
                                        button9.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox6.Text = "Select Sensor Type";
                                        break;
                                    case 2:
                                        //Soil Electrical Conductivity sensor
                                        serial_portform2.WriteLine(input3_command_first_eight_sensors[2]);
                                        wait(1000); //wait for data to process
                                        label24.Text = selected_sensor;
                                        textBox6.Text += timestamp + " " + selected_sensor + " is added to Input 3." + Environment.NewLine;
                                        button9.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox6.Text = "Select Sensor Type";
                                        break;
                                    case 3:
                                        //Soil Salinity sensor
                                        serial_portform2.WriteLine(input3_command_first_eight_sensors[3]);
                                        wait(1000); //wait for data to process
                                        label24.Text = selected_sensor;
                                        textBox6.Text += timestamp + " " + selected_sensor + " is added to Input 3." + Environment.NewLine;
                                        button9.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox6.Text = "Select Sensor Type";
                                        break;
                                    case 4:
                                        //Soil Temperature,Moisture, Salinity and EC sensor
                                        serial_portform2.WriteLine(input3_command_first_eight_sensors[4]);
                                        wait(1000); //wait for data to process
                                        label24.Text = selected_sensor;
                                        textBox6.Text += timestamp + " " + selected_sensor + " is added to Input 3." + Environment.NewLine;
                                        button9.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox6.Text = "Select Sensor Type";
                                        break;
                                    case 5:
                                        //Soil NPK (Nitrogen-Phosphorous Potassium) sensor
                                        serial_portform2.WriteLine(input3_command_first_eight_sensors[5]);
                                        wait(1000); //wait for data to process
                                        label24.Text = selected_sensor;
                                        textBox6.Text += timestamp + " " + selected_sensor + " is added to Input 3." + Environment.NewLine;
                                        button9.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox6.Text = "Select Sensor Type";
                                        break;
                                    case 6:
                                        //Water pH Sensor 
                                        serial_portform2.WriteLine(input3_command_first_eight_sensors[6]);
                                        wait(1000); //wait for data to process
                                        label24.Text = selected_sensor;
                                        textBox6.Text += timestamp + " " + selected_sensor + " is added to Input 3." + Environment.NewLine;
                                        button9.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox6.Text = "Select Sensor Type";
                                        break;
                                    case 7:
                                        //Water Dissolved Oxygen Sensor
                                        serial_portform2.WriteLine(input3_command_first_eight_sensors[7]);
                                        wait(1000); //wait for data to process
                                        label24.Text = selected_sensor;
                                        textBox6.Text += timestamp + " " + selected_sensor + " is added to Input 3." + Environment.NewLine;
                                        button9.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox6.Text = "Select Sensor Type";
                                        break;
                                    case 8:
                                        //Water Ammonia Sensor
                                        serial_portform2.WriteLine(input3_command_second_eight_sensors[0]);
                                        wait(1000); //wait for data to process
                                        label24.Text = selected_sensor;
                                        textBox6.Text += timestamp + " " + selected_sensor + " is added to Input 3." + Environment.NewLine;
                                        button9.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox6.Text = "Select Sensor Type";
                                        break;
                                    case 9:
                                        //Water Turbidity Sensor
                                        serial_portform2.WriteLine(input3_command_second_eight_sensors[1]);
                                        wait(1000); //wait for data to process
                                        label24.Text = selected_sensor;
                                        textBox6.Text += timestamp + " " + selected_sensor + " is added to Input 3." + Environment.NewLine;
                                        button9.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox6.Text = "Select Sensor Type";
                                        break;
                                    case 10:
                                        //Water Salinity Sensor
                                        serial_portform2.WriteLine(input3_command_second_eight_sensors[2]);
                                        wait(1000); //wait for data to process
                                        label24.Text = selected_sensor;
                                        textBox6.Text += timestamp + " " + selected_sensor + " is added to Input 3." + Environment.NewLine;
                                        button9.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox6.Text = "Select Sensor Type";
                                        break;
                                    case 11:
                                        //Ultrasonic Level Meter
                                        serial_portform2.WriteLine(input3_command_second_eight_sensors[3]);
                                        wait(1000); //wait for data to process
                                        label24.Text = selected_sensor;
                                        textBox6.Text += timestamp + " " + selected_sensor + " is added to Input 3." + Environment.NewLine;
                                        button9.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox6.Text = "Select Sensor Type";
                                        break;
                                    case 12:
                                        //Pneumatic Level Meter
                                        serial_portform2.WriteLine(input3_command_second_eight_sensors[4]);
                                        wait(1000); //wait for data to process
                                        label24.Text = selected_sensor;
                                        textBox6.Text += timestamp + " " + selected_sensor + " is added to Input 3." + Environment.NewLine;
                                        button9.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox6.Text = "Select Sensor Type";
                                        break;
                                    case 13:
                                        //Radar Level Meter
                                        serial_portform2.WriteLine(input3_command_second_eight_sensors[5]);
                                        wait(1000); //wait for data to process
                                        label24.Text = selected_sensor;
                                        textBox6.Text += timestamp + " " + selected_sensor + " is added to Input 3." + Environment.NewLine;
                                        button9.Enabled = true;
                                        groupBox6.Enabled = false;
                                        comboBox6.Text = "Select Sensor Type";
                                        break;
                                    default:
                                        textBox6.Text += timestamp + " " + "Error Adding Sensor Input 1." + Environment.NewLine;
                                        comboBox6.Text = "Select Sensor Type";
                                        break;

                                }
                            }
                            else
                            {
                                textBox6.Text += timestamp + " " + "Input 3 Already Have Sensor..." + Environment.NewLine;
                                wait(100);
                                textBox6.Text += timestamp + " " + "Please Reset" + Environment.NewLine;
                                button9.Enabled = true;
                            }
                        }
                    }
                    catch (Exception err)
                    {
                        MessageBox.Show(err.Message);
                    }

                }


            }
            else
            {
                string message_error_msg = "Complete the Option to Add Sensor";
                string title_error_msg = "Error";
                MessageBox.Show(message_error_msg, title_error_msg);
                button9.Enabled = false;
                groupBox6.Enabled = true;
                comboBox6.Text = "Select Sensor Type";
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            comboBox6.Text = String.Empty;
            comboBox6.Text = "Select Sensor Type";
        }

        private void button12_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            String timestamp = dateTime.ToString();
            //network reset button
            string message = "Do you sure want to reset network?";
            string title = "Confirm";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                try
                {
                    serial_portform2.Write("resetNet");
                    wait(1000);
                    label16.Text = "Not Available";
                    label17.Text = "Not Available";
                    textBox3.Text += timestamp + " " + "The network is reset." + Environment.NewLine;
                    button12.Enabled = false;
                    groupBox1.Enabled = true;
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }

            }

        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (!radioButton1.Checked && !radioButton2.Checked && !radioButton3.Checked && !radioButton4.Checked)
            {
                label2.Visible = false;
                textBox1.Visible = false;
                label3.Visible = false;
                textBox2.Visible = false;
            }

            if (label16.Text == "Not Available")
            {
                button12.Enabled = false;
            }
            else
            {
                button12.Enabled = true;
            }

            if (label25.Text == "Not Available")
            {
                button11.Enabled = false;
            }
            else
            {
                button11.Enabled = true;
            }
            if (label24.Text == "Not Available")
            {
                button9.Enabled = false;
            }
            else
            {
                button9.Enabled = true;
            }
            if (label21.Text == "Not Available")
            {
                button10.Enabled = false;
            }
            else
            {
                button10.Enabled = true;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            String timestamp = dateTime.ToString();
            //sensor input reset button
            string message = "Do you sure want to reset sensor?";
            string title = "Confirm";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                serial_portform2.WriteLine("reset1");
                label25.Text = "Not Available";
                textBox4.Text += timestamp + " " + "Input 1 Sensor is reset." + Environment.NewLine;
                groupBox2.Enabled = true;
                button11.Enabled = false;
            }


        }

        private void button10_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            String timestamp = dateTime.ToString();
            //sensor input reset button
            string message = "Do you sure want to reset sensor?";
            string title = "Confirm";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                serial_portform2.WriteLine("reset2");
                textBox5.Text += timestamp + " " + "Input 2 Sensor is reseting, Please Wait..." + Environment.NewLine;
                wait(1000);
                label21.Text = "Not Available";
                textBox5.Text += timestamp + " " + "Input 2 Sensor is reset." + Environment.NewLine;
                groupBox5.Enabled = true;
                button10.Enabled = false;
            }


        }

        private void button9_Click(object sender, EventArgs e)
        {
            DateTime dateTime = DateTime.Now;
            String timestamp = dateTime.ToString();
            //sensor input reset button
            string message = "Do you sure want to reset sensor?";
            string title = "Confirm";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                serial_portform2.WriteLine("reset3");
                label24.Text = "Not Available";
                textBox6.Text += timestamp + " " + "Input 2 Sensor is reseting, Please Wait..." + Environment.NewLine;
                wait(1000);
                textBox6.Text += timestamp + " " + "Input 3 Sensor is reset." + Environment.NewLine;
                groupBox6.Enabled = true;
                button9.Enabled = false;
            }
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (serial_portform2.IsOpen)
            {
                try
                {
                    DateTime dateTime = DateTime.Now;
                    String timestamp = dateTime.ToString();
                    textBox3.Text += timestamp + " Reading Data, Please Wait..." + Environment.NewLine;
                    serial_portform2.WriteLine("network");
                    wait(3000);
                    serial_portform2.WriteLine("network");
                    wait(3000);
                    if (dataReceivedStringForm2 == null)
                    {
                        sensor_id1 = " ";
                        if (sensor_id1 == " ")
                        {
                            label16.Text = "Not Available";
                            textBox3.Text += timestamp + " Reading Failed" + Environment.NewLine;
                        }
                    }
                    else
                    {
                        int networks = Convert.ToInt32(dataReceivedStringForm2);
                        switch (networks)
                        {
                            case 1:
                                label16.Text = "Wi-Fi";
                                textBox3.Text += timestamp + ": Data Loaded..." + Environment.NewLine;
                                textBox3.Text += timestamp + ": Connected to: " + label16.Text + Environment.NewLine;
                                break;
                            case 2:
                                label16.Text = "LoraWAN";
                                textBox3.Text += timestamp + ": Data Loaded..." + Environment.NewLine;
                                textBox3.Text += timestamp + ": Connected to: " + label16.Text + Environment.NewLine;
                                break;
                            case 3:
                                label16.Text = "Sigfox";
                                textBox3.Text += timestamp + ": Data Loaded..." + Environment.NewLine;
                                textBox3.Text += timestamp + ": Connected to: " + label16.Text + Environment.NewLine;
                                break;
                            case 4:
                                label16.Text = "4G Network";
                                textBox3.Text += timestamp + ": Data Loaded..." + Environment.NewLine;
                                textBox3.Text += timestamp + ": Connected to: " + label16.Text + Environment.NewLine;
                                break;
                            default:
                                label16.Text = "Not Available";
                                textBox3.Text += timestamp + ": Not Connected To Any COM Module " + Environment.NewLine;
                                break;
                        }
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }

            }
        }


        private void button15_Click(object sender, EventArgs e)
        {
            if(serial_portform2.IsOpen)
            {
                try
                {
                    DateTime dateTime = DateTime.Now;
                    String timestamp = dateTime.ToString();
                    textBox5.Text += timestamp + ": Reading Data, Please Wait..." + Environment.NewLine;
                    serial_portform2.WriteLine("read");
                    wait(3000);
                    serial_portform2.WriteLine("read");
                    wait(3000);
                    if (dataReceivedStringForm2 == null)
                    {
                        sensor_id2 = " ";
                        if (sensor_id2 == " ")
                        {
                            label24.Text = "Not Available";
                            textBox5.Text += timestamp + ": Reading Failed" + Environment.NewLine;
                        }
                    }
                    else
                    {
                        textBox5.Text += timestamp + ": Data Loaded..." + Environment.NewLine;
                        sensor_id2 = dataReceivedStringForm2;
                        String[] sensor2 = sensor_id2.Split(',');
                        if (sensor2[3] == "0")
                        {
                            int input2A = Convert.ToInt32(sensor2[2]);
                            switch (input2A)
                            {
                                case 128:
                                    label21.Text = "Soil pH";
                                    button10.Enabled = true;
                                    textBox5.Text += timestamp + ": " + "Connecting to: " + label21.Text + " Sensor" + Environment.NewLine;
                                    break;
                                case 64:
                                    label21.Text = "Soil Moisture and Temperature";
                                    button10.Enabled = true;
                                    textBox5.Text += timestamp + ": " + "Connecting to: " + label21.Text + " Sensor" + Environment.NewLine;
                                    break;
                                case 32:
                                    label21.Text = "Soil Electrical Conductivity";
                                    button10.Enabled = true;
                                    textBox5.Text += timestamp + ": " + "Connecting to: " + label21.Text + " Sensor" + Environment.NewLine;
                                    break;
                                case 16:
                                    label21.Text = "Soil Salinity";
                                    button10.Enabled = true;
                                    textBox5.Text += timestamp + ": " + "Connecting to: " + label21.Text + " Sensor" + Environment.NewLine;
                                    break;
                                case 8:
                                    label21.Text = "Soil Temperature, Moisture, Salinity and EC";
                                    button10.Enabled = true;
                                    textBox5.Text += timestamp + ": " + "Connecting to: " + label21.Text + " Sensor" + Environment.NewLine;
                                    break;
                                case 4:
                                    label21.Text = "Soil NPK (Nitrogen-PhosphorousPotassium)";
                                    button10.Enabled = true;
                                    textBox5.Text += timestamp + ": " + "Connecting to: " + label21.Text + " Sensor" + Environment.NewLine;
                                    break;
                                case 2:
                                    label21.Text = "Water pH Sensor";
                                    button10.Enabled = true;
                                    textBox5.Text += timestamp + ": " + "Connecting to: " + label21.Text + " Sensor" + Environment.NewLine;
                                    break;
                                case 1:
                                    label21.Text = "Water Dissolved Oxygen Sensor";
                                    button10.Enabled = true;
                                    textBox5.Text += timestamp + ": " + "Connecting to: " + label21.Text + " Sensor" + Environment.NewLine;
                                    break;
                            }
                        }
                        if (sensor2[2] == "0")
                        {
                            int input2B = Convert.ToInt32(sensor2[3]);
                            switch (input2B)
                            {
                                case 128:
                                    label21.Text = "Water Ammonia Sensor";
                                    button10.Enabled = true;
                                    textBox5.Text += timestamp + ": " + "Connecting to: " + label21.Text + " Sensor" + Environment.NewLine;
                                    break;
                                case 64:
                                    label21.Text = "Water Turbidity Sensor";
                                    button10.Enabled = true;
                                    textBox5.Text += timestamp + ": " + "Connecting to: " + label21.Text + " Sensor" + Environment.NewLine;
                                    break;
                                case 32:
                                    label21.Text = "Water Salinity Sensor";
                                    button10.Enabled = true;
                                    textBox5.Text += timestamp + ": " + "Connecting to: " + label21.Text + " Sensor" + Environment.NewLine;
                                    break;
                                case 16:
                                    label21.Text = "Ultrasonic Level Meter";
                                    button10.Enabled = true;
                                    textBox5.Text += timestamp + ": " + "Connecting to: " + label21.Text + " Sensor" + Environment.NewLine;
                                    break;
                                case 8:
                                    label21.Text = "Pneumatic Level Meter";
                                    button10.Enabled = true;
                                    textBox5.Text += timestamp + ": " + "Connecting to: " + label21.Text + " Sensor" + Environment.NewLine;
                                    break;
                                case 4:
                                    label21.Text = "Radar Level Meter";
                                    button10.Enabled = true;
                                    textBox5.Text += timestamp + ": " + "Connecting to: " + label21.Text + " Sensor" + Environment.NewLine;
                                    break;

                                default:
                                    label21.Text = "Not Available";
                                    button10.Enabled = false;
                                    textBox5.Text += timestamp + ": " + " Sensor " + label21.Text + Environment.NewLine;
                                    break;

                            }
                        }
                        if (sensor2[3] == "0" && sensor2[2] == "0")
                        {
                            label21.Text = "Not Available";
                            button10.Enabled = false;
                            textBox5.Text += timestamp + " " + "No Sensor in Input 2" + Environment.NewLine;
                        }
                        label20.Text = "INACTIVE";
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }

        }

        private void button14_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                String timestamp = dateTime.ToString();
                textBox4.Text += timestamp + " Reading Data, Please Wait..." + Environment.NewLine;
                serial_portform2.WriteLine("read");
                wait(3000);
                serial_portform2.WriteLine("read");
                wait(3000);
                if (dataReceivedStringForm2 == null)
                {
                    sensor_id1 = " ";
                    if (sensor_id1 == " ")
                    {
                        label25.Text = "Not Available";
                        textBox4.Text += timestamp + " Reading Failed" + Environment.NewLine;
                    }
                }
                else
                {
                    sensor_id1 = dataReceivedStringForm2;
                    String[] sensor1 = sensor_id1.Split(',');
                    if (sensor1[1] == "0")
                    {
                        int input1A = Convert.ToInt32(sensor1[0]);
                        switch (input1A)
                        {
                            case 128:
                                label25.Text = "Soil pH";
                                button11.Enabled = true;
                                textBox4.Text += timestamp + ": " + "Connecting to: " + label25.Text + " Sensor" + Environment.NewLine;
                                break;
                            case 64:
                                label25.Text = "Soil Moisture and Temperature";
                                button11.Enabled = true;
                                textBox4.Text += timestamp + ": " + "Connecting to: " + label25.Text + " Sensor" + Environment.NewLine;
                                break;
                            case 32:
                                label25.Text = "Soil Electrical Conductivity";
                                button11.Enabled = true;
                                textBox4.Text += timestamp + ": " + "Connecting to: " + label25.Text + " Sensor" + Environment.NewLine;
                                break;
                            case 16:
                                label25.Text = "Soil Salinity";
                                button11.Enabled = true;
                                textBox4.Text += timestamp + ": " + "Connecting to: " + label25.Text + " Sensor" + Environment.NewLine;
                                break;
                            case 8:
                                label25.Text = "Soil Temperature, Moisture, Salinity and EC";
                                button11.Enabled = true;
                                textBox4.Text += timestamp + ": " + "Connecting to: " + label25.Text + " Sensor" + Environment.NewLine;
                                break;
                            case 4:
                                label25.Text = "Soil NPK (Nitrogen-PhosphorousPotassium)";
                                button11.Enabled = true;
                                textBox4.Text += timestamp + ": " + "Connecting to: " + label25.Text + " Sensor" + Environment.NewLine;
                                break;
                            case 2:
                                label25.Text = "Water pH Sensor";
                                button11.Enabled = true;
                                textBox4.Text += timestamp + ": " + "Connecting to: " + label25.Text + " Sensor" + Environment.NewLine;
                                break;
                            case 1:
                                label25.Text = "Water Dissolved Oxygen Sensor";
                                button11.Enabled = true;
                                textBox4.Text += timestamp + ": " + "Connecting to: " + label25.Text + " Sensor" + Environment.NewLine;
                                break;
                        }
                    }
                    if (sensor1[0] == "0")
                    {
                        int input1B = Convert.ToInt32(sensor1[1]);
                        switch (input1B)
                        {
                            case 128:
                                label25.Text = "Water Ammonia Sensor";
                                button11.Enabled = true;
                                textBox4.Text += timestamp + ": " + "Connecting to: " + label25.Text + " Sensor" + Environment.NewLine;
                                break;
                            case 64:
                                label25.Text = "Water Turbidity Sensor";
                                button11.Enabled = true;
                                textBox4.Text += timestamp + ": " + "Connecting to: " + label25.Text + " Sensor" + Environment.NewLine;
                                break;
                            case 32:
                                label25.Text = "Water Salinity Sensor";
                                button11.Enabled = true;
                                textBox4.Text += timestamp + ": " + "Connecting to: " + label25.Text + " Sensor" + Environment.NewLine;
                                break;
                            case 16:
                                label25.Text = "Ultrasonic Level Meter";
                                button11.Enabled = true;
                                textBox4.Text += timestamp + ": " + "Connecting to: " + label25.Text + " Sensor" + Environment.NewLine;
                                break;
                            case 8:
                                label25.Text = "Pneumatic Level Meter";
                                button11.Enabled = true;
                                textBox4.Text += timestamp + ": " + "Connecting to: " + label25.Text + " Sensor" + Environment.NewLine;
                                break;
                            case 4:
                                label25.Text = "Radar Level Meter";
                                button11.Enabled = true;
                                textBox4.Text += timestamp + ": " + "Connecting to: " + label25.Text + " Sensor" + Environment.NewLine;
                                break;

                            default:
                                label25.Text = "Not Available";
                                button11.Enabled = false;
                                textBox4.Text += timestamp + ": " + "Sensor " + label25.Text + Environment.NewLine;
                                break;

                        }
                    }
                    if (sensor1[0] == "0" && sensor1[1] == "0")
                    {
                        label25.Text = "Not Available";
                        button11.Enabled = false;
                        textBox4.Text += timestamp + " " + "No Sensor in Input 1" + Environment.NewLine;
                    }
                    label26.Text = "INACTIVE";
                    button11.Enabled = true;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
            }

        private void button16_Click(object sender, EventArgs e)
        {
            if (serial_portform2.IsOpen)
            {//button to read Input3 configuration from device
                try
                {
                    DateTime dateTime = DateTime.Now;
                    String timestamp = dateTime.ToString();
                    textBox6.Text += timestamp + " Reading Data, Please Wait..." + Environment.NewLine;
                    serial_portform2.WriteLine("read");
                    wait(3000);
                    serial_portform2.WriteLine("read");
                    wait(3000);
                    if (dataReceivedStringForm2 == null)
                    {
                        sensor_id3 = "";
                        if (sensor_id3 == "")
                        {
                            label24.Text = "Not Available";
                            textBox6.Text += timestamp + " Reading Failed" + Environment.NewLine;
                        }
                    }
                    else
                    {

                        sensor_id3 = dataReceivedStringForm2;
                        String[] sensor3 = sensor_id3.Split(',');
                        if (sensor3[5] == "0")
                        {
                            int input3A = Convert.ToInt16(sensor3[4]);
                            switch(input3A)
                            {
                                case 128:
                                    label24.Text = "Soil pH";
                                    button9.Enabled = true;
                                    textBox6.Text += timestamp + ": " + "Connecting to: " + label24.Text + " Sensor" + Environment.NewLine;
                                    break;
                                case 64:
                                    label24.Text = "Soil Moisture and Temperature";
                                    button9.Enabled = true;
                                    textBox6.Text += timestamp + ": " + "Connecting to: " + label24.Text + Environment.NewLine;
                                    break;
                                case 32:
                                    label24.Text = "Soil Electrical Conductivity";
                                    button9.Enabled = true;
                                    textBox6.Text += timestamp + ": " + "Connecting to: " + label24.Text + Environment.NewLine;
                                    break;
                                case 16:
                                    label24.Text = "Soil Salinity";
                                    button9.Enabled = true;
                                    textBox6.Text += timestamp + ": " + "Connecting to: " + label24.Text + Environment.NewLine;
                                    break;
                                case 8:
                                    label24.Text = "Soil Temperature, Moisture, Salinity and EC";
                                    button9.Enabled = true;
                                    textBox6.Text += timestamp + ": " + "Connecting to: " + label24.Text + Environment.NewLine;
                                    break;
                                case 4:
                                    label24.Text = "Soil NPK (Nitrogen-PhosphorousPotassium)";
                                    button9.Enabled = true;
                                    textBox6.Text += timestamp + ": " + "Connecting to: " + label24.Text + Environment.NewLine;
                                    break;
                                case 2:
                                    label24.Text = "Water pH Sensor";
                                    button9.Enabled = true;
                                    textBox6.Text += timestamp + ": " + "Connecting to: " + label24.Text + Environment.NewLine;
                                    break;
                                case 1:
                                    label24.Text = "Water Dissolved Oxygen Sensor";
                                    button9.Enabled = true;
                                    textBox6.Text += timestamp + ": " + "Connecting to: " + label24.Text + Environment.NewLine;
                                    break;
                            }
                            
                        }
                        if (sensor3[4] == "0")
                        {
                            int input3B = Convert.ToInt16(sensor3[5]);
                            switch (input3B)
                            {
                                case 128:
                                    label24.Text = "Water Ammonia Sensor";
                                    button9.Enabled = true;
                                    textBox6.Text += timestamp + ": " + "Connecting to: " + label24.Text + " Sensor" + Environment.NewLine;
                                    break;
                                case 64:
                                    label24.Text = "Water Turbidity Sensor";
                                    button9.Enabled = true;
                                    textBox6.Text += timestamp + ": " + "Connecting to: " + label24.Text + " Sensor" + Environment.NewLine;
                                    break;
                                case 32:
                                    label24.Text = "Water Salinity Sensor";
                                    button9.Enabled = true;
                                    textBox6.Text += timestamp + ": " + "Connecting to: " + label24.Text + " Sensor" + Environment.NewLine;
                                    break;
                                case 16:
                                    label24.Text = "Ultrasonic Level Meter";
                                    button9.Enabled = true;
                                    textBox6.Text += timestamp + ": " + "Connecting to: " + label24.Text + " Sensor" + Environment.NewLine;
                                    break;
                                case 8:
                                    label24.Text = "Pneumatic Level Meter";
                                    button9.Enabled = true;
                                    textBox6.Text += timestamp + ": " + "Connecting to: " + label24.Text + " Sensor" + Environment.NewLine;
                                    break;
                                case 4:
                                    label24.Text = "Radar Level Meter";
                                    button9.Enabled = true;
                                    textBox6.Text += timestamp + ": " + "Connecting to: " + label24.Text + " Sensor" + Environment.NewLine;
                                    break;

                                default:
                                    label24.Text = "Not Available";
                                    button9.Enabled = false;
                                    textBox6.Text += timestamp + ": " + " Sensor " + label24.Text + Environment.NewLine;
                                    break;

                            }
                        }
                        if (sensor3[4] == "0" && sensor3[5] == "0")
                        {
                            label24.Text = "Not Available";
                            button9.Enabled = false;
                            textBox6.Text += timestamp + " " + "No Sensor in Input 3" + Environment.NewLine;
                        }
                            label23.Text = "INACTIVE";
                    }
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }
        }

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
        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (serial_portform2.IsOpen)
            {
                serial_portform2.DiscardOutBuffer();
                serial_portform2.DiscardInBuffer();
                serial_portform2.DataReceived -= new SerialDataReceivedEventHandler(Serial_portform2_DataReceived);
            }
        }
    }

}
