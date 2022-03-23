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
using System.IO;
using System.Globalization;
using System.Management;
using Lucene.Net.Search;
using USBPortScanner;
using System.Diagnostics;


namespace DQS_Factory_Tool_V2._0
{
    public partial class Form1 : Form
    {

        StringBuilder productString = new StringBuilder();
        static string filelog = @"C:\\NXP\\ProductionFlashProgrammer\\Log\\Log_Zigbee_Test_" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
        static string filelog_flash = @"C:\\NXP\\ProductionFlashProgrammer\\Log\\Log_Zigbee_Flash_" + DateTime.Now.ToString("yyyy_MM_dd") + ".txt";
        string fileName; // Select file .bin
        string strData;
        string arg,cmd;
       
        string cmd1, cmd2, cmd3, cmd4;
        //string sStr;
       
        uint baudRate = 1000000;
       
        string[] ports = SerialPort.GetPortNames(); // Get infor Port
        


        /// <summary>
        /// Test Function Zigbee
        /// </summary>
        /*
         *  GUI Test function On/Off with Switch
         * **/



        /*
         * Create file log
         * **/
        // Create Log
        private void vLog(string strData)
        {
            using (StreamWriter sw = File.AppendText(filelog))
            {
                
                sw.WriteLine(strData.Trim());
            }
        }

        private void vLog_flash(string strData)
        {
            using (StreamWriter sw = File.AppendText(filelog_flash))
            {

                sw.WriteLine(strData.Trim());
            }
        }

        /*
         * Start Zigbe Test
         * **/
       

       

        /// <summary>
        /// Main function
        /// </summary>
        public Form1()
        {
            InitializeComponent();
          
            vGetPort_Refresh();
            GUIinitilize();
            

        }

        


        ////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// <summary>
        /// Zigbee flash Code function
        /// </summary>
        /// 
        /**
        * Button Click : Open select file bin
        */
        private void btOpenData_Click(object sender, EventArgs e)
        {
            if (ofdOpen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                // Set file name
                cbOpenFile.Text = ofdOpen.SafeFileName;
                fileName = ofdOpen.FileName;
            }

        }

        /**
         *  Get Port USB flash firmware
         */
       

        /*
         *  Get port for refresh
         * **/
        private void vGetPort_Refresh()
        {
            string[] ports = SerialPort.GetPortNames(); // Get infor Port
            cbPort1.Text ="";
            cbPort2.Text = "";
            cbPort3.Text = "";
            cbPort4.Text = "";
            tbDeviceCount.Text = " 0 ";
            tbDeviceCount.BackColor = Color.White;
            lbShow1.Text = "N/A";
            lbShow2.Text = "N/A";
            lbShow3.Text = "N/A";
            lbShow4.Text = "N/A";

            lbShow1.BackColor = Color.LightGray;
            lbShow2.BackColor = Color.LightGray;
            lbShow2.BackColor = Color.LightGray;
            lbShow2.BackColor = Color.LightGray;


            lbValue.Text = "";
            lbValue2.Text = "";
            lbValue3.Text = "";
            lbValue4.Text = "";


           
            int i = ports.Length;
            switch (ports.Length)
            {
               
                case 1:
                    cbPort1.Text = ports[0];
                    //cbBaudrate1.SelectedIndex = 9;
                    //strData = cbPort1.Text + " - Đã được kết nối";
                    lbStatus.Text = cbPort1.Text + " - Đã được kết nối";
                    tbDeviceCount.Text = i.ToString();
                    tbDeviceCount.BackColor = Color.PaleTurquoise;
                    break;
                case 2:
                    cbPort1.Text = ports[0];
                    Thread.Sleep(5);
                    cbPort2.Text = ports[1];
                    Thread.Sleep(5);
                    //cbBaudrate1.SelectedIndex = 9;
                    //cbBaudrate2.SelectedIndex = 9;
                    lbStatus.Text = cbPort1.Text + " - " + cbPort2.Text + " - Đã được kết nối";
                    tbDeviceCount.Text = i.ToString();
                    tbDeviceCount.BackColor = Color.PaleTurquoise;
                    break;
                case 3:
                    cbPort1.Text = ports[0];
                    Thread.Sleep(5);
                    cbPort2.Text = ports[1];
                    Thread.Sleep(5);
                    cbPort3.Text = ports[2];
                    //cbBaudrate1.SelectedIndex = 9;
                    //cbBaudrate2.SelectedIndex = 9;
                    //cbBaudrate3.SelectedIndex = 9;
                    lbStatus.Text = cbPort1.Text + " - " + cbPort2.Text + " - " + cbPort3.Text + " - Đã được kết nối";
                    tbDeviceCount.Text = i.ToString();
                    tbDeviceCount.BackColor = Color.PaleTurquoise;
                    break;

                case 4:
                    cbPort1.Text = ports[0];
                    Thread.Sleep(5);
                    cbPort2.Text = ports[1];
                    Thread.Sleep(5);
                    cbPort3.Text = ports[2];
                    Thread.Sleep(5);
                    cbPort4.Text = ports[3];
                    Thread.Sleep(5);
                    //cbBaudrate1.SelectedIndex = 9;
                    //cbBaudrate2.SelectedIndex = 9;
                    //cbBaudrate3.SelectedIndex = 9;
                    //cbBaudrate4.SelectedIndex = 9;
                    lbStatus.Text = cbPort1.Text + " - " + cbPort2.Text + " - " + cbPort3.Text + " - " + cbPort4.Text + " - Đã được kết nối";
                    tbDeviceCount.Text = i.ToString();
                    tbDeviceCount.BackColor = Color.PaleTurquoise;
                    break;
            }
        }

        /*
         * Flash firmware programmer
         * **/
        private void btFirmware_Click(object sender, EventArgs e)
        {
           
            vGetPort_Refresh();
            cmd1 = cbPort1.Text;
            cmd2 = cbPort2.Text;
            cmd3 = cbPort3.Text;
            cmd4 = cbPort4.Text;

           
            strData += "-------------------------------------------------\n";
           
            if (cbOpenFile.Text == "")
            {
                MessageBox.Show(" Vui lòng chọn file .bin để chạy");
                //arg.Remove(1, 10);
            }
            else
            {
                
                cmd = @"C:\NXP\ProductionFlashProgrammer\JN51xxProgrammer.exe ";
                arg = " -V 0 -P " + baudRate + " -f " + fileName ;
                // Check verify click
                if (cbVerify.Checked == true)
                {
                    arg = arg + " -v";
                }

                // Check Protect
                if (cbProtect.Checked == true)
                 {
                    arg = arg + " -Y --deviceconfig=CRP_LEVEL1";
                    Thread.Sleep(10);
                }

                if (cbOptionUSB.Checked == true)
                {
                   
                    strData += "--------USB-------------\n";
                    if (cmd1 != "")
                    {
                        cmd1 = cbPort1.Text;
                        arg = arg + " -s " + cmd1;
                        vConfigGPIO_USB(0);
                        lbShow1.Text = "ĐANG XỬ LÝ...";
                    }
                    if (cmd2 != "")
                    {
                        cmd2 = cbPort2.Text;
                        arg = arg + " -s " + cmd2;
                        vConfigGPIO_USB(1);
                        lbShow1.Text = "ĐANG XỬ LÝ...";
                        lbShow2.Text = "ĐANG XỬ LÝ...";

                    }
                    if (cmd3 != "")
                    {
                        cmd3 = cbPort3.Text;
                        arg = arg + " -s " + cmd3;
                        vConfigGPIO_USB(2);
                        lbShow1.Text = "ĐANG XỬ LÝ...";
                        lbShow2.Text = "ĐANG XỬ LÝ...";
                        lbShow3.Text = "ĐANG XỬ LÝ...";

                    }
                    if (cmd4 != "")
                    {
                        cmd4 = cbPort4.Text;
                        arg = arg + " -s " + cmd4;
                        vConfigGPIO_USB(3);
                        lbShow1.Text = "ĐANG XỬ LÝ...";
                        lbShow2.Text = "ĐANG XỬ LÝ...";
                        lbShow3.Text = "ĐANG XỬ LÝ...";
                        lbShow4.Text = "ĐANG XỬ LÝ...";

                    }
                    vProcessProduct();
                    vProcessFirmware();
                   
                }
                else
                {
                    if (cmd1 != "")
                    {
                        cmd1 = cbPort1.Text;
                        arg = arg + " -s " + cmd1;
                        
                        lbShow1.Text = "ĐANG XỬ LÝ...";
                    }
                    if (cmd2 != "")
                    {
                        cmd2 = cbPort2.Text;
                        arg = arg + " -s " + cmd2;
                        
                        lbShow1.Text = "ĐANG XỬ LÝ...";
                        lbShow2.Text = "ĐANG XỬ LÝ...";

                    }
                    if (cmd3 != "")
                    {
                        cmd3 = cbPort3.Text;
                        arg = arg + " -s " + cmd3;
                       
                        lbShow1.Text = "ĐANG XỬ LÝ...";
                        lbShow2.Text = "ĐANG XỬ LÝ...";
                        lbShow3.Text = "ĐANG XỬ LÝ...";

                    }
                    if (cmd4 != "")
                    {
                        cmd4 = cbPort4.Text;
                        arg = arg + " -s " + cmd4;
                        lbShow1.Text = "ĐANG XỬ LÝ...";
                        lbShow2.Text = "ĐANG XỬ LÝ...";
                        lbShow3.Text = "ĐANG XỬ LÝ...";
                        lbShow4.Text = "ĐANG XỬ LÝ...";

                    }
                    vProcesSwitch();
                }    
            }

        }
        // Process all on a function
        
        //  Process firmware main
        private void vProcessFirmware()
        {
            
            try
            {
                
                Process deviceProcess = new Process();
               
                //var deviceProcess = Process.Start(cmd, arg1);
                deviceProcess.StartInfo.FileName = cmd;
                deviceProcess.StartInfo.UseShellExecute = false;
                deviceProcess.StartInfo.RedirectStandardOutput = true;
                deviceProcess.StartInfo.CreateNoWindow = true;
                //deviceProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                deviceProcess.StartInfo.Arguments = arg;
                
                deviceProcess.Start();
               
               

                deviceProcess.WaitForExit();
                StreamReader reader = deviceProcess.StandardOutput;
                string outPut = reader.ReadToEnd();
                //strData += "-----------------------USB--------------------------------\n";
                strData += DateTime.Now.ToString("yyyy/mm/dd - HH:mm:ss\n");
                strData += outPut;
                strData += "-----------------------END USB--------------------------------\n";
                vLog_flash(strData);
                Console.WriteLine(outPut);
                if (cbPort1.Text != "")
                {
                    
                    vGetProductString(0);
                    lbShow1.Text = productString.ToString();
             
                   string newShow1 = outPut.Substring(outPut.Length - 31, 31);
                   if(lbShow1.Text.Contains("DQSMART-ZBUSB-1.1") && newShow1.Contains("Flash programmed successfully"))
                   {
                      

                            lbValue.Text = "PASS";
                            lbValue.BackColor = Color.Lime;
                   }
                   else
                   {
                        lbValue.Text = "FAILED";
                        lbValue.BackColor = Color.Red;
                   }    
                    
                }

                if (cbPort2.Text != "")
                {
                    vGetProductString(0);
                    lbShow2.Text = productString.ToString();
                    string newShow2 = outPut.Substring(outPut.Length - 70, 31);
                    //Console.WriteLine(newShow2);
                    if (lbShow2.Text.Contains("DQSMART-ZBUSB-1.1") && newShow2.Contains("Flash programmed successfully"))
                    {
                       
                            lbValue2.Text = "PASS";
                            lbValue2.BackColor = Color.Lime;
                      
                              
                       
                    } 
                    else
                    {
                        lbValue2.Text = "FAILED";
                        lbValue2.BackColor = Color.Red;
                    }    
                   
                }
                if (cbPort3.Text != "")
                {
                    vGetProductString(2);
                    lbShow3.Text = productString.ToString();
                    string newShow3 = outPut.Substring(outPut.Length - 109, 31);
                    //Console.WriteLine(newShow3);
                    if (lbShow3.Text.Contains("DQSMART-ZBUSB-1.1") && newShow3.Contains("Flash programmed successfully"))
                    {
                       
                            lbValue3.Text = "PASS";
                            lbValue3.BackColor = Color.Lime;
                        
                       
                    }    
                   else
                   {
                        lbValue3.Text = "FAILED";
                        lbValue3.BackColor = Color.Red;
                   }    
                }
                if (cbPort4.Text != "")
                {
                    vGetProductString(3);
                    lbShow4.Text = productString.ToString();
                    string newShow4 = outPut.Substring(outPut.Length - 148, 31);
                    //Console.WriteLine(newShow4);
                    if (lbShow4.Text.Contains("DQSMART-ZBUSB-1.1") && newShow4.Contains("Flash programmed successfully"))
                    {
                       
                            lbValue4.Text = "PASS";
                            lbValue4.BackColor = Color.Lime;
                     
                    }    
                    else
                    {
                        lbValue4.Text = "FAILED";
                        lbValue4.BackColor = Color.Red;
                    }    
                }
                deviceProcess.Close();
              
              
            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error" + Ex);
            }
        }
       
