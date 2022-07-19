using AutoMapper;
using Core.Application.Interfaces;
using Core.Domain.Entities.Application;
using Microsoft.AspNetCore.Mvc;
using WebApi.DTOs;

namespace Presentation.WebApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactController : ControllerBase
{
    private readonly IContactService _contactService;
    private readonly IMapper _mapper;
    public ContactController(IContactService contactRepository, IMapper mapper)
    {
        _contactService = contactRepository;
        _mapper = mapper;

    }

    [HttpGet]
    public async Task<IActionResult> GetAllContact()
    {
        var contacts = await _contactService.GetAll();
        return Ok(_mapper.Map<List<ContactDto>>(contacts));
    }

    [HttpGet("getcontactbynamespec/{company}")]
    public async Task<IActionResult> GetContactByCompany(string company)
    {
        var contacts = await _contactService.GetContactByCompany(company);
        return Ok(_mapper.Map<List<ContactDto>>(contacts));
    }

    [HttpGet()]
    [Route("getfullcontact/{id}")]
    public async Task<IActionResult> GetFullContactById(int id)
    {
        var contacts = await _contactService.GetFullContact(id);
        return Ok(_mapper.Map<ContactDto>(contacts));
    }

    [HttpPost]
    [Route("addcontact")]
    public async Task<IActionResult> AddContact(ContactDto contact)
    {
        var result = await _contactService.Add(_mapper.Map<Contact>(contact));
        return Ok(_mapper.Map<ContactDto>(result));
    }

    [HttpPost]
    [Route("updatecontact")]
    public async Task<IActionResult> UpdateContact(ContactDto contact)
    {
        var result = await _contactService.Update(_mapper.Map<Contact>(contact));
        return Ok(_mapper.Map<ContactDto>(result));
    }

    [HttpPost]
    [Route("deletecontact/{id}")]
    public async Task<IActionResult> DeleteContact(int id)
    {
        await _contactService.Delete(id);
        return Ok();
    }
}
