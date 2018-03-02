using System.ComponentModel;

namespace Autyan.NiChiJou.Service.Blog.ServiceStatusCode
{
    public enum BlogUserStatus
    {
        [Description("blog user not exist!")]
        BlogUserNotExists = 2000,

        [Description("Create BlogUser Failed")]
        CreateBlogUserFailed = 3000
    }
}
