using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scrapper.Entity
{
    /// <summary>
    /// 스크래핑 진행 상태
    /// </summary>
    public enum SCRAPPING_STATUS
    {
        /// <summary>
        /// 준비
        /// </summary>
        READY = 0,
        /// <summary>
        /// 진행중
        /// </summary>
        DOING = 1,
        /// <summary>
        /// 완료
        /// </summary>
        COMPLETE = 2,
        /// <summary>
        /// 오류 발생
        /// </summary>
        ERROR = 9
    }
}
