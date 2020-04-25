
namespace Nursery.Model.Savers
{
    public interface ILogSaver
    {
        public void Save(decimal balanceOfOrganization);
        public void Load(decimal balanceOfOrganization);
    }
}