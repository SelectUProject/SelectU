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

        public async Task<List<ScholarshipApplicationUpdateDTO>> GetScholarshipApplicationsAsync(Guid scholarshipId, ScholarshipApplicationSearchDTO scholarshipApplicationSearchDTO)
        {
            var query = _unitOfWork.ScholarshipApplications.Where(x => x.ScholarshipId == scholarshipId)
                  .Include(x => x.Scholarship)
                  .Include(x => x.ScholarshipApplicant);

            //TODO: Add search functionality

            var applications = await query.ToListAsync();

            return applications.Select(scholarshipApplication => new ScholarshipApplicationUpdateDTO(scholarshipApplication)).ToList();
        }

        public async Task<ScholarshipApplication> GetScholarshipApplicationAsync(Guid id)
        {
            var application = await _unitOfWork.ScholarshipApplications.GetAsync(id);

            return application;
        }

        public async Task<List<ScholarshipApplicationUpdateDTO>> GetMyScholarshipApplicationsAsync(ScholarshipApplicationSearchDTO scholarshipApplicationSearchDTO, string userId, bool isStaff)
        {
            IQueryable<ScholarshipApplication> query;

            if (isStaff)
            {
                query = _unitOfWork.ScholarshipApplications.Where(x => x.Scholarship.ScholarshipCreatorId == userId)
                    .Include(x => x.Scholarship)
                    .Include(x => x.ScholarshipApplicant);
            }
            else
            {
                query = _unitOfWork.ScholarshipApplications.Where(x => x.ScholarshipApplicant.Id == userId)
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

        public async Task CreateScholarshipApplicationAsync(ScholarshipApplicationCreateDTO scholarshipApplicationCreateDTO, string userId)
        {
            var alreadyApplied = await _unitOfWork.ScholarshipApplications.AnyAsync(x => x.ScholarshipId == scholarshipApplicationCreateDTO.ScholarshipId && x.ScholarshipApplicantId == userId);
            if (alreadyApplied)
            {
                throw new ScholarshipApplicationException($"Applications are limited to one per user.");
            }

            await ValidateScholarshipApplication(scholarshipApplicationCreateDTO);

            ScholarshipApplication scholarshipApplication = new ScholarshipApplication
            {
                ScholarshipApplicantId = userId,
                ScholarshipId = scholarshipApplicationCreateDTO.ScholarshipId,
                ScholarshipFormAnswer = JsonSerializer.Serialize(scholarshipApplicationCreateDTO.ScholarshipFormAnswer),
                Status = ApplicationStatusEnum.Submitted,
                DateCreated = DateTimeOffset.Now,
                DateModified = DateTimeOffset.Now,
            };

            _unitOfWork.ScholarshipApplications.Add(scholarshipApplication);

            await _unitOfWork.CommitAsync();
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
    }
}

