namespace LM.WebUI.Areas.Admin.Models
{
    public class PageInfo
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int PageCount { get; set; }

        public int ItemCount { get; set; }

        public PageInfo()
        {
            PageIndex = 1;
            PageSize = 10;
        }

        public PageInfo(int pageIndex, int pageSize, int itemCount, int pageCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            PageCount = pageCount;
            ItemCount = itemCount;
        }
    }
}