using DI.Interfaces;
using DI.Interfaces.Services;

namespace DI.Controllers;

public class Controller
{
    private readonly IService _service;

    public Controller(IService service)
    {
        _service = service;
    }

    public void DoSmth()
    {
        
    }
}