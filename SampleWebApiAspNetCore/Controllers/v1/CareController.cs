using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Services;
using SampleWebApiAspNetCore.Models;
using SampleWebApiAspNetCore.Repositories;
using System.Text.Json;

namespace SampleWebApiAspNetCore.Controllers.v1
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CareController : ControllerBase
    {
        private readonly ICareRepository _careRepository;
        private readonly IMapper _mapper;
        private readonly ILinkService<CareController> _linkService;

        public CareController(
            ICareRepository careRepository,
            IMapper mapper,
            ILinkService<CareController> linkService)
        {
            _careRepository = careRepository;
            _mapper = mapper;
            _linkService = linkService;
        }

        [HttpGet(Name = nameof(GetAllCare))]
        public ActionResult GetAllCare(ApiVersion version, [FromQuery] QueryParameters queryParameters)
        {
            List<CareEntity> careItems = _careRepository.GetAll(queryParameters).ToList();

            var allItemCount = _careRepository.Count();

            var paginationMetadata = new
            {
                totalCount = allItemCount,
                pageSize = queryParameters.PageCount,
                currentPage = queryParameters.Page,
                totalPages = queryParameters.GetTotalPages(allItemCount)
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            var links = _linkService.CreateLinksForCollection(queryParameters, allItemCount, version);
            var toReturn = careItems.Select(x => _linkService.ExpandSingleItem(x, x.Id, version));

            return Ok(new
            {
                value = toReturn,
                links = links
            });
        }

        [HttpGet]
        [Route("{id:int}", Name = nameof(GetSingleCare))]
        public ActionResult GetSingleCare(ApiVersion version, int id)
        {
            CareEntity careItem = _careRepository.GetSingle(id);

            if (careItem == null)
            {
                return NotFound();
            }

            CareDto item = _mapper.Map<CareDto>(careItem);

            return Ok(_linkService.ExpandSingleItem(item, item.Id, version));
        }

        [HttpPost(Name = nameof(AddCare))]
        public ActionResult<CareDto> AddCare(ApiVersion version, [FromBody] CareCreateDto careCreateDto)
        {
            if (careCreateDto == null)
            {
                return BadRequest();
            }

            CareEntity toAdd = _mapper.Map<CareEntity>(careCreateDto);

            _careRepository.Add(toAdd);

            if (!_careRepository.Save())
            {
                throw new Exception("Creating a careitem failed on save.");
            }

            CareEntity newCareItem = _careRepository.GetSingle(toAdd.Id);
            CareDto careDto = _mapper.Map<CareDto>(newCareItem);

            return CreatedAtRoute(nameof(GetSingleCare),
                new { version = version.ToString(), id = newCareItem.Id },
                _linkService.ExpandSingleItem(careDto, careDto.Id, version));
        }

        [HttpPatch("{id:int}", Name = nameof(PartiallyUpdateCare))]
        public ActionResult<CareDto> PartiallyUpdateCare(ApiVersion version, int id, [FromBody] JsonPatchDocument<CareUpdateDto> patchDoc)
        {
            if (patchDoc == null)
            {
                return BadRequest();
            }

            CareEntity existingEntity = _careRepository.GetSingle(id);

            if (existingEntity == null)
            {
                return NotFound();
            }

            CareUpdateDto careUpdateDto = _mapper.Map<CareUpdateDto>(existingEntity);
            patchDoc.ApplyTo(careUpdateDto);

            TryValidateModel(careUpdateDto);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _mapper.Map(careUpdateDto, existingEntity);
            CareEntity updated = _careRepository.Update(id, existingEntity);

            if (!_careRepository.Save())
            {
                throw new Exception("Updating a careitem failed on save.");
            }

            CareDto foodDto = _mapper.Map<CareDto>(updated);

            return Ok(_linkService.ExpandSingleItem(foodDto, foodDto.Id, version));
        }

        [HttpDelete]
        [Route("{id:int}", Name = nameof(RemoveCare))]
        public ActionResult RemoveCare(int id)
        {
            CareEntity careItem = _careRepository.GetSingle(id);

            if (careItem == null)
            {
                return NotFound();
            }

            _careRepository.Delete(id);

            if (!_careRepository.Save())
            {
                throw new Exception("Deleting a careitem failed on save.");
            }

            return NoContent();
        }

        [HttpPut]
        [Route("{id:int}", Name = nameof(UpdateCare))]
        public ActionResult<CareDto> UpdateCare(ApiVersion version, int id, [FromBody] CareUpdateDto careUpdateDto)
        {
            if (careUpdateDto == null)
            {
                return BadRequest();
            }

            var existingCareItem = _careRepository.GetSingle(id);

            if (existingCareItem == null)
            {
                return NotFound();
            }

            _mapper.Map(careUpdateDto, existingCareItem);

            _careRepository.Update(id, existingCareItem);

            if (!_careRepository.Save())
            {
                throw new Exception("Updating a careitem failed on save.");
            }

            CareDto careDto = _mapper.Map<CareDto>(existingCareItem);

            return Ok(_linkService.ExpandSingleItem(careDto, careDto.Id, version));
        }

        [HttpGet("GetRandomCare", Name = nameof(GetRandomCare))]
        public ActionResult GetRandomCare()
        {
            ICollection<CareEntity> careItems = _careRepository.GetRandomCare();

            IEnumerable<CareDto> dtos = careItems.Select(x => _mapper.Map<CareDto>(x));

            var links = new List<LinkDto>();

            // self 
            links.Add(new LinkDto(Url.Link(nameof(GetRandomCare), null), "self", "GET"));

            return Ok(new
            {
                value = dtos,
                links = links
            });
        }
    }
}
