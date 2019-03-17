﻿using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Model;
using UserManager;
using Web.Models;

namespace Web.Controllers
{
    public class UsersController : ControllerBase
    {
        private readonly IUserReaderService _userReaderService;
        private readonly IUserWriterService _userWriterService;

        public UsersController(IUserReaderService userReaderService, IUserWriterService userWriterService)
        {
            _userReaderService = userReaderService;
            _userWriterService = userWriterService;
        }

        [HttpGet]
        public async Task<IEnumerable<UserOutputModel>> Index() =>
            await _userReaderService.List()
            .Select(UserOutputModel.Map)
            .ToAsyncEnumerable()
            .ToList();

        [HttpGet]
        [Route("users/{id}")]
        public async Task<UserOutputModel> Index(int id) => 
            await _userReaderService.List(new FilterInputModel { Id = id })
            .Select(UserOutputModel.Map)
            .ToAsyncEnumerable()
            .First();

        [HttpPut]
        [Route("users/{id}")]
        public async Task Update([FromBody]UpdateUserInputModel model, int id)
        {
            model.Id = id;
            await _userWriterService.Save(model);
        }

        [HttpPost]
        [Route("users")]
        public async Task Create([FromBody]NewUserInputModel model)
        {
            var userConnectedId = int.Parse(HttpContext.User.Claims.First(_ => _.Type == ClaimTypes.NameIdentifier).Value);
            model.ParentUser = userConnectedId;
            await _userWriterService.Save(model);
        }

        [HttpDelete]
        [Route("users/{id}")]
        public async Task Delete(int id)
        {
            var userConnectedId = int.Parse(HttpContext.User.Claims.First(_ => _.Type == ClaimTypes.NameIdentifier).Value);
            await _userWriterService.Delete(id, userConnectedId);
        }
    }
}
