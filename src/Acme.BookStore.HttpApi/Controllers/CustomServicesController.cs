using Acme.BookStore.Authors;
using Acme.BookStore.Books;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Services;
using static Acme.BookStore.Permissions.BookStorePermissions;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authorization;
using Acme.BookStore.Permissions;

namespace Acme.BookStore.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/app/[controller]")]
    public class CustomServicesController : BookStoreController, IApplicationService
    {
        private readonly IBookAppService _bookAppService;
        private readonly IAuthorAppService _authorAppService;

        public CustomServicesController(IBookAppService bookAppService, IAuthorAppService authorAppService)
        {
            _bookAppService = bookAppService;
            _authorAppService = authorAppService;
        }

        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("GetAuthorLookup")]
        public async Task<ActionResult> GetAuthorLookupAsync()
        {
            return new JsonResult(await _bookAppService.GetAuthorLookupAsync());
        }

        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("GetListByAuthorId")]
        public async Task<ActionResult> GetListByAuthorIdAsync(GetBookListByAuthorIdDto input)
        {
            return new JsonResult(await _bookAppService.GetListByAuthorIdAsync(input));
        }

        //[HttpPost("api/app/custom-services/CreateAuthor")]
        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("CreateAuthor")]
        public async Task<ActionResult> CreateAuthorAsync([FromBody] CreateAuthorDto input)
        {
            var temp = await _authorAppService.CreateAsync(input);

            return new JsonResult(temp);
        }

        //[HttpPut("api/app/custom-services/UpdateBook/{id}")]
        [HttpPut]
        [Microsoft.AspNetCore.Mvc.Route("UpdateBook")]
        public async Task<ActionResult> UpdateBookAsync(Guid id, [FromBody] CreateUpdateBookDto input)
        {
            var temp = await _bookAppService.UpdateAsync(id, input);
            return new JsonResult(temp);
        }
    }
}
