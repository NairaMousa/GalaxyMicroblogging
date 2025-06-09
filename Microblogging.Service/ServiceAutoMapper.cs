using AutoMapper;
using Microblogging.Repository.Models;
using Microblogging.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microblogging.Service
{
    public class ServiceAutoMapper : Profile
    {
        public ServiceAutoMapper()
        {
            // Example mapping


            CreateMap<PostModel, Posts_ResDto>().ReverseMap();





        }
    }
    }
