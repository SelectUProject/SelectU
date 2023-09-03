using Microsoft.EntityFrameworkCore;
using SelectU.Contracts;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Services;

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
        public async Task<List<ScholarshipApplication>> GetActiveScholarshipApplicationsAsync(ScholarshipApplicationSearchDTO scholarshipApplicationSearchDTO)
        {
            return await _unitOfWork.ScholarshipApplications
                .Where(x => x.Status == Contracts.Enums.StatusEnum.Pending)
                .ToListAsync();
        }


    }
}
