using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SelectU.Contracts;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Infrastructure;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
using SelectU.Core.Infrastructure;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace SelectU.Core.Services
{
    public class ScholarshipService : IScholarshipService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailClient _emailclient;

        public ScholarshipService(IUnitOfWork context, IEmailClient emailClient)
        {
            _unitOfWork = context;
            _emailclient = emailClient;
        }

        //TODO 
        //Create custom exceptions

        public async Task<List<ScholarshipUpdateDTO>> GetScholarshipsAsync(ScholarshipSearchDTO scholarshipSearchDTO)
        {
            var scholarships = await _unitOfWork.Scholarships.AsQueryable().ToListAsync();

            var filterScholarshps = await FilterQuery(scholarshipSearchDTO, scholarships);

            return filterScholarshps
                .Select(scholarship => new ScholarshipUpdateDTO(scholarship))
                .ToList();
        }

        public async Task<Scholarship> GetScholarshipAsync(Guid id)
        {
            return await _unitOfWork.Scholarships.GetAsync(id);
        }

        public async Task<List<ScholarshipUpdateDTO>> GetActiveScholarshipsAsync(ScholarshipSearchDTO scholarshipSearchDTO)
        {
            var scholarships = await _unitOfWork.Scholarships
                .Where(x => x.StartDate <= DateTime.Now && x.EndDate >= DateTime.Now && x.Status == ScholarshipStatusEnum.Active)
                .ToListAsync();

            var filterScholarshps = await FilterQuery(scholarshipSearchDTO, scholarships);

            return filterScholarshps
                .Select(scholarship => new ScholarshipUpdateDTO(scholarship))
                .ToList();
        }

        public async Task<List<ScholarshipUpdateDTO>> GetMyCreatedScholarshipsAsync(ScholarshipSearchDTO scholarshipSearchDTO, string userId)
        {
            var scholarships = await _unitOfWork.Scholarships.Where(x => x.ScholarshipCreatorId == userId).ToListAsync();

            var filterScholarshps = await FilterQuery(scholarshipSearchDTO, scholarships);

            return filterScholarshps
                .Select(scholarship => new ScholarshipUpdateDTO(scholarship))
                .ToList();
        }


        public async Task<ScholarshipUpdateDTO> CreateScholarshipAsync(ScholarshipCreateDTO scholarshipCreateDTO, string id)
        {
            //TODO
            //check the names align up and don't double up
            ScholarshipCreateDTO validatedScholarship = ValidateScholarship(scholarshipCreateDTO);


            Scholarship scholarship = new Scholarship
            {
                ScholarshipCreatorId = id,
                ScholarshipFormTemplate = JsonSerializer.Serialize(validatedScholarship.ScholarshipFormTemplate),
                School = validatedScholarship.School,
                Value = validatedScholarship.Value,
                ShortDescription = validatedScholarship.ShortDescription,
                Description = validatedScholarship.Description,
                Status = ScholarshipStatusEnum.Active,
                State = validatedScholarship.State,
                City = validatedScholarship.City,
                StartDate = validatedScholarship.StartDate,
                EndDate = validatedScholarship.EndDate,
                DateCreated = DateTimeOffset.Now,
                DateModified = DateTimeOffset.Now,
            };

            _unitOfWork.Scholarships.Add(scholarship);

            await _unitOfWork.CommitAsync();

            return new ScholarshipUpdateDTO(scholarship);
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
                filteredScholarships = filteredScholarships.Where(x => x.Description.ToLower().Contains(scholarshipSearchDTO.Description.ToLower()));
            }
            if (!string.IsNullOrEmpty(scholarshipSearchDTO.School))
            {
                filteredScholarships = filteredScholarships.Where(x => x.School.ToLower().Contains(scholarshipSearchDTO.School.ToLower()));
            }
            if (!string.IsNullOrEmpty(scholarshipSearchDTO.City))
            {
                filteredScholarships = filteredScholarships.Where(x => x.City.ToLower().Contains(scholarshipSearchDTO.City.ToLower()));
            }
            if (!string.IsNullOrEmpty(scholarshipSearchDTO.Value))
            {
                filteredScholarships = filteredScholarships.Where(x => x.Value.ToLower().Contains(scholarshipSearchDTO.Value.ToLower()));
            }
            if (scholarshipSearchDTO.Status != null)
            {
                filteredScholarships = filteredScholarships.Where(x => x.Status == scholarshipSearchDTO.Status);
            }
            if (scholarshipSearchDTO.StartDate != null)
            {
                filteredScholarships = filteredScholarships.Where(x => x.StartDate?.Date == scholarshipSearchDTO?.StartDate?.Date);
            }
            if (scholarshipSearchDTO.EndDate != null)
            {
                filteredScholarships = filteredScholarships.Where(x => x.EndDate?.Date == scholarshipSearchDTO.EndDate?.Date);
            }

            return await Task.FromResult(filteredScholarships.ToList());
        }

        public ScholarshipCreateDTO ValidateScholarship(ScholarshipCreateDTO scholarshipCreateDTO)
        {
            ScholarshipCreateDTO validatedScholarship = scholarshipCreateDTO;


            if (validatedScholarship.StartDate > validatedScholarship.EndDate)
            {
                throw new ScholarshipException("Start date is after end date.");
            }

            if (validatedScholarship.EndDate <= DateTime.Now)
            {
                throw new ScholarshipApplicationException("The end date has already passed.");
            }


            if (validatedScholarship.ScholarshipFormTemplate.Count > 0)
            {
                var seenNames = new HashSet<string>();

                foreach (var formTemplate in validatedScholarship.ScholarshipFormTemplate)
                {
                    if (string.IsNullOrEmpty(formTemplate.Name))
                    {
                        throw new ScholarshipException("Not all form sections have names.");
                    }

                    if (!seenNames.Add(formTemplate.Name))
                    {
                        throw new ScholarshipException($"Duplicate name found: {formTemplate.Name}");
                    }

                    if (formTemplate.Type == ScholarshipFormTypeEnum.Option && (formTemplate.Options == null || formTemplate.Options.Count == 0))
                    {
                        throw new ScholarshipException($"Form section '{formTemplate.Name}' of type 'option' has no Options.");
                    }
                }
            }
            else
            {
                throw new ScholarshipException("No form sections");
            }

            return validatedScholarship;
        }


        public void ValidateScholarship(ScholarshipUpdateDTO scholarshipUpdateDTO)
        {
            ScholarshipUpdateDTO validatedScholarship = scholarshipUpdateDTO;


            if (validatedScholarship.StartDate > validatedScholarship.EndDate)
            {
                throw new ScholarshipException("Start date is after end date.");
            }

            if (validatedScholarship.EndDate <= DateTime.Now)
            {
                throw new ScholarshipApplicationException("The end date has already passed.");
            }


            if (validatedScholarship.ScholarshipFormTemplate.Count > 0)
            {
                var seenNames = new HashSet<string>();

                foreach (var formTemplate in validatedScholarship.ScholarshipFormTemplate)
                {
                    if (string.IsNullOrEmpty(formTemplate.Name))
                    {
                        throw new ScholarshipException("Not all form sections have names.");
                    }

                    if (!seenNames.Add(formTemplate.Name))
                    {
                        throw new ScholarshipException($"Duplicate name found: {formTemplate.Name}");
                    }

                    if (formTemplate.Type == ScholarshipFormTypeEnum.Option && (formTemplate.Options == null || formTemplate.Options.Count == 0))
                    {
                        throw new ScholarshipException($"Form section '{formTemplate.Name}' of type 'option' has no Options.");
                    }
                }
            }
            else
            {
                throw new ScholarshipException("No form sections");
            }
            
        }

        public async Task UpdateScholarshipAsync(ScholarshipUpdateDTO scholarshipUpdateDTO)
        {
            var scholarship = await GetScholarshipAsync(scholarshipUpdateDTO.Id);
			ValidateScholarship(scholarshipUpdateDTO);
            if (scholarship == null)
            {
                throw new Exception("Scholarship not Found");
            }

            scholarship.School = scholarshipUpdateDTO.School;
            scholarship.ImageURL = scholarshipUpdateDTO.ImageURL;
            scholarship.Value = scholarshipUpdateDTO.Value;
            scholarship.ShortDescription = scholarshipUpdateDTO.ShortDescription;
            scholarship.Description = scholarshipUpdateDTO.Description;
            scholarship.ScholarshipFormTemplate = JsonSerializer.Serialize(scholarshipUpdateDTO.ScholarshipFormTemplate);
            scholarship.City = scholarshipUpdateDTO.City;
            scholarship.State = scholarshipUpdateDTO.State;
            scholarship.Status = scholarshipUpdateDTO.Status;
            scholarship.StartDate = scholarshipUpdateDTO.StartDate;
            scholarship.EndDate = scholarshipUpdateDTO.EndDate;
            scholarship.DateModified = DateTimeOffset.UtcNow;

            _unitOfWork.Scholarships.Update(scholarship);
            await _unitOfWork.CommitAsync();

        }

        public async Task ArchiveScholarshipAsync(Guid id)
        {
            var scholarship = _unitOfWork.Scholarships.Where(x => x.Id == id)
                .Include(x => x.ScholarshipApplications)
                .ThenInclude(x => x.ScholarshipApplicant).FirstOrDefault();

            if (scholarship == null)
            {
                throw new Exception("Scholarship not Found");
            }

            scholarship.Status = ScholarshipStatusEnum.Archived;
            scholarship.DateModified = DateTimeOffset.UtcNow;

            _unitOfWork.Scholarships.Update(scholarship);

            foreach (var scholarshipApplication in scholarship.ScholarshipApplications)
            {
                if (scholarshipApplication.Status != ApplicationStatusEnum.Accepted)
                {
                    scholarshipApplication.Status = ApplicationStatusEnum.Rejected;
                    //await _emailclient.SendUserUnsuccessfulApplicationAsync(scholarshipApplication);
                    _unitOfWork.ScholarshipApplications.Update(scholarshipApplication);
                }
            }

            await _unitOfWork.CommitAsync();
        }
    }
}
