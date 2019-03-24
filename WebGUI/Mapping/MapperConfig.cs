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
                cfg.CreateMap<Esp, Dtos.Esp>();
                cfg.CreateMap<Sensor, Dtos.Sensor>();
                cfg.CreateMap<SensorData, Dtos.SensorData>();
            });
            config.AssertConfigurationIsValid();

            var mapper = config.CreateMapper();
            return mapper;
        }
    }
}
