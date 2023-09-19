using Microsoft.EntityFrameworkCore;
using SelectU.Contracts;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
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

            var scholarshipApplications = await query.ToListAsync();

            return scholarshipApplications
                .Select(scholarshipApplication => new ScholarshipApplicationUpdateDTO(scholarshipApplication))
                .ToList();

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

