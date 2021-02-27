﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace MonolithConect
{
    public partial class MonolithConect : Form
    {
        private DataTable dt = new DataTable();
        static string date = DateTime.Now.ToString("yyyy-MM-dd");
        private int counter = 0;
        private int refreshTime = 10;
        private string DesURL = "http://190.211.102.10:8787/";
        private string requestXml = "<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
    "<soap:Header>" +
    "<HTNGHeader xmlns = \"http://htng.org/1.1/Header/\" >" +
    "<EX_HOTEL>" +
       " <login>" +
           "<username> p1 </username>" +
           "<password> p1 </password>" +
        "</login>" +
    "</EX_HOTEL>" +
    "</HTNGHeader>" +
    "</soap:Header>" +
    "<soap:Body>" +
    "<GET_EXT_INHOUSE>" +
       "<Query_Date>"+ date + "</Query_Date>" +
    "</GET_EXT_INHOUSE>" +
   " </soap:Body>" +
"</soap:Envelope>";


        public MonolithConect()
        {
            InitializeComponent();
            InitializeTimer();
            

        }

        public string postXMLData(string destinationUrl, string requestXml)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
                request.Timeout = -1;
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                requestStream.Close();
                Log(true);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr =  new StreamReader(responseStream).ReadToEnd();
                    string responseFinal = responseStr.Replace("\0\r", "");
                    using (StreamWriter outputFile = new StreamWriter(Path.Combine("data.xml")))
                    {
                        outputFile.WriteLine(responseFinal);
                    }
                    Log(false);
                    return responseFinal;
                }
                else
                {
                    Log(false);
                    return response.StatusCode.ToString();
                }
                
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return null;
        }
        
        private void Log(bool i)
        {
            if (i)
            {
                DataRow row = dt.NewRow();

                row["Fecha"] = date;
                row["Hora"] = DateTime.Now.ToString("HH:mm:ss"); ;
                row["Descripcion"] = "Request Send";
                dt.Rows.Add(row);
                textBoxRequest.Text = requestXml;
            }
            else if(!i)
            {
                DataRow row = dt.NewRow();

                row["Fecha"] = date;
                row["Hora"] = DateTime.Now.ToString("HH:mm:ss"); ;
                row["Descripcion"] = "Response received";
                dt.Rows.Add(row);
            }

        }

        private void InitializeTimer()
        {
            // Run this procedure in an appropriate event.  
            counter = 0;
            timer1.Interval = 1000;
            timer1.Enabled = true;
            // Hook up timer's tick event handler.  
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);

        }

        private void Timer1_Tick(object Sender, EventArgs e)
        {
            if (counter == refreshTime)
            { 
                counter = 0;
                label1.Text = counter.ToString();
                date = DateTime.Now.ToString("yyyy-MM-dd");
                textBoxResponse.Text = postXMLData(DesURL, requestXml);

            }
            else
            {
                counter = counter + 1;
                label1.Text = "Procedures Run: " + counter.ToString();
                
            }
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            String AllowedChars = @"^\d*$";
            if (Regex.IsMatch(textBoxRefresh.Text, AllowedChars))
            {
                int refresh = Int32.Parse(textBoxRefresh.Text.ToString());
                if (refresh > 0)
                {
                    refreshTime = refresh;
                }
                else
                {
                    textBoxRefresh.Text = "Use numeros positivos";
                }
            }
            else
            {
                textBoxRefresh.Text = "Use numeros positivos";
            }
            
        }

        private void MonolithConect_Load(object sender, EventArgs e)
        {
            
            dt.Columns.Add("Fecha");
            dt.Columns.Add("Hora");
            dt.Columns.Add("Descripcion");
            dataGridLog.DataSource = dt;
            textBoxResponse.Text = postXMLData(DesURL, requestXml);
        }
    }
}
