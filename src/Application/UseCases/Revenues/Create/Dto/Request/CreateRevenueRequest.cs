using ExpensesControl.Application.UseCases.Revenues.Create.Dto.Response;
using ExpensesControl.Domain.Enums;
using MediatR;

namespace ExpensesControl.Application.UseCases.Revenues.Create.Dto.Request;

public record CreateRevenueRequest(int UserCode,
	string? Description,
	decimal Amount,
	DateOnly ReceiptDate,
	TypeRevenue Type,
	CreateRevenueRecurrenceRequest Recurrence) : IRequest<CreateRevenueResponse>;
