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
            IQueryable<ScholarshipApplication> query = _unitOfWork.ScholarshipApplications
                .Where(x => x.Status == StatusEnum.Pending)
                .Include(x => x.Scholarship)
                .Include(x => x.ScholarshipApplicant);

            if (isStaff)
            {
                query = query.Where(x => x.Scholarship.ScholarshipCreatorId == id);
            }
            else
            {
                query = query.Where(x => x.ScholarshipApplicant.Id == id);
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
            //TODO
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

