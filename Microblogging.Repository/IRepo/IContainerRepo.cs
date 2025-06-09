using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Repository.IRepo
{
    public interface IContainerRepo : IDisposable
    {
        IUserRepo UserRepo { get; }
        IImageRepo ImageRepo { get; }

        IPostsRepo PostsRepo { get; }

        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<int> CompleteAsync();
        Task<int> SaveChangesAsync();
    }
}
