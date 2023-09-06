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

        public async Task<List<ScholarshipApplication>> GetMyScholarshipApplicationsAsync(ScholarshipApplicationSearchDTO scholarshipApplicationSearchDTO, string id, bool isAdmin)
        {
            if (isAdmin)
            {
                return await _unitOfWork.ScholarshipApplications
                             .Where(x => x.Status == Contracts.Enums.StatusEnum.Pending && x.Scholarship!.ScholarshipCreatorId == id)
                             .ToListAsync();
            }
            return await _unitOfWork.ScholarshipApplications
                .Where(x => x.Status == Contracts.Enums.StatusEnum.Pending && x.ScholarshipApplicant!.Id == id)
                .ToListAsync();
        }
    }
}
