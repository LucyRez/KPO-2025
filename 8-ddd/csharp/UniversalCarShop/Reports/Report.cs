namespace UniversalCarShop.Reports;

public record Report(string Title, string Content)
{
    public override string ToString()
    {
        return $"{Title}\n\n{Content}";
    }
}


