using Abot.Core;
using Abot.Poco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IR
{
    [Serializable]
    public class MadBidScheduler : IScheduler
    {
        Random rand1;
        int maxRandom = 141170520;
        int minRandom = 141004316;
        public int count=1000;
        ICrawledUrlRepository _crawledUrlRepo;
        bool _allowUriRecrawling;

        public MadBidScheduler(int count): this(false, null, null)
        {
            this.count = count;
        }

        public MadBidScheduler(bool allowUriRecrawling, ICrawledUrlRepository crawledUrlRepo, IPagesToCrawlRepository pagesToCrawlRepo)
        {
            rand1 = new Random((int)DateTime.Now.Ticks & 0x0000FFFF);
            _allowUriRecrawling = allowUriRecrawling;
            _crawledUrlRepo = crawledUrlRepo ?? new InMemoryCrawledUrlRepository();
        }

        public int Count
        {
            get { return count; }
        }

        public void Add(PageToCrawl page)
        {
            if (page == null)
                throw new ArgumentNullException("page");
            //throw new System.InvalidOperationException("dont use this method!");
        }

        public void Add(IEnumerable<PageToCrawl> pages)
        {
            if (pages == null)
                throw new ArgumentNullException("pages");

            foreach (PageToCrawl page in pages)
                Add(page);
        }

        public PageToCrawl GetNext()
        {
            int rnd = (int)((rand1.NextDouble() * (maxRandom - minRandom)) + minRandom);//296030
            Uri tempUri = new Uri("http://us.ebid.net/for-sale/a-" + rnd.ToString() + ".htm");
            //Uri tempUri = new Uri("http://us.ebid.net/for-sale/a-141378296.htm");
            while (_crawledUrlRepo.Contains(tempUri))
            {
                rnd = (int)((rand1.NextDouble() * (maxRandom - minRandom)) + minRandom);
                tempUri = new Uri("http://us.ebid.net/for-sale/a-" + rnd.ToString()+".htm");
            }
            count--;
            PageToCrawl page = new PageToCrawl(tempUri);
            page.ParentUri = new Uri("http://us.ebid.net/");
            page.CrawlDepth = 1;
            page.IsInternal = true;
            page.IsRoot = false;
            return page;
        }

        public void Clear()
        {
            count = 0;
            //throw new System.InvalidOperationException("dont use this method!");
        }
    }
}
