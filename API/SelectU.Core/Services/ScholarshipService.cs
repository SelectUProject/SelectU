using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SelectU.Contracts;
using SelectU.Contracts.DTO;
using SelectU.Contracts.Entities;
using SelectU.Contracts.Enums;
using SelectU.Contracts.Services;
using SelectU.Core.Exceptions;
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

        public async Task<List<ScholarshipUpdateDTO>> GetScholarshipsAsync(ScholarshipSearchDTO scholarshipSearchDTO)
        {
            var scholarships = await _unitOfWork.Scholarships.AsQueryable().ToListAsync();

            var filterScholarshps = await FilterQuery(scholarshipSearchDTO, scholarships);

            return filterScholarshps
                .Select(scholarship => new ScholarshipUpdateDTO(scholarship))
                .ToList();
        }

        public async Task<ScholarshipUpdateDTO> GetScholarshipAsync(Guid id)
        {
            var scholarship = await _unitOfWork.Scholarships.GetAsync(id);

            if (scholarship == null)
            {
                return null;
            }

            return new ScholarshipUpdateDTO(scholarship);
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

        public async Task<List<ScholarshipUpdateDTO>> GetMyCreatedScholarshipsAsync(ScholarshipSearchDTO scholarshipSearchDTO, string id)
        {
            var scholarships = await _unitOfWork.Scholarships.Where(x => x.ScholarshipCreatorId == id).ToListAsync();

            var filterScholarshps = await FilterQuery(scholarshipSearchDTO, scholarships);

            return filterScholarshps
                .Select(scholarship => new ScholarshipUpdateDTO(scholarship))
                .ToList();
        }

        public async Task<ResponseDTO> CreateScholarshipAsync(ScholarshipCreateDTO scholarshipCreateDTO, string id)
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

        public ScholarshipCreateDTO ValidateScholarship(ScholarshipCreateDTO scholarshipCreateDTO)
        {
            ScholarshipCreateDTO validatedScholarship = scholarshipCreateDTO;


            if (validatedScholarship.StartDate > validatedScholarship.EndDate)
            {
                throw new ScholarshipException("Start date is after end date.");
            }

            if (validatedScholarship.StartDate <= DateTime.Now)
            {
                throw new ScholarshipApplicationException("The start date has already passed.");
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

        public async Task<ResponseDTO> UpdateScholarshipAsync(ScholarshipUpdateDTO scholarshipUpdateDTO)
        {
            if (scholarshipUpdateDTO.Id != null)
            {
                var oldScholarship = await GetScholarshipAsync((Guid)scholarshipUpdateDTO.Id);
                if (oldScholarship == null)
                {
                    throw new NotFoundException("Scholarship not Found");
                }
                Scholarship updatedScholarship = new Scholarship
                {
                    Id = (Guid)scholarshipUpdateDTO.Id,
                    ScholarshipFormTemplate = JsonSerializer.Serialize(scholarshipUpdateDTO.ScholarshipFormTemplate) ?? JsonSerializer.Serialize(oldScholarship.ScholarshipFormTemplate),
                    School = scholarshipUpdateDTO.School ?? oldScholarship.School,
                    Value = scholarshipUpdateDTO.Value ?? oldScholarship.Value,
                    ShortDescription = scholarshipUpdateDTO.ShortDescription ?? oldScholarship.ShortDescription,
                    ScholarshipCreatorId = scholarshipUpdateDTO.ScholarshipCreatorId ?? oldScholarship.ScholarshipCreatorId,
                    Description = scholarshipUpdateDTO.Description ?? oldScholarship.Description,
                    Status = (ScholarshipStatusEnum)(scholarshipUpdateDTO.Status != null ? scholarshipUpdateDTO.Status : oldScholarship.Status),
                    State = scholarshipUpdateDTO.State ?? oldScholarship.State,
                    City = scholarshipUpdateDTO.City ?? oldScholarship.City,
                    StartDate = scholarshipUpdateDTO.StartDate ?? oldScholarship.StartDate,
                    EndDate = scholarshipUpdateDTO.EndDate ?? oldScholarship.EndDate,
                    DateModified = DateTimeOffset.Now
                };

                _unitOfWork.Scholarships.Update(updatedScholarship);

                await _unitOfWork.CommitAsync();

                return new ResponseDTO { Success = true, Message = "Scholarship updated successfully." };
            }
            throw new ArgumentNullException(nameof(ScholarshipUpdateDTO.Id), "Scholarship Id cannot be null");
        }

        public async Task<ResponseDTO> DeleteScholarshipAsync(Guid id)
        {
            var oldScholarship = await GetScholarshipAsync(id);
            if (oldScholarship == null)
            {
                throw new NotFoundException("Scholarship not Found");
            }

            await _unitOfWork.Scholarships.DeleteAsync(id);

            return new ResponseDTO { Success = true, Message = "Scholarship deleted successfully." };
        }
    }
}
