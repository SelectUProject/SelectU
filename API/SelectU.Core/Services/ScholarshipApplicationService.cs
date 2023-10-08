using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SelectU.Contracts;
using SelectU.Contracts.Config;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using System.Text;
using System.Text.Json;

namespace SelectU.Core.Services
{
    public class ScholarshipApplicationService : IScholarshipApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBlobStorageService _blobStorageService;
        private readonly AzureBlobSettingsConfig _azureBlobSettingsConfig;

        public ScholarshipApplicationService(
            IUnitOfWork context, 
            IBlobStorageService blobStorageService, 
            IOptions<AzureBlobSettingsConfig> azureBlobSettingsConfig
        )
        {
            _unitOfWork = context;
            _blobStorageService = blobStorageService;
            _azureBlobSettingsConfig = azureBlobSettingsConfig.Value;
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

            var filterScholarshipApplicationSearchDTO = await FilterQuery(scholarshipApplicationSearchDTO, await query.ToListAsync());
            

            return filterScholarshipApplicationSearchDTO
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

            var scholarshipApplicationWithUploadedFiles = await UploadFiles(scholarshipApplicationCreateDTO);


            ScholarshipApplication scholarshipApplication = new ScholarshipApplication
            {
                ScholarshipApplicantId = id,
                ScholarshipId = scholarshipApplicationWithUploadedFiles.ScholarshipId,
                ScholarshipFormAnswer = JsonSerializer.Serialize(scholarshipApplicationWithUploadedFiles.ScholarshipFormAnswer),
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

        public async Task<ScholarshipApplicationCreateDTO> UploadFiles(ScholarshipApplicationCreateDTO scholarshipApplicationCreateDTO)
        {
            ScholarshipApplicationCreateDTO scholarshipApplicationWithUploadedFiles = scholarshipApplicationCreateDTO;

            foreach (var formSection in scholarshipApplicationCreateDTO.ScholarshipFormAnswer)
            {
                if (formSection.Type == ScholarshipFormTypeEnum.File)
                {
                    Stream stream = new MemoryStream();
                    await formSection.File.CopyToAsync(stream);
                    stream.Seek(0, SeekOrigin.Begin);

                    string fileID = await _blobStorageService.UploadFileAsync(_azureBlobSettingsConfig.FileContainerName, stream);
                    formSection.Value = fileID;
                }
            }

            return await Task.FromResult(scholarshipApplicationWithUploadedFiles);
        }


        
    }
}

