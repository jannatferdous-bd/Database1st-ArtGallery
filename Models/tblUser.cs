using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ArtGallery.Models
{
    public class tblUser
    {


        public tblUser()
        {
            this.tblRoles = new HashSet<tblRole>();
        }
        public int Id { get; set; }
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }
        public virtual ICollection<tblRole> tblRoles { get; set; }


    }
}