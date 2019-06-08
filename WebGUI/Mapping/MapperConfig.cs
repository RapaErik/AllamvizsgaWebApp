using AutoMapper;
using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebGUI.Mapping
{
    public class MapperConfig
    {
        public static IMapper Configure()
        {
            var config = new MapperConfiguration(cfg => 
            {
                cfg.CreateMap<Room, Dtos.Room>();
                cfg.CreateMap<CommunicationUnit, Dtos.CommunicationUnit>();
                cfg.CreateMap<Device, Dtos.Device>();
                cfg.CreateMap<Log, Dtos.Log>();
            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            return mapper;
        }
    }
}
