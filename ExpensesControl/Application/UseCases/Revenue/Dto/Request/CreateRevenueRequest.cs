using ExpensesControl.Application.UseCases.Base;
using ExpensesControl.Application.UseCases.Revenue.Dto.Response;
using ExpensesControl.Domain.Enums;

namespace ExpensesControl.Application.UseCases.Revenue.Dto.Request
{
    public class CreateRevenueRequest : BaseRequest<CreateRevenueResponse>
    { 
        public int UserCode { get; set; }
        public string? Description { get; set; }
        public decimal Amount { get; set; }
        public DateOnly ReceiptDate { get; set; }
        public TypeIncome Type { get; set; }
        public CreateRevenueRecurrenceRequest Recurrence { get; set; } = new CreateRevenueRecurrenceRequest();
    }
}
