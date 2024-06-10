using System.Collections.Generic;
using System.Linq;

namespace CCMS.FE.UI.Services
{
    public class PageHistoryState
    {
        private List<string> previousPages;

        public PageHistoryState()
        {
            previousPages = new List<string>();
        }
        public void AddPageToHistory(string pageName)
        {
            previousPages.Add(pageName);
        }

        public string GetGoBackPage()
        {
            var page = previousPages.Last();

            previousPages = previousPages.SkipLast(1).ToList();

            return page;
        }

        public bool CanGoBack()
        {
            return previousPages.Count > 0;
        }
    }

}
