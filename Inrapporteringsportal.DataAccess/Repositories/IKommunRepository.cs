using InrapporteringsPortal.DomainModel;

namespace Inrapporteringsportal.DataAccess.Repositories
{
    public interface IKommunRepository
    {
        Kommun GetByShortName(string shortName);
    }
}
