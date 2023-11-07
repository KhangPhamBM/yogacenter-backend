using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Models;

namespace YogaCenter.BackEnd.DAL.Mapping
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMap()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<AttendanceDto, Attendance>()
                .ForMember(desc => desc.ScheduleId, act => act.MapFrom(src => src.ScheduleId))
                .ForMember(desc => desc.ClassDetailId, act => act.MapFrom(src => src.ClassDetailId))
                .ForMember(desc => desc.AttendanceStatusId, act => act.MapFrom(src => src.AttendanceStatusId))
                .ReverseMap();

                config.CreateMap<ClassRequest, Class>()
                  .ForMember(desc => desc.ClassId, act => act.MapFrom(src => src.ClassId))
                  .ForMember(desc => desc.ClassName, act => act.MapFrom(src => src.ClassName))
                  .ForMember(desc => desc.CourseId, act => act.MapFrom(src => src.CourseId))
                  .ForMember(desc => desc.IsDeleted, act => act.MapFrom(src => src.IsDeleted))
                  .ForMember(desc => desc.EndDate, act => act.MapFrom(src => src.EndDate))
                  .ReverseMap();
                config.CreateMap<ClassDto, Class>()
                 .ForMember(desc => desc.ClassId, act => act.MapFrom(src => src.ClassId))
                 .ForMember(desc => desc.ClassName, act => act.MapFrom(src => src.ClassName))
                 .ForMember(desc => desc.CourseId, act => act.MapFrom(src => src.CourseId))
                 .ForMember(desc => desc.IsDeleted, act => act.MapFrom(src => src.IsDeleted))
                 .ForMember(desc => desc.EndDate, act => act.MapFrom(src => src.EndDate))
                 .ReverseMap();

                config.CreateMap<ClassDetailDto, ClassDetail>()
                .ForMember(desc => desc.ClassDetailId, act => act.MapFrom(src => src.ClassDetailId))
                .ForMember(desc => desc.UserId, act => act.MapFrom(src => src.UserId))
                .ForMember(desc => desc.ClassId, act => act.MapFrom(src => src.ClassId))
                  .ReverseMap();

                config.CreateMap<CourseDto, Course>()
               .ForMember(desc => desc.CourseId, act => act.MapFrom(src => src.CourseId))
               .ForMember(desc => desc.CourseName, act => act.MapFrom(src => src.CourseName))
               .ForMember(desc => desc.CourseImageUrl, act => act.MapFrom(src => src.CourseImageUrl))
               .ForMember(desc => desc.IsDeleted, act => act.MapFrom(src => src.IsDeleted))
               .ForMember(desc => desc.CourseDescription, act => act.MapFrom(src => src.CourseDescription))
               .ForMember(desc => desc.Price, act => act.MapFrom(src => src.Price))
               .ForMember(desc => desc.Discount, act => act.MapFrom(src => src.Discount))
               .ReverseMap();

                config.CreateMap<PaymentResponseDto, PaymentRespone>()
                .ForMember(desc => desc.SubscriptionId, act => act.MapFrom(src => src.SubscriptionId))
                .ForMember(desc => desc.PaymentTypeId, act => act.MapFrom(src => src.PaymentTypeId))
                .ForMember(desc => desc.Success, act => act.MapFrom(src => src.Success))
                .ForMember(desc => desc.OrderInfo, act => act.MapFrom(src => src.OrderInfo))
                .ForMember(desc => desc.Amount, act => act.MapFrom(src => src.Amount))
                .ForMember(desc => desc.PaymentResponseId, act => act.MapFrom(src => src.PaymentResponseId))
                .ReverseMap();

                config.CreateMap<PaymentTypeDto, PaymentType>()
                .ForMember(desc => desc.PaymentTypeId, act => act.MapFrom(src => src.PaymentTypeId))
                .ForMember(desc => desc.Type, act => act.MapFrom(src => src.Type))
                .ReverseMap();

                config.CreateMap<RoomDto, Room>()
                .ForMember(desc => desc.RoomId, act => act.MapFrom(src => src.RoomId))
                .ForMember(desc => desc.RoomName, act => act.MapFrom(src => src.RoomName))
                .ReverseMap();




                config.CreateMap<SubscriptionDto, Subscription>()
                .ForMember(desc => desc.SubscriptionStatusId, act => act.MapFrom(src => src.SubscriptionStatusId))
                .ForMember(desc => desc.SubscriptionDate, act => act.MapFrom(src => src.SubscriptionDate))
                .ForMember(desc => desc.ClassId, act => act.MapFrom(src => src.ClassId))
                .ForMember(desc => desc.Total, act => act.MapFrom(src => src.Total))
                .ForMember(desc => desc.SubscriptionId, act => act.MapFrom(src => src.SubscriptionId))
                .ReverseMap();

                config.CreateMap<SubscriptionStatusDto, SubscriptionStatus>()
                .ForMember(desc => desc.SubscriptionStatusId, act => act.MapFrom(src => src.SubscriptionStatusId))
                .ForMember(desc => desc.SubscriptionStatusName, act => act.MapFrom(src => src.SubscriptionStatusName))
                .ReverseMap();

                config.CreateMap<TicketDto, Ticket>()
                .ForMember(desc => desc.TicketId, act => act.MapFrom(src => src.TicketId))
                .ForMember(desc => desc.TicketStatusId, act => act.MapFrom(src => src.TicketStatusId))
                .ForMember(desc => desc.Note, act => act.MapFrom(src => src.Note))
                .ForMember(desc => desc.UserId, act => act.MapFrom(src => src.UserId))
                .ReverseMap();

                config.CreateMap<TicketStatusDto, TicketStatus>()
                .ForMember(desc => desc.TicketStatusId, act => act.MapFrom(src => src.TicketStatusId))
                .ForMember(desc => desc.TicketStatusName, act => act.MapFrom(src => src.TicketStatusName))
                .ReverseMap();

                config.CreateMap<TicketTypeDto, TicketType>()
                .ForMember(desc => desc.TicketTypeId, act => act.MapFrom(src => src.TicketTypeId))
                .ForMember(desc => desc.TicketName, act => act.MapFrom(src => src.TicketName))
                .ReverseMap();

                config.CreateMap<TimeFrameDto, TimeFrame>()
                .ForMember(desc => desc.TimeFrameId, act => act.MapFrom(src => src.TimeFrameId))
                .ForMember(desc => desc.TimeFrameName, act => act.MapFrom(src => src.TimeFrameName))
                .ReverseMap();

                config.CreateMap<ScheduleOfClassDto, Schedule>()
             .ForMember(desc => desc.RoomId, act => act.MapFrom(src => src.RoomId))
             .ForMember(desc => desc.ScheduleId, act => act.MapFrom(src => src.ScheduleId))
             .ForMember(desc => desc.TimeFrameId, act => act.MapFrom(src => src.TimeFrameId))
             .ForMember(desc => desc.ClassId, act => act.MapFrom(src => src.ClassId))
             .ForMember(desc => desc.Date, act => act.MapFrom(src => src.Date))
             .ForMember(desc => desc.Room, act => act.MapFrom(src => src.RoomDto))
             .ForMember(desc => desc.TimeFrame, act => act.MapFrom(src => src.TimeFrameDto))


             .ReverseMap();

                config.CreateMap<ScheduleDto, Schedule>()
          .ForMember(desc => desc.RoomId, act => act.MapFrom(src => src.RoomId))
          .ForMember(desc => desc.ScheduleId, act => act.MapFrom(src => src.ScheduleId))
          .ForMember(desc => desc.TimeFrameId, act => act.MapFrom(src => src.TimeFrameId))
          .ForMember(desc => desc.ClassId, act => act.MapFrom(src => src.ClassId))
          .ForMember(desc => desc.Date, act => act.MapFrom(src => src.Date))
        


          .ReverseMap();
            });
            return mappingConfig;
        }
    }
}
