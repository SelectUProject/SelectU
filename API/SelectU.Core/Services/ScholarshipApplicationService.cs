using Microsoft.EntityFrameworkCore;
using SelectU.Contracts;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Services;
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

        public async Task<ScholarshipApplication> GetScholarshipApplicationAsync(Guid id)
        {
            return await _unitOfWork.ScholarshipApplications.GetAsync(id);
        }

        public async Task<List<ScholarshipApplication>> GetMyScholarshipApplicationsAsync(ScholarshipApplicationSearchDTO scholarshipApplicationSearchDTO, string id, bool isStaff)
        {
            if (isStaff)
            {
                return await _unitOfWork.ScholarshipApplications
                             .Where(x => x.Status == Contracts.Enums.StatusEnum.Pending && x.Scholarship!.ScholarshipCreatorId == id)
                             .ToListAsync();
            }
            return await _unitOfWork.ScholarshipApplications
                .Where(x => x.Status == Contracts.Enums.StatusEnum.Pending && x.ScholarshipApplicant!.Id == id)
                .ToListAsync();
        }

        public async Task<ResponseDTO> CreateScholarshipApplicationAsync(ScholarshipApplicationCreateDTO scholarshipApplicationCreateDTO, string id)
        {
            //TODO
            //check all required form feilds are answered and valid.
            //check the names align up
            
            ScholarshipApplication scholarshipApplication = new ScholarshipApplication
            {
                ScholarshipApplicantId = id,
                ScholarshipId = scholarshipApplicationCreateDTO.ScholarshipId,
                ScholarshipFormAnswer = JsonSerializer.Serialize(scholarshipApplicationCreateDTO.ScholarshipFormAnswer),
                Status = Contracts.Enums.StatusEnum.Pending,
                DateCreated = DateTimeOffset.Now,
                DateModified = DateTimeOffset.Now,
            };

            _unitOfWork.ScholarshipApplications.Add(scholarshipApplication);

            await _unitOfWork.CommitAsync();

            return new ResponseDTO { Success = true, Message = "Scholarship Application created successfully." };

        }
    }
}

