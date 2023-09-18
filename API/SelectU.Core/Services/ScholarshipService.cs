using Microsoft.EntityFrameworkCore;
using SelectU.Contracts;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Services;

namespace SelectU.Core.Services
{
    public class ScholarshipService : IScholarshipService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ScholarshipService(IUnitOfWork context)
        {
            _unitOfWork = context;
        }

        public async Task<Scholarship> GetScholarshipAsync(Guid id)
        {
            return await _unitOfWork.Scholarships.GetAsync(id);
        }

        public async Task<List<Scholarship>> GetActiveScholarshipAsync(ScholarshipSearchDTO scholarshipSearchDTO)
        {
            return await _unitOfWork.Scholarships
                .Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now && x.Status == StatusEnum.Pending)
                .ToListAsync();
        }

        public async Task<List<Scholarship>> GetMyCreatedScholarshipsAsync(ScholarshipSearchDTO scholarshipSearchDTO, string id)
        {
            return await _unitOfWork.Scholarships.Where(x => x.ScholarshipCreatorId == id).ToListAsync();
        }

        public async Task<ResponseDTO> CreateScholarshipAsync(CreateScholarshipDTO createScholarshipDTO, string id)
        {

            Scholarship scholarship = new Scholarship
            {
                ScholarshipCreatorId = id,
                ScholarshipFormTemplate = createScholarshipDTO.ScholarshipFormTemplate,
                School = createScholarshipDTO.School,
                Value = createScholarshipDTO.Value,
                ShortDescription = createScholarshipDTO.ShortDescription,
                Description = createScholarshipDTO.Description,
                State = createScholarshipDTO.State,
                City = createScholarshipDTO.City,
                StartDate = createScholarshipDTO.StartDate,
                EndDate = createScholarshipDTO.EndDate,
                DateCreated = DateTimeOffset.Now,
                DateModified = DateTimeOffset.Now,
            };

            _unitOfWork.Scholarships.Add(scholarship);

            return new ResponseDTO { Success = true, Message = "Scholarship created successfully." };
        }


    }
}
