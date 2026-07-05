using ArtGallery.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArtGallery.ViewModels
{
    public class UserRoleViewModel
    {
        public UserRoleViewModel()
        {
            UserList = new List<tblUser>();
        }

        [Required(ErrorMessage = "Please select a user")]
        [Display(Name = "User")]
        public int UserId { get; set; }

        public string UserName { get; set; }

        public int RoleId { get; set; }

        [Required(ErrorMessage = "Role name is required")]
        [Display(Name = "Role Name")]
        public string RoleName { get; set; }

        public ICollection<tblUser> UserList { get; set; }
    }
}