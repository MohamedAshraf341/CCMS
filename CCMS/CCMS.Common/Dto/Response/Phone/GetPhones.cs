using System.Collections.Generic;

namespace CCMS.Common.Dto.Response.Phone
{
    public class GetPhones:BaseResponse
    {
        public List<PhoneNumberDto>? PhoneNumbers { get; set; } 
    }
}
