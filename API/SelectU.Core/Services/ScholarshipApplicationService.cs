using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using SelectU.Contracts;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using System.Text.Json;

namespace SelectU.Core.Services
{
    public class ScholarshipApplicationService : IScholarshipApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ScholarshipApplicationService(IUnitOfWork context)
        {
            _unitOfWork = context;
        }

        public async Task<ScholarshipApplicationUpdateDTO> GetScholarshipApplicationAsync(Guid id)
        {
            var scholarshipApplication = await _unitOfWork.ScholarshipApplications.GetAsync(id);

            if (scholarshipApplication == null)
            {
                return null;
            }

            return new ScholarshipApplicationUpdateDTO(scholarshipApplication);
        }

        public async Task<List<ScholarshipApplicationUpdateDTO>> GetMyScholarshipApplicationsAsync(ScholarshipApplicationSearchDTO scholarshipApplicationSearchDTO, string id, bool isStaff)
        {
            IQueryable<ScholarshipApplication> query;

            if (isStaff)
            {
                query = _unitOfWork.ScholarshipApplications.Where(x => x.Scholarship.ScholarshipCreatorId == id)
                    .Include(x => x.Scholarship)
                    .Include(x => x.ScholarshipApplicant);
            }
            else
            {
                query = _unitOfWork.ScholarshipApplications.Where(x => x.ScholarshipApplicant.Id == id)
                  .Include(x => x.Scholarship)
                  .Include(x => x.ScholarshipApplicant);
            }

            if (scholarshipApplicationSearchDTO.Id != null)
            {
                query = query.Where(x => x.Id == scholarshipApplicationSearchDTO.Id);
            }
            if (!string.IsNullOrEmpty(scholarshipApplicationSearchDTO.Description))
            {
                query = query.Where(x => x.Scholarship.Description == scholarshipApplicationSearchDTO.Description);

            }
            if (!string.IsNullOrEmpty(scholarshipApplicationSearchDTO.School))
            {
                query = query.Where(x => x.Scholarship.School == scholarshipApplicationSearchDTO.School);

            }

            var scholarshipApplications = await query.ToListAsync();

            return scholarshipApplications
                .Select(scholarshipApplication => new ScholarshipApplicationUpdateDTO(scholarshipApplication))
                .ToList();

        }

        public async Task<ResponseDTO> CreateScholarshipApplicationAsync(ScholarshipApplicationCreateDTO scholarshipApplicationCreateDTO, string id)
        {
            var alreadyApplied = await _unitOfWork.ScholarshipApplications.AnyAsync(x => x.ScholarshipId == scholarshipApplicationCreateDTO.ScholarshipId && x.ScholarshipApplicantId == id);
            if (alreadyApplied)
            {
                throw new ScholarshipApplicationException($"Applications are limited to one per user.");
            }

            await ValidateScholarshipApplication(scholarshipApplicationCreateDTO);

            ScholarshipApplication scholarshipApplication = new ScholarshipApplication
            {
                ScholarshipApplicantId = id,
                ScholarshipId = scholarshipApplicationCreateDTO.ScholarshipId,
                ScholarshipFormAnswer = JsonSerializer.Serialize(scholarshipApplicationCreateDTO.ScholarshipFormAnswer),
                Status = StatusEnum.Pending,
                DateCreated = DateTimeOffset.Now,
                DateModified = DateTimeOffset.Now,
            };

            _unitOfWork.ScholarshipApplications.Add(scholarshipApplication);

            await _unitOfWork.CommitAsync();

            return new ResponseDTO { Success = true, Message = "Scholarship Application created successfully." };

        }

        public async Task<ScholarshipApplicationCreateDTO> ValidateScholarshipApplication(ScholarshipApplicationCreateDTO scholarshipApplicationCreateDTO)
        {

            var scholarShip = _unitOfWork.Scholarships.Where(x => x.Id == scholarshipApplicationCreateDTO.ScholarshipId).FirstOrDefault();

            if (scholarShip != null) {
                var updatedScholarShip = new ScholarshipUpdateDTO(scholarShip);
                var templateDictionary = updatedScholarShip.ScholarshipFormTemplate.ToDictionary(item => item.Name);


                // Check if each answer section's name and value match the template
                foreach (var answer in scholarshipApplicationCreateDTO.ScholarshipFormAnswer)
                {
                    if (!templateDictionary.TryGetValue(answer.Name, out var templateValue))
                    {
                        if (templateValue == null)
                        {
                            throw new ScholarshipApplicationException($"Unknown answer section '{answer.Name}' is not in the template.");
                        }
                    }
                }

                // Check if each required answer section's name is in the template
                foreach (var templateItem in updatedScholarShip.ScholarshipFormTemplate)
                {
                    if (templateItem.Required && !scholarshipApplicationCreateDTO.ScholarshipFormAnswer.Any(answer => answer.Name == templateItem.Name))
                    {
                        throw new ScholarshipApplicationException($"Required answer section '{templateItem.Name}' is missing.");
                    }
                }
            }
            else
            {
                throw new ScholarshipApplicationException("No scholarship found");
            }

            return scholarshipApplicationCreateDTO;
        
        }

