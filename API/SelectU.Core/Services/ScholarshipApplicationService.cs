using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SelectU.Contracts;
using SelectU.Contracts.Config;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Infrastructure;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using SelectU.Core.Infrastructure;
using System.Text;
using System.Text.Json;

namespace SelectU.Core.Services
{
    public class ScholarshipApplicationService : IScholarshipApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobStorageService _blobStorageService;
        private readonly IEmailClient _emailclient;
        private readonly AzureBlobSettingsConfig _azureBlobSettingsConfig;

        public ScholarshipApplicationService(
            IUnitOfWork context, 
            IBlobStorageService blobStorageService,
            IEmailClient emailClient,
            IOptions<AzureBlobSettingsConfig> azureBlobSettingsConfig
        )
        {
            _emailclient = emailClient;
            _unitOfWork = context;
            _blobStorageService = blobStorageService;
            _azureBlobSettingsConfig = azureBlobSettingsConfig.Value;
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

            var filterScholarshipApplicationSearchDTO = await FilterQuery(scholarshipApplicationSearchDTO, await query.ToListAsync());
            

            return filterScholarshipApplicationSearchDTO
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
        public async Task<ResponseDTO> SelectApplication(ScholarshipApplicationUpdateDTO scholarshipApplicationUpdateDTO, string id)
        {
            ScholarshipApplication scholarshipApplication = _unitOfWork.ScholarshipApplications.Where(x => x.Id == scholarshipApplicationUpdateDTO.Id)
                .Include(x => x.Scholarship)
                .Include(x => x.ScholarshipApplicant)
                .FirstOrDefault();

            if (scholarshipApplication == null)
            {
                throw new ScholarshipApplicationException($"Application was not found.");
            }

            if (scholarshipApplication.Status != ApplicationStatusEnum.Submitted)
            {
                throw new ScholarshipApplicationException($"Application is the wrong status.");
            }

            scholarshipApplication.Status = ApplicationStatusEnum.Accepted;

            //await _emailclient.SendUserSuccessfulApplicationAsync(scholarshipApplication);

            _unitOfWork.ScholarshipApplications.Update(scholarshipApplication);

            await _unitOfWork.CommitAsync();


            return new ResponseDTO { Success = true, Message = "Scholarship Application Successfully Selected." };
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

        public async Task<List<ScholarshipApplication>> FilterQuery(ScholarshipApplicationSearchDTO scholarshipApplicationSearchDTO, List<ScholarshipApplication> scholarshipApplication)
        {
            IEnumerable<ScholarshipApplication> filteredScholarshipApplications = scholarshipApplication;

            if (scholarshipApplicationSearchDTO.Id != null)
            {
                filteredScholarshipApplications = filteredScholarshipApplications.Where(x => x.Id == scholarshipApplicationSearchDTO.Id);
            }
            if (!string.IsNullOrEmpty(scholarshipApplicationSearchDTO.School))
            {
                filteredScholarshipApplications = filteredScholarshipApplications.Where(x => x.Scholarship.School.ToLower().Contains(scholarshipApplicationSearchDTO.School.ToLower()));
            }           
            if (scholarshipApplicationSearchDTO.DateCreated != null)
            {
                filteredScholarshipApplications = filteredScholarshipApplications.Where(x => x.DateCreated?.Date == scholarshipApplicationSearchDTO.DateCreated?.Date);
            }

            return await Task.FromResult(filteredScholarshipApplications.ToList());
        }  
    }
}

