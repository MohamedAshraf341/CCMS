using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Common.Dto.Response.MenuItem
{
    public class GetMenuItems:BaseResponse
    {
        public List<MenuItemDto>? MenuItems { get; set; }
    }
}
