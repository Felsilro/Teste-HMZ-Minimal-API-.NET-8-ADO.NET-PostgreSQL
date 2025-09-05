namespace HMZ.Domain.Models;

public readonly record struct PagedResult<T>(IEnumerable<T> Data, int Page, int PerPage, int Total, int TotalPages);
