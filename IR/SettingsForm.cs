using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IR
{
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            settings.crawlTimeoutSeconds = (int)nudMaxTimeoutCrawl.Value;
            settings.maxConcurrentThreads = (int)nudMaxConcurrentThreads.Value;
            //settings.maxPagesToCrawl = (int)nudMaxPagesCrawl.Value;
            settings.maxPagesToCrawl = (int)nudAuction.Value;
            settings.maxPageSizeInBytes = (int)nudMaxSizePage.Value;
            settings.userAgentString = txtUserAgents.Text;
            settings.crawlTimeoutSeconds = (int)nudMaxTimeoutCrawl.Value;
            settings.httpRequestTimeoutInSeconds = (int)nudMaxHttpTimeout.Value;
            settings.minCrawlDelayPerDomainMilliSeconds = (int)nudMinDelayCrawl.Value;
            settings.auction = (int)nudAuction.Value;
            this.DialogResult = DialogResult.OK;
        }

        private void SettingsForm_Shown(object sender, EventArgs e)
        {
            nudMaxTimeoutCrawl.Value = settings.crawlTimeoutSeconds;
            nudMaxConcurrentThreads.Value = settings.maxConcurrentThreads;
            //nudMaxPagesCrawl.Value = settings.maxPagesToCrawl;
            nudMaxPagesCrawl.Value = settings.auction;
            nudMaxSizePage.Value = settings.maxPageSizeInBytes;
            txtUserAgents.Text = settings.userAgentString;
            nudMaxTimeoutCrawl.Value = settings.crawlTimeoutSeconds;
            nudMaxHttpTimeout.Value = settings.httpRequestTimeoutInSeconds;
            nudMinDelayCrawl.Value = settings.minCrawlDelayPerDomainMilliSeconds;
            nudAuction.Value=settings.auction ;
        }
    }
}
