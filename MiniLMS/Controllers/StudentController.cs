using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
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
    private readonly IMediator _mediator;
    private readonly ILogger<StudentController> _logger;
    private Guid guid = Guid.NewGuid();
    public StudentController(IStudentService studentService, IMapper mapper, IMediator mediator, ILogger<StudentController> logger)
    {
        _studentService = studentService;
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }


    [HttpGet]
    [EnableRateLimiting("token")]
    public async void Rate_limiter()
    {
        await Console.Out.WriteLineAsync("Request:"+guid);
        // Thread.Sleep(5000);
        await Console.Out.WriteLineAsync("Response :  "+guid);
         
    }

    [HttpGet]
    [EnableRateLimiting("token")]
    public async Task<ResponseModel<IEnumerable<StudentGetDTO>>> GetAll()
    {
        IEnumerable<Student> student = await _studentService.GetAllAsync();
        IEnumerable<StudentGetDTO> students = _mapper.Map<IEnumerable<StudentGetDTO>>(student);

        return new(students);
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
