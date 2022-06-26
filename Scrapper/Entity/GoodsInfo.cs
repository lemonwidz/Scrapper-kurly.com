using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapper.Entity
{
    public class GoodsInfo
    {
        /// <summary>
        /// 상품명
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 판매가격
        /// </summary>
        public int Price { get; set; }
        /// <summary>
        /// 썸네일 이미지 URL
        /// </summary>
        public string ThumbnailURL { get; set; }
        /// <summary>
        /// 상품 상세 URL
        /// </summary>
        public string URL { get; set; }
        /// <summary>
        /// 한줄 설명
        /// </summary>
        public string Desc { get; set; }
        /// <summary>
        /// 상품 상세 HTML
        /// </summary>
        public string DescDetailHTML { get; set; }
        /// <summary>
        /// 스크래핑 진행 상태
        /// </summary>
        public SCRAPPING_STATUS ScrappingStatus { get; set; } = SCRAPPING_STATUS.READY;
    }
}
