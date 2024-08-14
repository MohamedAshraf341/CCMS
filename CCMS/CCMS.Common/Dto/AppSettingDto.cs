using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Common.Dto
{
    public class AppSettingDto
    {
        public Guid Id { get; set; }
        public string? Key { get; set; }
        public string? Value { get; set; }
        public string? ValueType { get; set; }
    }
}
