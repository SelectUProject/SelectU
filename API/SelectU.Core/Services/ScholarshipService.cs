using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SelectU.Contracts;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Services;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SelectU.Core.Services
{
    public class ScholarshipService : IScholarshipService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ScholarshipService(IUnitOfWork context)
        {
            _unitOfWork = context;
        }

        //TODO 
        //Create custom exceptions

        public async Task<ScholarshipUpdateDTO> GetScholarshipAsync(Guid id)
        {
            var Scholarship = await _unitOfWork.Scholarships.GetAsync(id);

            if (Scholarship == null)
            {
                return null;
            }

            return new ScholarshipUpdateDTO(Scholarship);
        }

        public async Task<List<ScholarshipUpdateDTO>> GetActiveScholarshipAsync(ScholarshipSearchDTO scholarshipSearchDTO)
        {
            var Scholarships = await _unitOfWork.Scholarships
                .Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now && x.Status == StatusEnum.Pending)
                .ToListAsync();

            var FilterScholarshps = await FilterQuery(scholarshipSearchDTO, Scholarships);

            return FilterScholarshps
                .Select(scholarship => new ScholarshipUpdateDTO(scholarship))
                .ToList();
        }

        public async Task<List<ScholarshipUpdateDTO>> GetMyCreatedScholarshipsAsync(ScholarshipSearchDTO scholarshipSearchDTO, string id)
        {
            var Scholarships = await _unitOfWork.Scholarships.Where(x => x.ScholarshipCreatorId == id).ToListAsync();

            var FilterScholarshps = await FilterQuery(scholarshipSearchDTO, Scholarships);

            return FilterScholarshps
                .Select(scholarship => new ScholarshipUpdateDTO(scholarship))
                .ToList();
        }

            public async Task<ResponseDTO> CreateScholarshipAsync(ScholarshipCreateDTO scholarshipCreateDTO, string id)
        {
            //TODO
            //check the names align up and don't double up

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

        public async Task<List<Scholarship>> FilterQuery(ScholarshipSearchDTO scholarshipSearchDTO, List<Scholarship> scholarships)
        {
            IEnumerable<Scholarship> filteredScholarships = scholarships;

            if (scholarshipSearchDTO.Id != null)
            {
                filteredScholarships = filteredScholarships.Where(x => x.Id == scholarshipSearchDTO.Id);
            }
            if (!string.IsNullOrEmpty(scholarshipSearchDTO.Description))
            {
                filteredScholarships = filteredScholarships.Where(x => x.Description == scholarshipSearchDTO.Description);
            }
            if (!string.IsNullOrEmpty(scholarshipSearchDTO.School))
            {
                filteredScholarships = filteredScholarships.Where(x => x.School == scholarshipSearchDTO.School);
            }
            if (!string.IsNullOrEmpty(scholarshipSearchDTO.City))
            {
                filteredScholarships = filteredScholarships.Where(x => x.City == scholarshipSearchDTO.City);
            }
            if (!string.IsNullOrEmpty(scholarshipSearchDTO.Value))
            {
                filteredScholarships = filteredScholarships.Where(x => x.Value == scholarshipSearchDTO.Value);
            }
            if (scholarshipSearchDTO.Status != null)
            {
                filteredScholarships = filteredScholarships.Where(x => x.Status == scholarshipSearchDTO.Status);
            }
            if (scholarshipSearchDTO.StartDate != null)
            {
                filteredScholarships = filteredScholarships.Where(x => x.StartDate == scholarshipSearchDTO.StartDate);
            }
            if (scholarshipSearchDTO.EndDate != null)
            {
                filteredScholarships = filteredScholarships.Where(x => x.EndDate == scholarshipSearchDTO.EndDate);
            }

            return await Task.FromResult(filteredScholarships.ToList());
        }


    }
}
