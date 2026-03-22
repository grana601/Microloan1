using LoanService.Models.DTOs;

namespace LoanService.Interfaces;

public interface ILoanService
{
    Task<Guid> CreateLoanAsync(CreateLoanDto dto);
    Task<LoanDto?> GetLoanByIdAsync(Guid id);
}
