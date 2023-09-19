using Microsoft.EntityFrameworkCore;
using SelectU.Contracts;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using System.Collections.Generic;
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

            return Scholarships 
                .Select(scholarship => new ScholarshipUpdateDTO(scholarship))
                .ToList();
        }

        public async Task<List<ScholarshipUpdateDTO>> GetMyCreatedScholarshipsAsync(ScholarshipSearchDTO scholarshipSearchDTO, string id)
        {
            var Scholarships = await _unitOfWork.Scholarships.Where(x => x.ScholarshipCreatorId == id).ToListAsync();

            return Scholarships
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


    }
}
