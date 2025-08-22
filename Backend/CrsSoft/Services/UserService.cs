using CrsSoft.Data;
using CrsSoft.Interfaces;

namespace CrsSoft.Services
{
    public class UserService : IUserService
    {   
        // TODO
        // Mail ve password değiştirme
        // Para ekleme
        // Başka bir kullanıcıya para transfer etme

        private readonly DataContext dataContext;

        public UserService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }
    }
}
