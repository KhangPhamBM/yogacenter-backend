using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class ClassDetailService : IClassDetailService
    {
        private readonly IClassDetailRepository _classDetailRepository;
        private IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ClassDetailService(IClassDetailRepository classDetailRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _classDetailRepository = classDetailRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task RegisterClass(ClassDetailDto detail)
        {
            var classDb = await _unitOfWork.GetRepository<Class>().GetById(detail.ClassId);
            var userDb = await _unitOfWork.GetRepository<ApplicationUser>().GetById(detail.UserId);
            var classDetail = _mapper.Map<ClassDetail>(detail);
            classDetail = await _classDetailRepository.GetByClassIdAndUserId(classDetail);
            if (classDb != null && userDb != null && classDetail == null)
            {
                await _unitOfWork.GetRepository<ClassDetail>().Insert(classDetail);
                _unitOfWork.SaveChange();
            }
        }
    }
}
