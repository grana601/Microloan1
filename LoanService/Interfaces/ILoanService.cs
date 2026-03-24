using LoanService.Models.DTOs;
using LoanService.Models.Entities;

namespace LoanService.Interfaces;

public interface ILoanService
{
    Task<bool> ApplyLoanAsync(ApplyLoanRequestDto dto);
    Task<List<loans>> GetLoanListAsync();
}
