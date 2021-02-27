using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace MonolithConect
{
    public partial class Form1 : Form
    {
        static string date = DateTime.Now.ToString("yyyy-MM-dd");
        private int counter;
        private string DesURL = "http://192.168.200.240:7038/";
        private string requestXml = "<soap:Envelope xmlns:soap=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\">" +
    "<soap:Header>" +
    "<HTNGHeader xmlns = \"http://htng.org/1.1/Header/\" >" +
    "< EX_HOTEL >" +
       " < login >" +
           "< username > p1 </ username >" +
           "< password > p1 </ password >" +
        "</ login >" +
    "</ EX_HOTEL >" +
    "</ HTNGHeader >" +
    "</ soap:Header>" +
    "<soap:Body>" +
    "<GET_EXT_INHOUSE>" +
       "<Query_Date>"+ date + "</Query_Date>" +
    "</GET_EXT_INHOUSE>" +
   " </soap:Body>" +
"</soap:Envelope>";


        public Form1()
        {
            InitializeComponent();
            InitializeTimer();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public string postXMLData(string destinationUrl, string requestXml)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(destinationUrl);
                byte[] bytes;
                bytes = System.Text.Encoding.ASCII.GetBytes(requestXml);
                request.ContentType = "text/xml; encoding='utf-8'";
                request.ContentLength = bytes.Length;
                request.Method = "POST";
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
                HttpWebResponse response;
                response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream responseStream = response.GetResponseStream();
                    string responseStr = new StreamReader(responseStream).ReadToEnd();
                    return responseStr;
                }
            
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return null;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

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
            if (counter >= 10)
            {
                // Exit loop code.  
                //timer1.Enabled = false;
                
                counter = 0;
                label1.Text = counter.ToString();
                date = DateTime.Now.ToString("yyyy-MM-dd");
                textBox1.Text = postXMLData(DesURL, requestXml);
            }
            else
            {
                // Run your procedure here.  
                // Increment counter.  
                counter = counter + 1;
                label1.Text = "Procedures Run: " + counter.ToString();
                
            }
        }

    }
}
