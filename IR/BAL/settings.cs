using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IR
{
    class settings
    {
        public static int maxConcurrentThreads = 1;
        public static int maxPagesToCrawl = 10;
        public static int maxPageSizeInBytes = 0;
        public static string userAgentString = "Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko";
        public static int crawlTimeoutSeconds = 0;
        public static int httpRequestTimeoutInSeconds = 15;
        public static int minCrawlDelayPerDomainMilliSeconds = 1000;
        public static int auction = 10;
    }
}
