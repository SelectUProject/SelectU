using Microsoft.EntityFrameworkCore;
using SelectU.Contracts;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Services;
using System.Text.Json;

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

        public async Task<ResponseDTO> CreateScholarshipAsync(ScholarshipCreateDTO scholarshipCreateDTO, string id)
        {

            Scholarship scholarship = new Scholarship
            {
                ScholarshipCreatorId = id,
                ScholarshipFormTemplate = JsonSerializer.Serialize(scholarshipCreateDTO.ScholarshipFormTemplate),
                School = scholarshipCreateDTO.School,
                Value = scholarshipCreateDTO.Value,
                ShortDescription = scholarshipCreateDTO.ShortDescription,
                Description = scholarshipCreateDTO.Description,
                Status = StatusEnum.Pending,
                State = scholarshipCreateDTO.State,
                City = scholarshipCreateDTO.City,
                StartDate = scholarshipCreateDTO.StartDate,
                EndDate = scholarshipCreateDTO.EndDate,
                DateCreated = DateTimeOffset.Now,
                DateModified = DateTimeOffset.Now,
            };

            _unitOfWork.Scholarships.Add(scholarship);

            await _unitOfWork.CommitAsync();

            return new ResponseDTO { Success = true, Message = "Scholarship created successfully." };
        }


    }
}
