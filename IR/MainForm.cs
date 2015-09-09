using Abot.Core;
using Abot.Crawler;
using Abot.Poco;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IR
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(!backgroundWorker.IsBusy)
            {
                dataGridView1.Rows.Clear();
                progressBar1.Maximum = settings.auction;
                backgroundWorker.RunWorkerAsync();
                progressBar1.Value = 0;
                btnStart.Enabled = false;
            }
            else
            {
                MessageBox.Show("لطفا منتظر بمانید", "خطا", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        delegate void SetTextCallback(string name,string seller,string buyer, string url);

        private void SetText(string name,string seller,string buyer, string url)
        {
            if (dataGridView1.InvokeRequired)
            {
                dataGridView1.BeginInvoke(new SetTextCallback(SetText), name, seller, buyer, url);
            }
            else
            {
                dataGridView1.Rows.Add(name, seller, buyer, url);
                Application.DoEvents();
            }
        }



        public void crawler_ProcessPageCrawlCompleted(object sender, PageCrawlCompletedArgs e)
        {
            backgroundWorker.ReportProgress(1);


            string strPage = e.CrawledPage.Content.Text;

            HtmlNodeCollection aTags = e.CrawledPage.HtmlDocument.DocumentNode.SelectNodes("//h1");
            string name = "", seller = "", buyer = "";
            if (aTags != null && aTags.Count > 0)
            {
                name = aTags[0].InnerText;

            }

            HtmlNodeCollection aTable = e.CrawledPage.HtmlDocument.DocumentNode.SelectNodes("//table[@id='auction-bidhist']");
            HtmlNodeCollection aTr = e.CrawledPage.HtmlDocument.DocumentNode.SelectNodes("//table[@id='auction-bidhist']//tr");
            //int index = strPage.IndexOf("col_half col_last single-product");
            //int devEnd = strPage.IndexOf("</h1>", index);
            //string str = strPage.Substring(index, devEnd - index);
            //str = str.Substring(str.IndexOf("<h1>") + 4);

            HtmlNodeCollection sellerA = e.CrawledPage.HtmlDocument.DocumentNode.SelectNodes("//h4//a");
            if (sellerA != null && sellerA.Count > 0)
            {
                seller = sellerA[0].InnerText;
            }

            if (aTr != null && aTr[1].InnerText.IndexOf("No") == -1)
            {
                if (aTr[0].InnerText.IndexOf("Winning") == -1)
                {
                    HtmlNodeCollection abbTr = aTr[1].SelectNodes("//td//a");
                    buyer = abbTr[0].InnerText;
                }
            }


            if (name.Trim().Length > 0)
            {
                SetText(name, seller, buyer, e.CrawledPage.Uri.ToString());

            }
           
            //Console.WriteLine(e.CrawledPage.Content);
            //Process data
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            CrawlConfiguration crawlConfig = AbotConfigurationSectionHandler.LoadFromXml().Convert();
            crawlConfig.MaxConcurrentThreads = settings.maxConcurrentThreads;
            crawlConfig.MaxPagesToCrawl = settings.maxPagesToCrawl;
            crawlConfig.MaxPageSizeInBytes = settings.maxPageSizeInBytes;
            crawlConfig.UserAgentString = settings.userAgentString;
            crawlConfig.CrawlTimeoutSeconds = settings.crawlTimeoutSeconds;
            crawlConfig.HttpRequestTimeoutInSeconds = settings.httpRequestTimeoutInSeconds;
            crawlConfig.MinCrawlDelayPerDomainMilliSeconds = settings.minCrawlDelayPerDomainMilliSeconds;


            IWebCrawler crawler;
            crawler = new MyWebCraweler(crawlConfig, null, null, new MadBidScheduler(settings.auction), null, null, null, null, null);
            crawler.PageCrawlCompletedAsync += crawler_ProcessPageCrawlCompleted;
            CrawlResult result = crawler.Crawl(new Uri("http://us.ebid.net/for-sale/a-141378296.htm"));
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = progressBar1.Value + 1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
           
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void تنظیماتToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SettingsForm frm = new SettingsForm();
            frm.ShowDialog();
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            btnStart.Enabled = true;
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            GraphForm gf = new GraphForm(dataGridView1);
            gf.ShowDialog();
        }

        private void ذخیرهجدولToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(saveFileDialog1.ShowDialog()==DialogResult.OK)
            {
                DataTable dT = GetDataTableFromDGV(dataGridView1);
                DataSet dS = new DataSet();
                dS.Tables.Add(dT);
                dS.WriteXml(File.OpenWrite(saveFileDialog1.FileName));
            }

        }

        private DataTable GetDataTableFromDGV(DataGridView dgv)
        {
            var dt = new DataTable();
            foreach (DataGridViewColumn column in dgv.Columns)
            {
                if (column.Visible)
                {
                    // You could potentially name the column based on the DGV column name (beware of dupes)
                    // or assign a type based on the data type of the data bound to this DGV column.
                    dt.Columns.Add();
                }
            }

            object[] cellValues = new object[dgv.Columns.Count];
            foreach (DataGridViewRow row in dgv.Rows)
            {
                for (int i = 0; i < row.Cells.Count; i++)
                {
                    cellValues[i] = row.Cells[i].Value;
                }
                dt.Rows.Add(cellValues);
            }

            return dt;
        }

        private void بازیابیجدولToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.Rows.Clear();
                DataSet dS = new DataSet();
                dS.ReadXml(File.OpenRead(openFileDialog1.FileName));
                for (int i = 0; i < dS.Tables[0].Rows.Count; i++)
                {
                    dataGridView1.Rows.Add(dS.Tables[0].Rows[i].ItemArray.GetValue(0).ToString(),
                        dS.Tables[0].Rows[i].ItemArray.GetValue(1).ToString(),
                        dS.Tables[0].Rows[i].ItemArray.GetValue(2).ToString(),
                        dS.Tables[0].Rows[i].ItemArray.GetValue(3).ToString());
                }
            }


           

        }

    }
}
