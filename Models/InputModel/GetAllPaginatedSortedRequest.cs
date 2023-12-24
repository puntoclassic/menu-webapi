using System.ComponentModel.DataAnnotations;
namespace MenuWebapi.Models.InputModel
{
    public class GetAllPaginatedSortedRequest
    {
        public bool Paginated { get; set; } = true;
        [Range(1, Int32.MaxValue, ErrorMessage = "Il numero di pagina deve essere maggiore di 1")]
        public int Page { get; set; } = 1;
        [Range(2, 500, ErrorMessage = "Inserisci un valore compreso tra 2 e 500")]
        public int PerPage { get; set; } = 5;
        public string OrderBy { get; set; } = "Id";
        public bool Ascending { get; set; } = true;
        public string? SearchKey { get; set; }
    }
}
