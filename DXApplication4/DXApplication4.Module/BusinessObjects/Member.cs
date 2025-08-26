using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EFCore;
using DevExpress.Persistent.Validation;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using DARequired = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace DXApplication4.Module.BusinessObjects
{
    [DefaultClassOptions]
    [NavigationItem("Library")]
    [XafDisplayName("Member")]
    [XafDefaultProperty(nameof(AdSoyad))]
    public class Member : BaseObject
    {
        public Member() : base() { }

        [XafDisplayName("Ad")]
        [RuleRequiredField("Member.Ad.Required", DefaultContexts.Save)]
        [DARequired, StringLength(128)]
        public virtual string Ad { get; set; }

        [XafDisplayName("Soyad")]
        [RuleRequiredField("Member.Soyad.Required", DefaultContexts.Save)]
        [DARequired, StringLength(128)]
        public virtual string Soyad { get; set; }

        [XafDisplayName("E-posta")]
        [RuleRequiredField("Member.Email.Required", DefaultContexts.Save)]
        [RuleRegularExpression("Member.Email.Format", DefaultContexts.Save,
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        CustomMessageTemplate = "Geçerli bir e-posta girin.")]
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

        [InverseProperty(nameof(Loan.Uye))]
        public virtual ObservableCollection<Loan> Loans { get; set; } = new();
    }
}
