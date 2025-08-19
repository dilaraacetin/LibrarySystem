using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DXApplication4.Module.BusinessObjects  
{
    [DefaultClassOptions]
    [NavigationItem("Library")]
    [XafDisplayName("Book")]
    public class Book : BaseObject  
    {
        [Required, StringLength(256)]
        public virtual string Title { get; set; }

        [StringLength(256)]
        public virtual string Author { get; set; }

        [StringLength(32)]
        public virtual string Isbn { get; set; }

        public virtual DateTime? PublishedOn { get; set; }

    }
}
