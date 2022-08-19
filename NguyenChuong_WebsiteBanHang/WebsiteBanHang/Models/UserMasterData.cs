using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebsiteBanHang.Models
{
    public class UserMasterData
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Tên người dùng")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Họ người dùng")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Email người dùng")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Mật Khẩu người dùng")]
        public string Password { get; set; }
        public Nullable<bool> IsAdmin { get; set; }
    }
}