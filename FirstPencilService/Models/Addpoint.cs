using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPencilService.Models
{
    class AddPoint
    {
        [Key]
        public int AddPointId { get; set; }

        public AddPointType Type { get; set; }

        public int? Count { get; set; }

        public string Remarks { get; set; }
    }

    public enum AddPointType
    {
        Unknown = 0,
        /// <summary>
        /// 添加投诉
        /// </summary>
        AddComplain = 1,
        /// <summary>
        /// 查询二维码
        /// </summary>
        QueryCode = 2,
        /// <summary>
        /// 注册供应商
        /// </summary>
        RegisterSalesman=3,

    }
}
