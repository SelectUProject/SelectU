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
            ScholarshipCreateDTO validatedScholarship = ValidateScholarship(scholarshipCreateDTO);
            

            Scholarship scholarship = new Scholarship
            {
                ScholarshipCreatorId = id,
                ScholarshipFormTemplate = JsonSerializer.Serialize(validatedScholarship.ScholarshipFormTemplate),
                School = validatedScholarship.School,
                Value = validatedScholarship.Value,
                ShortDescription = validatedScholarship.ShortDescription,
                Description = validatedScholarship.Description,
                Status = StatusEnum.Pending,
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

        public async Task<ResponseDTO> UpdateScholarshipsAsync(ScholarshipUpdateDTO ScholarshipUpdateDTO)
        {
            if (ScholarshipUpdateDTO.Id != null)
            {
                var oldScholarship = await GetScholarshipAsync((Guid)ScholarshipUpdateDTO.Id);
                if(oldScholarship == null)
                {
                    throw new NotFoundException("Scholarship not Found");
                }
                Scholarship updatedScholarship = new Scholarship
                {
                    Id = (Guid)ScholarshipUpdateDTO.Id,
                    ScholarshipFormTemplate = JsonSerializer.Serialize(ScholarshipUpdateDTO.ScholarshipFormTemplate) ?? JsonSerializer.Serialize(oldScholarship.ScholarshipFormTemplate),
                    School = ScholarshipUpdateDTO.School ?? oldScholarship.School,
                    Value = ScholarshipUpdateDTO.Value ??oldScholarship.Value,
                    ShortDescription = ScholarshipUpdateDTO.ShortDescription ?? oldScholarship.ShortDescription,
                    ScholarshipCreatorId = ScholarshipUpdateDTO.ScholarshipCreatorId ?? oldScholarship.ScholarshipCreatorId,
                    Description = ScholarshipUpdateDTO.Description ?? oldScholarship.Description,
                    Status = (StatusEnum)(ScholarshipUpdateDTO.Status != null ? ScholarshipUpdateDTO.Status : oldScholarship.Status),
                    State = ScholarshipUpdateDTO.State ?? oldScholarship.State,
                    City = ScholarshipUpdateDTO.City ?? oldScholarship.City,
                    StartDate = ScholarshipUpdateDTO.StartDate ?? oldScholarship.StartDate,
                    EndDate = ScholarshipUpdateDTO.EndDate ?? oldScholarship.EndDate,
                    DateModified = DateTimeOffset.Now
                };

                _unitOfWork.Scholarships.Update(updatedScholarship);

                await _unitOfWork.CommitAsync();

                return new ResponseDTO { Success = true, Message = "Scholarship updated successfully." };
            }
            throw new ArgumentNullException(nameof(ScholarshipUpdateDTO.Id), "Scholarship Id cannot be null");
        }

        public async Task<ResponseDTO> DeleteScholarshipsAsync(Guid id)
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
