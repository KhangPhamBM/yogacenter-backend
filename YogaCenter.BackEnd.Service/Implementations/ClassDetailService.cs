using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class ClassDetailService : IClassDetailService
    {
        private readonly IClassDetailRepository _classDetailRepository;
        private IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AppActionResult _result;

        public ClassDetailService(IClassDetailRepository classDetailRepository, IMapper mapper, IUnitOfWork unitOfWork)
        {
            _classDetailRepository = classDetailRepository;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _result = new();
        }

        public async Task<AppActionResult> RegisterClass(ClassDetailDto detail)
        {
            try
            {
                bool isValid = true;
                if (_unitOfWork.GetRepository<Class>().GetById(detail.ClassId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The class with id {detail.ClassDetailId} not found");
                }
                if (_unitOfWork.GetRepository<ApplicationUser>().GetById(detail.UserId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The user with id {detail.ClassDetailId} not found");
                }
                if (_unitOfWork.GetRepository<ClassDetail>().GetByExpression(c => c.UserId == detail.UserId && c.ClassId == detail.ClassId) != null)
                {
                    isValid = false;
                    _result.Message.Add($"The trainee has been registed in this class");
                }

                if (isValid)
                {
                    await _unitOfWork.GetRepository<ClassDetail>().Insert(_mapper.Map<ClassDetail>(detail));
                    _unitOfWork.SaveChange();

                    _result.Message.Add(SD.ResponeMessage.CREATE_SUCCESS);
                }
                else
                {
                    _result.isSuccess = false;
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public Task<IEnumerable<ClassDetailDto>> GetClassDetailsByClassId(int classId)
        {
            throw new NotImplementedException();
        }
    }
}
