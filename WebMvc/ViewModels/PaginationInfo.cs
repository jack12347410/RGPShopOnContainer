namespace WebMvc.ViewModels
{
    public class PaginationInfo
    {
        /// <summary>
        /// 商品總數
        /// </summary>
        public int TotalItems { get; set; }
        /// <summary>
        /// 每頁的數量
        /// </summary>
        public int ItemsPerPage { get; set; }
        /// <summary>
        /// 當前第N頁
        /// </summary>
        public int ActualPage { get; set; }
        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages { get; set; }
        /// <summary>
        /// 前一頁按鈕
        /// </summary>
        public string Previous { get; set; }
        /// <summary>
        /// 下一頁按鈕
        /// </summary>
        public string Next { get; set; }
    }
}