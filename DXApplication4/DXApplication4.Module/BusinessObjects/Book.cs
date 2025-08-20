using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.Validation;
using DevExpress.ExpressApp.Model;                 
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using DARequired = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace DXApplication4.Module.BusinessObjects
{
    public enum KitapDurum { Available = 0, Borrowed = 1, Lost = 2, Damaged = 3 }

    [DefaultClassOptions]
    [NavigationItem("Library")]
    [XafDisplayName("Book")]
    [XafDefaultProperty(nameof(Ad))]
    public class Book : BaseObject
    {
        [XafDisplayName("Ad")]
        [DARequired, StringLength(256)]             
        public virtual string Ad { get; set; }

        [XafDisplayName("Yazar")]
        [StringLength(256)]
        public virtual string Yazar { get; set; }

        [XafDisplayName("Yayın Yılı")]
        [ModelDefault("EditMask", "D0")]
        [ModelDefault("DisplayFormat", "{0:D0}")]
        [RuleRange("Book.YayinYili.Range", DefaultContexts.Save, 0, 2025)]
        [Range(0, 2025, ErrorMessage = "Yayın yılı 0–2025 aralığında olmalı.")]
        public virtual int? YayinYili { get; set; }

        [XafDisplayName("ISBN")]
        [StringLength(32)]
        [RuleUniqueValue("Book.ISBN.Unique", DefaultContexts.Save, CustomMessageTemplate = "Bu ISBN zaten kayıtlı.")]
        public virtual string ISBN { get; set; }

        [XafDisplayName("Durum")]
        public virtual KitapDurum Durum { get; set; } = KitapDurum.Available;

        public virtual ObservableCollection<Loan> Loans { get; set; }
            = new ObservableCollection<Loan>();
    }
}
