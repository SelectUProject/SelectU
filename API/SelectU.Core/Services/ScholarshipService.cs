using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using SelectU.Contracts;
using SelectU.Contracts.Constants;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Infrastructure;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using SelectU.Migrations;

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

    }
}
