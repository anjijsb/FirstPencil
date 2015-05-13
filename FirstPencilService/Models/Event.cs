using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstPencilService.Models
{
    class ScanEvent
    {
        [Key]
        public int EventId { get; set; }

        public string EventKey { get; set; }

        public bool? IsActive { get; set; }

        public string CodeContent { get; set; }

        public string CodeUrl { get; set; }

        public EventType Type { get; set; }

        public CodeType CodeType { get; set; }

        public string Remarks { get; set; }

    }

    enum EventType
    {
        Unknown = 0,
        AddSalesman = 1,
    }

    enum CodeType
    {
        Unknown = 0,
        /// <summary>
        /// 临时二维码
        /// </summary>
        QR_SCENE = 1,
        /// <summary>
        /// 永久二维码
        /// </summary>
        QR_LIMIT_SCENE = 2,
    }
}