        public async Task<ResponseDTO> CreateScholarshipApplicationRatingAsync(UserRatingDTO userRatingDTO)
        {
            var scholarshipApplication = await _unitOfWork.ScholarshipApplications.GetAsync(userRatingDTO.ScholarshipApplicationId);

            if (scholarshipApplication == null) throw new ScholarshipApplicationException($"Unable able to add rating as the application does not exist");

            if (scholarshipApplication.Ratings.FirstOrDefault(x => x.ReviewerId == userRatingDTO.ReviewerId) == null) throw new ScholarshipApplicationException($"Unable able to create rating as the reviewer has an existing review");

            UserRating userRating = new UserRating()
            {
                ScholarshipApplicationId = userRatingDTO.ScholarshipApplicationId,
                Rating = userRatingDTO.Rating,
                Id = userRatingDTO.Id,
                ReviewerId = userRatingDTO.ReviewerId,
                ApplicantId = userRatingDTO.ApplicantId,
                Comments = new Comment().CommentDTOsToComments(userRatingDTO.Comments),
            };

            scholarshipApplication.Ratings.Add(userRating);
            _unitOfWork.ScholarshipApplications.Update(scholarshipApplication);

            await _unitOfWork.CommitAsync();

            return new ResponseDTO { Success = true, Message = "Scholarship Application Rating created successfully." };
        }

        public async Task<ResponseDTO> UpdateScholarshipApplicationRatingAsync(UserRatingDTO userRatingDTO)
        {
            var scholarshipApplication = await _unitOfWork.ScholarshipApplications.GetAsync(userRatingDTO.ScholarshipApplicationId);

            if (scholarshipApplication == null)
            {
                throw new ScholarshipApplicationException($"Unable able to add rating as the application does not exist");
            }

            UserRating userRating = new UserRating() {
                ScholarshipApplicationId = userRatingDTO.ScholarshipApplicationId,
                Rating = userRatingDTO.Rating,
                Id = userRatingDTO.Id,
                ReviewerId = userRatingDTO.ReviewerId,
                ApplicantId = userRatingDTO.ApplicantId,
                Comments = new Comment().CommentDTOsToComments(userRatingDTO.Comments),
            };
            var originalRating = scholarshipApplication.Ratings.FirstOrDefault(x => x.Id == userRating.Id && x.ReviewerId == userRatingDTO.ReviewerId);
            if(originalRating == null) return new ResponseDTO { Success = false, Message = "Scholarship Application Rating was not updated successfully." };
            scholarshipApplication.Ratings.Remove(originalRating);
            scholarshipApplication.Ratings.Add(userRating);
            _unitOfWork.ScholarshipApplications.Update(scholarshipApplication);

            await _unitOfWork.CommitAsync();

            return new ResponseDTO { Success = true, Message = "Scholarship Application Rating created successfully." };
        }

        public async Task<ResponseDTO> DeleteScholarshipApplicationRatingAsync(Guid ScholarshipId, Guid RatingId)
        {
            var scholarshipApplication = await _unitOfWork.ScholarshipApplications.GetAsync(ScholarshipId.ToString());

            if (scholarshipApplication == null)
            {
                throw new ScholarshipApplicationException($"Unable able to delete rating as the application does not exist");
            }
            var rating = scholarshipApplication.Ratings.FirstOrDefault(x => x.Id == RatingId);
            if(rating == null) return new ResponseDTO { Success = false, Message = "Scholarship Application Rating could not be found." };

            scholarshipApplication.Ratings.Remove(rating);
            _unitOfWork.ScholarshipApplications.Update(scholarshipApplication);
            await _unitOfWork.CommitAsync();
            return new ResponseDTO { Success = true, Message = "Scholarship Application Rating deleted successfully." };
        }
    }
}

