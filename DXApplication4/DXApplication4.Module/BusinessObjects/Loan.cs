using DevExpress.Drawing;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.EF;
using DevExpress.Persistent.BaseImpl.EFCore;   
using DevExpress.Persistent.Validation;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

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

    [Appearance(
    "Loan_Overdue_Row",
    AppearanceItemType = "ViewItem",
    TargetItems = "*",
    Context = "ListView",
    Criteria = "([Durum] = 0 AND Not IsNull([BitisTarihi]) AND [BitisTarihi] < Now()) OR [Durum] = 2",
    FontColor = "Red",
    BackColor = "MistyRose",
    FontStyle = DXFontStyle.Bold   
)]
    [Appearance(
    "Loan_DueSoon_Row",
    AppearanceItemType = "ViewItem",
    TargetItems = "*",
    Context = "ListView",
    Criteria = "[Durum] = 0 AND Not IsNull([BitisTarihi]) AND [BitisTarihi] <= AddDays(Now(), 3) AND [BitisTarihi] >= Now()",
    BackColor = "LightGoldenrodYellow",
    FontStyle = DXFontStyle.Bold   
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
        [NotMapped]
        [Browsable(false)]
        [RuleFromBoolProperty(
            "Loan.Member.Max3ActiveLoans",
            DefaultContexts.Save,
            CustomMessageTemplate = "Bir üye aynı anda 3'ten fazla aktif kitap ödünç alamaz."
        )]
        public bool IsUnderMembersLoanLimit
        {
            get
            {
                if (Uye == null) return true;
                int otherActive = Uye.Loans?.Count(l => l.Durum == LoanDurum.Active && !ReferenceEquals(l, this)) ?? 0;
                int prospective = otherActive + (Durum == LoanDurum.Active ? 1 : 0);
                return prospective <= 3;
            }
        }
    }
}
