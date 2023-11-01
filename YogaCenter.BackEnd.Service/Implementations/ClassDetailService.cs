using AutoMapper;
using NPOI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Implementations;
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
                if (await _unitOfWork.GetRepository<Class>().GetById(detail.ClassId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The class with id {detail.ClassDetailId} not found");
                }
                if (await _unitOfWork.GetRepository<ApplicationUser>().GetById(detail.UserId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The user with id {detail.ClassDetailId} not found");
                }
                if (await _unitOfWork.GetRepository<ClassDetail>().GetByExpression(_classDetailRepository.GetByClassIdAndUserId(_mapper.Map<ClassDetail>(detail))) != null)
                {
                    isValid = false;
                    _result.Message.Add($"The trainee has been registed in this class");
                }

                if (isValid)
                {
                    await _unitOfWork.GetRepository<ClassDetail>().Insert(_mapper.Map<ClassDetail>(detail));
                    _unitOfWork.SaveChange();

                    _result.Message.Add(SD.ResponseMessage.CREATE_SUCCESSFUL);


                    if (isValid)
                    {
                        _result.Data = await _unitOfWork.GetRepository<ClassDetail>().GetByExpression(_classDetailRepository.GetClassDetailByUserId(detail.UserId));
                    }
                    else
                    {
                        _result.isSuccess = false;
                    }
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetClassDetailsByClassId(int classId, int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            try
            {
                bool isValid = true;
                if(await _unitOfWork.GetRepository<Class>().GetById(classId) == null)
                {
                    isValid = false;
                    _result.Message.Add($"The class with id {classId} not found");
                }
                if(isValid)
                {
                    var details = await _unitOfWork.GetRepository<ClassDetail>().GetListByExpression(cd => cd.ClassId == classId);
                    if (details != null) {
                        if (sortInfos != null)
                        {
                            details = DataPresentationHelper.ApplySorting(details, sortInfos);
                        }
                        if (pageIndex > 0 && pageSize > 0)
                        {
                            details = DataPresentationHelper.ApplyPaging(details, pageIndex, pageSize);
                        }
                        _result.Data = details;
                    }
                    else
                    {
                        _result.isSuccess = false;
                        _result.Message.Add($"The class detail with class id {classId} not found");
                    }
                }
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }
    }
}
