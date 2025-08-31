using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DXApplication4.Module.BusinessObjects;

namespace DXApplication4.Module.Dashboards
{
    [DomainComponent]
    [DefaultClassOptions]
    [XafDisplayName("Library Dashboard")]
    public class LibraryDashboard : IObjectSpaceLink
    {
        private IObjectSpace objectSpace;
        IObjectSpace IObjectSpaceLink.ObjectSpace { get => objectSpace; set => objectSpace = value; }

        [XafDisplayName("Toplam kitap")]
        public int TotalBooks =>
            objectSpace?.GetObjectsCount(typeof(Book), null) ?? 0;

        [XafDisplayName("Ödünçte olan")]
        public int ActiveLoans =>
            objectSpace?.GetObjectsCount(typeof(Loan),
                CriteriaOperator.Parse("[Durum] = ?", LoanDurum.Active)) ?? 0;

        [XafDisplayName("Geciken")]
        public int OverdueLoans =>
            objectSpace?.GetObjectsCount(typeof(Loan),
                CriteriaOperator.Parse("[Durum] = ?", LoanDurum.Overdue)) ?? 0;

        [XafDisplayName("Üye sayısı")]
        public int MemberCount =>
            objectSpace?.GetObjectsCount(typeof(Member), null) ?? 0;
    }
}
