using AutoMapper;
using LazyCache;
using Microsoft.AspNetCore.Mvc;
using MiniLMS.Application.Services;
using MiniLMS.Domain.Entities;
using MiniLMS.Domain.Models;
using MiniLMS.Domain.Models.TeacherDTO;

namespace MiniLMS.API.Controllers;

[Route("api/[controller]/[action]")]
[ApiController]
public class TeacherController : ControllerBase
{
    private readonly ITeacherService _teacherService;
    private readonly IMapper _mapper;
    private readonly IAppCache _appCache;
    private string _key = "salom";

    public TeacherController(ITeacherService teacherService, IMapper mapper, IAppCache appCache)
    {
        _teacherService = teacherService;
        _mapper = mapper;
        _appCache = appCache;
    }

    [HttpGet]
    public async Task<ResponseModel<IEnumerable<TeacherGetDTO>>> GetAll()
    {
        IEnumerable<Teacher> value = _appCache.Get<IEnumerable<Teacher>>(_key);
        //  var get = new IEnumerable<Teacher>;
        // IEnumerable<TeacherGetDTO> teachers = _mapper.Map<IEnumerable<TeacherGetDTO>>(get);
       // if (value==null)
       // {
            var get = await _teacherService.GetAllAsync();
           var teachers = _mapper.Map<IEnumerable<TeacherGetDTO>>(get);
            _appCache.Add(_key, teachers);
            return new(teachers);
        //}
       // return new(value);
    }
    [HttpGet]
    public async Task<ResponseModel<TeacherGetDTO>> GetById(int id)
    {
        Teacher teacherEntity = await _teacherService.GetByIdAsync(id);
        TeacherGetDTO teacherDto = _mapper.Map<TeacherGetDTO>(teacherEntity);
        return new(teacherDto);
    }
    [HttpPost]
    public async Task<ResponseModel<TeacherGetDTO>> Create(TeacherCreateDTO teacherCreateDto)
    {
        Teacher mappedTeacher = _mapper.Map<Teacher>(teacherCreateDto);

        Teacher teacherEntity = await _teacherService.CreateAsync(mappedTeacher);

        TeacherGetDTO teacherDto = _mapper.Map<TeacherGetDTO>(teacherEntity);
        return new(teacherDto);
    }

    [HttpDelete]
    public async Task<ResponseModel<string>> DeleteAsync(int id)
    {
        bool resultDelete = await _teacherService.DeleteAsync(id);
        string res = resultDelete ? "O'chirildi" : "Bunday id topilmadi";
        return new(res);
    }
    [HttpPut]
    public async Task<ResponseModel<TeacherGetDTO>> Update(TeacherCreateDTO createDTO)
    {
        Teacher teacher = _mapper.Map<Teacher>(createDTO);
        await _teacherService.UpdateAsync(teacher);
        TeacherGetDTO result = _mapper.Map<TeacherGetDTO>(teacher);
        return new(result);
    }
}
