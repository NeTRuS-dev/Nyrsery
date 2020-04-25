
namespace Nursery.Model.Savers
{
    public interface IUserSaver
    {
        public void Save(decimal balanceOfOrganization);
        public void Load(decimal balanceOfOrganization);
    }
}