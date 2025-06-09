using AutoMapper;
using Microblogging.Helper.Models;
using Microblogging.Repository.IRepo;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Service.IServices
{
    public class BaseService
    {
        protected readonly IContainerRepo _ContainerRepo;
        protected readonly IMapper _mapper;
        protected readonly IConfiguration _configuration;
       

        public BaseService(IContainerRepo ContainerRepo, IMapper mapper, IConfiguration configuration)
        {
            _ContainerRepo = ContainerRepo;
            _mapper = mapper;
            _configuration = configuration;

        }
    }
}
