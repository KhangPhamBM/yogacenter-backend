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
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IMapper _mapper;
        private readonly AppActionResult _result;
        public FeedbackService(IUnitOfWork unitOfWork, IFeedbackRepository feedbackRepository, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
            _result = new();
        }
        public async Task<AppActionResult> CreateFeedback(FeedbackDto feedback)
        {
            try
            {
                bool isValid = true;
                var classDetailDB = await _unitOfWork.GetRepository<ClassDetail>().GetByExpression(f => f.UserId == feedback.UserId && f.ClassId == feedback.ClassId);
                if (classDetailDB == null)
                {
                    isValid = false;
                    _result.Message.Add($"This user didn't join class this class");
                }

                if (feedback.Status != FeedbackDto.FeedbackStatus.Approved &&
                    feedback.Status != FeedbackDto.FeedbackStatus.Pending &&
                    feedback.Status != FeedbackDto.FeedbackStatus.Rejected)
                {
                    isValid = false;
                    _result.Message.Add($"This status is not available please check enum");


                }
                if (feedback.Rating != FeedbackDto.RatingStar.OneStar &&
                    feedback.Rating != FeedbackDto.RatingStar.TwoStar &&
                    feedback.Rating != FeedbackDto.RatingStar.ThreeStar &&
                    feedback.Rating != FeedbackDto.RatingStar.FourStar &&
                    feedback.Rating != FeedbackDto.RatingStar.FiveStar
                    )
                {
                    isValid = false;
                    _result.Message.Add($"This ratitng is not available please check enum");


                }

                if (isValid)
                {
                    var feedbackDb = _mapper.Map<Feedback>(feedback);
                    feedbackDb.ClassDetailId = classDetailDB.ClassDetailId;
                    await _unitOfWork.GetRepository<Feedback>().Insert(feedbackDb);
                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponseMessage.CREATE_SUCCESSFUL);
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

        public async Task<AppActionResult> DeleteFeedback(int id)
        {
            try
            {
                bool isValid = true;

                if (await _unitOfWork.GetRepository<Feedback>().GetById(id) == null)
                {
                    isValid = false;
                    _result.Message.Add($"Duplicated Title");
                }

                if (isValid)
                {
                    await _unitOfWork.GetRepository<Feedback>().DeleteById(id);
                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponseMessage.DELETE_SUCCESSFUL);
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

        public async Task<AppActionResult> GetAll(int pageIndex, int pageSize, IList<SortInfo> sortInfos)
        {
            try
            {
                var feedbacks = await _unitOfWork.GetRepository<Feedback>().GetAll();
                if (pageIndex <= 0) pageIndex = 1;
                if (pageSize <= 0) pageSize = SD.MAX_RECORD_PER_PAGE;
                int totalPage = DataPresentationHelper.CalculateTotalPageSize(feedbacks.Count(), pageSize);
                if (sortInfos != null)
                {
                    feedbacks = DataPresentationHelper.ApplySorting(feedbacks, sortInfos);
                }

                if (pageIndex > 0 && pageSize > 0)
                {
                    feedbacks = DataPresentationHelper.ApplyPaging(feedbacks, pageIndex, pageSize);
                }

                _result.Result.Data = feedbacks;
                _result.Result.TotalPage = totalPage;
            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> GetFeedbackById(int id)
        {
            try
            {
                bool isValid = true;

                if (await _unitOfWork.GetRepository<Feedback>().GetById(id) == null)
                {
                    _result.Message.Add($"The feedback with id {id} not found");
                    isValid = false;

                }
                if (isValid)
                {
                    _result.Result.Data = await _unitOfWork.GetRepository<Feedback>().GetById(id);
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

        public async Task<AppActionResult> SearchApplyingSortingAndFiltering(BaseFilterRequest filterRequest)
        {
            try
            {
                var source = await _unitOfWork.GetRepository<Feedback>().GetAll();
                int pageIndex = filterRequest.pageIndex;
                if (pageIndex <= 0) pageIndex = 1;
                int pageSize = filterRequest.pageSize;
                if (pageSize <= 0) pageSize = SD.MAX_RECORD_PER_PAGE;
                int totalPage = DataPresentationHelper.CalculateTotalPageSize(source.Count(), pageSize);
                if (filterRequest != null)
                {
                    if (filterRequest.pageIndex <= 0 || filterRequest.pageSize <= 0)
                    {
                        _result.Message.Add($"Invalid value of pageIndex or pageSize");
                        _result.isSuccess = false;
                    }
                    else
                    {
                        if (filterRequest.keyword != "" && filterRequest.keyword != null)
                        {
                            source = await _unitOfWork.GetRepository<Feedback>().GetListByExpression(b => b.Content.Contains(filterRequest.keyword), null);
                        }
                        if (filterRequest.filterInfoList != null)
                        {
                            source = DataPresentationHelper.ApplyFiltering(source, filterRequest.filterInfoList);
                        }
                        totalPage = DataPresentationHelper.CalculateTotalPageSize(source.Count(), filterRequest.pageSize);

                        if (filterRequest.sortInfoList != null)
                        {
                            source = DataPresentationHelper.ApplySorting(source, filterRequest.sortInfoList);
                        }
                        source = DataPresentationHelper.ApplyPaging(source, filterRequest.pageIndex, filterRequest.pageSize);
                        _result.Result.Data = source;

                    }
                }
                else
                {
                    _result.Result.Data = source;
                }
                _result.Result.TotalPage = totalPage;

            }
            catch (Exception ex)
            {
                _result.isSuccess = false;
                _result.Message.Add(ex.Message);
            }
            return _result;
        }

        public async Task<AppActionResult> UpdateFeedback(FeedbackDto feedback)
        {
            try
            {
                bool isValid = true;
                var classDetailDB = await _unitOfWork.GetRepository<ClassDetail>().GetByExpression(f => f.UserId == feedback.UserId && f.ClassId == feedback.ClassId);
                if (classDetailDB == null)
                {
                    isValid = false;
                    _result.Message.Add($"This user didn't join class this class");
                }

                if (feedback.Status != FeedbackDto.FeedbackStatus.Approved &&
                  feedback.Status != FeedbackDto.FeedbackStatus.Pending &&
                  feedback.Status != FeedbackDto.FeedbackStatus.Rejected)
                {
                    isValid = false;
                    _result.Message.Add($"This status is not available please check enum");


                }
                if (feedback.Rating != FeedbackDto.RatingStar.OneStar &&
                    feedback.Rating != FeedbackDto.RatingStar.TwoStar &&
                    feedback.Rating != FeedbackDto.RatingStar.ThreeStar &&
                    feedback.Rating != FeedbackDto.RatingStar.FourStar &&
                    feedback.Rating != FeedbackDto.RatingStar.FiveStar
                    )
                {
                    isValid = false;
                    _result.Message.Add($"This ratitng is not available please check enum");


                }

                if (isValid)
                {
                    var feedbackDb = _mapper.Map<Feedback>(feedback);
                    feedbackDb.ClassDetailId = classDetailDB.ClassDetailId;
                    await _unitOfWork.GetRepository<Feedback>().Update(_mapper.Map<Feedback>(feedbackDb));
                    _unitOfWork.SaveChange();
                    _result.Message.Add(SD.ResponseMessage.UPDATE_SUCCESSFUL);
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
    }
}
