namespace LoanService.Models.DTOs;

public class ApplyLoanRequestDto
{
    public string username { get; set; } = string.Empty;
    public string email { get; set; } = string.Empty;
    public string? password { get; set; }
    public string firstname { get; set; } = string.Empty;
    public string lastname { get; set; } = string.Empty;
    public string phone { get; set; } = string.Empty;
    public AddressDto address { get; set; } = new();
    public BankDetailsDto bankdetails { get; set; } = new();
    public EmploymentDetailsDto employmentdetails { get; set; } = new();
    public DebtInfoDto debtinfo { get; set; } = new();
    public decimal loanamount { get; set; }
    public int termmonths { get; set; }
    public string purpose { get; set; } = string.Empty;
}

public class AddressDto
{
    public string street { get; set; } = string.Empty;
    public string city { get; set; } = string.Empty;
    public string state { get; set; } = string.Empty;
    public string zipcode { get; set; } = string.Empty;
    public string country { get; set; } = string.Empty;
}

public class BankDetailsDto
{
    public string bankname { get; set; } = string.Empty;
    public string accountnumber { get; set; } = string.Empty;
    public string routingnumber { get; set; } = string.Empty;
}

public class EmploymentDetailsDto
{
    public string employername { get; set; } = string.Empty;
    public string jobtitle { get; set; } = string.Empty;
    public decimal monthlyincome { get; set; }
    public int yearsemployed { get; set; }
}

public class DebtInfoDto
{
    public decimal totaldebt { get; set; }
    public decimal monthlydebtpayment { get; set; }
}
