using Microblogging.Data.Entities;
using Microblogging.Repository.IRepo;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Repository.Repo
{
    public class ContainerRepo : IContainerRepo
    {
        private readonly microbloggingContext _context;
        private readonly Dictionary<Type, object> _repositories;

        public ContainerRepo(
        microbloggingContext context)

        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _repositories = new Dictionary<Type, object>();

        }

        IUserRepo _UserRepo;
        public IUserRepo UserRepo
        {
            get
            {
                if (_UserRepo == null)
                    _UserRepo = new UserRepo(_context);

                return _UserRepo;
            }
        }


        IPostsRepo _PostsRepo;
        public IPostsRepo PostsRepo
        {
            get
            {
                if (_PostsRepo == null)
                    _PostsRepo = new PostsRepo(_context);

                return _PostsRepo;
            }
        }


        IImageRepo _IImageRepo;
        public IImageRepo ImageRepo
        {
            get
            {
                if (_IImageRepo == null)
                    _IImageRepo = new ImageRepo(_context);

                return _IImageRepo;
            }
        }

      

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            return await _context.Database.BeginTransactionAsync();
        }
        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            try
            {

                var result = await _context.SaveChangesAsync();

                return result;
            }
            catch (DbUpdateException ex)
            {


                throw;
            }

        }
    }
    }
