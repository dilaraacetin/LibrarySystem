using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Model;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using System.Collections.ObjectModel;


using DARequired = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace DXApplication4.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Library")]
    [XafDisplayName("Member")]
    [XafDefaultProperty(nameof(AdSoyad))]
    public class Member : BaseObject
    {
        [XafDisplayName("Ad")]
        [DARequired, StringLength(128)]
        public virtual string Ad { get; set; }

        [XafDisplayName("Soyad")]
        [DARequired, StringLength(128)]
        public virtual string Soyad { get; set; }

        [XafDisplayName("E-posta")]
        [StringLength(256)]
        [EmailAddress(ErrorMessage = "Geçerli bir e-posta girin.")]
        [RuleUniqueValue("Member.Email.Unique", DefaultContexts.Save,
            CustomMessageTemplate = "Bu e-posta zaten kayıtlı.")]
        public virtual string Email { get; set; }

        [XafDisplayName("Telefon")]
        [StringLength(32)]
        [ModelDefault("EditMask", "+90 (000) 000 00 00")]  
        [ModelDefault("DisplayFormat", "{0}")]
        public virtual string Telefon { get; set; }

        [NotMapped]
        public string AdSoyad => $"{Ad} {Soyad}".Trim();

        public virtual ObservableCollection<Loan> Loans { get; set; }
            = new ObservableCollection<Loan>();
    }
}
