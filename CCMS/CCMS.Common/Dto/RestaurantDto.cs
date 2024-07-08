using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CCMS.Common.Dto
{
    public class RestaurantDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int BranchCount { get; set; }
    }
}
