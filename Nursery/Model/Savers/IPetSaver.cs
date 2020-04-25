
namespace Nursery.Model.Savers
{
    public interface IPetSaver
    {
        public void Save(decimal balanceOfOrganization);
        public void Load(decimal balanceOfOrganization);
    }
}