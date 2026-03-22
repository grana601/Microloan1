using LoanService.Data;
using LoanService.Interfaces;
using LoanService.Models.DTOs;
using LoanService.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoanService.Services;

public class LoanAppService : ILoanService
{
    private readonly loandbcontext _dbContext;

    public LoanAppService(loandbcontext dbContext)
    {
        _dbContext = dbContext;
    }


}
