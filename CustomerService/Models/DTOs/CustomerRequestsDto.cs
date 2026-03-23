namespace CustomerService.Models.DTOs;

public class CustomerAddressRequest
{
    public Guid CustomerId { get; set; }
    public AddressDto Address { get; set; } = new();
}

public class CustomerBankRequest
{
    public Guid CustomerId { get; set; }
    public BankDetailsDto BankDetails { get; set; } = new();
}

public class CustomerEmploymentRequest
{
    public Guid CustomerId { get; set; }
    public EmploymentDetailsDto EmploymentDetails { get; set; } = new();
}

public class CustomerDebtRequest
{
    public Guid CustomerId { get; set; }
    public DebtInfoDto DebtInfo { get; set; } = new();
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
