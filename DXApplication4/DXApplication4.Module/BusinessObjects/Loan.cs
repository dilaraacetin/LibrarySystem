using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EFCore;   
using DevExpress.Persistent.Validation;
using System;

namespace DXApplication4.Module.BusinessObjects
{
    public enum LoanDurum { Active = 0, Returned = 1, Overdue = 2 }

    [DefaultClassOptions]
    [NavigationItem("Library")]
    [XafDisplayName("Loan")]

    [RuleCriteria(
        "Loan_Dates_Valid",
        DefaultContexts.Save,
        "[BitisTarihi] >= [BaslangicTarihi]",
        CustomMessageTemplate = "Bitiş tarihi başlangıçtan önce olamaz."
    )]

    [RuleCombinationOfPropertiesIsUnique(
        "Loan_UniqueActiveLoanForBook",
        DefaultContexts.Save,
        "Kitap;Durum",
        CustomMessageTemplate = "Bu kitap zaten aktif bir ödünçte.",
        TargetCriteria = "[Durum] = ##Enum#DXApplication4.Module.BusinessObjects.LoanDurum,Active#"
    )]
    public class Loan : BaseObject
    {
        public Loan() : base() { }

        [XafDisplayName("Kitap")]
        [RuleRequiredField("Loan.Kitap.Required", DefaultContexts.Save)]
        public virtual Book Kitap { get; set; }

        [XafDisplayName("Üye")]
        [RuleRequiredField("Loan.Uye.Required", DefaultContexts.Save)]
        public virtual Member Uye { get; set; }

        [XafDisplayName("Başlangıç Tarihi")]
        [RuleRequiredField("Loan.Baslangic.Required", DefaultContexts.Save)]
        public virtual DateTime? BaslangicTarihi { get; set; }

        [XafDisplayName("Bitiş Tarihi")]
        [RuleRequiredField("Loan.Bitis.Required", DefaultContexts.Save)]
        public virtual DateTime? BitisTarihi { get; set; }

        [XafDisplayName("Durum")]
        public virtual LoanDurum Durum { get; set; } = LoanDurum.Active;
    }
}
