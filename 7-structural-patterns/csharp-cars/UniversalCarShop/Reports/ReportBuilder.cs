using System.Text;
using UniversalCarShop.Customers;

namespace UniversalCarShop.Reports;

public class ReportBuilder
{
    private readonly StringBuilder _content = new();

    public ReportBuilder AddCustomers(IEnumerable<Customer> customers)
    {
        _content.AppendLine("Покупатели:");

        foreach (var customer in customers)
        {
            _content.AppendLine($" - {customer}");
        }

        _content.AppendLine();

        return this;
    }


    public ReportBuilder AddOperation(string operation)
    {
        _content
            .AppendLine($"Операция: {operation}")
            .AppendLine();

        return this;
    }

    public Report Build()
    {
        return new Report($"Отчет за {DateTime.Now:yyyy-MM-dd}", _content.ToString());
    }
}


