using ExpensesControl.Application.UseCases.Expenses.Import.Dto.Response;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace ExpensesControl.Application.UseCases.Expenses.Import.Dto.Request;
public record ImportExpensesRequest(int UserCode, IFormFile File) : IRequest<ImportExpensesResponse>;

