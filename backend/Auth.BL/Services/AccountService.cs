using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auth.DAL.Data.Entities;
using AutoMapper;
using Common.AuthInterfaces;
using Common.DTO;
using Common.Enums;
using Microsoft.AspNetCore.Identity;

namespace Auth.DAL.Services {
    public class AccountService : IAccountService {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        public AccountService(UserManager<User> userManager, IMapper mapper,
                SignInManager<User> signInManager) {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
        }
        public async Task Register(RegisterModelDTO model) {
           var user = _mapper.Map<User>(model);

            var result = await _userManager.CreateAsync(user, model.Password); // Создание нового пользователя в системе с указанными данными и введенным паролем
            if (result.Succeeded) // результат может быть успешным, может также возникнуть ошибка, если был введен пароль, не отвечающий требованиям
            {
                // Если регистрация прошла успешно, авторизуем пользователя в системе. Следующая строка создает cookie, который будет использоватся в следующих запросах от пользователя
                await _signInManager.SignInAsync(user, false);
                return;
            }
            // Если произошла ошибка, собираем все ошибки в одну строку и выбрасываем наверх исключение
            var errors = string.Join(", ", result.Errors.Select(x => x.Description));
            throw new ArgumentException(errors);
        }

    }
}
