using System;
using System.Collections.Generic;
using System.Text;
using Volo.Abp.Application.Dtos;

namespace Acme.BookStore.Books
{
    public class GetBookListByAuthorIdDto : PagedAndSortedResultRequestDto
    {
        public Guid? AuthorId { get; set; }
    }
}
