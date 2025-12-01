using Microsoft.AspNetCore.Mvc;
using ShopTARgv24.ApplicationServices.Services;
using ShopTARgv24.Core.Dto;

namespace ShopTARgv24.Controllers
{
    public class EmailController : Controller
    {
        // 1. Объявляем переменную для сервиса
        private readonly EmailServices _emailServices;
        // 2. Внедряем сервис через конструктор (Dependency Injection)
        public EmailController(EmailServices emailServices)
        {
            _emailServices = emailServices;
        }
        public IActionResult Index()
        {
            return View();
        }

        //teha meetod nimega SendEmail, mis votab vastu EmailDto objekti
        //kasutab EmailServices klassi, et saata emaili

        // POST: Принимает данные из формы и отправляет письмо
        [HttpPost]
        public IActionResult SendEmail(EmailDto dto)
        {
            // Вызываем метод отправки из сервиса
            _emailServices.SendEmail(dto);

            // После отправки возвращаем пользователя на ту же страницу
            // (или можно перенаправить на страницу "Спасибо")
            return RedirectToAction(nameof(Index));
        }

    }
}
