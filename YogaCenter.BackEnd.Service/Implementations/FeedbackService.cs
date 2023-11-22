using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YogaCenter.BackEnd.Common.Dto.Request;
using YogaCenter.BackEnd.DAL.Contracts;
using YogaCenter.BackEnd.DAL.Implementations;
using YogaCenter.BackEnd.DAL.Models;
using YogaCenter.BackEnd.DAL.Util;
using YogaCenter.BackEnd.Service.Contracts;

namespace YogaCenter.BackEnd.Service.Implementations
{
    public class FeedbackService : GenericBackendService, IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFeedbackRepository _feedbackRepository;
        private readonly IMapper _mapper;
        private readonly AppActionResult _result;
        public FeedbackService(IUnitOfWork unitOfWork, IFeedbackRepository feedbackRepository, IMapper mapper, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _unitOfWork = unitOfWork;
            _feedbackRepository = feedbackRepository;
            _mapper = mapper;
            _result = new();
        }
        public async Task<AppActionResult> CreateFeedback(FeedbackRequestDto feedback)
        {
            try
            {
                var classDetailRepository = Resolve<IClassDetailRepository>();
                bool isValid = true;
                var classDetailDB = await classDetailRepository.GetByExpression(f => f.UserId == feedback.UserId && f.ClassId == feedback.ClassId);
                if (classDetailDB == null)
                {
                    isValid = false;
                    _result.Message.Add($"This user didn't join class this class");
                }

                if (feedback.Status != FeedbackRequestDto.FeedbackStatus.Approved &&
                    feedback.Status != FeedbackRequestDto.FeedbackStatus.Pending &&
                    feedback.Status != FeedbackRequestDto.FeedbackStatus.Rejected)
                {
                    isValid = false;
                    _result.Message.Add($"This status is not available please check enum");


                }
                if (feedback.Rating != FeedbackRequestDto.RatingStar.OneStar &&
                    feedback.Rating != FeedbackRequestDto.RatingStar.TwoStar &&
                    feedback.Rating != FeedbackRequestDto.RatingStar.ThreeStar &&
                    feedback.Rating != FeedbackRequestDto.RatingStar.FourStar &&
                    feedback.Rating != FeedbackRequestDto.RatingStar.FiveStar
                    )
                {
                    isValid = false;
                    _result.Message.Add($"This ratitng is not available please check enum");


                }

                if (isValid)
                {
                    var feedbackDb = _mapper.Map<Feedback>(feedback);
                    feedbackDb.ClassDetailId = classDetailDB.ClassDetailId;
                    await _feedbackRepository.Insert(feedbackDb);
                    await _unitOfWork.SaveChangeAsync();
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

                if (await _feedbackRepository.GetById(id) == null)
                {
                    isValid = false;
                    _result.Message.Add($"Duplicated Title");
                }

                if (isValid)
                {
                    await _feedbackRepository.DeleteById(id);
                    await _unitOfWork.SaveChangeAsync();
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
                var feedbacks = await _feedbackRepository.GetAll();
                var SD = Resolve<YogaCenter.BackEnd.DAL.Util.SD>();

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
                var feedbackDb = await _feedbackRepository.GetById(id);
                if (feedbackDb == null)
                {
                    _result.Message.Add($"The feedback with id {id} not found");
                    isValid = false;

                }
                if (isValid)
                {
                    _result.Result.Data = feedbackDb;
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
                var source = await _feedbackRepository.GetAll();
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
                            source = await _feedbackRepository.GetListByExpression(b => b.Content.Contains(filterRequest.keyword), null);
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

        public async Task<AppActionResult> UpdateFeedback(FeedbackRequestDto feedback)
        {
            try
            {
                bool isValid = true;
                var classDetailRepository = Resolve<IClassDetailRepository>();
                var classDetailDB = await classDetailRepository.GetByExpression(f => f.UserId == feedback.UserId && f.ClassId == feedback.ClassId);
                if (classDetailDB == null)
                {
                    isValid = false;
                    _result.Message.Add($"This user didn't join class this class");
                }

                if (feedback.Status != FeedbackRequestDto.FeedbackStatus.Approved &&
                  feedback.Status != FeedbackRequestDto.FeedbackStatus.Pending &&
                  feedback.Status != FeedbackRequestDto.FeedbackStatus.Rejected)
                {
                    isValid = false;
                    _result.Message.Add($"This status is not available please check enum");


                }
                if (feedback.Rating != FeedbackRequestDto.RatingStar.OneStar &&
                    feedback.Rating != FeedbackRequestDto.RatingStar.TwoStar &&
                    feedback.Rating != FeedbackRequestDto.RatingStar.ThreeStar &&
                    feedback.Rating != FeedbackRequestDto.RatingStar.FourStar &&
                    feedback.Rating != FeedbackRequestDto.RatingStar.FiveStar
                    )
                {
                    isValid = false;
                    _result.Message.Add($"This ratitng is not available please check enum");


                }

                if (isValid)
                {
                    var feedbackDb = _mapper.Map<Feedback>(feedback);
                    feedbackDb.ClassDetailId = classDetailDB.ClassDetailId;
                    await _feedbackRepository.Update(_mapper.Map<Feedback>(feedbackDb));
                    await _unitOfWork.SaveChangeAsync();
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