        private void vProcesSwitch()
        {
            try
            {

                Process deviceProcess = new Process();

                //var deviceProcess = Process.Start(cmd, arg1);
                deviceProcess.StartInfo.FileName = cmd;
                deviceProcess.StartInfo.UseShellExecute = false;
                deviceProcess.StartInfo.RedirectStandardOutput = true;
                deviceProcess.StartInfo.CreateNoWindow = true;
                //deviceProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                deviceProcess.StartInfo.Arguments = arg;

                deviceProcess.Start();
                
                deviceProcess.WaitForExit();
                StreamReader reader = deviceProcess.StandardOutput;
                string outPut = reader.ReadToEnd();
                strData += "----------------------SWITCH---------------------------------\n";
                strData += DateTime.Now.ToString("yyyy/mm/dd - HH:mm:ss \n");
                strData += outPut;
                strData += "----------------------END SWITCH---------------------------------\n";
                vLog_flash(strData);
                Console.WriteLine(outPut);
                if (cbPort1.Text != "")
                {
                    
                    string newShow1 = outPut.Substring(outPut.Length - 31, 31);
                    Console.WriteLine(newShow1);
                    if (newShow1.Contains("Flash programmed successfully"))
                    {

                        lbShow1.Text = "";
                        lbValue.Text = "PASS";
                        lbValue.BackColor = Color.Lime;

                    }
                    else if (newShow1.Contains("Flash verified successfully"))
                    {
                        lbShow1.Text = "";
                        lbValue.Text = "PASS";
                        lbValue.BackColor = Color.Lime;
                    }    
                    else
                    {
                        lbShow1.Text = "";
                        lbValue.Text = "FAILED";
                        lbValue.BackColor = Color.Red;
                    }

                }

                if (cbPort2.Text != "")
                {
                    
                    string newShow2 = outPut.Substring(outPut.Length - 70, 31);
                    Console.WriteLine(newShow2);
                    if (newShow2.Contains("Flash programmed successfully"))
                    {
                        lbShow1.Text = "";
                        lbShow2.Text = "";
                        lbValue2.Text = "PASS";
                        lbValue2.BackColor = Color.Lime;
                    }
                    else if (newShow2.Contains("Flash verified successfully"))
                    {
                        lbShow2.Text = "";
                        lbValue2.Text = "PASS";
                        lbValue2.BackColor = Color.Lime;
                    }
                    else
                    {
                        lbShow1.Text = "";
                        lbShow2.Text = "";
                        lbValue2.Text = "FAILED";
                        lbValue2.BackColor = Color.Red;
                    }
                }
                if (cbPort3.Text != "")
                {
                   
                    string newShow3 = outPut.Substring(outPut.Length - 109, 31);
                    Console.WriteLine(newShow3);
                    if (newShow3.Contains("Flash programmed successfully"))
                    {
                        lbShow1.Text = "";
                        lbShow2.Text = "";
                        lbShow3.Text = "";
                        lbValue3.Text = "PASS";
                        lbValue3.BackColor = Color.Lime;
                    }
                    else if (newShow3.Contains("Flash verified successfully"))
                    {
                        lbShow3.Text = "";
                        lbValue2.Text = "PASS";
                        lbValue2.BackColor = Color.Lime;
                    }
                    else
                    {
                        lbShow1.Text = "";
                        lbShow2.Text = "";
                        lbShow3.Text = "";
                        lbValue3.Text = "FAILED";
                        lbValue3.BackColor = Color.Red;
                    }
                }
                if (cbPort4.Text != "")
                {
                   
                    string newShow4 = outPut.Substring(outPut.Length - 148, 31);
                    Console.WriteLine(newShow4);
                    if (newShow4.Contains("Flash programmed successfully"))
                    {
                        lbShow1.Text = "";
                        lbShow2.Text = "";
                        lbShow3.Text = "";
                        lbShow4.Text = "";
                        lbValue4.Text = "PASS";
                        lbValue4.BackColor = Color.Lime;
                    }
                    else if (newShow4.Contains("Flash verified successfully"))
                    {
                        lbShow4.Text = "";
                        lbValue4.Text = "PASS";
                        lbValue4.BackColor = Color.Lime;
                    }
                    else
                    {
                        lbShow1.Text = "";
                        lbShow2.Text = "";
                        lbShow3.Text = "";
                        lbShow4.Text = "";
                        lbValue4.Text = "FAILED";
                        lbValue4.BackColor = Color.Red;
                    }
                }
                deviceProcess.Close();

            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error" + Ex);
            }
        }
        // Process set config product string
        private void vProcessProduct()
        {
            string[] ports = SerialPort.GetPortNames();
            try
            {
                string sStr = " --device-count " + ports.Length + " --set-config cp2102n_a02_gqfn20.configuration.1.1";
                using (Process process = new Process())
                {
                    process.StartInfo.FileName = @"C:\NXP\ProductionFlashProgrammer\LibDll\cp210xsmt.exe";
                 
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.Arguments = sStr;
                    process.Start();

                    // Synchronously read the standard output of the spawned process.
                    StreamReader reader = process.StandardOutput;
                    string output = reader.ReadToEnd();

                    strData += output;
                }
            
               

            }
            catch (Exception Ex)
            {
                MessageBox.Show("Error" + Ex);
            }
        }

        // Option set config
        /*private void vProcessSetProduct(int count)
        {
            int device = count;
            Int32 numOfDevices = 0;
            Thread.Sleep(500);

            IntPtr handle = IntPtr.Zero;
            CP210x.CP210x.Reset(handle);
            CP210x.CP210x.GetNumDevices(ref numOfDevices);
            StringBuilder productString = new StringBuilder("DQSMART-ZBUSB-1.2");
            byte length = 17;


            Byte[] prtNum = new Byte[1];
            UInt16[] latch = new UInt16[4];
            //UInt16 mask = 0x01;

            CP210x.CP210x.Open(device, ref handle);
            //CP210x.CP210x.GetPartNumber(handle, prtNum);
            CP210x.CP210x.SetProductString(handle, productString, ref length, true);


            CP210x.CP210x.Close(handle);
            //tbStatus.Text = productString.ToString();
        
        }

       

        /*
         *  Config GPIO CP210x
         * **/
        private void vConfigGPIO_USB(int count)
        {
            int device = count;
            Int32 numOfDevices = 0;
            CP210x.CP210x.GetNumDevices(ref numOfDevices);
            IntPtr handle = IntPtr.Zero;
            Thread.Sleep(200);
            CP210x.CP210x.Reset(handle);
            //StringBuilder productString = new StringBuilder();
            //byte length = 0;
           
           
            Byte[] prtNum = new Byte[1];
            UInt16[] latch = new UInt16[4];
            UInt16 mask = 0x01;
           
                CP210x.CP210x.Open(device, ref handle);
                CP210x.CP210x.GetPartNumber(handle, prtNum);
               // CP210x.CP210x.GetDeviceProductString(handle, productString, ref length, true);
                //CP210x.CP210x.ReadLatch(handle, latch);
                CP210x.CP210x.WriteLatch(handle, (UInt16)(mask << 0), (UInt16)(0x00) << 1); //GPIO0 Low
                CP210x.CP210x.WriteLatch(handle, (UInt16)(mask << 1), (UInt16)(0x00) << 1); //GPIO1 Low
                Thread.Sleep(500);
                CP210x.CP210x.WriteLatch(handle, (UInt16)(mask << 0), (UInt16)(0x01) << 0); //GPIO0 High

                Thread.Sleep(700);
                CP210x.CP210x.WriteLatch(handle, (UInt16)(mask << 0), (UInt16)(0x01) << 0); //GPIO0 High

                 CP210x.CP210x.WriteLatch(handle, (UInt16)(mask << 1), (UInt16)(0x01) << 0); //GPIO1 High

                 CP210x.CP210x.Close(handle);
                
                
        }

        /*
         * 
         * Reset after flash firmware
         */
        /*private void vResetGPIO(int count)
        {
            int device = count;
            Int32 numOfDevices = 0;
            CP210x.CP210x.GetNumDevices(ref numOfDevices);
            IntPtr handle = IntPtr.Zero;
          
            Byte[] prtNum = new Byte[1];
            UInt16 mask = 0x01;

            CP210x.CP210x.Open(device, ref handle);
            //CP210x.CP210x.GetPartNumber(handle, prtNum);
            CP210x.CP210x.WriteLatch(handle, (UInt16)(mask << 1), (UInt16)(0x01) << 0); //GPIO1 High
            
            CP210x.CP210x.WriteLatch(handle, (UInt16)(mask << 0), (UInt16)(0x00) << 1); //GPIO0 Low
            Thread.Sleep(500);
            CP210x.CP210x.WriteLatch(handle, (UInt16)(mask << 0), (UInt16)(0x01) << 0); //GPIO0 High
            //Thread.Sleep(500);
            //tbStatus.Text = "Set thanh cong";
            CP210x.CP210x.Close(handle);
        }
        */
        /// <summary>
        /// Function Test Zigbee
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        private bool bPortConfigured = false;
        private void btStart_Click(object sender, EventArgs e)
        {
               
                //vGetPort();
         
                if(serialPort1.IsOpen)
                {
                    lbConnectTesst.Text = "COM KHÔNG ĐƯỢC KẾT NỐI";
                    btStart.Text = "BẮT ĐẦU (2)";
                    tbStatusButton1.Text = "IDLE";
                    tbStatusButton1.BackColor = System.Drawing.Color.Teal;
                    tbStatusButton2.Text = "IDLE";
                    tbStatusButton2.BackColor = System.Drawing.Color.Teal;
                    tbStatusButton3.Text = "IDLE";
                    tbStatusButton3.BackColor = System.Drawing.Color.Teal;
                    tbStatusButton4.Text = "IDLE";
                    tbStatusButton4.BackColor = System.Drawing.Color.Teal;
                    tbStatusDevice.Text = "";
                    tbStatusDevice.BackColor = System.Drawing.Color.White;
                    textBoxOnOffAddr.Text = "";
                    tbAddress.Text = "";
                    tbVersion.Text = "";
                    tbStatusPermitJoin.Text = "";
                    tbStatusPermitJoin.BackColor = Color.White;

                }  
                else
                {
                    if(cbPort.Text != "")
                    {
                         btStart.Text = "DỪNG LẠI (2)";
                         lbConnectTesst.Text = cbPort.Text + " ĐƯỢC KẾT NỐI";
                         serialPort1.PortName = cbPort.Text;
                         serialPort1.BaudRate = 115200;
                         serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceivedHandler);
                         bPortConfigured = true;
                    }
                    else
                    {
                    MessageBox.Show("COM Port không được chọn");
                    tbStatusButton1.Text = "IDLE";
                    tbStatusButton1.BackColor = System.Drawing.Color.Teal;
                    tbStatusButton2.Text = "IDLE";
                    tbStatusButton2.BackColor = System.Drawing.Color.Teal;
                    tbStatusButton3.Text = "IDLE";
                    tbStatusButton3.BackColor = System.Drawing.Color.Teal;
                    tbStatusButton4.Text = "IDLE";
                    tbStatusButton4.BackColor = System.Drawing.Color.Teal;
                    tbStatusDevice.Text = "";
                    tbStatusDevice.BackColor = System.Drawing.Color.White;
                    textBoxOnOffAddr.Text = "";
                    tbAddress.Text = "";
                    tbVersion.Text = "";
                    //tbStatusPermitJoin.Text = "";
                    //tbStatusPermitJoin.BackColor = Color.White;
                }    
                   
                }    
               

               
            
            if (bPortConfigured == true)
            {
                try
                {
                    if (serialPort1.IsOpen)
                    {
                        serialPort1.Close();

                    }
                    else
                    {
                        //lbStatusCOM.Text = cbPort.Text + " được kết nối !";
                        serialPort1.Open();
                        //vResetZigbee();
                        //Thread.Sleep(2000);
                        //vErasePD();
                        //Thread.Sleep(1000);
                        vGetVersion();
                        Thread.Sleep(100);
                        StartNWK();
                        vPermitJoin();
                        Thread.Sleep(100);
                        vSendPermitRejoinStateRequest();
                    }

                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Error: " + ex);
                }
            }
          
        }

        private void vGetVersion()
        {
            transmitCommand(0x0010, 0, null);
        }

