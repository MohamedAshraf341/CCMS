namespace CCMS.Common.Dto.Response
{
    public class AddUserResponse: BaseResponse
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }

    }
}
