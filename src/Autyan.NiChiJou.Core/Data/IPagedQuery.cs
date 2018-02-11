namespace Autyan.NiChiJou.Core.Data
{
    public interface IPagedQuery
    {
        int? Take { get; set; }

        int? Skip { get; set; }
    }
}