        // Function reset zigbee
        private void vResetZigbee()
        {
            transmitCommand(0x0011, 0, null);
        }
        // Function reset zigbee
        private void vErasePD()
        {
            transmitCommand(0x0012, 0, null);
                
        }
        private void vSendPermitRejoinStateRequest()
        {
            // Transmit command
            transmitCommand(0x0014, 0, null);
        }
        private void vPortTest()
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string s in ports)
            {
                if (s != "")
                {
                    //cbPort.Items.Insert(i++, s);
                    cbPort.Text = s;
                }
            }
        }

        private void addrModeComboBoxZCLInit(ref ComboBox comboBox)
        {
            comboBox.Items.Add("Bound");
            comboBox.Items.Add("Group");
            comboBox.Items.Add("Short");
            comboBox.Items.Add("IEEE");
            comboBox.Items.Add("Broadcast");
            comboBox.Items.Add("No Transmit");
            comboBox.Items.Add("Bound No Ack");
            comboBox.Items.Add("Short No Ack");
            comboBox.Items.Add("IEEE No Ack");
            comboBox.SelectedIndex = 2;
        }
        private void shortAddrTextBoxInit(ref TextBox textBox)
        {
            //textBox.ForeColor = System.Drawing.Color.Gray;
            //textBox.Text = "Address (16-bit Hex)";
        }

        private void transmitCommand(int iCommand, int iLength, byte[] data)
        {
            int i;
            byte[] specialCharacter = null;
            specialCharacter = new byte[1];
            byte[] message = null;
            message = new byte[256];

            // Build message payload, starting with the type field                
            message[0] = (byte)(iCommand >> 8);
            message[1] = (byte)iCommand;

            // Add message length
            message[2] = (byte)(iLength >> 8);
            message[3] = (byte)iLength;

            // Calculate checksum of header
            byte csum = 0;
            csum ^= message[0];
            csum ^= message[1];
            csum ^= message[2];
            csum ^= message[3];

            // Add message data and update checksum
            if (data != null)
            {
                for (i = 0; i < iLength; i++)
                {
                    message[5 + i] = data[i];
                    csum ^= data[i];
                }
            }

            // Add checksum               
            message[4] = csum;

          
            specialCharacter[0] = 1;
            if (iCommand == 0x502)
            {
                writeByteNoRawDisplay(specialCharacter[0]);
            }
            else
            {
                writeByte(specialCharacter[0]);
            }

            // Transmit message payload with byte stuffing as required                
            for (i = 0; i < iLength + 5; i++)
            {
                // Check if stuffing is required
                if (message[i] < 0x10)
                {
                    // First send escape character then message byte XOR'd with 0x10
                    specialCharacter[0] = 2;
                    if (iCommand == 0x502)
                    {
                        writeByteNoRawDisplay(specialCharacter[0]);
                    }
                    else
                    {
                        writeByte(specialCharacter[0]);
                    }

                    int msg = message[i];
                    msg = msg ^ 0x10;
                    message[i] = (byte)msg;

                    if (iCommand == 0x502)
                    {
                        writeByteNoRawDisplay(message[i]);
                    }
                    else
                    {
                        writeByte(message[i]);
                    }
                }
                else
                {
                    // Send the character with no modification
                    if (iCommand == 0x502)
                    {
                        writeByteNoRawDisplay(message[i]);
                    }
                    else
                    {
                        writeByte(message[i]);
                    }
                }
            }
            // Send end character
            specialCharacter[0] = 3;
            if (iCommand == 0x502)
            {
                writeByteNoRawDisplay(specialCharacter[0]);
            }
            else
            {
                writeByte(specialCharacter[0]);
            }
            //richTextBoxCommandResponse.Text += "\n";

        }

        void writeByte(byte data)
        {
            byte[] dataArray = null;
            dataArray = new byte[1];
            dataArray[0] = data;

            // Display data byte in terminal window            
            //richTextBoxCommandResponse.Text += Convert.ToByte(dataArray[0]).ToString("X2");
            //richTextBoxCommandResponse.Text += " ";

            // Write data byte to serial port
            serialPort1.Write(dataArray, 0, 1);
        }

        void writeByteNoRawDisplay(byte data)
        {
            byte[] dataArray = null;
            dataArray = new byte[1];
            dataArray[0] = data;

            // Write data byte to serial port
            serialPort1.Write(dataArray, 0, 1);
        }

        public delegate void MessageParser();

        // define an instance of the delegate
        MessageParser messageParser;

        // Received message parser
        public void myMessageParser()
        {
            // Display raw message data first 
            //displayRawCommandData(rxMessageType, rxMessageLength, rxMessageChecksum, rxMessageData);

            // Display decoded message
            displayDecodedCommand(rxMessageType, rxMessageLength, rxMessageData);
        }
        private byte[] rxMessageData = new byte[1024];
        private byte rxMessageChecksum = 0;
        private UInt16 rxMessageLength = 0;
        private uint rxMessageState = 0;
        private UInt16 rxMessageType = 0;
        private uint rxMessageCount = 0;
        private bool rxMessageInEscape = false;


        private void serialPort1_DataReceivedHandler(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            while (serialPort1.BytesToRead > 0)
            {
                byte rxByte = (byte)serialPort1.ReadByte();

                if (rxByte == 0x01)
                {
                    // Start character received
                    rxMessageChecksum = 0;
                    rxMessageLength = 0;
                    rxMessageType = 0;
                    rxMessageState = 0;
                    rxMessageCount = 0;
                    rxMessageInEscape = false;
                }
                else if (rxByte == 0x02)
                {
                    rxMessageInEscape = true;
                }
                else if (rxByte == 0x03)
                {
                    // instantiate the delegate to be invoked by this thread
                    messageParser = new MessageParser(myMessageParser);

                    // invoke the delegate in the MainForm thread
                    this.Invoke(messageParser);
                }
                else
                {
                    if (rxMessageInEscape == true)
                    {
                        rxByte ^= 0x10;
                        rxMessageInEscape = false;
                    }

                    // Parse character
                    switch (rxMessageState)
                    {
                        case 0:
                            {
                                rxMessageType = rxByte;
                                rxMessageType <<= 8;
                                rxMessageState++;
                            }
                            break;

                        case 1:
                            {
                                rxMessageType |= rxByte;
                                rxMessageState++;
                            }
                            break;

                        case 2:
                            {
                                rxMessageLength = rxByte;
                                rxMessageLength <<= 8;
                                rxMessageState++;
                            }
                            break;

                        case 3:
                            {
                                rxMessageLength |= rxByte;
                                rxMessageState++;
                            }
                            break;

                        case 4:
                            {
                                rxMessageChecksum = rxByte;
                                rxMessageState++;
                            }
                            break;

                        default:
                            {
                                rxMessageData[rxMessageCount++] = rxByte;
                            }
                            break;
                    }
                }
            }
        }
        private void displayDecodedCommand(UInt16 u16Type, UInt16 u16Length, byte[] au8Data)
        {
            if ((u16Type != 0x8011 && u16Type != 0x8012))
            {
                strData += "Type: 0x";
                strData += u16Type.ToString("X4");
                strData += "\n";
            }

            switch (u16Type)
            {
                case 0x8000:
                    {
                        strData += " (Status)";
                        strData += "\n";
                        strData += "  Length: " + u16Length.ToString();
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[0].ToString("X2");

                        switch (au8Data[0])
                        {
                            case 0:
                                {
                                    strData += " (Success)";
                                }
                                break;

                            case 1:
                                {
                                    strData += " (Incorrect Parameters)";
                                }
                                break;

                            case 2:
                                {
                                    strData += " (Unhandled Command)";
                                }
                                break;

                            case 3:
                                {
                                    strData += " (Command Failed)";
                                }
                                break;

                            case 4:
                                {
                                    strData += " (Busy)";
                                }
                                break;

                            case 5:
                                {
                                    strData += " (Stack Already Started)";
                                }
                                break;

                            default:
                                {
                                    strData += " (ZigBee Error Code)";
                                }
                                break;
                        }

                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[1].ToString("X2");

                        if (u16Length > 2)
                        {
                            strData += "\n";
                            strData += "  Message: ";
                            string errorMessage = System.Text.Encoding.Default.GetString(au8Data);
                            strData += errorMessage.Substring(2, (u16Length - 2));
                        }
                        strData += "\n";
                    }
                    break;

               
                case 0x8001:
                    {
                        strData += " (Log)";
                        strData += "\n";
                        strData += "  Level: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Message: ";

                        string logMessage = System.Text.Encoding.Default.GetString(au8Data);
                        strData += logMessage.Substring(1, (u16Length - 1));
                        strData += "\n";
                    }
                    break;

                case 0x8002:
                    {
                        UInt16 u16ProfileID = 0;
                        UInt16 u16ClusterID = 0;

                        u16ProfileID = au8Data[1];
                        u16ProfileID <<= 8;
                        u16ProfileID |= au8Data[2];

                        u16ClusterID = au8Data[3];
                        u16ClusterID <<= 8;
                        u16ClusterID |= au8Data[4];

                        strData += " (Data Indication)";
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        //displayProfileId(u16ProfileID);
                        strData += "\n";
                        displayClusterId(u16ClusterID);
                        strData += "\n";
                        strData += "  Source EndPoint: 0x" + au8Data[5].ToString("X2");
                        strData += "\n";
                        strData += "  Destination EndPoint: 0x" + au8Data[6].ToString("X2");
                        strData += "\n";
                        strData += "  Source Address Mode: 0x" + au8Data[7].ToString("X2");
                        strData += "\n";
                        strData += "  Source Address: ";

                        byte nextIndex = 0;

                        if (au8Data[9] == 0)
                        {
                            //0x00 = DstAddress and DstEndpoint not present                        
                            strData += "Not Present";
                            strData += "\n";

                            nextIndex = 10;
                        }
                        else if (au8Data[9] == 1)
                        {
                            UInt16 u16GroupAddr = 0;

                            u16GroupAddr = au8Data[10];
                            u16GroupAddr <<= 8;
                            u16GroupAddr |= au8Data[11];

                            //0x01 = 16-bit group address for DstAddress; DstEndpoint not present
                            strData += u16GroupAddr.ToString("X4");
                            strData += "\n";

                            nextIndex = 12;
                        }
                        else if (au8Data[9] == 2)
                        {
                            UInt16 u16DstAddress = 0;
                            UInt16 u16DstEndPoint1 = 0;

                            u16DstAddress = au8Data[10];
                            u16DstAddress <<= 8;
                            u16DstAddress |= au8Data[11];

                            u16DstEndPoint1 = au8Data[12];
                            u16DstEndPoint1 <<= 8;
                            u16DstEndPoint1 |= au8Data[13];

                            //0x02 = 16-bit address for DstAddress and DstEndpoint present
                            strData += u16DstAddress.ToString("X4");
                            strData += "  EndPoint: 0x" + u16DstEndPoint1.ToString("X4");
                            strData += "\n";

                            nextIndex = 14;
                        }
                        else if (au8Data[9] == 3)
                        {
                            //0x03 = 64-bit extended address for DstAddress and DstEndpoint present
                        }
                        else
                        {
                            //0x04 - 0xff = reserved
                            nextIndex = 10;
                            strData += "Not Valid";
                            strData += "\n";
                        }

                        strData += "  Destination Address Mode: 0x" + au8Data[nextIndex].ToString("X2");
                        strData += "\n";
                    }
                    break;

                case 0x8003:
                    {
                        UInt16 u16Entries = 0;
                        UInt16 u16ProfileID = 0;

                        u16ProfileID = au8Data[1];
                        u16ProfileID <<= 8;
                        u16ProfileID |= au8Data[2];

                        u16Entries = (UInt16)((u16Length - 3) / 2);

                        strData += " (Cluster List - Entries: ";
                        strData += u16Entries.ToString();
                        strData += ")\n";
                        strData += "  Source EndPoint: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        //displayProfileId(u16ProfileID);

                        for (int i = 3; i < u16Length; i += 2)
                        {
                            UInt16 u16ClusterID;

                            u16ClusterID = au8Data[i];
                            u16ClusterID <<= 8;
                            u16ClusterID |= au8Data[i + 1];

                            displayClusterId(u16ClusterID);
                            strData += "\n";
                        }
                    }
                    break;

                case 0x8004:
                    {
                        UInt16 u16Entries = 0;
                        UInt16 u16ProfileID = 0;
                        UInt16 u16ClusterID = 0;

                        u16ProfileID = au8Data[1];
                        u16ProfileID <<= 8;
                        u16ProfileID |= au8Data[2];

                        u16ClusterID = au8Data[3];
                        u16ClusterID <<= 8;
                        u16ClusterID |= au8Data[4];

                        u16Entries = (UInt16)((u16Length - 5) / 2);

                        strData += " (Cluster Attributes - Entries: ";
                        strData += u16Entries.ToString();
                        strData += ")\n";
                        strData += " Source EndPoint: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        //displayProfileId(u16ProfileID);
                        strData += "\n";
                        displayClusterId(u16ClusterID);
                        strData += "\n";

                        for (int i = 5; i < u16Length; i += 2)
                        {
                            UInt16 u16AttributeID = 0;

                            u16AttributeID = au8Data[i];
                            u16AttributeID <<= 8;
                            u16AttributeID |= au8Data[i + 1];

                            strData += " Attribute ID: 0x" + u16AttributeID.ToString("X4");
                            strData += "\n";
                        }
                    }
                    break;

                case 0x8005:
                    {
                        UInt16 u16Entries = 0;
                        UInt16 u16ProfileID = 0;
                        UInt16 u16ClusterID = 0;

                        u16ProfileID = au8Data[1];
                        u16ProfileID <<= 8;
                        u16ProfileID |= au8Data[2];

                        u16ClusterID = au8Data[3];
                        u16ClusterID <<= 8;
                        u16ClusterID |= au8Data[4];

                        u16Entries = (UInt16)(u16Length - 5);

                        strData += " (Command IDs - Entries: ";
                        strData += u16Entries.ToString();
                        strData += ")\n";
                        strData += " Source EndPoint: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        //displayProfileId(u16ProfileID);
                        strData += "\n";
                        displayClusterId(u16ClusterID);
                        strData += "\n";

                        for (int i = 5; i < u16Length; i++)
                        {
                            strData += " Command ID: 0x" + au8Data[i].ToString("X2");
                            strData += "\n";
                        }
                    }
                    break;

                case 0x8009:
                    {

                        UInt16 u16PanId = 0;
                        UInt16 u16ShortAddr = 0;
                        UInt64 u64ExtendedPANID = 0;
                        UInt64 u64ExtendedAddr = 0;

                        u16ShortAddr = au8Data[0];
                        u16ShortAddr <<= 8;
                        u16ShortAddr |= au8Data[1];

                        for (int i = 0; i < 8; i++)
                        {
                            u64ExtendedAddr <<= 8;
                            u64ExtendedAddr |= au8Data[2 + i];
                        }

                        u16PanId = au8Data[10];
                        u16PanId <<= 8;
                        u16PanId |= au8Data[11];

                        for (int i = 0; i < 8; i++)
                        {
                            u64ExtendedPANID <<= 8;
                            u64ExtendedPANID |= au8Data[12 + i];
                        }

                        strData += " (Network State Response)";
                        strData += "\n";
                        strData += "  Short Address: 0x" + u16ShortAddr.ToString("X4");
                        strData += "\n";
                        strData += "  Extended Address: 0x" + u64ExtendedAddr.ToString("X8");
                        strData += "\n";
                        strData += "  PAN ID: " + u16PanId.ToString("X4");
                        strData += "\n";
                        strData += "  Ext PAN ID: 0x" + u64ExtendedPANID.ToString("X8");
                        strData += "\n";
                        strData += "  Channel: " + au8Data[20].ToString();
                        strData += "\n";
                    }
                    break;

                case 0x8010:
                    {
                        UInt16 u16Major = 0;
                        UInt16 u16Installer = 0;

                        u16Major = au8Data[0];
                        u16Major <<= 8;
                        u16Major |= au8Data[1];

                        u16Installer = au8Data[2];
                        u16Installer <<= 8;
                        u16Installer |= au8Data[3];

                        strData += " (Version)";
                        strData += "\n";
                        strData += "  Length: " + u16Length.ToString();
                        strData += "\n";
                        strData += "  Application: " + u16Major.ToString();
                        strData += "\n";
                        strData += "  SDK: " + u16Installer.ToString();
                        strData += "\n";
                        tbVersion.Text = u16Major.ToString() + "." + u16Installer.ToString();

                        vLog(strData);
                    }
                    break;

                case 0x8024:
                    {
                        UInt16 u16ShortAddr = 0;
                        UInt64 u64ExtAddr = 0;

                        u16ShortAddr = au8Data[1];
                        u16ShortAddr <<= 8;
                        u16ShortAddr |= au8Data[2];

                        for (int i = 0; i < 8; i++)
                        {
                            u64ExtAddr <<= 8;
                            u64ExtAddr |= au8Data[3 + i];
                        }

                        strData += " (Network Up)";
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Short Address: 0x" + u16ShortAddr.ToString("X4");
                        strData += "\n";
                        strData += "  Extended Address: 0x" + u64ExtAddr.ToString("X8");
                        strData += "\n";
                        strData += "  Channel: " + au8Data[11].ToString();
                        strData += "\n";
                    }
                    break;

                case 0x8014:
                    {
                        strData += " (Permit Join State)";
                        strData += "\n";
                        strData += "Permit Join: " + (au8Data[0] == 1 ? "TRUE" : "FALSE");
                        strData += "\n";
                        if (au8Data[0] == 1)
                        {
                            tbStatusPermitJoin.Text = "Thành công";
                            tbStatusPermitJoin.BackColor = Color.Lime;
                        }
                        else
                        {
                            tbStatusPermitJoin.Text = "Zigbee Error Code";
                            tbStatusPermitJoin.BackColor = Color.Red;
                            //vResetZigbee();
                            //vErasePD();
                        }
                    }
                    break;

                case 0x8015:
                    {
                        UInt16 u16PanId = 0;
                        UInt16 u16ShortAddr = 0;
                        UInt16 u16SuperframeSpec = 0;
                        UInt32 u32TimeStamp = 0;
                        UInt64 u64ExtendedPANID = 0;

                        u16PanId = au8Data[1];
                        u16PanId <<= 8;
                        u16PanId |= au8Data[2];

                        u16ShortAddr = au8Data[3];
                        u16ShortAddr <<= 8;
                        u16ShortAddr |= au8Data[4];

                        u16SuperframeSpec = au8Data[11];
                        u16SuperframeSpec <<= 8;
                        u16SuperframeSpec |= au8Data[12];

                        for (int i = 0; i < 4; i++)
                        {
                            u32TimeStamp <<= 8;
                            u32TimeStamp |= au8Data[13 + i];
                        }

                        for (int i = 0; i < 8; i++)
                        {
                            u64ExtendedPANID <<= 8;
                            u64ExtendedPANID |= au8Data[17 + i];
                        }

                        strData += " (Discovery Only Scan Response)";
                        strData += "\n";
                        strData += "  Address Mode: " + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  PAN ID: " + u16PanId.ToString("X4");
                        strData += "\n";
                        strData += "  Short Address: 0x" + u16ShortAddr.ToString("X4");
                        strData += "\n";
                        strData += "  Channel: " + au8Data[5].ToString();
                        strData += "\n";
                        strData += "  GTS Permit: " + au8Data[6].ToString("X2");
                        strData += "\n";
                        strData += "  Link Quality: " + au8Data[7];
                        strData += "\n";
                        strData += "  Security Use: " + au8Data[8].ToString("X2");
                        strData += "\n";
                        strData += "  ACL Entry: " + au8Data[9].ToString("X2");
                        strData += "\n";
                        strData += "  Security Failure: " + au8Data[10].ToString("X2");
                        strData += "\n";
                        strData += "  Superframe Specification: " + u16SuperframeSpec.ToString("X4");
                        strData += "\n";
                        strData += "  Time Stamp: " + u32TimeStamp.ToString("X4");
                        strData += "\n";
                        strData += "  Ext PAN ID: 0x" + u64ExtendedPANID.ToString("X8");

                    }
                    break;

                case 0x8029:
                    {
                        UInt64 u64AddrData = 0;
                        UInt64 u64Key = 0;
                        UInt64 u64HostAddrData = 0;
                        UInt64 u64ExtPANID = 0;
                        UInt32 u32Mic = 0;
                        UInt16 u16PANID = 0;
                        UInt16 u16ShortAddr = 0;
                        UInt16 u16DeviceId = 0;

                        for (int i = 0; i < 8; i++)
                        {
                            u64AddrData <<= 8;
                            u64AddrData |= au8Data[0 + i];
                        }

                        for (int i = 0; i < 16; i++)
                        {
                            u64Key <<= 8;
                            u64Key |= au8Data[8 + i];
                        }

                        for (int i = 0; i < 4; i++)
                        {
                            u32Mic <<= 8;
                            u32Mic |= au8Data[24 + i];
                        }

                        for (int i = 0; i < 8; i++)
                        {
                            u64HostAddrData <<= 8;
                            u64HostAddrData |= au8Data[28 + i];
                        }

                        u16PANID = au8Data[38];
                        u16PANID <<= 8;
                        u16PANID |= au8Data[39];

                        for (int i = 0; i < 8; i++)
                        {
                            u64ExtPANID <<= 8;
                            u64ExtPANID |= au8Data[40 + i];
                        }

                        u16ShortAddr = au8Data[48];
                        u16ShortAddr <<= 8;
                        u16ShortAddr |= au8Data[49];

                        u16DeviceId = au8Data[50];
                        u16DeviceId <<= 8;
                        u16DeviceId |= au8Data[51];

                        strData += " (Encrypted Data)";
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[52].ToString("X2");
                        strData += "\n";
                        strData += "  Device Extended Address: " + u64AddrData.ToString("X8");
                        strData += "\n";
                        strData += "  Key: " + u64Key.ToString("X8");
                        strData += "\n";
                        strData += "  Mic: " + u32Mic.ToString("X4");
                        strData += "\n";
                        strData += "  Host Extended Address: " + u64HostAddrData.ToString("X8");
                        strData += "\n";
                        strData += "  Active Key Sequence Number: " + au8Data[36].ToString("X2");
                        strData += "\n";
                        strData += "  Channel: " + au8Data[37].ToString();
                        strData += "\n";
                        strData += "  PAN ID: " + u16PANID.ToString("X4");
                        strData += "\n";
                        strData += "  Extended PAN ID: " + u64ExtPANID.ToString("X8");
                        strData += "\n";
                        strData += "  Short Address: " + u16ShortAddr.ToString("X4");
                        strData += "\n";
                        strData += "  Device ID: " + u16DeviceId.ToString("X4");
                        strData += "\n";

                    }
                    break;

                // NciCmdNotify
                case 0x802E:
                    {
                        UInt16 u16DeviceId = 0;
                        UInt64 u64ExtAddr = 0;

                        u16DeviceId = au8Data[1];
                        u16DeviceId <<= 8;
                        u16DeviceId |= au8Data[2];

                        for (int i = 0; i < 8; i++)
                        {
                            u64ExtAddr <<= 8;
                            u64ExtAddr |= au8Data[3 + i];
                        }

                        strData += " (NCI Command Notify)";
                        strData += "\n";
                        if (au8Data[0] == 0xA1)
                        {
                            strData += "  Command: Commission";
                        }
                        else if (au8Data[0] == 0xA0)
                        {
                            strData += "  Command: Decommission";
                        }
                        else
                        {
                            strData += "  Command: Unknown";
                        }
                        strData += "\n";
                        strData += "  Device ID: 0x" + u16DeviceId.ToString("X4");
                        strData += "\n";
                        strData += "  Extended Address: 0x" + u64ExtAddr.ToString("X8");
                        strData += "\n";
                    }
                    break;

                case 0x8030:
                    {
                        strData += " (Bind Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                    }
                    break;

                case 0x8031:
                    {
                        strData += " (UnBind Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                    }
                    break;

                case 0x8041:
                    {
                        UInt64 u64ExtAddr = 0;
                        UInt16 u16ShortAddr = 0;

                        for (int i = 0; i < 8; i++)
                        {
                            u64ExtAddr <<= 8;
                            u64ExtAddr |= au8Data[2 + i];
                        }

                        u16ShortAddr = au8Data[10];
                        u16ShortAddr <<= 8;
                        u16ShortAddr |= au8Data[11];

                        strData += " (IEEE Address Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Extended Address: 0x" + u64ExtAddr.ToString("X8");
                        strData += "\n";
                        strData += "  Short Address: 0x" + u16ShortAddr.ToString("X4");
                        strData += "\n";

                        if (u16Length > 14)
                        {
                            strData += "  Associated End Devices: " + au8Data[12].ToString();
                            strData += "\n";
                        }
                    }
                    break;

                case 0x8042:
                    {
                        UInt16 u16ShortAddr = 0;
                        UInt16 u16ManufacturerCode = 0;
                        UInt16 u16RxSize = 0;
                        UInt16 u16TxSize = 0;
                        UInt16 u16ServerMask = 0;
                        UInt16 u16BitFields = 0;
                        byte u8DescriptorCapability = 0;
                        byte u8MacCapability = 0;
                        byte u8MaxBufferSize = 0;

                        u16ShortAddr = au8Data[2];
                        u16ShortAddr <<= 8;
                        u16ShortAddr |= au8Data[3];

                        u16ManufacturerCode = au8Data[4];
                        u16ManufacturerCode <<= 8;
                        u16ManufacturerCode |= au8Data[5];

                        u16RxSize = au8Data[6];
                        u16RxSize <<= 8;
                        u16RxSize |= au8Data[7];

                        u16TxSize = au8Data[8];
                        u16TxSize <<= 8;
                        u16TxSize |= au8Data[9];

                        u16ServerMask = au8Data[10];
                        u16ServerMask <<= 8;
                        u16ServerMask |= au8Data[11];

                        u8DescriptorCapability = au8Data[12];
                        u8MacCapability = au8Data[13];
                        u8MaxBufferSize = au8Data[14];

                        u16BitFields = au8Data[15];
                        u16BitFields <<= 8;
                        u16BitFields |= au8Data[16];

                        strData += " (Node Descriptor Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Short Address: 0x" + u16ShortAddr.ToString("X4");
                        strData += "\n";
                        strData += "  Manufacturer Code: 0x" + u16ManufacturerCode.ToString("X4");
                        strData += "\n";
                        strData += "  Max Rx Size: " + u16RxSize.ToString();
                        strData += "\n";
                        strData += "  Max Tx Size: " + u16TxSize.ToString();
                        strData += "\n";
                        strData += "  Server Mask: 0x" + u16ServerMask.ToString("X4");
                        strData += "\n";
                        //displayDescriptorCapability(u8DescriptorCapability);
                        //displayMACcapability(u8MacCapability);
                        strData += "  Max Buffer Size: " + u8MaxBufferSize.ToString();
                        strData += "\n";
                        strData += "  Bit Fields: 0x" + u16BitFields.ToString("X4");
                        strData += "\n";
                    }
                    break;

                case 0x8043:
                    {
                        UInt16 u16ShortAddr = 0;
                        byte u8Length = 0;

                        u16ShortAddr = au8Data[2];
                        u16ShortAddr <<= 8;
                        u16ShortAddr |= au8Data[3];
                        u8Length = au8Data[4];

                        strData += " (Simple Descriptor Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Short Address: 0x" + u16ShortAddr.ToString("X4");
                        strData += "\n";
                        strData += "  Length: " + u8Length.ToString("");
                        strData += "\n";

                        if (u8Length > 0)
                        {
                            byte u8InputClusterCount = 0;
                            UInt16 u16ProfileId = 0;
                            UInt16 u16DeviceId = 0;

                            u16ProfileId = au8Data[6];
                            u16ProfileId <<= 8;
                            u16ProfileId |= au8Data[7];
                            u16DeviceId = au8Data[8];
                            u16DeviceId <<= 8;
                            u16DeviceId |= au8Data[9];
                            u8InputClusterCount = au8Data[11];

                            strData += "  EndPoint: 0x" + au8Data[5].ToString("X2");
                            strData += "\n";
                            //displayProfileId(u16ProfileId);
                            strData += "\n";
                            //displayDeviceId(u16DeviceId);
                            strData += "\n";
                            strData += "  Input Cluster Count: " + u8InputClusterCount.ToString();
                            strData += "\n";

                            UInt16 u16Index = 12;
                            for (int i = 0; i < u8InputClusterCount; i++)
                            {
                                UInt16 u16ClusterId = 0;

                                u16ClusterId = au8Data[(i * 2) + 12];
                                u16ClusterId <<= 8;
                                u16ClusterId |= au8Data[(i * 2) + 13];

                                strData += "    Cluster " + i.ToString();
                                strData += ":";
                                displayClusterId(u16ClusterId);
                                strData += "\n";
                                u16Index += 2;
                            }

                            byte u8OutputClusterCount = au8Data[u16Index];
                            u16Index++;

                            strData += "  Output Cluster Count: " + u8OutputClusterCount.ToString();
                            strData += "\n";

                            for (int i = 0; i < u8OutputClusterCount; i++)
                            {
                                UInt16 u16ClusterId = 0;

                                u16ClusterId = au8Data[u16Index];
                                u16ClusterId <<= 8;
                                u16ClusterId |= au8Data[u16Index + 1];

                                strData += "    Cluster " + i.ToString();
                                strData += ":";
                                displayClusterId(u16ClusterId);
                                strData += "\n";
                                u16Index += 2;
                            }
                        }
                    }
                    break;
                /*
            case 0x8044:
            {
                strData += " (Power Descriptor Response)";
                strData += "\n";
                strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                strData += "\n";
                strData += "  Status: 0x" + au8Data[1].ToString("X2");
                strData += "\n";
            }
            break;
                */
                case 0x8045:
                    {
                        UInt16 u16ShortAddr = 0;

                        u16ShortAddr = au8Data[2];
                        u16ShortAddr <<= 8;
                        u16ShortAddr |= au8Data[3];

                        strData += " (Active Endpoints Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Short Address: 0x" + u16ShortAddr.ToString("X4");
                        strData += "\n";
                        strData += "  Endpoint Count: " + au8Data[4].ToString();
                        strData += "\n";
                        strData += "  Endpoint List: ";
                        strData += "\n";

                        for (int i = 0; i < au8Data[4]; i++)
                        {
                            strData += "    Endpoint " + i.ToString();
                            strData += ": ";
                            strData += "0x" + au8Data[i + 5].ToString("X2");
                            strData += "\n";
                        }
                    }
                    break;

                case 0x8047:
                    {
                        strData += " (Leave Confirmation)";
                        strData += "\n";

                        if (u16Length == 2)
                        {
                            strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                            strData += "\n";
                        }

                        strData += "  Status: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";

                        if (u16Length == 9)
                        {
                            UInt64 u64ExtAddr = 0;

                            for (int i = 0; i < 8; i++)
                            {
                                u64ExtAddr <<= 8;
                                u64ExtAddr |= au8Data[1 + i];
                            }

                            strData += "  Extended Address: 0x" + u64ExtAddr.ToString("X8");
                            strData += "\n";
                        }
                    }
                    break;

                case 0x8048:
                    {
                        UInt64 u64ExtAddr = 0;

                        for (int i = 0; i < 8; i++)
                        {
                            u64ExtAddr <<= 8;
                            u64ExtAddr |= au8Data[i];
                        }

                        strData += " (Leave Indication)";
                        strData += "\n";
                        strData += "  Extended Address: 0x" + u64ExtAddr.ToString("X8");
                        strData += "\n";
                        strData += "  Rejoin Status: 0x" + au8Data[8].ToString("X2");
                        strData += "\n";
                    }
                    break;

                case 0x804A:
                    {
                        byte u8ScannedChannelsListCount;
                        UInt16 u16TotalTx = 0;
                        UInt16 u16TxFailures = 0;
                        UInt32 u32ScannedChannels = 0;

                        u16TotalTx = au8Data[2];
                        u16TotalTx <<= 8;
                        u16TotalTx |= au8Data[3];

                        u16TxFailures = au8Data[4];
                        u16TxFailures <<= 8;
                        u16TxFailures |= au8Data[5];

                        u32ScannedChannels = au8Data[6];
                        u32ScannedChannels <<= 8;
                        u32ScannedChannels |= au8Data[7];
                        u32ScannedChannels <<= 8;
                        u32ScannedChannels |= au8Data[8];
                        u32ScannedChannels <<= 8;
                        u32ScannedChannels |= au8Data[9];

                        u8ScannedChannelsListCount = au8Data[10];

                        strData += " (Mgmt Nwk Update Notify)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Total Tx: " + u16TotalTx.ToString();
                        strData += "\n";
                        strData += "  Tx Failures: " + u16TxFailures.ToString();
                        strData += "\n";
                        strData += "  Scanned Channels: 0x" + u32ScannedChannels.ToString("X8");
                        strData += "\n";
                        strData += "  Scanned Channels List Count: " + u8ScannedChannelsListCount.ToString();
                        strData += "\n";

                        for (int x = 0; x < u8ScannedChannelsListCount; x++)
                        {
                            strData += "  Value " + x.ToString();
                            strData += ":  0x" + au8Data[11 + x].ToString("X2");
                            strData += "\n";
                        }
                    }
                    break;

                case 0x804E:
                    {
                        byte u8NbTableEntries = 0;
                        byte u8StartIx = 0;
                        byte u8NbTableListCount = 0;

                        UInt16[] au16NwkAddr = new UInt16[16];

                        u8NbTableEntries = au8Data[2];
                        u8NbTableListCount = au8Data[3];
                        u8StartIx = au8Data[4];

                        strData += " (Mgmt LQI Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Nb Table Entries: " + u8NbTableEntries.ToString();
                        strData += "\n";
                        strData += "  Start Index: " + u8StartIx.ToString();
                        strData += "\n";
                        strData += "  Nb Table List Count: " + u8NbTableListCount.ToString();
                        strData += "\n";

                        //comboBoxAddressList.Items.Clear();

                        if (u8NbTableListCount > 0)
                        {
                            byte i;
                            UInt64 u64PanID = 0;
                            UInt64 u64ExtAddr = 0;
                            UInt16 u16NwkAddr = 0;
                            byte u8Lqi = 0;
                            byte u8Depth = 0;
                            byte u8Flags = 0;
                            byte u8PayloadIndex = 5;

                            for (i = 0; i < u8NbTableListCount; i++)
                            {
                                u16NwkAddr = 0;
                                for (int x = 0; x < 2; x++, u8PayloadIndex++)
                                {
                                    u16NwkAddr <<= 8;
                                    u16NwkAddr |= au8Data[u8PayloadIndex];
                                }

                                u64PanID = 0;
                                for (int x = 0; x < 8; x++, u8PayloadIndex++)
                                {
                                    u64PanID <<= 8;
                                    u64PanID |= au8Data[u8PayloadIndex];
                                }

                                u64ExtAddr = 0;
                                for (int x = 0; x < 8; x++, u8PayloadIndex++)
                                {
                                    u64ExtAddr <<= 8;
                                    u64ExtAddr |= au8Data[u8PayloadIndex];
                                }

                                au16NwkAddr[i] = u16NwkAddr;

                                //au64ExtAddr[i] = u64ExtAddr;

                                u8Depth = au8Data[u8PayloadIndex++];
                                u8Lqi = au8Data[u8PayloadIndex++];
                                u8Flags = au8Data[u8PayloadIndex++];

                                strData += "  Neighbor " + i.ToString();
                                strData += ":";
                                strData += "\n";
                                strData += "    Extended Pan ID: 0x" + u64PanID.ToString("X8");
                                strData += "\n";
                                strData += "    Extended Address: 0x" + u64ExtAddr.ToString("X8");
                                strData += "\n";
                                strData += "    Nwk Address: 0x" + u16NwkAddr.ToString("X4");
                                strData += "\n";
                                strData += "    LQI: " + u8Lqi.ToString();
                                strData += "\n";
                                strData += "    Depth: " + u8Depth.ToString();
                                strData += "\n";
                                strData += "    Flags: 0x" + u8Flags.ToString("X2");
                                strData += "\n";

                                byte u8DeviceType = (byte)(u8Flags & 0x03);
                                strData += "    Device Type: ";

                                if (u8DeviceType == 0)
                                {
                                    strData += "Coordinator";
                                }
                                else if (u8DeviceType == 1)
                                {
                                    strData += "Router";
                                }
                                else if (u8DeviceType == 2)
                                {
                                    strData += "End Device";
                                }
                                else
                                {
                                    strData += "Unknown";
                                }
                                strData += "\n";

                                byte u8PermitJoin = (byte)((u8Flags & 0x0C) >> 2);
                                strData += "    Permit Joining: ";

                                if (u8PermitJoin == 0)
                                {
                                    strData += "Off";
                                }
                                else if (u8PermitJoin == 1)
                                {
                                    strData += "On";
                                }
                                else
                                {
                                    strData += "Unknown";
                                }
                                strData += "\n";

                                byte u8Relationship = (byte)((u8Flags & 0x30) >> 4);
                                strData += "    Relationship: ";

                                if (u8Relationship == 0)
                                {
                                    strData += "Parent";
                                }
                                else if (u8Relationship == 1)
                                {
                                    strData += "Child";
                                }
                                else if (u8Relationship == 2)
                                {
                                    strData += "Sibling";
                                }
                                else if (u8Relationship == 4)
                                {
                                    strData += "Previous Child";
                                }
                                else
                                {
                                    strData += "Unknown";
                                }
                                strData += "\n";

                                byte u8RxOnWhenIdle = (byte)((u8Flags & 0xC0) >> 6);
                                strData += "    RxOnWhenIdle: ";

                                if (u8RxOnWhenIdle == 0)
                                {
                                    strData += "No";
                                }
                                else if (u8RxOnWhenIdle == 1)
                                {
                                    strData += "Yes";
                                }
                                else
                                {
                                    strData += "Unknown";
                                }
                                strData += "\n";
                            }
                            for (i = 0; i < u8NbTableListCount; i++)
                            {
                                //comboBoxAddressList.Items.Add(au16NwkAddr[i].ToString("X4"));
                            }
                        }
                    }
                    break;

                case 0x8050:
                    {
                        UInt16 u16TableSize;
                        UInt16 u16TableEntries;

                        u16TableSize = au8Data[1];
                        u16TableSize <<= 8;
                        u16TableSize |= au8Data[2];

                        u16TableEntries = au8Data[3];
                        u16TableEntries <<= 8;
                        u16TableEntries |= au8Data[4];

                        strData += " (Addr Map Table Response)";
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Table Size: " + u16TableSize.ToString();
                        strData += "\n";
                        strData += "  Entries: " + u16TableEntries.ToString();
                        strData += "\n";

                        byte i;
                        for (i = 0; i < u16TableEntries; i++)
                        {
                            UInt16 u16Addr;
                            UInt64 u64Addr;

                            u16Addr = au8Data[5 + (i * 8)];
                            u16Addr <<= 8;
                            u16Addr |= au8Data[6 + (i * 8)];

                            u64Addr = au8Data[7 + (i * 8)];
                            u64Addr <<= 8;
                            u64Addr |= au8Data[8 + (i * 8)];
                            u64Addr <<= 8;
                            u64Addr |= au8Data[9 + (i * 8)];
                            u64Addr <<= 8;
                            u64Addr |= au8Data[10 + (i * 8)];
                            u64Addr <<= 8;
                            u64Addr |= au8Data[11 + (i * 8)];
                            u64Addr <<= 8;
                            u64Addr |= au8Data[12 + (i * 8)];
                            u64Addr <<= 8;
                            u64Addr |= au8Data[13 + (i * 8)];
                            u64Addr <<= 8;
                            u64Addr |= au8Data[14 + (i * 8)];

                            strData += "  Entry " + i.ToString();
                            strData += ": 0x" + u16Addr.ToString("X4");
                            strData += " 0x" + u64Addr.ToString("X8");

                            strData += "\n";
                        }

                    }
                    break;

                case 0x8060:
                    {
                        UInt16 u16ClusterId = 0;
                        UInt16 u16GroupId = 0;

                        u16ClusterId = au8Data[2];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[3];

                        u16GroupId = au8Data[5];
                        u16GroupId <<= 8;
                        u16GroupId |= au8Data[6];

                        strData += " (Add Group Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  EndPoint: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        displayClusterId(u16ClusterId);
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[4].ToString("X2");
                        strData += "\n";
                        strData += "  Group: 0x" + u16GroupId.ToString("X4");
                        strData += "\n";
                    }
                    break;

                case 0x8061:
                    {
                        UInt16 u16ClusterId = 0;
                        UInt16 u16GroupId = 0;

                        u16ClusterId = au8Data[2];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[3];

                        u16GroupId = au8Data[5];
                        u16GroupId <<= 8;
                        u16GroupId |= au8Data[6];

                        strData += " (View Group Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  EndPoint: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        displayClusterId(u16ClusterId);
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[4].ToString("X2");
                        strData += "\n";
                        strData += "  Group: 0x" + u16GroupId.ToString("X4");
                        strData += "\n";
                    }
                    break;

                case 0x8062:
                    {
                        UInt16 u16ClusterId = 0;
                        u16ClusterId = au8Data[2];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[3];

                        strData += " (Get Group Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  EndPoint: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        displayClusterId(u16ClusterId);
                        strData += "\n";
                        strData += "  Capacity: " + au8Data[4].ToString();
                        strData += "\n";
                        strData += "  Count: " + au8Data[5].ToString();
                        strData += "\n";

                        byte i;
                        for (i = 0; i < au8Data[5]; i++)
                        {
                            UInt16 u16GroupId;

                            u16GroupId = au8Data[6 + (i * 2)];
                            u16GroupId <<= 8;
                            u16GroupId |= au8Data[7 + (i * 2)];

                            strData += "  Group " + i.ToString();
                            strData += ": 0x" + u16GroupId.ToString("X4");
                            strData += "\n";
                        }
                    }
                    break;

                case 0x8063:
                    {
                        UInt16 u16ClusterId = 0;
                        UInt16 u16GroupId = 0;

                        u16ClusterId = au8Data[2];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[3];

                        u16GroupId = au8Data[5];
                        u16GroupId <<= 8;
                        u16GroupId |= au8Data[6];

                        strData += " (Remove Group Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  EndPoint: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        displayClusterId(u16ClusterId);
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[4].ToString("X2");
                        strData += "\n";
                        strData += "  Group: 0x" + u16GroupId.ToString("X4");
                        strData += "\n";
                    }
                    break;

                case 0x807A:
                    {
                        strData += " (Identify Local Active)";
                        strData += "\n";
                        if (au8Data[0] == 1)
                        {
                            strData += "  Status: Start Identifying";
                            strData += "\n";
                        }
                        else if (au8Data[0] != 1)
                        {
                            strData += "  Status: Stop Identifying";
                            strData += "\n";
                        }
                    }
                    break;

                case 0x8095:
                    {
                        UInt16 u16ClusterId = 0;

                        u16ClusterId = au8Data[2];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[3];

                        strData += " (On/Off Update)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  EndPoint: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        displayClusterId(u16ClusterId);
                        strData += "\n";
                        strData += "  Src Addr Mode: 0x" + au8Data[4].ToString("X2");
                        strData += "\n";

                        if (au8Data[4] == 0x03)
                        {
                        }
                        else
                        {
                            UInt16 u16Addr = 0;

                            u16Addr = au8Data[5];
                            u16Addr <<= 8;
                            u16Addr |= au8Data[6];

                            strData += "  Src Addr Mode: 0x" + u16Addr.ToString("X4");
                            strData += "\n";
                            strData += "  Status: 0x" + au8Data[7].ToString("X2");
                            strData += "\n";
                        }
                    }
                    break;

                case 0x80A0:
                    {
                        UInt16 u16ClusterId = 0, u16GroupId = 0, u16TransTime = 0, u16SceneLength = 0;
                        byte u8Status;

                        u16ClusterId = au8Data[2];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[3];

                        u8Status = au8Data[4];

                        u16GroupId = au8Data[5];
                        u16GroupId <<= 8;
                        u16GroupId |= au8Data[6];


                        strData += " (View Scene)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  EndPoint: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Cluster ID: 0x" + u16ClusterId.ToString("X4");
                        strData += "\n";
                        strData += "  Status: 0x" + u8Status.ToString("X2");
                        strData += "\n";
                        strData += "  Group ID: 0x" + u16GroupId.ToString("X4");
                        strData += "\n";
                        strData += "  Scene Id: 0x" + au8Data[7].ToString("X2");

                        if (0 == u8Status)
                        {
                            u16TransTime = au8Data[8];
                            u16TransTime <<= 8;
                            u16TransTime |= au8Data[9];

                            strData += "\n";
                            strData += "  Transition Time: 0x" + u16TransTime.ToString("X4");
                            strData += "\n";
                            strData += "  Scene Name Length: 0x" + au8Data[10].ToString("X2");
                            strData += "\n";
                            strData += "  Scene Name Max Length: 0x" + au8Data[11].ToString("X2");
                            strData += "\n";

                            strData += "  Scene Name: ";

                            byte i = 0;
                            for (i = 0; i < au8Data[10]; i++)
                            {
                                strData += Convert.ToChar(au8Data[12 + i]);
                            }

                            u16SceneLength = au8Data[12 + i];
                            u16SceneLength <<= 8;
                            u16SceneLength |= au8Data[13 + i];

                            strData += "\n";
                            strData += "  Ext Scene Length: 0x" + u16SceneLength.ToString("X4");
                            strData += "\n";
                            strData += "  Ext Max Length: 0x" + au8Data[14 + i].ToString("X2");
                            strData += "\n";
                            strData += "  Scene Data: ";
                            strData += "\n      ";

                            for (byte c = 0; i < u16SceneLength; i++)
                            {
                                strData += "0x" + au8Data[15 + i + c].ToString("X2") + " ";
                            }
                        }

                    }
                    break;

                case 0x80A3:
                    {
                        UInt16 u16ClusterId, u16GroupId;

                        u16ClusterId = au8Data[2];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[3];

                        u16GroupId = au8Data[5];
                        u16GroupId <<= 8;
                        u16GroupId |= au8Data[6];

                        strData += " (Remove All Scenes)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  EndPoint: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Cluster ID: 0x" + u16ClusterId.ToString("X4");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[4].ToString("X2");
                        strData += "\n";
                        strData += "  Group ID: 0x" + u16GroupId.ToString("X4");
                    }
                    break;

                case 0x80A2:
                    {
                        UInt16 u16ClusterId, u16GroupId;

                        u16ClusterId = au8Data[2];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[3];

                        u16GroupId = au8Data[5];
                        u16GroupId <<= 8;
                        u16GroupId |= au8Data[6];

                        strData += " (Remove Scene)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  EndPoint: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Cluster ID: 0x" + u16ClusterId.ToString("X4");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[4].ToString("X2");
                        strData += "\n";
                        strData += "  Group ID: 0x" + u16GroupId.ToString("X4");
                        strData += "\n";
                        strData += "  Scene ID: 0x" + au8Data[7].ToString("X2");
                    }
                    break;

                case 0x8100: // Read attribute response
                    {
                        UInt16 u16ClusterId = 0;
                        UInt16 u16AttribId = 0;
                        UInt16 u16SrcAddr = 0;
                        UInt16 u16AttributeSize = 0;

                        u16SrcAddr = au8Data[1];
                        u16SrcAddr <<= 8;
                        u16SrcAddr |= au8Data[2];

                        u16ClusterId = au8Data[4];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[5];

                        u16AttribId = au8Data[6];
                        u16AttribId <<= 8;
                        u16AttribId |= au8Data[7];

                        u16AttributeSize = au8Data[10];
                        u16AttributeSize <<= 8;
                        u16AttributeSize |= au8Data[11];

                        strData += " (Read Attrib Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Src Addr: 0x" + u16SrcAddr.ToString("X4");
                        strData += "\n";
                        strData += "  EndPoint: 0x" + au8Data[3].ToString("X2");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[8].ToString("X2");
                        strData += "\n";
                        displayClusterId(u16ClusterId);
                        strData += "\n";
                        displayAttribute(u16AttribId, au8Data[9], au8Data, 12, u16AttributeSize);
                    }
                    break;

                case 0x8101:
                    {
                        UInt16 u16ClusterId = 0;
                        UInt16 u16DstAddr = 0;

                        u16ClusterId = au8Data[13];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[14];

                        u16DstAddr = au8Data[1];
                        u16DstAddr <<= 8;
                        u16DstAddr |= au8Data[2];

                        strData += " (Default Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Short Address: 0x" + u16DstAddr.ToString("X4");
                        strData += "\n";
                        strData += "  Source EndPoint: 0x" + au8Data[11].ToString("X2");
                        strData += "\n";
                        strData += "  Destination EndPoint: 0x" + au8Data[12].ToString("X2");
                        strData += "\n";
                        displayClusterId(u16ClusterId);
                        strData += "\n";
                        strData += "  Command: 0x" + au8Data[15].ToString("X2");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[16].ToString("X2");
                        strData += "\n";
                    }
                    break;

                case 0x8120:
                    {
                        UInt16 u16ClusterId = 0;
                        UInt16 u16SrcAddr = 0;

                        u16ClusterId = au8Data[4];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[5];

                        u16SrcAddr = au8Data[1];
                        u16SrcAddr <<= 8;
                        u16SrcAddr |= au8Data[2];

                        strData += " (Report Config Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Src Addr: 0x" + u16SrcAddr.ToString("X4");
                        strData += "\n";
                        strData += "  EndPoint: 0x" + au8Data[3].ToString("X2");
                        strData += "\n";
                        displayClusterId(u16ClusterId);
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[6].ToString("X2");
                        strData += "\n";
                    }
                    break;

                case 0x8102:
                    {
                        UInt16 u16SrcAddr = 0;
                        UInt16 u16ClusterId = 0;
                        UInt16 u16AttribId = 0;
                        UInt16 u16AttributeSize = 0;

                        u16SrcAddr = au8Data[1];
                        u16SrcAddr <<= 8;
                        u16SrcAddr |= au8Data[2];

                        u16ClusterId = au8Data[4];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[5];

                        u16AttribId = au8Data[6];
                        u16AttribId <<= 8;
                        u16AttribId |= au8Data[7];

                        u16AttributeSize = au8Data[10];
                        u16AttributeSize <<= 8;
                        u16AttributeSize |= au8Data[11];

                        strData += " (Attribute Report)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Src Addr: 0x" + u16SrcAddr.ToString("X4");
                        strData += "\n";
                        strData += "  Src Ep: 0x" + au8Data[3].ToString("X2");
                        strData += "\n";
                        displayClusterId(u16ClusterId);
                        strData += "\n";
                        strData += "  Attribute Status: 0x" + au8Data[8].ToString("X2");
                        strData += "\n";
                        displayAttribute(u16AttribId, au8Data[9], au8Data, 12, u16AttributeSize);
                        tbAddress.Text = u16SrcAddr.ToString("X4");
                        textBoxOnOffAddr.Text = u16SrcAddr.ToString("X4");
                        if (au8Data[8] == 0x00)
                        {
                            tbStatusDevice.Text = " Pair thiết bị thành công! ";
                            tbStatusDevice.BackColor = Color.Lime;
                        }
                        else
                        {
                            tbStatusDevice.Text = " Pair thiết bị thất bại! ";
                            tbStatusDevice.BackColor = Color.Red;
                        }

                        vLog(strData);
                    }
                    break;

                case 0x8122:
                    {
                        UInt16 u16SrcAddr = 0;
                        UInt16 u16ClusterId = 0;
                        UInt16 u16AttribId = 0;
                        UInt16 u16MaxInterval = 0;
                        UInt16 u16MinInterval = 0;

                        u16SrcAddr = au8Data[1];
                        u16SrcAddr <<= 8;
                        u16SrcAddr |= au8Data[2];

                        u16ClusterId = au8Data[4];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[5];

                        u16AttribId = au8Data[8];
                        u16AttribId <<= 8;
                        u16AttribId |= au8Data[9];

                        u16MaxInterval = au8Data[10];
                        u16MaxInterval <<= 8;
                        u16MaxInterval |= au8Data[11];

                        u16MinInterval = au8Data[12];
                        u16MinInterval <<= 8;
                        u16MinInterval |= au8Data[13];

                        strData += " (Attribute Config Report)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Src Addr: 0x" + u16SrcAddr.ToString("X4");
                        strData += "\n";
                        strData += "  Src Ep: 0x" + au8Data[3].ToString("X2");
                        strData += "\n";
                        displayClusterId(u16ClusterId);
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[6].ToString("X2");
                        strData += "\n";
                        
                        strData += "\n";
                        strData += "  Attribute: 0x" + u16AttribId.ToString("X4");
                        strData += "\n";
                        strData += "  Min Interval: " + u16MinInterval.ToString();
                        strData += "\n";
                        strData += "  Max Interval: " + u16MaxInterval.ToString();
                        strData += "\n";
                    }
                    break;

                case 0x8103: // Read local attribute response
                    {
                        UInt16 u16ClusterId = 0;
                        UInt16 u16AttribId = 0;
                        UInt16 u16SrcAddr = 0;
                        UInt16 u16AttributeSize = 0;

                        u16SrcAddr = au8Data[1];
                        u16SrcAddr <<= 8;
                        u16SrcAddr |= au8Data[2];

                        u16ClusterId = au8Data[4];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[5];

                        u16AttribId = au8Data[6];
                        u16AttribId <<= 8;
                        u16AttribId |= au8Data[7];

                        u16AttributeSize = au8Data[10];
                        u16AttributeSize <<= 8;
                        u16AttributeSize |= au8Data[11];

                        strData += " (Read Local Attrib Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        //strData += "  Src Addr: 0x" + u16SrcAddr.ToString("X4");
                        //strData += "\n";                    
                        strData += "  EndPoint: 0x" + au8Data[3].ToString("X2");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[8].ToString("X2");
                        strData += "\n";
                        displayClusterId(u16ClusterId);
                        strData += "\n";
                        displayAttribute(u16AttribId, au8Data[9], au8Data, 12, u16AttributeSize);
                    }
                    break;

                case 0x8140: // Discover attribute response
                    {
                        UInt16 u16AttribId = 0;

                        u16AttribId = au8Data[2];
                        u16AttribId <<= 8;
                        u16AttribId |= au8Data[3];

                        strData += " (Discover Attrib Response)";
                        strData += "\n";
                        strData += "  Complete: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                       
                        strData += "\n";
                        strData += "  Attribute: 0x" + u16AttribId.ToString("X4");
                        strData += "\n";
                    }
                    break;

                case 0x8141: // Discover extended attribute response
                    {
                        UInt16 u16AttribId = 0;

                        u16AttribId = au8Data[2];
                        u16AttribId <<= 8;
                        u16AttribId |= au8Data[3];

                        strData += " (Discover Attrib Response)";
                        strData += "\n";
                        strData += "  Complete: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                       
                        strData += "\n";
                        strData += "  Attribute: 0x" + u16AttribId.ToString("X4");
                        strData += "\n";
                        strData += "  Flags: 0x" + au8Data[4].ToString("X2");
                        strData += "\n";
                    }
                    break;

                case 0x8150: // Discover command received individual response
                    {
                        strData += " (Discover Command Received Individual Response)";
                        strData += "\n";
                        strData += "  Command: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Index: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                    }
                    break;

                case 0x8151: // Discover command received response
                    {
                        strData += " (Discover Command Received Response)";
                        strData += "\n";
                        strData += "  Complete: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Commands: " + au8Data[1].ToString();
                        strData += "\n";
                    }
                    break;

                case 0x8160: // Discover command generated individual response
                    {
                        strData += " (Discover Command Generated Individual Response)";
                        strData += "\n";
                        strData += "  Command: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Index: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                    }
                    break;

                case 0x8161: // Discover command generated response
                    {
                        strData += " (Discover Command Generated Response)";
                        strData += "\n";
                        strData += "  Complete: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Commands: " + au8Data[1].ToString();
                        strData += "\n";
                    }
                    break;

                case 0x8401:
                    {
                        UInt16 u16ClusterId = 0;
                        UInt16 u16ZoneStatus = 0;
                        UInt16 u16Delay = 0;

                        u16ClusterId = au8Data[2];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[3];

                        strData += " (IAS Zone Status Change)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  EndPoint: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        displayClusterId(u16ClusterId);
                        strData += "\n";
                        strData += "  Src Addr Mode: 0x" + au8Data[4].ToString("X2");
                        strData += "\n";

                        if (au8Data[4] == 0x03)
                        {
                        }
                        else
                        {
                            UInt16 u16Addr = 0;

                            u16Addr = au8Data[5];
                            u16Addr <<= 8;
                            u16Addr |= au8Data[6];

                            strData += "  Src Addr Mode: 0x" + u16Addr.ToString("X4");
                            strData += "\n";
                        }

                        u16ZoneStatus = au8Data[7];
                        u16ZoneStatus <<= 8;
                        u16ZoneStatus |= au8Data[8];

                        strData += "  Zone Status: 0x" + u16ZoneStatus.ToString("X4");
                        strData += "\n";
                        strData += "  Ext Status: 0x" + au8Data[9].ToString("X2");
                        strData += "\n";
                        strData += "  Zone ID: 0x" + au8Data[10].ToString("X2");
                        strData += "\n";

                        u16Delay = au8Data[11];
                        u16Delay <<= 8;
                        u16Delay |= au8Data[12];

                        strData += "  Delay: 0x" + u16Delay.ToString("X4");
                        strData += "\n";
                    }
                    break;

                case 0x004D:
                    {
                        UInt16 u16ShortAddr = 0;
                        UInt64 u64ExtAddr = 0;

                        u16ShortAddr = au8Data[0];
                        u16ShortAddr <<= 8;
                        u16ShortAddr |= au8Data[1];

                        for (int i = 0; i < 8; i++)
                        {
                            u64ExtAddr <<= 8;
                            u64ExtAddr |= au8Data[2 + i];
                        }

                        strData += " (End Device Announce)";
                        strData += "\n";
                        strData += "  Short Address: 0x" + u16ShortAddr.ToString("X4");
                        strData += "\n";
                        strData += "  Extended Address: 0x" + u64ExtAddr.ToString("X8");
                        strData += "\n";
                        
                    }
                    break;

              
                   
                case 0x8503:
                    {

                        byte u8Offset = 0;
                        byte u8SQN;
                        byte u8SrcEndpoint;
                        UInt16 u16ClusterId;
                        UInt16 u16SrcAddr;
                        byte u8SrcAddrMode;
                        UInt32 u32FileVersion;
                        UInt16 u16ImageType;
                        UInt16 u16ManufactureCode;
                        byte u8Status;

                        u8SQN = au8Data[u8Offset++];

                        u8SrcEndpoint = au8Data[u8Offset++];

                        u16ClusterId = au8Data[u8Offset++];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[u8Offset++];

                        u8SrcAddrMode = au8Data[u8Offset++];

                        u16SrcAddr = au8Data[u8Offset++];
                        u16SrcAddr <<= 8;
                        u16SrcAddr |= au8Data[u8Offset++];

                        u32FileVersion = au8Data[u8Offset++];
                        u32FileVersion <<= 8;
                        u32FileVersion |= au8Data[u8Offset++];
                        u32FileVersion <<= 8;
                        u32FileVersion |= au8Data[u8Offset++];
                        u32FileVersion <<= 8;
                        u32FileVersion |= au8Data[u8Offset++];

                        u16ImageType = au8Data[u8Offset++];
                        u16ImageType <<= 8;
                        u16ImageType |= au8Data[u8Offset++];

                        u16ManufactureCode = au8Data[u8Offset++];
                        u16ManufactureCode <<= 8;
                        u16ManufactureCode |= au8Data[u8Offset++];

                        u8Status = au8Data[u8Offset++];

                        strData += " (OTA End Request)";
                        strData += "\n";
                        strData += "  SQN: 0x" + u8SQN.ToString("X2");
                        strData += "\n";
                        strData += "  Src Addr Mode: 0x" + u8SrcAddrMode.ToString("X2");
                        strData += "\n";
                        strData += "  Src Addr: 0x" + u16SrcAddr.ToString("X4");
                        strData += "\n";
                        strData += "  EndPoint: 0x" + u8SrcEndpoint.ToString("X2");
                        strData += "\n";
                        displayClusterId(u16ClusterId);
                        strData += "\n";
                        strData += "  File Version: 0x" + u32FileVersion.ToString("X8");
                        strData += "\n";
                        strData += "  Image Type: 0x" + u16ImageType.ToString("X4");
                        strData += "\n";
                        strData += "  Manu Code: 0x" + u16ManufactureCode.ToString("X4");
                        strData += "\n";
                        strData += "  Status: 0x" + u8Status.ToString("X2");
                        strData += "\n";

                        //sendOtaEndResponse(u8SrcAddrMode, u16SrcAddr, 1, u8SrcEndpoint, u8SQN, 5, 10, u32FileVersion, u16ImageType, u16ManufactureCode);


                       
                    }
                    break;

                case 0x8110:
                    {
                        UInt16 u16SrcAddr = 0;
                        UInt16 u16ClusterId = 0;
                        UInt16 u16AttribId = 0;
                        UInt16 u16AttributeSize = 0;

                        u16SrcAddr = au8Data[1];
                        u16SrcAddr <<= 8;
                        u16SrcAddr |= au8Data[2];

                        u16ClusterId = au8Data[4];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[5];

                        u16AttribId = au8Data[6];
                        u16AttribId <<= 8;
                        u16AttribId |= au8Data[7];

                        u16AttributeSize = au8Data[10];
                        u16AttributeSize <<= 8;
                        u16AttributeSize |= au8Data[11];

                        strData += " (Write Attrib Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Src Addr: 0x" + u16SrcAddr.ToString("X4");
                        strData += "\n";
                        strData += "  Src Ep: 0x" + au8Data[3].ToString("X2");
                        strData += "\n";
                        displayClusterId(u16ClusterId);
                        strData += "\n";
                        displayAttribute(u16AttribId, au8Data[9], au8Data, 12, u16AttributeSize);
                        strData += "  Status: 0x" + au8Data[8].ToString("X2");
                        strData += "\n";
                    }
                    break;

               

                case 0x8601:
                    {
                        strData += " (Restore Network Recovery Response)";
                        strData += "\n";
                        strData += "  Success = " + au8Data[0];
                        strData += "\n";
                    }
                    break;

                case 0x80A4:
                    {
                        UInt16 u16GroupId;
                        UInt16 u16ClusterId;

                        u16ClusterId = au8Data[2];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[3];

                        u16GroupId = au8Data[5];
                        u16GroupId <<= 8;
                        u16GroupId |= au8Data[6];

                        strData += " (Store Scene Response)";
                        strData += "\n";
                        strData += "  Tx Num: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Source Endpoint: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Cluster ID: 0x" + u16ClusterId.ToString("X4");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[4].ToString("X2");
                        strData += "\n";
                        strData += "  Group ID: 0x" + u16GroupId.ToString("X4");
                        strData += "\n";
                        strData += "  Scene ID: 0x" + au8Data[7].ToString("X2");
                        strData += "\n";
                    }
                    break;

                case 0x80A1:
                    {
                        UInt16 u16GroupId;
                        UInt16 u16ClusterId;

                        u16ClusterId = au8Data[2];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[3];

                        u16GroupId = au8Data[5];
                        u16GroupId <<= 8;
                        u16GroupId |= au8Data[6];

                        strData += " (Add Scene Response)";
                        strData += "\n";
                        strData += "  Tx Num: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Source Endpoint: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Cluster ID: 0x" + u16ClusterId.ToString("X4");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[4].ToString("X2");
                        strData += "\n";
                        strData += "  Group ID: 0x" + u16GroupId.ToString("X4");
                        strData += "\n";
                        strData += "  Scene ID: 0x" + au8Data[7].ToString("X2");
                        strData += "\n";
                    }
                    break;

                case 0x80A6:
                    {
                        UInt16 u16GroupId;
                        UInt16 u16ClusterId;

                        u16ClusterId = au8Data[2];
                        u16ClusterId <<= 8;
                        u16ClusterId |= au8Data[3];

                        u16GroupId = au8Data[6];
                        u16GroupId <<= 8;
                        u16GroupId |= au8Data[7];

                        strData += " (Get Scene Membership Response)";
                        strData += "\n";
                        strData += "  Tx Num: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Source Endpoint: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Cluster ID: 0x" + u16ClusterId.ToString("X4");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[4].ToString("X2");
                        strData += "\n";
                        strData += "  Capacity: 0x" + au8Data[5].ToString("X2");
                        strData += "\n";
                        strData += "  Group ID: 0x" + u16GroupId.ToString("X4");
                        strData += "\n";
                        strData += "  Scene Count: 0x" + au8Data[8].ToString("X2");

                        if (au8Data[8] != 0)
                        {
                            strData += "\n";
                            strData += "  Scene List: ";
                        }

                        byte i;

                        for (i = 0; i < au8Data[8]; i++)
                        {

                            strData += "\n";
                            strData += "    Scene: 0x" + au8Data[i + 9].ToString("X2");
                        }
                        strData += "\n";
                    }
                    break;
                case 0x8046:
                    {
                        UInt16 u16AddrOfInterest;

                        u16AddrOfInterest = au8Data[2];
                        u16AddrOfInterest <<= 8;
                        u16AddrOfInterest |= au8Data[3];

                        strData += " (Match Descriptor Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Address Of Interest: 0x" + u16AddrOfInterest.ToString("X4");
                        strData += "\n";
                        strData += "  Match Length: " + au8Data[4];

                        if (au8Data[4] != 0)
                        {
                            strData += "  Matched Endpoints: ";
                        }

                        byte i;
                        for (i = 0; i < au8Data[4]; i++)
                        {
                            strData += "\n";
                            strData += "    Endpoint " + au8Data[5 + i].ToString("X2");
                            strData += "\n";
                        }
                    }
                    break;
                case 0x8044:
                    {
                        strData += " (Power Descriptor Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Power Source Level: " + Convert.ToString(au8Data[2] & 0x7, 2).PadLeft(4, '0');
                        strData += "\n";
                        strData += "  Current Power Source: " + Convert.ToString((au8Data[2] >> 4) & 0x7, 2).PadLeft(4, '0');
                        strData += "\n";
                        strData += "  Available Power Source: " + Convert.ToString((au8Data[3]) & 0x7, 2).PadLeft(4, '0');
                        strData += "\n";
                        strData += "  Current Power Mode: " + Convert.ToString((au8Data[3] >> 4) & 0x7, 2).PadLeft(4, '0');
                        strData += "\n";
                    }
                    break;

                case 0x8701:
                    {
                        strData += " (Route Discovery Confirm)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Network Status: 0x" + au8Data[2].ToString("X2");
                        strData += "\n";
                    }
                    break;
                case 0x8702:
                    {
                        UInt16 u16DestAddr;

                        u16DestAddr = au8Data[4];
                        u16DestAddr <<= 8;
                        u16DestAddr |= au8Data[5];

                        strData += " (APS Data Confirm Fail)";
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Source Endpoint: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Destination Endpoint: 0x" + au8Data[2].ToString("X2");
                        strData += "\n";
                        strData += "  Destination Mode: 0x" + au8Data[3].ToString("X2");
                        strData += "\n";
                        strData += "  Destination Address: 0x" + u16DestAddr.ToString("X4");
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[6].ToString("X2");
                        strData += "\n";
                    }
                    break;

                case 0x8531:
                    {
                        UInt16 u16AddressOfInterest;

                        u16AddressOfInterest = au8Data[2];
                        u16AddressOfInterest <<= 8;
                        u16AddressOfInterest |= au8Data[3];

                        strData += " (Complex Descriptor Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Address of Interest: 0x" + u16AddressOfInterest.ToString("X2");
                        strData += "\n";
                        strData += "  Length: " + au8Data[4].ToString("X2");
                        strData += "\n";

                        if (au8Data[1] == 0)
                        {
                            strData += "        XML Tag: " + au8Data[5].ToString("X2");
                            strData += "\n";
                            strData += "        Field Count: " + au8Data[6].ToString("X2");
                            strData += "\n";
                            strData += "        Complex Description: ";
                            for (int i = 0; i < au8Data[6]; i++)
                            {
                                char c = (char)au8Data[6 + i + 1];
                                strData += c.ToString();
                            }
                            strData += "\n";
                        }
                    }
                    break;

                case 0x8532:
                    {
                        byte u8StrLen;
                        UInt16 u16NwkAddr = 0;

                        u16NwkAddr = au8Data[2];
                        u16NwkAddr <<= 8;
                        u16NwkAddr |= au8Data[3];
                        u8StrLen = au8Data[4];

                        strData += " (User Descriptor Request Response)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Nwk Address: 0x" + u16NwkAddr.ToString("X4");
                        strData += "\n";

                        if (au8Data[1] == 0)
                        {
                            strData += "  Length: " + u8StrLen.ToString();
                            strData += "\n";
                            strData += "  Descriptor: ";

                            for (int i = 0; i < u8StrLen; i++)
                            {
                                char c = (char)au8Data[5 + i];
                                strData += c.ToString();
                            }
                            strData += "\n";
                        }
                    }
                    break;

                case 0x8533:
                    {
                        byte u8StrLen;
                        UInt16 u16NwkAddr = 0;

                        u16NwkAddr = au8Data[2];
                        u16NwkAddr <<= 8;
                        u16NwkAddr |= au8Data[3];
                        u8StrLen = au8Data[4];

                        strData += " (User Descriptor Set Confirm)";
                        strData += "\n";
                        strData += "  SQN: 0x" + au8Data[0].ToString("X2");
                        strData += "\n";
                        strData += "  Status: 0x" + au8Data[1].ToString("X2");
                        strData += "\n";
                        strData += "  Nwk Address: 0x" + u16NwkAddr.ToString("X4");
                        strData += "\n";
                    }
                    break;


                default:
                    {
                        strData += " (Unrecognized)";
                        strData += "\n";
                    }
                    break;
            }

            strData += "  LQI: 0x" + au8Data[u16Length - 1].ToString("X2");
            strData += "\n";
        }




        /*private void displayRawCommandData(UInt16 u16Type, UInt16 u16Length, byte u8Checksum, byte[] au8Data)
        {
            byte tempByte;

            if (((u16Type != 0x8000) && (u16Type != 0x8501) && (u16Type != 0x0502)))
            {
                

                if (u16Type != 0x8501)
                {
                    tempByte = (byte)(u16Type >> 8);
                    
                    tempByte = (byte)u16Type;
                   

                    tempByte = (byte)(u16Length >> 8);
                  
                    tempByte = (byte)u16Length;
                  

                }
            }
        }
        */
        private void vPermitJoin()
        {
            UInt16 u16ShortAddr;
            byte u8Interval;

            if (bStringToUint16("FFFC", out u16ShortAddr) == true)
            {
                if (bStringToUint8("FC", out u8Interval) == true)
                {
                    setPermitJoin((UInt16)u16ShortAddr, u8Interval, (byte)0);
                }
            }
        }


        private bool bStringToUint16(string inputString, out UInt16 u16Data)
        {
            bool bResult = true;

            if (UInt16.TryParse(inputString, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out u16Data) == false)
            {
                // Show error message
                MessageBox.Show("Invalid Parameter");
                bResult = false;
            }
            return bResult;
        }


        private void setPermitJoin(UInt16 u16ShortAddr, byte u8Interval, byte u8TCsignificance)
        {
            byte[] commandData = null;
            commandData = new byte[4];

            // Build command payload
            commandData[0] = (byte)(u16ShortAddr >> 8);
            commandData[1] = (byte)u16ShortAddr;
            commandData[2] = u8Interval;
            commandData[3] = u8TCsignificance;

            // Transmit command
            transmitCommand(0x0049, 4, commandData);
        }

        private void StartNWK()
        {
            transmitCommand(0x0024, 0, null);
        }


        private bool bStringToUint8(string inputString, out byte u8Data)
        {
            bool bResult = true;

            if (Byte.TryParse(inputString, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out u8Data) == false)
            {
                // Show error message
                MessageBox.Show("Invalid Parameter");
                bResult = false;
            }
            return bResult;
        }

        private void displayClusterId(UInt16 u16ClusterId)
        {
            Dictionary<int, string> clusterList = new Dictionary<int, string>();

            clusterList.Add(0x0006, " (General: On/Off)");
            clusterList.Add(0x0007, " (General: On/Off Config)");


            //strData += "  Cluster ID: 0x" + u16ClusterId.ToString("X4");
            strData += "Cluster ID: 0x" + u16ClusterId.ToString("X4");

            // The indexer throws an exception if the requested key is 
            // not in the dictionary. 
            try
            {
                strData += clusterList[u16ClusterId];
            }
            catch (KeyNotFoundException)
            {
                //strData += " (Unknown)";
                strData += " (Unknown)";
            }

            vLog(strData);
        }
        private void displayAttribute(UInt16 u16AttribId, byte u8AttribType, byte[] au8AttribData, byte u8AttribIndex, UInt16 u16AttrSize)
        {
            strData += "  Attribute ID: 0x" + u16AttribId.ToString("X4");
            strData += "\n";
            strData += "  Attribute Size: 0x" + u16AttrSize.ToString("X4");
            strData += "\n";
            strData += "  Attribute Type: 0x" + u8AttribType.ToString("X2");

            switch (u8AttribType)
            {
                case 0x10:
                    strData += " (Boolean)";
                    strData += "\n";
                    strData += "  Attribute Data: 0x" + au8AttribData[u8AttribIndex].ToString("X2");
                    strData += "\n";
                    break;
                case 0x18:
                    strData += " (8-bit Bitmap)";
                    strData += "\n";
                    strData += "  Attribute Data: 0x" + au8AttribData[u8AttribIndex].ToString("X2");
                    strData += "\n";
                    break;
                case 0x20:
                    strData += " (UINT8)";
                    strData += "\n";
                    strData += "  Attribute Data: 0x" + au8AttribData[u8AttribIndex].ToString("X2");
                    strData += "\n";
                    break;
                case 0x21:
                    UInt16 u16Data;
                    u16Data = au8AttribData[u8AttribIndex];
                    u16Data <<= 8;
                    u16Data |= au8AttribData[u8AttribIndex + 1];
                    strData += " (UINT16)";
                    strData += "\n";
                    strData += "  Attribute Data: 0x" + u16Data.ToString("X4");
                    strData += "\n";
                    break;
                case 0x23:
                    UInt32 u32Data;
                    u32Data = au8AttribData[u8AttribIndex];
                    u32Data <<= 8;
                    u32Data |= au8AttribData[u8AttribIndex + 1];
                    u32Data <<= 8;
                    u32Data |= au8AttribData[u8AttribIndex + 2];
                    u32Data <<= 8;
                    u32Data |= au8AttribData[u8AttribIndex + 3];
                    strData += " (UINT32)";
                    strData += "\n";
                    strData += "  Attribute Data: 0x" + u32Data.ToString("X8");
                    strData += "\n";
                    break;
                case 0x29:
                    strData += " (INT16)";
                    strData += "\n";
                    break;
                case 0x30:
                    strData += " (8-bit Enumeration)";
                    strData += "\n";
                    strData += "  Attribute Data: 0x" + au8AttribData[u8AttribIndex].ToString("X2");
                    strData += "\n";
                    break;
                case 0x42:
                    strData += " (Character String)";
                    strData += "\n";
                    strData += "  Attribute Data (Len - " + u16AttrSize.ToString() + "): ";
                    for (int i = 0; i < u16AttrSize; i++)
                    {
                        char c = (char)au8AttribData[u8AttribIndex + i];
                        strData += c.ToString();
                    }
                    strData += "\n";
                    break;
                case 0xF0:
                    strData += " (IEEE Address)";
                    strData += "\n";
                    strData += "  Attribute Data: " + au8AttribData[u8AttribIndex].ToString("X2");
                    strData += ":" + au8AttribData[u8AttribIndex + 1].ToString("X2");
                    strData += ":" + au8AttribData[u8AttribIndex + 2].ToString("X2");
                    strData += ":" + au8AttribData[u8AttribIndex + 3].ToString("X2");
                    strData += ":" + au8AttribData[u8AttribIndex + 4].ToString("X2");
                    strData += ":" + au8AttribData[u8AttribIndex + 5].ToString("X2");
                    strData += ":" + au8AttribData[u8AttribIndex + 6].ToString("X2");
                    strData += ":" + au8AttribData[u8AttribIndex + 7].ToString("X2");
                    strData += "\n";
                    break;
                default:
                    strData += " (Unknown)";
                    strData += "\n";
                    break;
            }
        }

        private void srcEndPointTextBoxInit(ref TextBox textBox)
        {
            //textBox.ForeColor = System.Drawing.Color.Gray;
            textBox.Text = "1";
        }


        private void btOnOff_Click(object sender, EventArgs e)
        {

            //strData = "-------------------------ON/OFF---------------------------";
            UInt16 u16ShortAddr;
            byte u8SrcEndPoint;
            byte u8DstEndPoint;
            if (bStringToUint16(textBoxOnOffAddr.Text, out u16ShortAddr) == true)
            {
                
                if (bStringToUint8("1", out u8SrcEndPoint) == true)
                {
                    if (bStringToUint8("1", out u8DstEndPoint) == true)
                    {
                        
                        Thread.Sleep(10);
                        sendClusterOnOff((byte)comboBoxOnOffAddrMode.SelectedIndex, u16ShortAddr, u8SrcEndPoint, u8DstEndPoint, (byte)comboBoxOnOffCommand.SelectedIndex);
                        tbStatusButton1.Text = "PASSED";
                        tbStatusButton1.BackColor = System.Drawing.Color.Lime;
                        strData = "PASSED \n";
                    }
                    else
                    {
                        tbStatusButton1.Text = "FAILED";
                        tbStatusButton1.BackColor = System.Drawing.Color.Lime;
                        strData = "FAILED \n";
                    }

                    if (bStringToUint8("2", out u8DstEndPoint) == true)
                    {
                        Thread.Sleep(10);
                        sendClusterOnOff((byte)comboBoxOnOffAddrMode.SelectedIndex, u16ShortAddr, u8SrcEndPoint, u8DstEndPoint, (byte)comboBoxOnOffCommand.SelectedIndex);
                        tbStatusButton2.Text = "PASSED";
                        tbStatusButton2.BackColor = System.Drawing.Color.Lime;
                        strData = "PASSED \n";
                    }
                    else
                    {
                        tbStatusButton2.Text = "FAILED";
                        tbStatusButton2.BackColor = System.Drawing.Color.Lime;
                        strData = "FAILED";
                    }

                    if (bStringToUint8("3", out u8DstEndPoint) == true)
                    {
                        Thread.Sleep(10);
                        sendClusterOnOff((byte)comboBoxOnOffAddrMode.SelectedIndex, u16ShortAddr, u8SrcEndPoint, u8DstEndPoint, (byte)comboBoxOnOffCommand.SelectedIndex);
                        tbStatusButton3.Text = "PASSED";
                        tbStatusButton3.BackColor = System.Drawing.Color.Lime;
                        strData = "PASSED \n";
                    }
                    else
                    {
                        tbStatusButton3.Text = "FAILED";
                        tbStatusButton3.BackColor = System.Drawing.Color.Lime;
                        strData = "FAILED \n";
                    }

                    if (bStringToUint8("4", out u8DstEndPoint) == true)
                    {
                        Thread.Sleep(10);
                        sendClusterOnOff((byte)comboBoxOnOffAddrMode.SelectedIndex, u16ShortAddr, u8SrcEndPoint, u8DstEndPoint, (byte)comboBoxOnOffCommand.SelectedIndex);
                        tbStatusButton4.Text = "PASSED";
                        tbStatusButton4.BackColor = System.Drawing.Color.Lime;
                        strData = "PASSED \n";
                    }
                    else
                    {
                        tbStatusButton4.Text = "FAILED \n";
                        tbStatusButton4.BackColor = System.Drawing.Color.Lime;

                    }

                }

            }
        }
        private void sendClusterOnOff(byte u8AddrMode, UInt16 u16ShortAddr, byte u8SrcEndPoint, byte u8DstEndPoint, byte u8CommandID)
        {
            byte[] commandData = null;
            commandData = new byte[6];

            // Build command payload
            commandData[0] = u8AddrMode;
            commandData[1] = (byte)(u16ShortAddr >> 8);
            commandData[2] = (byte)u16ShortAddr;
            commandData[3] = u8SrcEndPoint;
            commandData[4] = u8DstEndPoint;
            commandData[5] = u8CommandID;

            // Transmit command
            transmitCommand(0x0092, 6, commandData);
        }




        private void textBoxOnOffSrcEndPoint_TextChanged(object sender, EventArgs e)
        {
            textBoxOnOffSrcEndPoint.Enabled = false;
        }



        private void GUIinitilize()
        {
            addrModeComboBoxZCLInit(ref comboBoxOnOffAddrMode);
            shortAddrTextBoxInit(ref textBoxOnOffAddr);
            // dstEndPointTextBoxInit(ref textBoxOnOffDstEndPoint);
            srcEndPointTextBoxInit(ref textBoxOnOffSrcEndPoint);
            comboBoxOnOffCommand.Items.Add("Off");
            comboBoxOnOffCommand.Items.Add("On");
            comboBoxOnOffCommand.Items.Add("Toggle");
            comboBoxOnOffCommand.SelectedIndex = 2;
        }

        private void btRefeshTest_Click(object sender, EventArgs e)
        {
            serialPort1.Close();
            tbStatusPermitJoin.Text = "";
            tbStatusPermitJoin.BackColor = Color.White;
            cbBaud.Text = "";
            cbPort.Text = "";
            vPortTest();
            tbStatusButton1.Text = "IDLE";
            tbStatusButton1.BackColor = System.Drawing.Color.Teal;
            tbStatusButton2.Text = "IDLE";
            tbStatusButton2.BackColor = System.Drawing.Color.Teal;
            tbStatusButton3.Text = "IDLE";
            tbStatusButton3.BackColor = System.Drawing.Color.Teal;
            tbStatusButton4.Text = "IDLE";
            tbStatusButton4.BackColor = System.Drawing.Color.Teal;
            tbStatusDevice.Text = "";
            tbStatusDevice.BackColor = System.Drawing.Color.White;
            textBoxOnOffAddr.Text = "";
            tbAddress.Text = "";
            tbVersion.Text = "";
            btStart.Text = "BẮT ĐẦU (2)";
            if(cbPort.Text!= "")
            {
                lbConnectTesst.Text = cbPort.Text + " - ĐANG HOẠT ĐỘNG";
            }    
            else
            {
                lbConnectTesst.Text = " COM KHÔNG ĐƯỢC KẾT NỐI";
            }    
            //richTextBoxCommandResponse.Text = "";
            //strData = "";
            cbBaud.Items.AddRange(new object[] { "9600", "19200", "38400", "115200", "250000", "500000", "1000000" });
            cbBaud.Text = "115200";
        }

      

      

        private void vGetProductString(int count)
        {
            int device = count;
            Int32 numOfDevices = 0;
            Thread.Sleep(200);
          
            IntPtr handle = IntPtr.Zero;
            //CP210x.CP210x.Reset(handle);
            CP210x.CP210x.GetNumDevices(ref numOfDevices);
            //StringBuilder productString = new StringBuilder();
            byte length = 0;


            Byte[] prtNum = new Byte[1];
            UInt16[] latch = new UInt16[4];
            //UInt16 mask = 0x01;

            CP210x.CP210x.Open(device, ref handle);
            //CP210x.CP210x.GetPartNumber(handle, prtNum);
            CP210x.CP210x.GetDeviceProductString(handle, productString, ref length, true);
           
            
            CP210x.CP210x.Close(handle);
            strData += "-----ID Config-----\n";
            strData += DateTime.Now.ToString("yyyy/MM/dd_hh-mm-ss\n");
            strData += "ID: ";
            strData += productString.ToString() + " \n";
            strData += "-----END-------\n";
           
            vLog_flash(strData);
        }

        // Check verify click
        private void cbVerify_CheckedChanged(object sender, EventArgs e)
        {
            if (cbVerify.Checked == true)
            {
                cbProtect.Enabled = false;

            }
            else
            {
                cbProtect.Enabled = true;

            }
        }


        // Select baurate 4 device
       
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void bErasePDM_Click(object sender, EventArgs e)
        {
            vResetZigbee();
            Thread.Sleep(1000);
            vErasePD();

            DialogResult result = MessageBox.Show("Khởi động lại thiết bị để có hiệu lực", "Confirmation", MessageBoxButtons.OK);
            if(result == DialogResult.OK)
            {
                //serialPort1.Close();
                tbStatusPermitJoin.Text = "";
                tbStatusPermitJoin.BackColor = Color.White;
                cbBaud.Text = "";
                cbPort.Text = "";
                //vPortTest();
                tbStatusButton1.Text = "IDLE";
                tbStatusButton1.BackColor = System.Drawing.Color.Teal;
                tbStatusButton2.Text = "IDLE";
                tbStatusButton2.BackColor = System.Drawing.Color.Teal;
                tbStatusButton3.Text = "IDLE";
                tbStatusButton3.BackColor = System.Drawing.Color.Teal;
                tbStatusButton4.Text = "IDLE";
                tbStatusButton4.BackColor = System.Drawing.Color.Teal;
                tbStatusDevice.Text = "";
                tbStatusDevice.BackColor = System.Drawing.Color.White;
                textBoxOnOffAddr.Text = "";
                tbAddress.Text = "";
                tbVersion.Text = "";
                btStart.Text = "BẮT ĐẦU (2)";
                if (cbPort.Text != "")
                {
                    lbConnectTesst.Text = cbPort.Text + " - ĐANG HOẠT ĐỘNG";
                }
                else
                {
                    lbConnectTesst.Text = " COM KHÔNG ĐƯỢC KẾT NỐI";
                }
                //richTextBoxCommandResponse.Text = "";
                //strData = "";
                cbBaud.Items.AddRange(new object[] { "9600", "19200", "38400", "115200", "250000", "500000", "1000000" });
                cbBaud.Text = "115200";
            }    
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using (Process process = new Process())
            {
                process.StartInfo.FileName = @"C:\NXP\ProductionFlashProgrammer\Huong dan\PPKT Cong tac cam ung Ver2.3-02-V01-220110-IoT-thanhnq2.pdf";

                process.Start();

            }
        }


        // Check Option USB flash
        private void cbOptionUSB_CheckedChanged(object sender, EventArgs e)
        {

        }

       
        // Process Select Protect 
        private void cbProtect_CheckedChanged(object sender, EventArgs e)
        {
            if (cbProtect.Checked == true)
            {
                string message = "Xác nhận trước khi chống đọc chip";
                string title = " Confirm";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    MessageBox.Show(" Thiết bị sẵn sàng");
                    cbVerify.Enabled = false;

                }
                else
                {
                    MessageBox.Show("Vui lòng xác nhận trước khi chống đọc chip");
                    cbVerify.Checked = true;
                    cbProtect.Checked = false;
                    cbVerify.Enabled = true;

                }
                //cbVerify.Enabled = false;
                //cbOTP.Enabled = false;
            }
            else
            {
                cbVerify.Enabled = true;

            }
        }

       

        // Process refresh Zigbee Flash code
        private void btRefresh_Click(object sender, EventArgs e)
        {
           
           
          
            vGetPort_Refresh();
           

        }

       

    }
}
