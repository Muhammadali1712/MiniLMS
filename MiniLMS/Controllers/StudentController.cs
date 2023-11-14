using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using MiniLMS.Application.Services;
using MiniLMS.Domain.Entities;
using MiniLMS.Domain.Models;
using MiniLMS.Domain.Models.StudentDTO;
using Newtonsoft.Json;

namespace MiniLMS.API.Controllers;
[Route("api/[controller]/[action]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly IStudentService _studentService;
    private readonly IMapper _mapper;
    private readonly IDistributedCache _cache;
    private string key = "Mykey1";
    private Serilog.ILogger _logger;
    private readonly IMediator _mediator;
    public StudentController(IStudentService studentService, IMapper mapper, IDistributedCache cache, Serilog.ILogger logger, IMediator mediator)
    {
        _studentService = studentService;
        _mapper = mapper;
        _cache = cache;
        _logger = logger;
        _mediator = mediator;
    }

    [HttpGet]
    public void WriteDatabase()
    {
        _logger.Information("Bu log");
    }

    [HttpGet]

    public async Task<ResponseModel<IEnumerable<StudentGetDTO>>> GetAll()
    {
        string? cachevalue = _cache.GetString(key);


        if (cachevalue == null)
        {
            IEnumerable<Student> student = await _studentService.GetAllAsync();
            IEnumerable<StudentGetDTO> students = _mapper.Map<IEnumerable<StudentGetDTO>>(student);
            cachevalue = JsonConvert.SerializeObject(students);
            _cache.SetString(key, cachevalue);
        }

        IEnumerable<StudentGetDTO>? st = JsonConvert.DeserializeObject<IEnumerable<StudentGetDTO>>(cachevalue);
        return new(st);
    }

    [HttpGet]
    public async Task<ResponseModel<StudentGetDTO>> GetById(int id)
    {
        Student studentEntity = await _studentService.GetByIdAsync(id);
        StudentGetDTO studentDto = _mapper.Map<StudentGetDTO>(studentEntity);
        return new(studentDto);
    }
    [HttpPost]
    public async Task<ResponseModel<StudentGetDTO>> Create(StudentCreateDTO studentCreateDto)
    {
        Student mappedStudent = _mapper.Map<Student>(studentCreateDto);

        Student studentEntity = await _studentService.CreateAsync(mappedStudent);

        StudentGetDTO studentDto = _mapper.Map<StudentGetDTO>(mappedStudent);
        _mediator.Publish(mappedStudent);

        return new(studentDto);
    }

    [HttpDelete]
    public async Task<string> Delete(int id)
    {
        bool result = await _studentService.DeleteAsync(id);
        string s = result ? "O'chirildi" : "Bunday id topilmadi";
        return s;
    }

    [HttpPatch]
    public async Task<ResponseModel<StudentGetDTO>> Update(UpdateStudentDTO update)
    {
        Student Mylogin = await _studentService.GetByIdAsync(update.Id);
        Student mapped = _mapper.Map<Student>(update);
        mapped.Login = Mylogin.Login;
        await _studentService.UpdateAsync(mapped);
        StudentGetDTO studentDto = _mapper.Map<StudentGetDTO>(mapped);
        return new(studentDto);

    }
}
