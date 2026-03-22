using LoanService.Models.DTOs;

namespace LoanService.Interfaces;

public interface ILoanService
{
    Task<bool> ApplyLoanAsync(ApplyLoanRequestDto dto);
}
