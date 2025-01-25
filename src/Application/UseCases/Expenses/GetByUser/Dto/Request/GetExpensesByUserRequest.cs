using ExpensesControl.Application.UseCases.Expenses.GetByUser.Dto.Response;
using MediatR;

namespace ExpensesControl.Application.UseCases.Expenses.GetByUser.Dto.Request;

public record GetExpensesByUserCodeRequest(int UserCode) : IRequest<GetExpensesByUserResponse>;
